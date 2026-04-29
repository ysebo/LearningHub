using LearningHub.Domain.Enums;

namespace LearningHub.Domain.Entities;

public sealed class Enrollment
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime EnrolledAt { get; set; }

    public EnrollmentStatus Status { get; set; }

    public Student? Student { get; set; }

    public Course? Course { get; set; }
}
