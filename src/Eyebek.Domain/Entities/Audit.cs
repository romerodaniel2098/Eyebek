namespace Eyebek.Domain.Entities;

public class Audit
{
    public long Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public int? UserId { get; set; }
    public User? User { get; set; }

    public string Action { get; set; } = default!;
    public string Entity { get; set; } = default!;
    public string? EntityId { get; set; }

    public string? Description { get; set; }

    public string? PreviousData { get; set; }
    public string? NewData { get; set; }

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}