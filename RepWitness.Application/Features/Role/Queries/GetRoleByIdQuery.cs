using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Features.Role.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.Role.Queries;

public class GetRoleByIdQuery : IRequest<Result<ResponseType<RoleResponseDto>>>
{
    public Guid RoleId { get; set; }
}

public sealed class GetRoleByIdQueryHandler(IRoleRepository roleRepository, IMapper mapper) : IRequestHandler<GetRoleByIdQuery, Result<ResponseType<RoleResponseDto>>>
{
    public async Task<Result<ResponseType<RoleResponseDto>>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = roleRepository.GetOne(request.RoleId);

        if (role is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
            {
                IsSuccess = false,
                Message = role.Message
            });
        }

        return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
        {
            IsSuccess = true,
            Message = role.Message,
            Object = mapper.Map<RoleResponseDto>(role.Object)
        });
    }
}