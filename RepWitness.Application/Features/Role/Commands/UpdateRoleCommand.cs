using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Application.Features.Role.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.Role.Commands;

public class UpdateRoleCommand : IRequest<Result<ResponseType<RoleResponseDto>>>
{
    public required UpdateRoleRequestDto Role { get; set; }
}

public sealed class UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper) : IRequestHandler<UpdateRoleCommand, Result<ResponseType<RoleResponseDto>>>
{
    public async Task<Result<ResponseType<RoleResponseDto>>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.Role.Id is null)
        {
            return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
            {
                IsSuccess = false,
                Message = "RoleId must be filled!"
            });
        }

        var role = roleRepository.GetOne(request.Role.Id.Value);

        if (role is not { IsSuccess: true, Object: not null })
        {
            return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
            {
                IsSuccess = false,
                Message = role.Message
            });
        }

        var requiredFields = ValidationHelperUtils.ValidateOneRequiredFields(request.Role);
        if (!requiredFields.Key)
        {
            return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
            {
                IsSuccess = false,
                Message = requiredFields.Value
            });
        }

        var editedRole = mapper.Map<Domain.Entities.Role>(request.Role);

        var res = roleRepository.Update(editedRole);

        if (res is { IsSuccess: true, Object: not null })
        {
            return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
            {
                Object = mapper.Map<RoleResponseDto>(res.Object),
                IsSuccess = true,
                Message = res.Message
            });
        }

        return Result<ResponseType<RoleResponseDto>>.Success(new ResponseType<RoleResponseDto>
        {
            IsSuccess = false,
            Message = res.Message
        });
    }
}