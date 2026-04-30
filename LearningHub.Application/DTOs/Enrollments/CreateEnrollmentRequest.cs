using System.ComponentModel.DataAnnotations;
using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Enrollments;

public sealed record CreateEnrollmentRequest(
    [Range(1, int.MaxValue, ErrorMessage = "StudentId must be greater than 0.")]
    int StudentId,

    [Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0.")]
    int CourseId,

    EnrollmentStatus Status = EnrollmentStatus.Active);
