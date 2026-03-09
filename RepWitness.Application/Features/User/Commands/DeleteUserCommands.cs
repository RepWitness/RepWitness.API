using Ardalis.Result;
using MediatR;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;

namespace RepWitness.Application.Features.User.Commands;

public class DeleteUserCommand : IRequest<Result<ResponseType<bool>>>
{
    public required Guid UserId { get; set; }
}

public sealed class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Result<ResponseType<bool>>>
{
    public async Task<Result<ResponseType<bool>>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetOne(request.UserId);

        if (user is not { IsSuccess: true, Object: not null })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = user.Message
            });
        }

        var res = userRepository.Delete(request.UserId);

        if (res is { IsSuccess: true })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                Object = true,
                IsSuccess = true,
                Message = res.Message
            });
        }

        return Result<ResponseType<bool>>.Success(new ResponseType<bool>
        {
            IsSuccess = false,
            Message = res.Message,
            Object = false
        });
    }
}