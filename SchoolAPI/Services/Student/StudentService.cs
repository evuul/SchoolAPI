using System.Text.RegularExpressions;
using AutoMapper;
using SchoolAPI.Models.DTOs;
using SchoolAPI.Models.DTOs.Student;
using SchoolAPI.Repositories;

namespace SchoolAPI.Services.Student;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository repo, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<StudentReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var students = await _repo.GetAllAsync(ct);
        return _mapper.Map<List<StudentReadDto>>(students);
    }

    public async Task<ServiceResult<StudentReadDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var student = await _repo.GetByIdAsync(id, ct);
        if (student is null)
            return ServiceResult<StudentReadDto>.Fail(Err.NotFound("student.not_found", "Studenten finns inte."));
        return ServiceResult<StudentReadDto>.Ok(_mapper.Map<StudentReadDto>(student));
    }

    public async Task<ServiceResult<int>> CreateAsync(StudentCreateDto dto, CancellationToken ct = default)
    {
        // 1) Grundvalidering
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            return ServiceResult<int>.Fail(Err.Validation(nameof(dto.FirstName), "Förnamn är obligatoriskt.", "student.first_name_required"));
        if (string.IsNullOrWhiteSpace(dto.LastName))
            return ServiceResult<int>.Fail(Err.Validation(nameof(dto.LastName), "Efternamn är obligatoriskt.", "student.last_name_required"));

        // 2) Email (valfri men om satt: format + unik)
        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            if (!IsValidEmail(dto.Email!))
                return ServiceResult<int>.Fail(Err.Validation(nameof(dto.Email), "Ogiltig e-postadress.", "student.email_invalid"));

            if (await _repo.EmailExistsAsync(dto.Email!, exceptId: null, ct))
                return ServiceResult<int>.Fail(Err.Validation(nameof(dto.Email), "E-postadressen används redan.", "student.email_unique"));
        }

        // 3) Ålder (om BirthDate satt)
        if (dto.BirthDate.HasValue && !IsAgeWithin(dto.BirthDate.Value, 6, 100))
            return ServiceResult<int>.Fail(Err.Validation(nameof(dto.BirthDate), "Ålder måste vara mellan 6 och 100 år.", "student.age_range"));

        var entity = _mapper.Map<Models.Student>(dto);
        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);
        return ServiceResult<int>.Ok(entity.Id);
    }

    public async Task<ServiceResult<Unit>> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null)
            return ServiceResult<Unit>.Fail(Err.NotFound("student.not_found", "Studenten finns inte."));

        // Namn (om skickat)
        if (dto.FirstName is not null && string.IsNullOrWhiteSpace(dto.FirstName))
            return ServiceResult<Unit>.Fail(Err.Validation(nameof(dto.FirstName), "Förnamn kan inte vara tomt.", "student.first_name_required"));
        if (dto.LastName is not null && string.IsNullOrWhiteSpace(dto.LastName))
            return ServiceResult<Unit>.Fail(Err.Validation(nameof(dto.LastName), "Efternamn kan inte vara tomt.", "student.last_name_required"));

        // Email (om skickat)
        if (dto.Email is not null)
        {
            if (dto.Email.Length == 0 || !IsValidEmail(dto.Email))
                return ServiceResult<Unit>.Fail(Err.Validation(nameof(dto.Email), "Ogiltig e-postadress.", "student.email_invalid"));

            if (await _repo.EmailExistsAsync(dto.Email, exceptId: id, ct))
                return ServiceResult<Unit>.Fail(Err.Validation(nameof(dto.Email), "E-postadressen används redan.", "student.email_unique"));
        }

        // Ålder (om skickat)
        if (dto.BirthDate.HasValue && !IsAgeWithin(dto.BirthDate.Value, 6, 100))
            return ServiceResult<Unit>.Fail(Err.Validation(nameof(dto.BirthDate), "Ålder måste vara mellan 6 och 100 år.", "student.age_range"));

        _mapper.Map(dto, entity); // din MappingProfile ignorerar null vid update
        await _repo.UpdateAsync(entity, ct);
        var ok = await _repo.SaveChangesAsync(ct);
        return ok ? ServiceResult.Ok()
                  : ServiceResult<Unit>.Fail(Err.Unexpected("Kunde inte uppdatera studenten.", "student.update_failed"));
    }

    public async Task<bool> StudentExistsAsync(int studentId, CancellationToken ct = default)
        => await _repo.StudentExistsAsync(studentId, ct);

    public async Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null)
            return ServiceResult<Unit>.Fail(Err.NotFound("student.not_found", "Studenten finns inte."));

        // Affärsregel: hindra delete om aktiva inskrivningar finns
        if (await _repo.HasActiveEnrollmentsAsync(id, ct))
            return ServiceResult<Unit>.Fail(Err.Forbidden("student.cannot_delete_with_active_enrollments",
                                                          "Student med aktiva inskrivningar kan inte tas bort."));

        await _repo.DeleteAsync(entity, ct);
        var ok = await _repo.SaveChangesAsync(ct);
        return ok ? ServiceResult.Ok()
                  : ServiceResult<Unit>.Fail(Err.Unexpected("Kunde inte ta bort studenten.", "student.delete_failed"));
    }

    // === Helpers ===
    private static bool IsValidEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private static bool IsAgeWithin(DateOnly birthDate, int min, int max)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year - (today < birthDate.AddYears(today.Year - birthDate.Year) ? 1 : 0);
        return age >= min && age <= max;
    }
}