using LearningHub.Application.DTOs.Students;
using System.Threading;
using System.Threading.Tasks;

namespace LearningHub.Application.Interfaces.Services;

public interface IStudentService
{
    Task<IReadOnlyList<StudentSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<StudentDetailsResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<StudentSummaryResponse> CreateAsync(
        StudentRequest request,
        CancellationToken cancellationToken = default);

    Task<StudentSummaryResponse> UpdateAsync(
        int id,
        StudentRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
