using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.User.Commands;

public class UpdateUserCommand : IRequest<Result<ResponseType<UserResponseDto>>>
{
    public required UpdateUserRequestDto User { get; set; }
}

public sealed class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<UpdateUserCommand, Result<ResponseType<UserResponseDto>>>
{
    public async Task<Result<ResponseType<UserResponseDto>>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.User.Id is null)
        {
            return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
            {
                IsSuccess = false,
                Message = "UserId must be filled!"
            });
        }

        var user = userRepository.GetOne(request.User.Id.Value);

        if (user is not { IsSuccess: true, Object: not null })
        {
            return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
            {
                IsSuccess = false,
                Message = user.Message
            });
        }

        var requiredFields = ValidationHelperUtils.ValidateOneRequiredFields(request.User);
        if (!requiredFields.Key)
        {
            return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
            {
                IsSuccess = false,
                Message = requiredFields.Value
            });
        }


        var editedUser = mapper.Map<Domain.Entities.User>(request.User);

        var res = userRepository.Update(editedUser);

        if (res is { IsSuccess: true, Object: not null })
        {
            return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
            {
                Object = mapper.Map<UserResponseDto>(res.Object),
                IsSuccess = true,
                Message = res.Message
            });
        }

        return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
        {
            IsSuccess = false,
            Message = res.Message
        });
    }
}