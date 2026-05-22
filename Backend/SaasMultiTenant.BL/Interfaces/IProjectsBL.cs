using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;

namespace SaasMultiTenant.BL.Interfaces
{
    public interface IProjectsBL
    {
        Task<List<ProjectsResponse>> GetProjects(int workspaceId);
        Task<ProjectCreateResponse> CreateProject(ProjectCreateRequest createRequest, int userId);
    }
}
