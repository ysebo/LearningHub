using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Enrollments;

public sealed record UpdateEnrollmentRequest(EnrollmentStatus Status);
