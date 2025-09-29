using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Auth;
using SecShare.Core.Dtos;
using SecShare.Servicer.Auth;

namespace AuthAPI.Controllers;
[Route("api/auth/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthAPIService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthAPIService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        var response = await _authService.Register(registrationRequestDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var response = await _authService.Login(loginRequestDto);
        if (!response.IsSuccess)
        {
            return Unauthorized(response);
        }
        return Ok(response);
    }

    [HttpGet("getUserInfor")]
    public async Task<IActionResult> GetUserInfor(string UserId)
    {
        var response = await _userService.getUserInfor(UserId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
