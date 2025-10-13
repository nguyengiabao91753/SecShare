using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Document;
using SecShare.Core.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecShare.DocumentAPI.Controllers;
[Route("api/document")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentAPIService _documentAPIService;
    public DocumentController(IDocumentAPIService documentAPIService)
    {
        _documentAPIService = documentAPIService;
    }
    //// GET: api/<DocumentController>
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    //// GET api/<DocumentController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //POST api/<DocumentController>
    [Authorize]
    [HttpPost("uploadMyFile")]
    public async Task<IActionResult> UploadMyFile([FromForm] UploadMyFileDto uploadMyFileDto)
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        uploadMyFileDto.UserId = UserId!;
        var rs = await _documentAPIService.UploadMyFileAsync(uploadMyFileDto);
        if (rs.IsSuccess)
        {
            return Ok(rs);
        }
        return BadRequest(rs);

    }


    [Authorize]
    [HttpPost("ShareMyFile")]
    public async Task<IActionResult> ShareMyFile([FromBody] ShareFileDto shareFileDto)
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        
        var rs = await _documentAPIService.ShareFileAsync(shareFileDto, UserId);
        if (rs.IsSuccess)
        {
            return Ok(rs);
        }
        return BadRequest(rs);

    }

    //// PUT api/<DocumentController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<DocumentController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
