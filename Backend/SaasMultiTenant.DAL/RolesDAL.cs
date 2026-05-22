using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SaasMultiTenant.DAL.Context;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL
{
    public class RolesDAL : IRolesDAL
    {
        private readonly AppDbContext _context;


        public RolesDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleById(int id)
        {
            try
            {
                return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de roles.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Role?> GetRoleByName(string name)
        {
            try
            {
                return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de roles.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
