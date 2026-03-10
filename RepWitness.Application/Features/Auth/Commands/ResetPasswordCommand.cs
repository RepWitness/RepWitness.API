using Ardalis.Result;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Models;

namespace RepWitness.Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<Result<ResponseType<bool>>>
{
    public required Guid Id { get; set; }
    public required string Password { get; set; }
}

public sealed class ResetPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
    : IRequestHandler<ResetPasswordCommand, Result<ResponseType<bool>>>
{
    public async Task<Result<ResponseType<bool>>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var passwordValidation = AuthenticationValidationHelper.PasswordValidation(request.Password);
        if (!passwordValidation.Key)
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = passwordValidation.Value
            });
        }

        var registerResponse = userRepository.ResetPassword(request.Id, request.Password);


        if (registerResponse is { IsSuccess: false, Object: false })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = registerResponse.Message,
                Collection = null,
                Object = false
            });
        }

        var user = userRepository.GetOne(request.Id);

        if (user is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = user.Message
            });
        }

        await emailService.SendEmailAsync(new EmailDto
        {
            To = user.Object!.Email,
            Subject = "Password reset successfully!",
            Body = $"Password reset successfully in at {DateTime.UtcNow}."
        });


        return Result<ResponseType<bool>>.Success(new ResponseType<bool>
        {
            IsSuccess = true,
            Message = "Password reset successfully!",
            Object = true,
        });
    }
}