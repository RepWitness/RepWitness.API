using RepWitness.Domain.Enums;

namespace RepWitness.Application.Features.User.Dtos;

public class UpdateUserRequestDto
{
    public Guid? Id { get; set; }
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Password { get; set; } = null!;
    public string? Username { get; set; } = null!;
    public Gender? Gender { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; } = null!;
    public int? Height { get; set; } = null!;
    public float? Weight { get; set; } = null!;
    public Guid? RoleId { get; set; } = null!;
}
