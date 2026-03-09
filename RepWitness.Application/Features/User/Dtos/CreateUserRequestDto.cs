using RepWitness.Domain.Enums;

namespace RepWitness.Application.Features.User.Dtos;

public class CreateUserRequestDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Height { get; set; }
    public float Weight { get; set; }
    public Guid RoleId { get; set; }
}
