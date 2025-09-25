using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Auth;
using SecShare.Core.Dtos;

namespace AuthAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IAuthService _authService;

    public RegisterController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
    {
        var response = await _authService.Register(registrationRequestDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
