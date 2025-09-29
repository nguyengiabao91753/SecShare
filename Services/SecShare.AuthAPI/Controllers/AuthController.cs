using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecShare.Base.Auth;
using SecShare.Core.Dtos;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
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
}
