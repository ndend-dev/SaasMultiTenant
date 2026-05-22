using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
    }
}
