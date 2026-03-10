using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;

namespace RepWitness.Domain.Interfaces;

public interface IPasswordResetRepository : IRepository<PasswordReset>
{
    public ResponseType<Guid?> IsLinkValid(Guid linkId);
    public ResponseType<bool> UseLink(Guid linkId);
}
