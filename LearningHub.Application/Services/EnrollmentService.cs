using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Application.Interfaces.Services;
using LearningHub.Application.DTOs.Enrollments;
using LearningHub.Application.Exceptions;
using LearningHub.Domain.Entities;

namespace LearningHub.Application.Services;

public sealed class EnrollmentService(IEnrollmentRepository enrollmentRepository, IStudentRepository studentRepository, ICourseRepository courseRepository) : IEnrollmentService
{
    public async Task<IReadOnlyList<EnrollmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var enrollments = await enrollmentRepository.GetAllAsync(cancellationToken);

        return enrollments.Select(MapEnrollment).ToList();
    }

    public async Task<EnrollmentResponse> CreateAsync(
        CreateEnrollmentRequest request,
        CancellationToken cancellationToken = default)
    {
        await EnsureStudentExistsAsync(request.StudentId, cancellationToken);
        await EnsureCourseExistsAsync(request.CourseId, cancellationToken);

        if (await enrollmentRepository.ExistsAsync(request.StudentId, request.CourseId, cancellationToken))
        {
            throw new ConflictException("This student is already enrolled in this course.");
        }

        var enrollment = new Enrollment
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            EnrolledAt = DateTime.UtcNow,
            Status = request.Status
        };

        await enrollmentRepository.AddAsync(enrollment, cancellationToken);

        var createdEnrollment = await enrollmentRepository.GetByIdsAsync(
            request.StudentId,
            request.CourseId,
            cancellationToken)
            ?? throw new NotFoundException("Enrollment could not be loaded after creation.");

        return MapEnrollment(createdEnrollment);
    }

    public async Task<EnrollmentResponse> UpdateAsync(
        int studentId,
        int courseId,
        UpdateEnrollmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await enrollmentRepository.GetByIdsAsync(studentId, courseId, cancellationToken)
            ?? throw new NotFoundException("Enrollment was not found.");

        enrollment.Status = request.Status;

        await enrollmentRepository.UpdateAsync(enrollment, cancellationToken);

        var updatedEnrollment = await enrollmentRepository.GetByIdsAsync(studentId, courseId, cancellationToken)
            ?? throw new NotFoundException("Enrollment was not found after update.");

        return MapEnrollment(updatedEnrollment);
    }

    public async Task DeleteAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await enrollmentRepository.GetByIdsAsync(studentId, courseId, cancellationToken)
            ?? throw new NotFoundException("Enrollment was not found.");

        await enrollmentRepository.DeleteAsync(enrollment, cancellationToken);
    }

    public async Task<IReadOnlyList<StudentCourseResponse>> GetCoursesForStudentAsync(
        int studentId,
        CancellationToken cancellationToken = default)
    {
        await EnsureStudentExistsAsync(studentId, cancellationToken);

        var enrollments = await enrollmentRepository.GetStudentEnrollmentsAsync(studentId, cancellationToken);

        return enrollments
            .Select(enrollment => new StudentCourseResponse(
                enrollment.CourseId,
                enrollment.Course!.Title,
                enrollment.Course.Price,
                enrollment.Course.Category?.Name ?? string.Empty,
                enrollment.EnrolledAt,
                enrollment.Status))
            .ToList();
    }

    public async Task<IReadOnlyList<CourseStudentResponse>> GetStudentsForCourseAsync(
        int courseId,
        CancellationToken cancellationToken = default)
    {
        await EnsureCourseExistsAsync(courseId, cancellationToken);

        var enrollments = await enrollmentRepository.GetCourseEnrollmentsAsync(courseId, cancellationToken);

        return enrollments
            .Select(enrollment => new CourseStudentResponse(
                enrollment.StudentId,
                enrollment.Student!.FullName,
                enrollment.Student.Email,
                enrollment.Student.DateOfBirth,
                enrollment.EnrolledAt,
                enrollment.Status))
            .ToList();
    }

    private async Task EnsureStudentExistsAsync(int studentId, CancellationToken cancellationToken)
    {
        if (!await studentRepository.ExistsAsync(studentId, cancellationToken))
        {
            throw new NotFoundException($"Student with id {studentId} was not found.");
        }
    }

    private async Task EnsureCourseExistsAsync(int courseId, CancellationToken cancellationToken)
    {
        if (!await courseRepository.ExistsAsync(courseId, cancellationToken))
        {
            throw new NotFoundException($"Course with id {courseId} was not found.");
        }
    }

    private static EnrollmentResponse MapEnrollment(Enrollment enrollment)
    {
        return new EnrollmentResponse(
            enrollment.StudentId,
            enrollment.Student?.FullName ?? string.Empty,
            enrollment.CourseId,
            enrollment.Course?.Title ?? string.Empty,
            enrollment.EnrolledAt,
            enrollment.Status);
    }
}
