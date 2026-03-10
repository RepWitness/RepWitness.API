using MediatR;
using Microsoft.AspNetCore.Mvc;
using RepWitness.Application.Features.Auth.Commands;
using RepWitness.Application.Features.Auth.Dtos;
using RepWitness.Application.Features.Auth.Query;
using RepWitness.Application.Features.User.Commands;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Generic;

namespace RepWitness.API.Controllers;

public class AuthController(ISender sender) : BaseAPIController
{
    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<bool>> Register(CreateUserRequestDto user)
    {
        return await sender.Send(new CreateUserCommand { User = user });
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<UserLogInResponseDto>> LogIn(UserLogInRequestDto user)
    {
        return await sender.Send(new LogInQuery { User = user });
    }

    [HttpPut("password-reset/{userId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<bool>> ResetPassword(Guid userId, PasswordReset password)
    {
        return await sender.Send(new ResetPasswordCommand { Id = userId, Password = password.Password });
    }

}