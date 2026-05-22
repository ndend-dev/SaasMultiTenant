using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL.Interfaces
{
    public interface IWorkspaceDAL
    {
        Task<Workspace?> GetWorkspaceById(int id);
        Task<List<Workspace>> GetWorkspacesByUserId(int userId);
        Task<UserWorkspace?> GetUserWorkspaceByUserIdAndWorkspaceId(int userId, int workspaceId);
    }
}
