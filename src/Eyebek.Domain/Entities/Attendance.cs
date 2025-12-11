using Eyebek.Domain.Enums;

namespace Eyebek.Domain.Entities;

public class Attendance
{
    public long Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public AttendanceType Type { get; set; }
    public AttendanceMethod Method { get; set; }

    public decimal? Confidence { get; set; }
    public string? CapturePhoto { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}