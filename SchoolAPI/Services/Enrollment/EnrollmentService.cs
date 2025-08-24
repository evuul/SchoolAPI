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

    // CREATE — förhindra dubbla enrollments (StudentId, CourseId)
    public async Task<ServiceResult<int>> CreateAsync(EnrollmentCreateDto dto, CancellationToken ct = default)
    {
        if (dto.StudentId <= 0)
            return ServiceResult<int>.Fail(Err.Validation(nameof(dto.StudentId), "Ogiltigt student-id."));
        if (dto.CourseId <= 0)
            return ServiceResult<int>.Fail(Err.Validation(nameof(dto.CourseId), "Ogiltigt kurs-id."));

        // Dubbelregistrering?
        if (await _repository.EnrollmentExistsAsync(dto.StudentId, dto.CourseId, ct))
            return ServiceResult<int>.Fail(
                Err.Validation(field: null, message: "Studenten är redan inskriven på kursen.", code: "enrollment.duplicate")
            );

        var entity = _mapper.Map<Models.Enrollment>(dto);
        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);
        return ServiceResult<int>.Ok(entity.Id);
    }

    // READ ALL
    public async Task<List<EnrollmentReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var enrollments = await _repository.GetAllAsync(ct);
        return _mapper.Map<List<EnrollmentReadDto>>(enrollments);
    }

    // READ BY ID (valfritt: gör den också ServiceResult för 404-hantering)
    public async Task<ServiceResult<EnrollmentReadDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var enrollment = await _repository.GetByIdAsync(id, ct);
        if (enrollment is null)
            return ServiceResult<EnrollmentReadDto>.Fail(Err.NotFound("enrollment.not_found", "Inskrivningen finns inte."));
        return ServiceResult<EnrollmentReadDto>.Ok(_mapper.Map<EnrollmentReadDto>(enrollment));
    }

    // DELETE (kan lämnas som är, men bättre med ServiceResult för enhetlig felhantering)
    public async Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var enrollment = await _repository.GetByIdAsync(id, ct);
        if (enrollment is null)
            return ServiceResult<Unit>.Fail(Err.NotFound("enrollment.not_found", "Inskrivningen finns inte."));

        await _repository.DeleteAsync(enrollment, ct);
        var ok = await _repository.SaveChangesAsync(ct);
        return ok ? ServiceResult.Ok()
                  : ServiceResult<Unit>.Fail(Err.Unexpected("Kunde inte ta bort inskrivningen."));
    }
}