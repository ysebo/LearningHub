using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Enrollments;

public sealed record CourseStudentResponse(
    int StudentId,
    string FullName,
    string Email,
    DateOnly DateOfBirth,
    DateTime EnrolledAt,
    EnrollmentStatus Status);
