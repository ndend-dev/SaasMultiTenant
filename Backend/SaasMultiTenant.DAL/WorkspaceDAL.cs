using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SaasMultiTenant.DAL.Context;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL
{
    public class WorkspaceDAL : IWorkspaceDAL
    {
        private readonly AppDbContext _context;

        public WorkspaceDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Workspace?> GetWorkspaceById(int id)
        {
            try
            {
                return await _context.Workspaces.FirstOrDefaultAsync(w => w.Id == id);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de workspaces.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Workspace>> GetWorkspacesByUserId(int userId)
        {
            try
            {
                var userWorkspaces = await _context.UserWorkspaces.Where(w => w.UserId == userId)
                                    .Select(w => w.WorkspaceId).ToListAsync();

                if (!userWorkspaces.Any())
                {
                    return new List<Workspace>();
                }

                return await _context.Workspaces
                .Where(w => userWorkspaces.Contains(w.Id)).ToListAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de workspaces.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserWorkspace?> GetUserWorkspaceByUserIdAndWorkspaceId(int userId, int workspaceId)
        {
            try
            {
                return await _context.UserWorkspaces
                .FirstOrDefaultAsync(w => w.UserId == userId && w.WorkspaceId == workspaceId);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de workspaces.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
