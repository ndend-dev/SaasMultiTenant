using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SaasMultiTenant.DAL.Context;
using SaasMultiTenant.DAL.Interfaces;

namespace SaasMultiTenant.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, string conecctionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(conecctionString);
            });

            services.AddScoped<IProjectsDAL, ProjectDAL>();
            services.AddScoped<IRolesDAL, RolesDAL>();
            services.AddScoped<IUserDAL, UserDAL>();
            services.AddScoped<IWorkspaceDAL, WorkspaceDAL>();

            return services;
        }
    }
}
