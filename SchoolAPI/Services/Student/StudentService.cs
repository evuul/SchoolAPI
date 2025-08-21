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

    public async Task<StudentReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var student = await _repo.GetByIdAsync(id, ct);
        return student is null ? null : _mapper.Map<StudentReadDto>(student);
    }

    public async Task<int> CreateAsync(StudentCreateDto dto, CancellationToken ct = default)
    {
        var studentToCreate = _mapper.Map<Models.Student>(dto);
        await _repo.AddAsync(studentToCreate, ct);
        await _repo.SaveChangesAsync(ct);
        return studentToCreate.Id;
    }

    public async Task<bool> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken ct = default)
    {
        var studentToUpdate = await _repo.GetByIdAsync(id, ct);
        if (studentToUpdate == null) return false;

        _mapper.Map(dto, studentToUpdate);
        await _repo.UpdateAsync(studentToUpdate, ct);
        return await _repo.SaveChangesAsync(ct);
    }

    public async Task<bool> StudentExistsAsync(int studentId, CancellationToken ct = default)
        => await _repo.StudentExistsAsync(studentId, ct);

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var studentToDelete = await _repo.GetByIdAsync(id, ct);
        if (studentToDelete == null) return false;

        await _repo.DeleteAsync(studentToDelete, ct);
        return await _repo.SaveChangesAsync(ct);
    }
}