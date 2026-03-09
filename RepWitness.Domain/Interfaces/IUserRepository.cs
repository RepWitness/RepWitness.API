using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;

namespace RepWitness.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public ResponseType<bool> AreEmailAndUsernameUnique(string email, string username, Guid? userId = null);
    public ResponseType<User> GetUserByEmail(string email);
}
