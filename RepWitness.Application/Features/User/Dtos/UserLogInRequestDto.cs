namespace RepWitness.Application.Features.User.Dtos;

public class UserLogInRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
