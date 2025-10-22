using Microsoft.AspNetCore.Identity;
using SecShare.Base.Document;
using SecShare.Base.File;
using SecShare.Core.Auth;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Core.Document;
using SecShare.Helper.Security;
using SecShare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecShare.Helper.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace SecShare.Servicer.Document;
public class DocumentAPIService : IDocumentAPIService
{
    private readonly SecShareDbContext _db;
    private readonly IClouDinaryService _cloudinaryService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DocumentAPIService(SecShareDbContext db, IClouDinaryService cloudinaryService, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _cloudinaryService = cloudinaryService;
        _userManager = userManager;
    }

    public async Task<ResponseDTO> ListUserFileAsync(string UserId)
    {
        try
        {
            var documents = await _db.Documents
                            .Where(d => d.OwnerId == UserId)
                            .ToListAsync();

            var documentDtos = documents.Select(u => new DocumentDto
            {
                Id = u.Id,
                FileName = u.FileName,
                FileSize = u.FileSize,
                FileType = Path.GetExtension(u.FileName)?.TrimStart('.'),
                UpdatedAt = u.UpdatedAt,
            }).ToList();


            return new ResponseDTO
            {
                IsSuccess = true,
                Result = documentDtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = ex.Message
            };
        }
    }

    public async Task<Stream> GetFileAsync(Guid DocumentId, string UserId)
    {
        // Lấy thông tin user
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
            throw new FileNotFoundException("User not found");

        // Lấy document
        var document = await _db.Documents.FindAsync(DocumentId);
        if (document == null)
            throw new FileNotFoundException("Document not found");

        // Lấy bản ghi share (AESKey đã mã hóa bằng publicKey của người xem)
        var share = await _db.Shares
            .FirstOrDefaultAsync(s => s.DocumentId == DocumentId && s.ReceiverId == UserId);

        if (share == null)
            throw new FileNotFoundException("Document Shared not found");

        try
        {
            // Giải mã private key
            byte[] privateKeyPem = RsaKeyPairHelper.DecryptPrivateKey(user.RsaPrivateKeyEncrypted, user.PasswordHash);

            // Dùng private key giải mã AESKey
            byte[] aesKey = RsaKeyPairHelper.DecryptAESKey(share.EncryptedAESKey, privateKeyPem);

            // Giải mã file (CipherText + IV)
            byte[] iv = Convert.FromBase64String(document.IV);
            byte[] plainBytes = await AESHelper.DecryptFileAsync(document.Ciphertext, aesKey, iv);

            var memoryStream = new MemoryStream(plainBytes);
            memoryStream.Position = 0;

            return memoryStream;
        }
        catch (CryptographicException ex)
        {
            throw new UnauthorizedAccessException("Cannot decrypt document (invalid key or IV).", ex);
        }


    }

    private string GetMimeType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".pdf" => "application/pdf",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".doc" => "application/msword",
            _ => "application/octet-stream"
        };
    }

    public async Task<ResponseDTO> ShareFileAsync(ShareFileDto share, string UserId)
    {
        if (share.ReceiverEmail == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = "Receiver is required"
            };
        }
        if (share.DocumentId == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = "DocumentId is required"
            };
        }
        var sender = await _userManager.FindByIdAsync(UserId);
        if (sender == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = "Sender not found!"
            };
        }
        var receiver = await _userManager.FindByEmailAsync(share.ReceiverEmail);
        if (receiver == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = "Receiver not found!"
            };
        }

        var document = await _db.Documents.FindAsync(share.DocumentId);
        var origirinalShare = await _db.Shares
            .Where(s => s.DocumentId == share.DocumentId && s.ReceiverId == UserId).FirstOrDefaultAsync();
        if (document == null || origirinalShare == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = "Document not found or you don't have permission to share this file!"
            };
        }
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var aesKey = RsaKeyPairHelper.DecryptAESKey(origirinalShare.EncryptedAESKey, RsaKeyPairHelper.DecryptPrivateKey(sender.RsaPrivateKeyEncrypted, sender.PasswordHash));
            var encryptedKey = RsaKeyPairHelper.EncryptAESKey(aesKey, receiver.PublicKey!);
            var newShare = new Share
            {
                DocumentId = share.DocumentId,
                SenderId = UserId,
                ReceiverId = receiver.Id,
                EncryptedAESKey = encryptedKey,
                Permissions = share.Permissions
            };
            await _db.Shares.AddAsync(newShare);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
            return new ResponseDTO
            {
                IsSuccess = true,
                Message = "File shared successfully"
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = ex.Message
            };
        }


    }

    public async Task<ResponseDTO> UploadMyFileAsync(UploadMyFileDto uploadMyFile)
    {
        if (uploadMyFile == null || uploadMyFile.File.Length == 0)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = "File is empty"
            };
        }
        var user = await _userManager.FindByIdAsync(uploadMyFile.UserId);
        if (user == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = "User not found!"
            };
        }

        using var transaction = await _db.Database.BeginTransactionAsync();
        //var filePath = await _cloudinaryService.UploadFileAsync(uploadMyFile.AttachFile.File, fileFolder: user.Id);
        //ClouDinaryResult clouDinaryResult = (ClouDinaryResult)filePath.Result;
        try
        {
            var (aesKey, iv) = AESHelper.GenerateAESKey();
            byte[] cipherText = await AESHelper.EncryptFileAsync(uploadMyFile.File, aesKey, iv);

            var encryptedKey = RsaKeyPairHelper.EncryptAESKey(aesKey, user.PublicKey!);

            var document = new Documents
            {
                OwnerId = uploadMyFile.UserId,
                FileName = uploadMyFile.FileName + "." + uploadMyFile.Type,
                FileSize = uploadMyFile.File.Length,
                FilePath = " ",
                Ciphertext = cipherText,
                IV = Convert.ToBase64String(iv),

            };
            await _db.Documents.AddAsync(document);
            await _db.SaveChangesAsync();

            var sharedFile = new Share
            {
                DocumentId = document.Id,
                SenderId = user.Id!,
                ReceiverId = user.Id!,
                EncryptedAESKey = encryptedKey,
                Permissions = SD.FileAccess.All.ToString()

            };
            await _db.Shares.AddAsync(sharedFile);
            await _db.SaveChangesAsync();


            await transaction.CommitAsync();
            return new ResponseDTO
            {
                IsSuccess = true,
                Message = "File shared successfully"
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            //await _cloudinaryService.DeleteFileAsync(clouDinaryResult.PublicId);
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ResponseDTO> GetListUsersShared(string UserId, Guid docId)
    {
        var rs = new ResponseDTO();
        var receiverIds = await _db.Shares
                        .Where(s => s.SenderId == UserId && s.DocumentId == docId && s.ReceiverId != UserId)
                        .Select(s => s.ReceiverId)
                        .ToListAsync();

        rs.Result = receiverIds;
        rs.IsSuccess = true;
        return rs;
    }

    public async Task<ResponseDTO> ListReceiveFileAsync(string UserId)
    {
        var rs = new ResponseDTO();
        try
        {
            var listDocuments = await _db.Shares
                                .Where(s => s.ReceiverId == UserId)
                                .ToListAsync();
            var documentDtos = new List<DocumentDto>();
            foreach (var share in listDocuments)
            {
                var document = await _db.Documents.FindAsync(share.DocumentId);
                if (document != null)
                {
                    documentDtos.Add(new DocumentDto
                    {
                        Id = document.Id,
                        FileName = document.FileName,
                        FileSize = document.FileSize,
                        FileType = Path.GetExtension(document.FileName)?.TrimStart('.'),
                        UpdatedAt = document.UpdatedAt,
                    });
                }
            }
            rs.Result = documentDtos;
            rs.IsSuccess = true;

        }
        catch (Exception ex)
        {
            rs.IsSuccess = false;
            rs.Code = "-1";
            rs.Message = ex.Message;
        }
        return rs;
    }
}
