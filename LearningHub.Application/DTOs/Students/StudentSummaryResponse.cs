namespace LearningHub.Application.DTOs.Students;

public sealed record StudentSummaryResponse(
    int Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth,
    int CoursesCount);
