using LearningHub.Application.DTOs.Enrollments;

namespace LearningHub.Application.Interfaces.Services;

public interface IEnrollmentService
{
    Task<IReadOnlyList<EnrollmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<EnrollmentResponse> CreateAsync(
        CreateEnrollmentRequest request,
        CancellationToken cancellationToken = default);

    Task<EnrollmentResponse> UpdateAsync(
        int studentId,
        int courseId,
        UpdateEnrollmentRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentCourseResponse>> GetCoursesForStudentAsync(
        int studentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CourseStudentResponse>> GetStudentsForCourseAsync(
        int courseId,
        CancellationToken cancellationToken = default);
}
