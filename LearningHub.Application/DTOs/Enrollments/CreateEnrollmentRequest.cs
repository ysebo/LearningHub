using System.ComponentModel.DataAnnotations;
using LearningHub.Domain.Enums;

namespace LearningHub.Application.DTOs.Enrollments;

public sealed record CreateEnrollmentRequest(
    [property: Range(1, int.MaxValue, ErrorMessage = "StudentId must be greater than 0.")]
    int StudentId,

    [property: Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0.")]
    int CourseId,

    EnrollmentStatus Status = EnrollmentStatus.Active);
