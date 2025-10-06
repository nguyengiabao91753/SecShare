using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Helper.Security;
public static class AESHelper
{
    public static (byte[] Key, byte[] IV) GenerateAESKey(int keySize = 256)
    {
        using (var aes = Aes.Create())
        {
            aes.KeySize = keySize;
            aes.GenerateKey();
            aes.GenerateIV();
            return (aes.Key, aes.IV);
        }
    }
    public static async Task<byte[]> EncryptFileAsync(IFormFile file, byte[] aesKey, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = iv;

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

        await file.CopyToAsync(cryptoStream);  // copy trực tiếp dữ liệu file vào crypto stream
        await cryptoStream.FlushAsync();
        cryptoStream.FlushFinalBlock();

        return memoryStream.ToArray();
    }

    public static async Task<byte[]> DecryptFileAsync(IFormFile encryptedFile, byte[] aesKey, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = iv;

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(encryptedFile.OpenReadStream(), aes.CreateDecryptor(), CryptoStreamMode.Read);

        await cryptoStream.CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    public static byte[] GenerateRandomKey(int size = 32) // 32 bytes = 256-bit
    {
        using var rng = RandomNumberGenerator.Create();
        var key = new byte[size];
        rng.GetBytes(key);
        return key;
    }
}