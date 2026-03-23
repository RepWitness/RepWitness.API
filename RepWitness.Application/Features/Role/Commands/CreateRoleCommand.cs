using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Application.Features.Role.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.Role.Commands;

public class CreateRoleCommand : IRequest<Result<ResponseType<bool>>>
{
    public required CreateRoleRequestDto Role { get; set; }
}

public sealed class CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
    : IRequestHandler<CreateRoleCommand, Result<ResponseType<bool>>>
{
    public async Task<Result<ResponseType<bool>>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var requiredFields = ValidationHelperUtils.ValidateRequiredFields(request.Role);
        if (requiredFields.Key)
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = requiredFields.Value
            });
        }

        var registerRole = mapper.Map<Domain.Entities.Role>(request.Role);

        var registerResponse = roleRepository.Add(registerRole);

        if (registerResponse is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = registerResponse.Message,
                Collection = null,
                Object = false
            });
        }

        return Result<ResponseType<bool>>.Success(new ResponseType<bool>
        {
            IsSuccess = true,
            Message = "Role created succesffully!",
            Object = true,
        });
    }
}