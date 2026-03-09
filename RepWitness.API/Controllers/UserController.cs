using MediatR;
using Microsoft.AspNetCore.Mvc;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Application.Features.User.Queries;
using RepWitness.Domain.Generic;

namespace RepWitness.API.Controllers;

public class UserController(ISender sender) : BaseAPIController
{
    [HttpGet("all")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<UserResponseDto>> GetAllUsers()
    {
        return await sender.Send(new GetAllUsersQuery());
    }

    [HttpGet("/{userId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<UserResponseDto>> GetOneUser(Guid userId)
    {
        return await sender.Send(new GetUserById { UserId = userId });
    }
}