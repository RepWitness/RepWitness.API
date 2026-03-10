using Ardalis.Result;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Application.Features.Auth.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Models;

namespace RepWitness.Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<Result<ResponseType<bool>>>
{
    public required Guid LinkId { get; set; }
    public required PasswordResetDto Password { get; set; }
}

public sealed class ResetPasswordCommandHandler(IUserRepository userRepository,IPasswordResetRepository passwordResetRepository, IEmailService emailService)
    : IRequestHandler<ResetPasswordCommand, Result<ResponseType<bool>>>
{
    public async Task<Result<ResponseType<bool>>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if(request.Password.Password != request.Password.ConfirmPassword)
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = "Password and confirm password must be the same"
            });
        }

        var passwordValidation = AuthenticationValidationHelper.PasswordValidation(request.Password.Password);
        if (!passwordValidation.Key)
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = passwordValidation.Value
            });
        }

        var passwordReset = passwordResetRepository.IsLinkValid(request.LinkId);

        if (passwordReset is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = passwordReset.Message,
                Collection = null,
                Object = false
            });
        }

        var registerResponse = userRepository.ResetPassword(passwordReset.Object!.Value, request.Password.Password);

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

        var userLink = passwordResetRepository.UseLink(request.LinkId);

        if(userLink is { IsSuccess: false, Object: false })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = userLink.Message,
                Collection = null,
                Object = false
            });
        }

        var user = userRepository.GetOne(passwordReset.Object!.Value);

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
            Body = $"Password reset successfully in at {DateTime.UtcNow}.Link: Localhost..."
        });


        return Result<ResponseType<bool>>.Success(new ResponseType<bool>
        {
            IsSuccess = true,
            Message = "Password reset successfully!",
            Object = true,
        });
    }
}