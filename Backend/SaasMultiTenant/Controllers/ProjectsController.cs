using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaasMultiTenant.BL.Interfaces;
using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;

namespace SaasMultiTenant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsBL _projectsBL;
        public ProjectsController(IProjectsBL projectsBL)
        {
            _projectsBL = projectsBL;
        }


        [HttpGet]
        [Route("{workspaceId}")]
        [Authorize]
        public async Task<IActionResult> Get(int workspaceId)
        {
            try
            {
                List<ProjectsResponse> projects = await _projectsBL.GetProjects(workspaceId);

                return Ok(projects);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { Message = "El usuario no tiene permisos para esta acción" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([FromBody] ProjectCreateRequest projectsRequest)
        {
            try
            {
                string? userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                if (!int.TryParse(userIdStr, out int userId))
                {
                    return Unauthorized(new { Message = "El token de usuario no es válido o ha expirado." });
                }

                ProjectCreateResponse createResponse = await _projectsBL.CreateProject(projectsRequest, userId);

                return Ok(createResponse);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized( new { Message = "El usuario no tiene permisos para esta acción" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
