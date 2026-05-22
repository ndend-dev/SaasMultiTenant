using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaasMultiTenant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {

            try
            {
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                // Ejemplo: El usuario del token no tiene permisos para esta acción
                return StatusCode(403, new { Message = ex.Message });
            }
        }
    }
}
