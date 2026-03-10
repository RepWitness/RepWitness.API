using Ardalis.Result;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Domain.Entities;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Models;

namespace RepWitness.Application.Features.Auth.Commands;

public class ResetPassworkLinkCommand : IRequest<Result<ResponseType<bool>>>
{
    public required string Email { get; set; }
}

public sealed class ResetPassworkLinkCommandHandler(IUserRepository userRepository, IPasswordResetRepository passwordResetRepository, 
    IEmailService emailService)
    : IRequestHandler<ResetPassworkLinkCommand, Result<ResponseType<bool>>>
{
    public async Task<Result<ResponseType<bool>>> Handle(ResetPassworkLinkCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserByEmail(request.Email);

        if (user is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = user.Message
            });
        }

        var passwordkResetLink = new PasswordReset
        {
            UserId = user.Object!.Id,
            IsActive = true,
            IsDeleted = false,
            ExpirationDate = DateTime.UtcNow.AddHours(1)
        };

        var result = passwordResetRepository.Add(passwordkResetLink);

        if (result is { IsSuccess: false, Object: null })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = result.Message
            });
        }

        await emailService.SendEmailAsync(new EmailDto
        {
            To = user.Object!.Email,
            Subject = "Password reset link!",
            Body = $"Here is you password reset link. Will expire in one hour. Here is the link: https://localhost:7277/api/auth/password-reset/2ded10e8-2322-4d89-b10f-23906ed6cf27"
        });


        return Result<ResponseType<bool>>.Success(new ResponseType<bool>
        {
            IsSuccess = true,
            Message = "Password reset link sent successfully!",
            Object = true,
        });
    }
}