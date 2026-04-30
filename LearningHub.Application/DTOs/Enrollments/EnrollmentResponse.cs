using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Enrollments;

public sealed record EnrollmentResponse(
    int StudentId,
    string StudentName,
    int CourseId,
    string CourseTitle,
    DateTime EnrolledAt,
    EnrollmentStatus Status);
