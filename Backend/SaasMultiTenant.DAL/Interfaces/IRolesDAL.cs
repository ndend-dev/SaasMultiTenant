using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL.Interfaces
{
    public interface IRolesDAL
    {
        Task<Role?> GetRoleById(int id);
        Task<Role?> GetRoleByName(string name);
    }
}
