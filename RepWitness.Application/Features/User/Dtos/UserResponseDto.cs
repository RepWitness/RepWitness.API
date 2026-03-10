using RepWitness.Domain.Enums;

namespace RepWitness.Application.Features.User.Dtos;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public string Username { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Height { get; set; }
    public float Weight { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public string RoleDescription { get; set; } = null!;
}
