using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SaasMultiTenant.DAL.Context;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.DAL
{
    public class ProjectDAL : IProjectsDAL
    {
        private readonly AppDbContext _context;

        public ProjectDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsByWorkspaceId(int workspaceId)
        {
            try
            {
                return await _context.Projects.Where(p => p.WorkspaceId == workspaceId)
                    .Include(p => p.CreatedBy).ToListAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la información de projectos.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Project?> CreateProject(Project project)
        {
            try
            {
                _context.Projects.Add(project);

                int numChanges = await _context.SaveChangesAsync();
                
                if(numChanges > 0)
                {
                    return project;
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al crear projecto.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
