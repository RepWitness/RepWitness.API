namespace RepWitness.Application.Features.Role.Dtos;

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
