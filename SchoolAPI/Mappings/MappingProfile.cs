using AutoMapper;

namespace SchoolAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity -> DTO
        CreateMap<Models.Student, Models.DTOs.Student.StudentReadDto>()
            .ForMember(dto => dto.FullName, opt 
                => opt.MapFrom(stud => $"{stud.FirstName} {stud.LastName}"));
        CreateMap<Models.Course, Models.DTOs.Course.CourseReadDto>()
            .ForMember(dto => dto.CourseName, opt 
                => opt.MapFrom(course => course.Title))
            .ForMember(dto => dto.Credits, opt 
                => opt.MapFrom(course => course.Credits));
        CreateMap<Models.Enrollment, Models.DTOs.Enrollment.EnrollmentReadDto>()
            .ForMember(dto => dto.StudentFullName, opt
                => opt.MapFrom(enr => $"{enr.Student.FirstName} {enr.Student.LastName}"))
            .ForMember(dto => dto.CourseTitle, opt
                => opt.MapFrom(enr => enr.Course.Title));
        

        // DTO -> Entity
        CreateMap<Models.DTOs.StudentCreateDto, Models.Student>();
        CreateMap<Models.DTOs.StudentUpdateDto, Models.Student>();
        CreateMap<Models.DTOs.Course.CourseCreateDto, Models.Course>();
        CreateMap<Models.DTOs.Course.CourseUpdateDto, Models.Course>();
        CreateMap<Models.DTOs.Enrollment.EnrollmentReadDto, Models.Enrollment>();
        CreateMap<Models.DTOs.Enrollment.EnrollmentCreateDto, Models.Enrollment>();
    }
}