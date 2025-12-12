using Eyebek.Domain.Enums;

namespace Eyebek.Application.DTOs.Users;

public class UserListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public string Document { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Photo { get; set; }
}