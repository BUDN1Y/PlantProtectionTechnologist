using System;
using System.Collections.Generic;

namespace PlantProtectionTechnologist.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ActionType { get; set; } = null!;

    public string EntityType { get; set; } = null!;

    public int EntityId { get; set; }

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public DateTime? ActionTime { get; set; }

    public string? IpAddress { get; set; }

    public virtual User User { get; set; } = null!;
}
