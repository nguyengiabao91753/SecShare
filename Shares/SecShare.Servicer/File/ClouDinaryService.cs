using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SecShare.Base.File;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Servicer.File;
public class ClouDinaryService : IClouDinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;

    public ClouDinaryService(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("CloudinarySettings");
        var account = new Account(_configuration["CloudName"], _configuration["ApiKey"], _configuration["ApiSecret"]);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<ResponseDTO> DeleteFileAsync(string PublicId)
    {
        var deletionParams = new DeletionParams(PublicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        if (result.Result == "ok")
        {
            return new ResponseDTO
            {
                IsSuccess = true,
                Message = "File deleted successfully",
                Result = null
            };
        }
        else
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = "File deletion failed",
                Result = null
            };
        }
    }

    public async Task<ResponseDTO> UploadFileAsync(IFormFile file, string PublicId = null, string fileFolder = "default_folder")
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = fileFolder,
            UniqueFilename = false,
            PublicId = PublicId,
            Overwrite = true
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return new ResponseDTO
            {
                IsSuccess = true,
                Message = "File uploaded successfully",
                Result = new ClouDinaryResult
                {
                    Url = result.SecureUrl.ToString(),
                    PublicId = result.PublicId
                }
            };
        }
        else
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = "File upload failed",
                Result = null
            };
        }
    }
}
