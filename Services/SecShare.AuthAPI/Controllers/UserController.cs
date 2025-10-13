using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Auth;
using SecShare.Core.Dtos;

namespace AuthAPI.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserAPIService _userAPIService;

    public UserController(IUserAPIService userAPIService)
    {
        _userAPIService = userAPIService;
    }

    [HttpGet("getUsersShared")]
    public async Task<IActionResult> GetUsersShared([FromBody] IEnumerable<string> listUserId)
    {
        var response = await _userAPIService.getAllUsersShared(listUserId);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}
