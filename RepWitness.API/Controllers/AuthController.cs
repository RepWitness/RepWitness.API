using MediatR;
using Microsoft.AspNetCore.Mvc;
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
}