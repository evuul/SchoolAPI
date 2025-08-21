using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;

namespace SchoolAPI.Data;

public class SchoolContext :DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }
    
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(s =>
        {
            s.HasKey(x => x.Id);
            s.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            s.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            s.Property(x => x.Email).HasMaxLength(100);
            s.Property(x => x.BirthDate);
        });

        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                Id = 1,
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada.lovelace@example.com",
                BirthDate = new DateOnly(1815, 12, 10)
            },
            new Student
            {
                Id = 2,
                FirstName = "Alan",
                LastName = "Turing",
                Email = "alan.turing@example.com",
                BirthDate = new DateOnly(1912, 6, 23)
            },
            new Student
            {
                Id = 3,
                FirstName = "Grace",
                LastName = "Hopper",
                Email = "grace.hopper@example.com",
                BirthDate = new DateOnly(1906, 12, 9)
            }
        );

        modelBuilder.Entity<Course>(c =>
        {
            c.HasKey(x => x.Id);
            c.Property(x => x.Title).IsRequired().HasMaxLength(100);
            c.Property(x => x.Credits);
        });

        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = 1,
                Title = "Computer Science 101",
                Credits = 3
            },
            new Course
            {
                Id = 2,
                Title = "Advanced Algorithms",
                Credits = 4
            },
            new Course
            {
                Id = 3,
                Title = "Database Systems",
                Credits = 3
            }
        );
    }
}