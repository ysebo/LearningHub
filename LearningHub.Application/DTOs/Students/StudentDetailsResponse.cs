using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Students;

public sealed record StudentDetailsResponse(
    int Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth,
    IReadOnlyList<StudentEnrollmentDetailsResponse> Courses);

public sealed record StudentEnrollmentDetailsResponse(
    int CourseId,
    string CourseTitle,
    decimal Price,
    DateTime EnrolledAt,
    EnrollmentStatus Status);
