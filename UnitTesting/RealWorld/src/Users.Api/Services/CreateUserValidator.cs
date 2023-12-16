using FluentValidation;
using Users.Api.DTOs;

namespace Users.Api.Services;

public sealed class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(15)
            .WithMessage("Validation Error");
    }    
}