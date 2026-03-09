namespace RepWitness.Application.Features.Auth.Dtos;

public class UserLogInResponseDto
{
    public Guid Id { get; init; }
    public string Token { get; init; } = string.Empty;
}
