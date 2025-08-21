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

    public async Task<CourseReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var course = await _repo.GetByIdAsync(id, ct);
        return course is null ? null : _mapper.Map<CourseReadDto>(course);
    }

    public async Task<int> CreateAsync(CourseCreateDto dto, CancellationToken ct = default)
    {
        var courseToCreate = _mapper.Map<Models.Course>(dto);
        await _repo.AddAsync(courseToCreate, ct);
        await _repo.SaveChangesAsync(ct);
        return courseToCreate.Id;
    }

    public async Task<bool> UpdateAsync(int id, CourseUpdateDto dto, CancellationToken ct = default)
    {
        var courseToUpdate = await _repo.GetByIdAsync(id, ct);
        if (courseToUpdate == null)
        {
            return false;
        }
        await _repo.UpdateAsync(courseToUpdate, ct);
        return await _repo.SaveChangesAsync(ct);
    }
    
public async Task<bool> CourseExistsAsync(int courseId, CancellationToken ct = default)
    {
        return await _repo.CourseExistsAsync(courseId, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var courseToDelete = await _repo.GetByIdAsync(id, ct);
        if (courseToDelete == null)
        {
            return false;
        }
        await _repo.DeleteAsync(courseToDelete, ct);
        return await _repo.SaveChangesAsync(ct);
    }
}