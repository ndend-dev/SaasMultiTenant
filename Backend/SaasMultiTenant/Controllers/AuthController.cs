using Microsoft.AspNetCore.Mvc;
using SaasMultiTenant.BL.Interfaces;
using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;
using SaasMultiTenant.Utils;

namespace SaasMultiTenant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBL _authBL;
        public AuthController(IAuthBL authBL)
        {
            _authBL = authBL;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                LoginResponse response = await _authBL.Login(login);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
