using Microsoft.Extensions.DependencyInjection;
using SaasMultiTenant.BL.Interfaces;
using SaasMultiTenant.DAL;

namespace SaasMultiTenant.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBussinessLogicLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDataAccessLayer(connectionString);

            services.AddScoped<IAuthBL, AuthBL>();
            services.AddScoped<IProjectsBL, ProjectsBL>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
