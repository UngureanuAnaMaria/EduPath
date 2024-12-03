using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.LastLogin).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            modelBuilder.Entity<Professor>(entity =>
            {
                entity.ToTable("Professors");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.LastLogin).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.ToTable("StudentCourse");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Professor>()
                .HasMany(e => e.Courses)
                .WithOne(e => e.Professor)
                .HasForeignKey(e => e.ProfessorId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<StudentCourse>()
                .HasOne(s => s.Student)
                .WithMany(sc => sc.StudentCourses)
                .HasForeignKey(si => si.StudentId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<StudentCourse>()
                .HasOne(c => c.Course)
                .WithMany(sc => sc.StudentCourses)
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lessons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("uuid_generate_v4()")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(50000);
                entity.Property(e => e.CourseId).IsRequired();
            });

            modelBuilder.Entity<Course>()
               .HasMany(e => e.Lessons)
               .WithOne(e => e.Course)
               .HasForeignKey(e => e.CourseId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}