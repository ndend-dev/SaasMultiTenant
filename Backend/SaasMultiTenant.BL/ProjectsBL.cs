using SaasMultiTenant.BL.Interfaces;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;
using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;
using SaasMultiTenant.Utils;

namespace SaasMultiTenant.BL
{
    public class ProjectsBL : IProjectsBL
    {
        private readonly IProjectsDAL _projectDAL;
        private readonly IWorkspaceDAL _workspaceDAL;

        public ProjectsBL(IProjectsDAL projectsDAL, IWorkspaceDAL workspaceDAL)
        {
            _projectDAL = projectsDAL;
            _workspaceDAL = workspaceDAL;
        }

        public async Task<List<ProjectsResponse>> GetProjects(int workspaceId)
        {
            try
            {
                Workspace? workspace = await _workspaceDAL.GetWorkspaceById(workspaceId);

                if (workspace == null)
                {
                    throw new ArgumentException("El espacio de trabajo no es valido.");
                }

                List<Project> projects = await _projectDAL.GetProjectsByWorkspaceId(workspaceId);

                if (!projects.Any())
                    return new List<ProjectsResponse>();

                List<ProjectsResponse> response = projects.Select(item => new ProjectsResponse
                {
                    Id = item.Id,
                    WorkspaceName = workspace.Name,
                    WorkspaceId = workspace.Id,
                    CreatedByName = item.CreatedBy?.Firstname + " " + item.CreatedBy?.Lastname,
                    CreatedById = item.CreatedById,
                    Name = item.Name,
                    Description = item.Description,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectCreateResponse> CreateProject(ProjectCreateRequest createRequest, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createRequest.Name) || string.IsNullOrWhiteSpace(createRequest.Description) || createRequest.WorkspaceId <= 0)
                {
                    throw new ArgumentException("No se puede crear el proyecto. Faltan datos obligatorios por completar.");
                }

                Project newProject = new Project();
                newProject.WorkspaceId = createRequest.WorkspaceId;
                newProject.Name = createRequest.Name;
                newProject.Description = createRequest.Description;
                newProject.Status = Constants.Status.Activo;
                newProject.CreatedById = userId;


                Project? project = await _projectDAL.CreateProject(newProject);

                if (project == null)
                {
                    throw new ArgumentException("Se presento un problema al crear el proyecto. Por favor, intentelo mas tarde.");
                }

                ProjectCreateResponse response = new ProjectCreateResponse();
                response.ProjectId = project.Id;
                response.WorkspaceId = project.WorkspaceId;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
