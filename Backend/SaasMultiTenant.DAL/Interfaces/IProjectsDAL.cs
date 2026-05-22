using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL.Interfaces
{
    public interface IProjectsDAL
    {
        Task<List<Project>> GetProjectsByWorkspaceId(int workspaceId);
        Task<Project?> CreateProject(Project project);
    }
}
