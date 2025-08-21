using FluentValidation;
using SchoolAPI.Models.DTOs.Enrollment;
using SchoolAPI.Services.Course;
using SchoolAPI.Services.Student;

namespace SchoolAPI.Validators;

public sealed class EnrollmentCreateDtoValidator : AbstractValidator<EnrollmentCreateDto>
{
    public EnrollmentCreateDtoValidator(IStudentService studentService, ICourseService courseService)
    {
        // StudentId måste vara positivt och existera
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("StudentId must be a positive number.")
            .MustAsync(async (id, ct) => await studentService.StudentExistsAsync(id, ct))
            .WithMessage("Student does not exist.");

        // CourseId måste vara positivt och existera
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("CourseId must be a positive number.")
            .MustAsync(async (id, ct) => await courseService.CourseExistsAsync(id, ct))
            .WithMessage("Course does not exist.");

        // Grade max 5 tecken
        RuleFor(x => x.Grade)
            .MaximumLength(5)
            .WithMessage("Grade cannot exceed 5 characters.");
    }
}