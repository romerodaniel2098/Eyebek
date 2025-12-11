namespace Eyebek.Domain.Entities;

public class Session
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public string Token { get; set; } = default!; 

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpirationDate { get; set; }

    public bool Active { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}