using Eyebek.Domain.Enums;

namespace Eyebek.Application.DTOs.Attendance;

public class AttendanceListItemDto
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    
    public DateTime Timestamp { get; set; }
    public AttendanceType Type { get; set; }
    public AttendanceMethod Method { get; set; }
    public AttendanceStatus Status { get; set; }
    
    public decimal? Confidence { get; set; }
    public string? CapturePhoto { get; set; }
}
