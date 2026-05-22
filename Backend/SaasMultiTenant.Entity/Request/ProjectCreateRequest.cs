namespace SaasMultiTenant.Entity.Request
{
    public class ProjectCreateRequest
    {
        public int WorkspaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
