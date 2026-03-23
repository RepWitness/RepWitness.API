namespace RepWitness.Application.Features.Role.Dtos;

public class UpdateRoleRequestDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
}
