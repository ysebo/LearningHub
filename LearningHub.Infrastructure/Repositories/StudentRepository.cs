using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Domain.Entities;
using LearningHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LearningHub.Infrastructure.Repositories;

public sealed class StudentRepository(LearningHubDbContext dbContext)
    : GenericRepository<Student>(dbContext), IStudentRepository
{
    public override async Task<IReadOnlyList<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Students
            .Include(student => student.Enrollments)
            .AsNoTracking()
            .OrderBy(student => student.FullName)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Students
            .Include(student => student.Enrollments)
            .AsNoTracking()
            .SingleOrDefaultAsync(student => student.Id == id, cancellationToken);
    }

    public async Task<Student?> GetDetailsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Students
            .Include(student => student.Enrollments)
                .ThenInclude(enrollment => enrollment.Course)
            .AsNoTracking()
            .SingleOrDefaultAsync(student => student.Id == id, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        int? ignoreStudentId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await DbContext.Students
            .AsNoTracking()
            .AnyAsync(
                student => student.Email == normalizedEmail &&
                           (!ignoreStudentId.HasValue || student.Id != ignoreStudentId.Value),
                cancellationToken);
    }
}