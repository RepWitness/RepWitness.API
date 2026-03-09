using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.User.Queries;

public class GetAllUsersQuery : IRequest<Result<ResponseType<UserResponseDto>>>
{
}

public sealed class GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetAllUsersQuery, Result<ResponseType<UserResponseDto>>>
{
    public async Task<Result<ResponseType<UserResponseDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = userRepository.GetAll();

        if (users is { IsSuccess: false, Collection: null })
        {
            return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
            {
                IsSuccess = false,
                Message = users.Message
            });
        }

        return Result<ResponseType<UserResponseDto>>.Success(new ResponseType<UserResponseDto>
        {
            IsSuccess = true,
            Message = users.Message,
            Collection = [.. users.Collection!.Select(mapper.Map<UserResponseDto>)]
        });
    }
}