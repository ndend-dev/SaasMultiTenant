namespace SaasMultiTenant.Entity.Request
{
    public class TokenRequest
    {
        public int UserId { get; set; }
        public int WorkspaceId { get; set; }
    }
}
