using HospitalManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Consultation> Consultations => Set<Consultation>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<AdminStaff> AdminStaff => Set<AdminStaff>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            // Unicité
            entity.HasIndex(p => p.FileNumber).IsUnique();
            entity.HasIndex(p => p.Email).IsUnique();

            // Index pour recherche rapide par nom (étape 7)
            entity.HasIndex(p => p.LastName)
                  .HasDatabaseName("IX_Patient_LastName");

            // Owned Entity Address
            entity.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Address_Street");
                address.Property(a => a.City).HasColumnName("Address_City");
                address.Property(a => a.ZipCode).HasColumnName("Address_ZipCode");
                address.Property(a => a.Country).HasColumnName("Address_Country");
            });

            // Concurrency Token
            entity.Property(p => p.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(d => d.Name).IsUnique();

            entity.HasOne(d => d.ParentDepartment)
                  .WithMany(d => d.SubDepartments)
                  .HasForeignKey(d => d.ParentDepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Doctor ───────────────────────────────────────────────
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasIndex(d => d.LicenseNumber).IsUnique();

            entity.HasOne(d => d.Department)
                  .WithMany(dep => dep.Doctors)
                  .HasForeignKey(d => d.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Department>()
            .HasOne(dep => dep.HeadDoctor)
            .WithOne()
            .HasForeignKey<Department>(dep => dep.HeadDoctorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasIndex(c => new { c.PatientId, c.DoctorId, c.Date })
                  .IsUnique();

            // Index pour lister les consultations d'un médecin par date
            entity.HasIndex(c => new { c.DoctorId, c.Date })
                  .HasDatabaseName("IX_Consultation_DoctorId_Date");

            entity.HasOne(c => c.Patient)
                  .WithMany(p => p.Consultations)
                  .HasForeignKey(c => c.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Doctor)
                  .WithMany(d => d.Consultations)
                  .HasForeignKey(c => c.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Staff>()
            .HasDiscriminator<string>("StaffType")
            .HasValue<Doctor>("Doctor")
            .HasValue<Nurse>("Nurse")
            .HasValue<AdminStaff>("AdminStaff");
    }
}