namespace SaasMultiTenant.BL.Interfaces
{
    public interface ITokenService
    {
        string GenerateUserToken(string username, int userId, string email, string role);
    }
}
