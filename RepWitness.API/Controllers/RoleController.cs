using MediatR;
using Microsoft.AspNetCore.Mvc;
using RepWitness.Application.Features.Role.Dtos;
using RepWitness.Application.Features.Role.Queries;
using RepWitness.Domain.Generic;

namespace RepWitness.API.Controllers;

public class RoleController(ISender sender) : BaseAPIController
{
    [HttpGet("all")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<RoleResponseDto>> GetAllRoles()
    {
        return await sender.Send(new GetAllRolesQuery());
    }

    [HttpGet("{roleId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ResponseType<RoleResponseDto>> GetOneRole(Guid roleId)
    {
        return await sender.Send(new GetRoleByIdQuery { RoleId = roleId });
    }
}
