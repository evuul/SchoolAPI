using FluentValidation;
using SchoolAPI.Models.DTOs;
using SchoolAPI.Models.DTOs.Student;

namespace SchoolAPI.Validators;

public sealed class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
{
    public StudentCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
        
    }
}