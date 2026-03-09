using Microsoft.EntityFrameworkCore;
using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Persistence.Context;

namespace RepWitness.Persistence.Repositories;

public class UserRepository(RepWitnessContext _context) : IUserRepository
{
    public ResponseType<User> Add(User entity)
    {
        try
        {
            _context.Users.Add(entity);
            _context.SaveChanges();

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
                return new ResponseType<User>
                {
                    Message = user.Message,
                    IsSuccess = false
                };
            }

            user.Object.IsDeleted = true;

            _context.Users.Update(user.Object);
            _context.SaveChanges();

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
            // Use EF Core Include (Microsoft.EntityFrameworkCore) so Role is eagerly loaded.
            var users = _context.Users.Include(u => u.Role).ToList();

            return new ResponseType<User>
            {
                Object = null,
                Collection = users,
                Message = "Users were returned successfully",
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
                return new ResponseType<User>
                {
                    Message = getAllUsers.Message,
                    IsSuccess = false
                };
            }

            var user = getAllUsers.Collection.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return new ResponseType<User>
                {
                    Message = "User could not be found!",
                    IsSuccess = false
                };
            }

            return new ResponseType<User>
            {
                Object = user,
                Message = "User found successfully!",
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

    public ResponseType<bool> AreEmailAndUsernameUnique(string email, string username, Guid? userId = null)
    {
        try
        {
            var getAllUsers = GetAll();
            if (getAllUsers is not { IsSuccess: true, Collection: not null })
            {
                return new ResponseType<bool>
                {
                    Message = getAllUsers.Message,
                    IsSuccess = false
                };
            }

            var emails = getAllUsers.Collection.Count(u => u.Email == email && u.Id != userId);
            if (emails > 0)
            {
                return new ResponseType<bool>
                {
                    Object = false,
                    Message = "Email already taken, try other option!",
                    IsSuccess = false
                };
            }

            var usernames = getAllUsers.Collection.Count(u => u.Username == username && u.Id != userId);
            if (usernames > 0)
            {
                return new ResponseType<bool>
                {
                    Object = false,
                    Message = "Username already taken, try other option!",
                    IsSuccess = false
                };
            }

            return new ResponseType<bool>
            {
                Object = true,
                Message = "Email and username are available!",
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
        }
    }

    public ResponseType<User> Update(User entity)
    {
        try
        {
            _context.Users.Update(entity);
            _context.SaveChanges();

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
    }
}
