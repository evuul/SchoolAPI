using AutoMapper;
using SchoolAPI.Models.DTOs.Course;
using SchoolAPI.Repositories.Course;

namespace SchoolAPI.Services.Course;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repo;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository repo, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<CourseReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var courses = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
    }

    public async Task<ServiceResult<CourseReadDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var course = await _repo.GetByIdAsync(id, ct);
        if (course is null)
            return ServiceResult<CourseReadDto>.Fail(Err.NotFound("course.not_found", "Kursen finns inte."));

        return ServiceResult<CourseReadDto>.Ok(_mapper.Map<CourseReadDto>(course));
    }

    public async Task<ServiceResult<int>> CreateAsync(CourseCreateDto dto, CancellationToken ct = default)
    {
        // Affärsregel: Credits får aldrig vara negativt
        if (dto.Credits < 0)
            return ServiceResult<int>.Fail(
                Err.Validation(nameof(dto.Credits), "Credits kan inte vara negativt.", "course.credits_non_negative"));

        var entity = _mapper.Map<Models.Course>(dto);
        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return ServiceResult<int>.Ok(entity.Id);
    }

    public async Task<ServiceResult<Unit>> UpdateAsync(int id, CourseUpdateDto dto, CancellationToken ct = default)
    {
        var course = await _repo.GetByIdAsync(id, ct);
        if (course is null)
            return ServiceResult<Unit>.Fail(Err.NotFound("course.not_found", "Kursen finns inte."));

        // Om din CourseUpdateDto har nullable Credits (rekommenderat för partial update):
        //   if (dto.Credits.HasValue && dto.Credits.Value < 0) ...
        // Om den är icke-nullable, använd raden nedan:
        if (dto.Credits < 0)
            return ServiceResult<Unit>.Fail(
                Err.Validation(nameof(dto.Credits), "Credits kan inte vara negativt.", "course.credits_non_negative"));

        // Viktigt: mappa ändringarna
        _mapper.Map(dto, course);

        await _repo.UpdateAsync(course, ct);
        var ok = await _repo.SaveChangesAsync(ct);

        return ok ? ServiceResult.Ok()
                  : ServiceResult<Unit>.Fail(Err.Unexpected("Kunde inte uppdatera kursen.", "course.update_failed"));
    }

    public async Task<bool> CourseExistsAsync(int courseId, CancellationToken ct = default)
        => await _repo.CourseExistsAsync(courseId, ct);

    public async Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var course = await _repo.GetByIdAsync(id, ct);
        if (course is null)
            return ServiceResult<Unit>.Fail(Err.NotFound("course.not_found", "Kursen finns inte."));

        await _repo.DeleteAsync(course, ct);
        var ok = await _repo.SaveChangesAsync(ct);

        return ok ? ServiceResult.Ok()
                  : ServiceResult<Unit>.Fail(Err.Unexpected("Kunde inte radera kursen.", "course.delete_failed"));
    }
}