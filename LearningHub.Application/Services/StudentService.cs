using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Application.Interfaces.Services;
using LearningHub.Application.DTOs.Students;
using LearningHub.Application.Exceptions;
using LearningHub.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace LearningHub.Application.Interfaces.Services;

public sealed class StudentService(IStudentRepository studentRepository) : IStudentService
{
    public async Task<IReadOnlyList<StudentSummaryResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var students = await studentRepository.GetAllAsync(cancellationToken);

        return students
            .Select(student => new StudentSummaryResponse(
                student.Id,
                student.FullName,
                student.Email,
                student.DateOfBirth,
                student.Enrollments.Count))
            .ToList();
    }

    public async Task<StudentDetailsResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetDetailsByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Student with id {id} was not found.");

        return new StudentDetailsResponse(
            student.Id,
            student.FullName,
            student.Email,
            student.DateOfBirth,
            student.Enrollments
                .OrderBy(enrollment => enrollment.Course!.Title)
                .Select(enrollment => new StudentEnrollmentDetailsResponse(
                    enrollment.CourseId,
                    enrollment.Course!.Title,
                    enrollment.Course.Price,
                    enrollment.EnrolledAt,
                    enrollment.Status))
                .ToList());
    }

    public async Task<StudentSummaryResponse> CreateAsync(
        StudentRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        if (await studentRepository.EmailExistsAsync(normalizedEmail, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Student with email {normalizedEmail} already exists.");
        }

        var student = new Student
        {
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            DateOfBirth = request.DateOfBirth
        };

        var createdStudent = await studentRepository.AddAsync(student, cancellationToken);

        return new StudentSummaryResponse(
            createdStudent.Id,
            createdStudent.FullName,
            createdStudent.Email,
            createdStudent.DateOfBirth,
            0);
    }

    public async Task<StudentSummaryResponse> UpdateAsync(
        int id,
        StudentRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var student = await studentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Student with id {id} was not found.");

        if (await studentRepository.EmailExistsAsync(normalizedEmail, id, cancellationToken))
        {
            throw new ConflictException($"Student with email {normalizedEmail} already exists.");
        }

        student.FullName = request.FullName.Trim();
        student.Email = normalizedEmail;
        student.DateOfBirth = request.DateOfBirth;

        await studentRepository.UpdateAsync(student, cancellationToken);

        return new StudentSummaryResponse(
            student.Id,
            student.FullName,
            student.Email,
            student.DateOfBirth,
            student.Enrollments.Count);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Student with id {id} was not found.");

        await studentRepository.DeleteAsync(student, cancellationToken);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
