using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;

namespace SaasMultiTenant.BL.Interfaces
{
    public interface IAuthBL
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<TokenResponse> GenerateToken(TokenRequest tokenRequest);
    }
}
