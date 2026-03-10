using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.User.Queries;

public class GetUserById : IRequest<Result<ResponseType<UserResponseDto>>>
{
    public Guid UserId { get; set; }
}

public sealed class GetOnePostQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserById, Result<ResponseType<UserResponseDto>>>
{
    public async Task<Result<ResponseType<UserResponseDto>>> Handle(GetUserById request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetOne(request.UserId);

        if (user is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
            {
                IsSuccess = false,
                Message = user.Message
            });
        }

        return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
        {
            IsSuccess = true,
            Message = user.Message,
            Object = mapper.Map<UserResponseDto>(user)
        });
    }
}