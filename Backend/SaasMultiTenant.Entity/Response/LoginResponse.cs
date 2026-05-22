using SaasMultiTenant.Entity.Models;

namespace SaasMultiTenant.Entity.Response
{
    public class LoginResponse
    {
        public User User { get; set; }
        public List<Workspace> Workspaces { get; set; } 
    }
}
