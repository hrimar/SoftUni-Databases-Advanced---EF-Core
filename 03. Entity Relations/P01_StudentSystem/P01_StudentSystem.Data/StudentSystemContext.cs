namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {        public StudentSystemContext() { }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            //base.OnConfiguring(optionBuilder);

            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer(Confoguration.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.Name)
                    .IsRequired()
                     .IsUnicode()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsUnicode(false)
                    .IsRequired(false)
                .HasColumnType("NCHAR(10)");          //   !!!!!!!!!!!!!!!!

                entity.Property(e => e.Birthday);
                //.IsRequired(false)
                //.HasDefaultValueSql("GETDATE()"); // false ?!?


            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(80);

                entity.Property(e => e.Description)
                    .IsUnicode()
                .IsRequired(false);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.ResourceId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Resources)
                    .HasForeignKey(e => e.CourseId);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(e => e.HomeworkId);

                entity.Property(e => e.Content)
                      .IsRequired()
                   .IsUnicode(false)
                    .HasMaxLength(50);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.HomeworkSubmissions)
                    .HasForeignKey(e => e.CourseId);

                entity.HasOne(e => e.Student)
                   .WithMany(c => c.HomeworkSubmissions)
                   .HasForeignKey(e => e.StudentId);

            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                //entity.ToTable("StudentCourses");

                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity.HasOne(e => e.Course)
                    .WithMany(s => s.StudentsEnrolled)
                    .HasForeignKey(e => e.CourseId);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.CourseEnrollments)
                    .HasForeignKey(e => e.StudentId);
            });
        }

    }
}
