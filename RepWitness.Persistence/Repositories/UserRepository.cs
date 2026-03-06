using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Persistence.Context;
using System.Data.Entity;

namespace RepWitness.Persistence.Repositories;

public class UserRepositor(RepWitnessContext context) : IUserRepository
{
    public ResponseType<User> Add(User entity)
    {
        try
        {
            context.Users.Add(entity);
            context.SaveChanges();

            return new ResponseType<User>
            {
                Object = entity,
                Message = "User was added successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<User>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<User> Delete(Guid id)
    {
        try
        {
            var user = GetOne(id);

            if (user is not { IsSuccess: true, Object: not null })
            {
                return new ResponseType<User>()
                {
                    Message = user.Message,
                    IsSuccess = false
                };
            }

            user.Object.IsDeleted = true;

            context.Users.Update(user.Object);
            context.SaveChanges();

            return new ResponseType<User>
            {
                Object = null,
                Message = "User updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<User>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<User> GetAll()
    {
        try
        {
            var users = context.Users.Include(u=>u.Role);

            return new ResponseType<User>
            {
                Object = null,
                Collection = [.. users],
                Message = "Users was sent successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<User>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<User> GetOne(Guid id)
    {
        try
        {
            var getAllUsers = GetAll();
            if (getAllUsers is not { IsSuccess: true, Collection: not null })
            {
                return new ResponseType<User>()
                {
                    Message = getAllUsers.Message,
                    IsSuccess = false
                };
            }

            var user = getAllUsers.Collection.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return new ResponseType<User>()
                {
                    Message = "User could not be find!",
                    IsSuccess = false
                };
            }

            return new ResponseType<User>()
            {
                Object = user,
                Message = "User find successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<User>
            {
                Object = null,
                Collection = null,
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }

    public ResponseType<User> Update(User entity)
    {
        try
        {
            context.Users.Update(entity);
            context.SaveChanges();

            return new ResponseType<User>
            {
                Object = entity,
                Message = "User updated successfully!",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseType<User>
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
