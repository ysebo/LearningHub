using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Enrollments;

public sealed record StudentCourseResponse(
    int CourseId,
    string Title,
    decimal Price,
    string CategoryName,
    DateTime EnrolledAt,
    EnrollmentStatus Status);
