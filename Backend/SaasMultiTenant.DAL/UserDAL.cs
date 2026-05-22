using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SaasMultiTenant.DAL.Context;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL
{
    public class UserDAL : IUserDAL
    {

        private readonly AppDbContext _context;

        public UserDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de usuarios.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> GetUserById(int id)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de usuarios.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
