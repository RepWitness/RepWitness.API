using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Persistence.Context;

namespace RepWitness.Persistence.Repositories;

public class RoleRepository(RepWitnessContext context) : IRoleRepository
{
    public ResponseType<Role> Add(Role entity)
    {
        try
        {
            context.Roles.Add(entity);
            context.SaveChanges();

            return new ResponseType<Role>
            {
                Object = entity,
                Message = "Role was added successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<Role>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<Role> Delete(Guid id)
    {
        try
        {
            var role = GetOne(id);

            if (role is not { IsSuccess: true, Object: not null })
            {
                return new ResponseType<Role>()
                {
                    Message = role.Message,
                    IsSuccess = false
                };
            }

            role.Object.IsDeleted = true;

            context.Roles.Update(role.Object);
            context.SaveChanges();

            return new ResponseType<Role>
            {
                Object = null,
                Message = "Role updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<Role>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<Role> GetAll()
    {
        try
        {
            var roles = context.Roles;

            return new ResponseType<Role>
            {
                Object = null,
                Collection = [.. roles],
                Message = "Roles was sent successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<Role>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<Role> GetOne(Guid id)
    {
        try
        {
            var getAllRoles = GetAll();
            if (getAllRoles is not { IsSuccess: true, Collection: not null })
            {
                return new ResponseType<Role>()
                {
                    Message = getAllRoles.Message,
                    IsSuccess = false
                };
            }

            var role = getAllRoles.Collection.FirstOrDefault(u => u.Id == id);
            if (role == null)
            {
                return new ResponseType<Role>()
                {
                    Message = "Role could not be find!",
                    IsSuccess = false
                };
            }

            return new ResponseType<Role>()
            {
                Object = role,
                Message = "Role find successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<Role>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<Role> Update(Role entity)
    {
        try
        {
            context.Roles.Update(entity);
            context.SaveChanges();

            return new ResponseType<Role>
            {
                Object = entity,
                Message = "Role updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<Role>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
        ;
    }
}
