using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Domain.Entities;
using LearningHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearningHub.Infrastructure.Repositories;

public sealed class EnrollmentRepository(LearningHubDbContext dbContext) : IEnrollmentRepository
{
    public async Task<IReadOnlyList<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Enrollments
            .Include(enrollment => enrollment.Student)
            .Include(enrollment => enrollment.Course)
            .AsNoTracking()
            .OrderBy(enrollment => enrollment.Student == null ? string.Empty : enrollment.Student.FullName)
            .ThenBy(enrollment => enrollment.Course == null ? string.Empty : enrollment.Course.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetByIdsAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Enrollments
            .Include(enrollment => enrollment.Student)
            .Include(enrollment => enrollment.Course)
                .ThenInclude(course => course!.Category)
            .SingleOrDefaultAsync(
                enrollment => enrollment.StudentId == studentId && enrollment.CourseId == courseId,
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Enrollments
            .AsNoTracking()
            .AnyAsync(
                enrollment => enrollment.StudentId == studentId && enrollment.CourseId == courseId,
                cancellationToken);
    }

    public async Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        await dbContext.Enrollments.AddAsync(enrollment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return enrollment;
    }

    public async Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        dbContext.Enrollments.Update(enrollment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        dbContext.Enrollments.Remove(enrollment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsAsync(
        int studentId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Enrollments
            .Include(enrollment => enrollment.Course)
                .ThenInclude(course => course!.Category)
            .AsNoTracking()
            .Where(enrollment => enrollment.StudentId == studentId)
            .OrderBy(enrollment => enrollment.Course == null ? string.Empty : enrollment.Course.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Enrollment>> GetCourseEnrollmentsAsync(
        int courseId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Enrollments
            .Include(enrollment => enrollment.Student)
            .AsNoTracking()
            .Where(enrollment => enrollment.CourseId == courseId)
            .OrderBy(enrollment => enrollment.Student == null ? string.Empty : enrollment.Student.FullName)
            .ToListAsync(cancellationToken);
    }
}
