namespace SaasMultiTenant.Entity.Models;

public partial class UserWorkspace
{
    public int UserId { get; set; }

    public int WorkspaceId { get; set; }

    public int RoleId { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;
}
