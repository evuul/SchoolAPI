using FluentValidation;
using SchoolAPI.Models.DTOs.Course;

namespace SchoolAPI.Validators;

public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateDtoValidator()
    {
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course title cannot be empty.")
            .MaximumLength(100).WithMessage("Course title cannot exceed 100 characters.");
        RuleFor(x => x.Credits)
            .InclusiveBetween(1, 10).WithMessage("Credits must be between 1 and 10.")
            .NotNull().WithMessage("Credits cannot be null.");
    }
    
    
}