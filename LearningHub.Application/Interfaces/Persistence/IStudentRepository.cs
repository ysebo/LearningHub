using LearningHub.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace LearningHub.Application.Interfaces.Persistence;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetDetailsByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(
        string email,
        int? ignoreStudentId = null,
        CancellationToken cancellationToken = default);
}