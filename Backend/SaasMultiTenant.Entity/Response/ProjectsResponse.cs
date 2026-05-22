namespace SaasMultiTenant.Entity.Response
{
    public class ProjectsResponse
    {
        public int Id { get; set; }

        public string WorkspaceName { get; set; } = string.Empty;
        public int? WorkspaceId { get; set; }

        public string CreatedByName { get; set; } = string.Empty;
        public int? CreatedById { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
