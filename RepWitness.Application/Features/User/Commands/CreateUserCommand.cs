using Ardalis.Result;
using AutoMapper;
using MediatR;
using RepWitness.Application.Common.Behaviors;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Generic;
using RepWitness.Domain.Interfaces;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Models;

namespace RepWitness.Application.Features.User.Commands;

public class CreateUserCommand : IRequest<Result<ResponseType<bool>>>
{
    public required CreateUserRequestDto User { get; set; }
}

public sealed class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IEmailService emailService)
    : IRequestHandler<CreateUserCommand, Result<ResponseType<bool>>>
{
    public async Task<Result<ResponseType<bool>>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var requiredFields = ValidationHelperUtils.ValidateRequiredFields(request.User);
        if (requiredFields.Key)
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = requiredFields.Value
            });
        }

        var emailValidation = AuthenticationValidationHelper.EmailValid(request.User.Email!);
        if (!emailValidation.Key)
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = emailValidation.Value
            });
        }

        var areEmailAndUsernameAvailable = userRepository.AreEmailAndUsernameUnique(request.User.Email!, request.User.Username!);

        if (areEmailAndUsernameAvailable is { Object: false, IsSuccess: false })
        {
            return Result<ResponseType<bool>>.Success(new ResponseType<bool>
            {
                IsSuccess = false,
                Message = areEmailAndUsernameAvailable.Message
            });
        }

        var registerUser = mapper.Map<Domain.Entities.User>(request.User);

        var registerResponse = userRepository.Add(registerUser);

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

        await emailService.SendEmailAsync(new EmailDto
        {
            To = registerResponse.Object!.Email,
            Subject = "Account made successfully!",
            Body = $"You have successfully created an account in at {DateTime.UtcNow}."
        });


        return Result<ResponseType<bool>>.Success(new ResponseType<bool>
        {
            IsSuccess = true,
            Message = "Account made successfully!",
            Object = true,
        });
    }
}