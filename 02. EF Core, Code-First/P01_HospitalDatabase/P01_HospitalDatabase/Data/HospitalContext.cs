

namespace P01_HospitalDatabase
{

    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class HospitalContext : DbContext
    {
        public HospitalContext()  { }

        public HospitalContext(DbContextOptions options)
                :base(options)  { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)   
        {                       // пишем това за да не станат по подразбиране nvarchar(MAX)
            modelBuilder.Entity<Patient>(entity =>
                {
                    entity.HasKey(e => e.PatientId);

                    entity.Property(e => e.FirstName)
                        .IsRequired()
                        .IsUnicode(true)
                        .HasMaxLength(50);

                    entity.Property(e => e.LastName)
                       .IsRequired()
                       .IsUnicode(true)
                       .HasMaxLength(50);

                    entity.Property(e => e.Address)
                       .IsRequired()
                       .IsUnicode(true)
                       .HasMaxLength(250);

                    entity.Property(e => e.Email)
                       .IsRequired()
                       .IsUnicode(false)
                       .HasMaxLength(80);

                    entity.Property(e => e.HasInsurance)
                       .HasDefaultValue(true);

                });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DoctorId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(100);

                entity.Property(e => e.Speciality)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Visitation>(entity => 
            {
                entity.HasKey(e => e.VisitationId);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("DATETIME2");
                    //.HasColumnType("DATETIME2");
                //.HasDefaultValueSql("GETDATE()")

                entity.Property(e => e.Comments)
                        .IsRequired(false) // t.e. ne sa zadalzitelni 
                        .IsUnicode()
                        .HasMaxLength(250);

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Visitations)
                    .HasForeignKey(p => p.PatientId)
                    .HasConstraintName("FK_Visitation_Patient");

                entity.Property(e => e.DoctortId)
                    .IsRequired(false);

                entity.HasOne(e => e.Doctor)
                .WithMany(d => d.Visitations)
                .HasForeignKey(e => e.DoctortId);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(e => e.DiagnoseId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);

                entity.Property(e => e.Comments)
                      .IsRequired(false) // t.e. ne sa zadalzitelni 
                      .IsUnicode()
                      .HasMaxLength(250);
                                
                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(p => p.PatientId)
                    .HasConstraintName("FK_Diagnose_Patient");
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.MedicamentId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);                
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(e => new { e.MedicamentId, e.PatientId });

                entity.HasOne(e => e.Medicament)
                    .WithMany(m => m.Prescriptions)
                    .HasForeignKey(e => e.MedicamentId);

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(e => e.PatientId);
            });
        }

    }
}
