namespace RepWitness.Application.Features.Auth.Dtos;

public class PasswordResetDto
{
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
