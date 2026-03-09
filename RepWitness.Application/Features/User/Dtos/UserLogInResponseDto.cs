namespace RepWitness.Application.Features.User.Dtos;

public class UserLogInResponseDto
{
    public Guid Id { get; init; }
    public string Token { get; init; } = string.Empty;
}
