using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecShare.Base.Document;
using SecShare.Core.BaseClass;
using SecShare.Helper.Generals;
using SecShare.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecShare.DocumentAPI.Controllers;
[Route("api/file")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IDocumentAPIService _documentAPIService;
    private readonly IConfiguration _config;
    private readonly SecShareDbContext _db;
    public FileController(IDocumentAPIService documentAPIService, IConfiguration config, SecShareDbContext secShareDb)
    {
        _documentAPIService = documentAPIService;
        _config = config;
        _db = secShareDb;
    }

    [HttpGet("view/{id}")]
    [Authorize]
    public async Task<IActionResult> GetTemporaryViewUrl(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Tạo token ký ngắn hạn (ví dụ JWT chỉ sống 3 phút)
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["ApiSettings:TempUrlKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
            new Claim("docId", id.ToString()),
            new Claim("userId", userId)
        }),
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);
        var signedUrl = $"{Request.Scheme}://{Request.Host}/api/file/stream/{tokenHandler.WriteToken(token)}";


        return Ok(new ResponseDTO
        {
            IsSuccess = true,
            Result = signedUrl
        });
    }

    [HttpGet("stream/{token}")]
    [Authorize]
    public async Task<IActionResult> StreamDecryptedFile(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["ApiSettings:TempUrlKey"]);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            var docId = Guid.Parse(principal.FindFirst("docId")!.Value);
            var userId = principal.FindFirst("userId")!.Value;

            var stream = await _documentAPIService.GetFileAsync(docId, userId);
            var document = await _db.Documents.FindAsync(docId);
            var contentType = FileHelper.GetMimeType(document!.FileName);
            Response.Headers.Add("Content-Disposition", $"inline; filename={document.FileName}");
            return File(stream, contentType);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid or expired link.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}
