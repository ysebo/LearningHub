using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Courses;

public sealed record CourseDetailsResponse(
    int Id,
    string Title,
    string Description,
    decimal Price,
    int CategoryId,
    string CategoryName,
    IReadOnlyList<CourseStudentDetailsResponse> Students);

public sealed record CourseStudentDetailsResponse(
    int Id,
    string FullName,
    string Email,
    DateTime EnrolledAt,
    EnrollmentStatus Status);
