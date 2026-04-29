using LearningHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningHub.Infrastructure.Persistence;

public sealed class LearningHubDbContext(DbContextOptions<LearningHubDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(category => category.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(category => category.Description)
                .HasMaxLength(400);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(course => course.Title)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(course => course.Description)
                .HasMaxLength(800);

            entity.Property(course => course.Price)
                .HasPrecision(10, 2);

            entity.HasOne(course => course.Category)
                .WithMany(category => category.Courses)
                .HasForeignKey(course => course.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(student => student.FullName)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(student => student.Email)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(student => student.Email)
                .IsUnique();
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(enrollment => new { enrollment.StudentId, enrollment.CourseId });

            entity.Property(enrollment => enrollment.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasOne(enrollment => enrollment.Student)
                .WithMany(student => student.Enrollments)
                .HasForeignKey(enrollment => enrollment.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(enrollment => enrollment.Course)
                .WithMany(course => course.Enrollments)
                .HasForeignKey(enrollment => enrollment.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
