using AutoMapper;
using SchoolAPI.Models.DTOs.Enrollment;
using SchoolAPI.Repositories.Enrollment;

namespace SchoolAPI.Services.Enrollment;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repository;
    private readonly IMapper _mapper;

    public EnrollmentService(IEnrollmentRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<int> CreateAsync(EnrollmentCreateDto dto, CancellationToken ct = default)
    {
        var enrollmentToCreate = _mapper.Map<Models.Enrollment>(dto);
        await _repository.AddAsync(enrollmentToCreate, ct);
        await _repository.SaveChangesAsync(ct);
        return enrollmentToCreate.Id;
    }

    public async Task<List<EnrollmentReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var enrollments = await _repository.GetAllAsync(ct);
        return _mapper.Map<List<EnrollmentReadDto>>(enrollments);
    }

    public async Task<EnrollmentReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var enrollment = await _repository.GetByIdAsync(id, ct);
        return enrollment == null ? null : _mapper.Map<EnrollmentReadDto>(enrollment);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var enrollmentToDelete = await _repository.GetByIdAsync(id, ct);
        if (enrollmentToDelete == null)
            return false;

        await _repository.DeleteAsync(enrollmentToDelete, ct);
        return await _repository.SaveChangesAsync(ct);
    }
}