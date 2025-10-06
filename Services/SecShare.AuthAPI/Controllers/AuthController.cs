using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Auth;
using SecShare.Core.Dtos;
using SecShare.Servicer.Auth;
using System.Security.Claims;

namespace AuthAPI.Controllers;
[Route("api/auth/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthAPIService _authService;
    private readonly IUserAPIService _userService;

    public AuthController(IAuthAPIService authService, IUserAPIService userService)
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

    [Authorize]
    [HttpGet("getUserInfor")]
    public async Task<IActionResult> GetUserInfor()
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _userService.getUserInfor(UserId);
        if (!response.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = "User not found";
            return BadRequest(response);
        }
        return Ok(response);
    }

    [Authorize]
    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _userService.changePasswordAsync(changePasswordDto, UserId);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [Authorize]
    [HttpPost("updateInformation")]
    public async Task<IActionResult> UpdateInformation([FromBody] UserDto userDto)
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _userService.updateInformation(userDto, UserId);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}
