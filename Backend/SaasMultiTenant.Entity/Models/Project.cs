using System;
using System.Collections.Generic;

namespace SaasMultiTenant.Entity.Models;

public partial class Project
{
    public int Id { get; set; }

    public int? WorkspaceId { get; set; }

    public int? CreatedById { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? CreatedBy { get; set; }

    public virtual Workspace? Workspace { get; set; }
}
