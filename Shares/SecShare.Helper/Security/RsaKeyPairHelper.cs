using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Helper.Security;
public static class RsaKeyPairHelper
{
    public static (string PublicKeyPem, byte[] PrivateKeyEncrypted) GenerateKeyPair( string passwordHash)
    {
        using var rsa = System.Security.Cryptography.RSA.Create(2048);
        var publicKey = rsa.ExportSubjectPublicKeyInfo();
        string publicKeyPem = ToPem(publicKey, "PUBLIC KEY");

        // Export private key PKCS#8
        var privateKey = rsa.ExportPkcs8PrivateKey();

        byte[] privateKeyEncrypted = EncryptPrivateKey(privateKey, passwordHash);

        return (publicKeyPem, privateKeyEncrypted);
    }
    // =================================
    // 2. Re-encrypt khi user đổi password
    // =================================
    public static byte[] ReEncryptPrivateKey(byte[] oldEncryptedPrivateKey, string oldPassword, string newPassword)
    {
        // Giải mã bằng password cũ
        byte[] privateKeyBytes = DecryptPrivateKey(oldEncryptedPrivateKey, oldPassword);

        // Mã hóa lại bằng password mới
        return EncryptPrivateKey(privateKeyBytes, newPassword);
    }

    // ========================
    // Encrypt private key (AES)
    // ========================
    private static byte[] EncryptPrivateKey(byte[] privateKey, string password)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;

        // Salt cố định 16 bytes (random để bảo mật hơn → lưu kèm với ciphertext)
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        // Derive key từ password + salt
        using var derive = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        aes.Key = derive.GetBytes(32);
        aes.IV = derive.GetBytes(16);

        using var encryptor = aes.CreateEncryptor();
        byte[] cipher = encryptor.TransformFinalBlock(privateKey, 0, privateKey.Length);

        // Trả về (Salt + Cipher) để khi decrypt có salt gốc
        byte[] result = new byte[salt.Length + cipher.Length];
        Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
        Buffer.BlockCopy(cipher, 0, result, salt.Length, cipher.Length);

        return result;


    }

    // ========================
    // Decrypt private key (AES)
    // ========================
    private static byte[] DecryptPrivateKey(byte[] encryptedData, string password)
    {
        

        // Tách salt (16 bytes) và cipher
        byte[] salt = new byte[16];
        Buffer.BlockCopy(encryptedData, 0, salt, 0, salt.Length);

        byte[] cipher = new byte[encryptedData.Length - salt.Length];
        Buffer.BlockCopy(encryptedData, salt.Length, cipher, 0, cipher.Length);

        using var aes = Aes.Create();
        aes.KeySize = 256;

        using var derive = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        aes.Key = derive.GetBytes(32);
        aes.IV = derive.GetBytes(16);

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
    }


    // Xuất sang PEM format
    private static string ToPem(byte[] data, string label)
    {
        var base64 = Convert.ToBase64String(data);
        var sb = new StringBuilder();
        sb.AppendLine($"-----BEGIN {label}-----");
        for (int i = 0; i < base64.Length; i += 64)
        {
            sb.AppendLine(base64.Substring(i, Math.Min(64, base64.Length - i)));
        }
        sb.AppendLine($"-----END {label}-----");
        return sb.ToString();
    }
}
