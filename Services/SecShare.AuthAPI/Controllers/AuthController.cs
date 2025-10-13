using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    private readonly IotpService _otpService;
    private readonly IEmailAPIService _emailAPIService;

    public AuthController(IAuthAPIService authService, IUserAPIService userService, IMemoryCache memoryCache, IotpService otpService, IEmailAPIService emailAPIService)
    {
        _authService = authService;
        _userService = userService;
        _memoryCache = memoryCache;
        _otpService = otpService;
        _emailAPIService = emailAPIService;
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

    [Authorize]
    [HttpPost("sendOTP")]
    public async Task<IActionResult> SendOTPAsync([FromBody] OtpDto otpDto)
    {
        var response = await _otpService.sendOtpAsync(otpDto);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [Authorize]
    [HttpPost("verifyOTP")]
    public async Task<IActionResult> VerifyOTPAsync([FromBody] OtpDto otpDto)
    {
        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _otpService.verifyOtpAsync(otpDto);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpGet("checkEmailConfirmed")]
    public async Task<IActionResult> CheckEmailConfirmedAsync (string userName)
    {
        var response = await _emailAPIService.checkEmailConfirmed(userName);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}
