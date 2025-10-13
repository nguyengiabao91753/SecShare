using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecShare.Base.Document;
using SecShare.Core.Dtos;
using SecShare.DocumentAPI.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecShare.DocumentAPI.Controllers;
[Route("api/document")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentAPIService _documentAPIService;
    private readonly IUServiceConnect _userService;

    public DocumentController(IDocumentAPIService documentAPIService, IUServiceConnect userService)
    {
        _documentAPIService = documentAPIService;
        _userService = userService;
    }

    // GET: api/<DocumentController>
    [Authorize]
    [HttpGet("getListDoc")]
    public async Task<IActionResult> GetListDoc()
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var rs = await _documentAPIService.ListUserFileAsync(UserId!);
        if (rs.IsSuccess)
        {

            return Ok(rs);
        }
        return BadRequest(rs);
    }

    [Authorize]
    [HttpGet("getListUsersShare/{docId}")]
    public async Task<IActionResult> GetListUsersShared( string docId)
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var rs = await _documentAPIService.GetListUsersShared(UserId!,Guid.Parse(docId));
        if (rs.IsSuccess)
        {
            var userIds = (IEnumerable<string>)rs.Result!;
            var userRs = await _userService.GetUsersShared(userIds);
            if (userRs.IsSuccess)
            {
                IEnumerable<UserDto> userList = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(Convert.ToString(userRs.Result!));
                rs.Result = userList;
            }
            return Ok(rs);
        }
        return BadRequest(rs);
    }

    //POST api/<DocumentController>
    [Authorize]
    [HttpPost("uploadMyFile")]
    [DisableRequestSizeLimit]
    [Consumes("multipart/form-data")]
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
    [HttpPost("shareMyFile")]

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
