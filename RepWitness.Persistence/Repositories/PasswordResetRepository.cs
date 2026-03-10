using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Persistence.Context;
using System.Data.Entity;

namespace RepWitness.Persistence.Repositories;

public class PasswordResetRepository(RepWitnessContext context) : IPasswordResetRepository
{
    public ResponseType<PasswordReset> Add(PasswordReset entity)
    {
        try
        {
            context.PasswordResets.Add(entity);
            context.SaveChanges();

            return new ResponseType<PasswordReset>
            {
                Object = entity,
                Message = "PasswordReset was added successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<PasswordReset>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<PasswordReset> Delete(Guid id)
    {
        try
        {
            var passwordReset = GetOne(id);

            if (passwordReset is not { IsSuccess: true, Object: not null })
            {
                return new ResponseType<PasswordReset>()
                {
                    Message = passwordReset.Message,
                    IsSuccess = false
                };
            }

            passwordReset.Object.IsDeleted = true;

            context.PasswordResets.Update(passwordReset.Object);
            context.SaveChanges();

            return new ResponseType<PasswordReset>
            {
                Object = null,
                Message = "PasswordReset updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<PasswordReset>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<PasswordReset> GetAll()
    {
        try
        {
            var passwordResets = context.PasswordResets.Include(p=>p.User);

            return new ResponseType<PasswordReset>
            {
                Object = null,
                Collection = [.. passwordResets],
                Message = "PasswordResets was sent successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<PasswordReset>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<PasswordReset> GetOne(Guid id)
    {
        try
        {
            var getAllPasswordResets = GetAll();
            if (getAllPasswordResets is not { IsSuccess: true, Collection: not null })
            {
                return new ResponseType<PasswordReset>()
                {
                    Message = getAllPasswordResets.Message,
                    IsSuccess = false
                };
            }

            var passwordReset = getAllPasswordResets.Collection.FirstOrDefault(u => u.Id == id);
            if (passwordReset == null)
            {
                return new ResponseType<PasswordReset>()
                {
                    Message = "PasswordReset could not be find!",
                    IsSuccess = false
                };
            }

            return new ResponseType<PasswordReset>()
            {
                Object = passwordReset,
                Message = "PasswordReset find successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<PasswordReset>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<Guid?> IsLinkValid(Guid linkId)
    {
        try
        {
            var passwordReset = GetOne(linkId);

            if (passwordReset is not { IsSuccess: true, Object: not null })
            {
                return new ResponseType<Guid?>()
                {
                    Message = passwordReset.Message,
                    IsSuccess = false
                };
            }

            if (passwordReset.Object.ExpirationDate < DateTime.UtcNow)
            {
                return new ResponseType<Guid?>()
                {
                    Object = null,
                    Message = "Link has expired!",
                    IsSuccess = false
                };
            }

            if (!passwordReset.Object.IsActive)
            {
                return new ResponseType<Guid?>()
                {
                    Object = null,
                    Message = "Link has already been used!",
                    IsSuccess = false
                };
            }

            return new ResponseType<Guid?>()
            {
                Object = passwordReset.Object!.UserId,
                Message = "Link has already been used!",
                IsSuccess = false
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<Guid?>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
        ;
    }

    public ResponseType<PasswordReset> Update(PasswordReset entity)
    {
        try
        {
            context.PasswordResets.Update(entity);
            context.SaveChanges();

            return new ResponseType<PasswordReset>
            {
                Object = entity,
                Message = "PasswordReset updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<PasswordReset>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
        ;
    }

    public ResponseType<bool> UseLink(Guid linkId)
    {
        try
        {
            var passwordReset = GetOne(linkId);

            if (passwordReset is not { IsSuccess: true, Object: not null })
            {
                return new ResponseType<bool>()
                {
                    Message = passwordReset.Message,
                    IsSuccess = false
                };
            }

            passwordReset.Object.IsActive = false;

            context.PasswordResets.Update(passwordReset.Object);
            context.SaveChanges();

            return new ResponseType<bool>
            {
                Object = true,
                Message = "PasswordReset updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<bool>
            {
                Object = false,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        };
    }
}
