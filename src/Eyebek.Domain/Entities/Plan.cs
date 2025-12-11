using System.ComponentModel.DataAnnotations.Schema;

namespace Eyebek.Domain.Entities;

[Table("plans")] 
public class Plan
{
    public int Id { get; set; }

    public string Category { get; set; } = default!;
    public decimal Price { get; set; }
    
    public int Duration { get; set; }

    public string Description { get; set; } = default!;

    public int UserCapacity { get; set; }

    public string? Features { get; set; }

    public bool Active { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}