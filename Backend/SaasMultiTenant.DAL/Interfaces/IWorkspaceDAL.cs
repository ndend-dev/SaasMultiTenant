using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL.Interfaces
{
    public interface IWorkspaceDAL
    {
        Task<List<Workspace>> GetWorkspacesByUserId(int userId);
    }
}
