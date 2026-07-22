using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Managment.BLL.DTOS.Auth;
using Task_Managment.BLL.Services;

namespace Task_Managment.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")] 
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDTO dto) 
        { 
            var result = await _authService.RegisterAsync(dto); 
            return Ok(result); 
        }

        [HttpPost("login")] 
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDTO dto) 
        { 
            
            var result = await _authService.LoginAsync(dto); 
            return Ok(result); 
        }
    }
}
