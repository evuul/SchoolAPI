using AutoMapper;

namespace SchoolAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ===== Entity -> DTO =====
        CreateMap<Models.Student, Models.DTOs.Student.StudentReadDto>()
            .ForMember(dto => dto.FullName, opt =>
                opt.MapFrom(stud => $"{stud.FirstName} {stud.LastName}"));

        CreateMap<Models.Course, Models.DTOs.Course.CourseReadDto>()
            .ForMember(dto => dto.CourseName, opt => opt.MapFrom(c => c.Title));
            // Credits har samma namn -> mappas automatiskt

        CreateMap<Models.Enrollment, Models.DTOs.Enrollment.EnrollmentReadDto>()
            .ForMember(dto => dto.StudentFullName, opt =>
                opt.MapFrom(e => $"{e.Student.FirstName} {e.Student.LastName}"))
            .ForMember(dto => dto.CourseTitle, opt =>
                opt.MapFrom(e => e.Course.Title));
            // EnrollmentDate och Grade mappas by-name

        // ===== DTO -> Entity =====
        // Student
        CreateMap<Models.DTOs.StudentCreateDto, Models.Student>();
        CreateMap<Models.DTOs.StudentUpdateDto, Models.Student>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        // Course
        CreateMap<Models.DTOs.Course.CourseCreateDto, Models.Course>();
        CreateMap<Models.DTOs.Course.CourseUpdateDto, Models.Course>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Enrollment
        CreateMap<Models.DTOs.Enrollment.EnrollmentCreateDto, Models.Enrollment>()
            // Servicet sätter datumet centralt (DateTime.UtcNow), så ignorera inkommande värde om DTO har det
            .ForMember(e => e.EnrollmentDate, opt => opt.Ignore());

        // Viktigt: INGEN mappning från ReadDto -> Entity (undvik att råka spara läs-DTO tillbaka)
        // CreateMap<Models.DTOs.Enrollment.EnrollmentReadDto, Models.Enrollment>(); // <-- ta bort
    }
}