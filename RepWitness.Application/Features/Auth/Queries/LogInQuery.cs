using Ardalis.Result;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Application.Common.Interfaces;
using RepWitness.Application.Features.Auth.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Models;
using RepWitness.Infrastructure.Security;

namespace RepWitness.Application.Features.Auth.Query;

public class LogInQuery : IRequest<Result<ResponseType<UserLogInResponseDto>>>
{
    public required UserLogInRequestDto User { get; set; }
}

public sealed class LogInQueryHandler(IUserRepository userRepository, ITokenService tokenService) 
    : IRequestHandler<LogInQuery, Result<ResponseType<UserLogInResponseDto>>>
{
    public async Task<Result<ResponseType<UserLogInResponseDto>>> Handle(LogInQuery request, CancellationToken cancellationToken)
    {
        var requiredFields = ValidationHelperUtils.ValidateRequiredFields(request.User);
        if (requiredFields.Key)
        {
            return Result<ResponseType<UserLogInResponseDto>>.Success(new ResponseType<UserLogInResponseDto>
            {
                IsSuccess = false,
                Message = requiredFields.Value
            });
        }

        var user = userRepository.GetUserByEmail(request.User.Email!);

        if (user is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<UserLogInResponseDto>>.Success(new ResponseType<UserLogInResponseDto>
            {
                IsSuccess = false,
                Message = "Email or password is incorrect."
            });
        }

        var logIn = PasswordHasher.VerifyPasswordHash(request.User.Password!, user.Object!.PasswordHash!, user.Object.PasswordSalt!);

        if(!logIn)
        {
            return Result<ResponseType<UserLogInResponseDto>>.Success(new ResponseType<UserLogInResponseDto>
            {
                IsSuccess = false,
                Message = "Email or password is incorrect."
            });
        }

        var loggedUser= new UserLogInResponseDto
        {
            Id = user.Object.Id,
            Token = tokenService.CreateToken(user.Object!.Email)
        };

        return Result<ResponseType<UserLogInResponseDto>>.Success(new ResponseType<UserLogInResponseDto>
        {
            IsSuccess = true,
            Message = "LogIn Successfull!",
            Object = loggedUser
        });
    }
}