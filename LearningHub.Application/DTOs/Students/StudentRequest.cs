using System.ComponentModel.DataAnnotations;

namespace LearningHub.Application.DTOs.Students;

public sealed record StudentRequest(
    string FullName,
    string Email,
    DateOnly DateOfBirth);
