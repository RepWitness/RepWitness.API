namespace RepWitness.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateToken(string email);
}