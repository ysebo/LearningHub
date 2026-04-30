using LearningHub.Domain.Entities;

namespace LearningHub.Application.Interfaces.Persistence;

public interface IEnrollmentRepository
{
    Task<IReadOnlyList<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Enrollment?> GetByIdsAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken = default);

    Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsAsync(
        int studentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Enrollment>> GetCourseEnrollmentsAsync(
        int courseId,
        CancellationToken cancellationToken = default);
}
