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

    public async Task<ResponseDTO> ShareFileAsync(ShareFileDto share, string UserId)
    {
        if(share.ReceiverEmail == null)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Code = "-1",
                Message = "Receiver is required"
            };
        }
        if(share.DocumentId == null)
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
        if (uploadMyFile.AttachFile == null || uploadMyFile.AttachFile.File.Length == 0)
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
            byte[] cipherText = await AESHelper.EncryptFileAsync(uploadMyFile.AttachFile.File, aesKey, iv);

            var encryptedKey = RsaKeyPairHelper.EncryptAESKey(aesKey, user.PublicKey!);

            var document = new Documents
            {
                OwnerId = uploadMyFile.UserId,
                FileName = uploadMyFile.AttachFile.FileName,
                FileSize = uploadMyFile.AttachFile.File.Length,
                FilePath =" ",
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
}
