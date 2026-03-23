using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.User.Queries;

public class GetUserByIdQuery : IRequest<Result<ResponseType<UserResponseDto>>>
{
    public Guid UserId { get; set; }
}

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByIdQuery, Result<ResponseType<UserResponseDto>>>
{
    public async Task<Result<ResponseType<UserResponseDto>>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
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
            Object = mapper.Map<UserResponseDto>(user.Object)
        });
    }
}