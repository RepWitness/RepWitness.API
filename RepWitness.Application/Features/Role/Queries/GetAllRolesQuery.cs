using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Features.Role.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.Role.Queries;

public class GetAllRolesQuery : IRequest<Result<ResponseType<RoleResponseDto>>>
{
}

public sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper) : IRequestHandler<GetAllRolesQuery, Result<ResponseType<RoleResponseDto>>>
{
    public async Task<Result<ResponseType<RoleResponseDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = roleRepository.GetAll();

        if (roles is { IsSuccess: false, Collection: null })
        {
            return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
            {
                IsSuccess = false,
                Message = roles.Message
            });
        }

        return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
        {
            IsSuccess = true,
            Message = roles.Message,
            Collection = [.. roles.Collection!.Select(mapper.Map<RoleResponseDto>)]
        });
    }
}