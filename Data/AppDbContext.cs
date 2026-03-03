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

        // ── Patient ──────────────────────────────────────────────
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasIndex(p => p.FileNumber).IsUnique();
            entity.HasIndex(p => p.Email).IsUnique();
            entity.ToTable("Patients", t => t.HasCheckConstraint(
                "CK_Patient_DateOfBirth",
                "\"DateOfBirth\" < date('now')"
            ));

            // Owned Entity Address — colonnes préfixées Address_
            entity.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Address_Street");
                address.Property(a => a.City).HasColumnName("Address_City");
                address.Property(a => a.ZipCode).HasColumnName("Address_ZipCode");
                address.Property(a => a.Country).HasColumnName("Address_Country");
            });
        });

        // ── Department ───────────────────────────────────────────
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(d => d.Name).IsUnique();

            // Auto-référence pour la hiérarchie de départements
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

        // ── Consultation ─────────────────────────────────────────
        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasIndex(c => new { c.PatientId, c.DoctorId, c.Date })
                  .IsUnique();

            entity.HasOne(c => c.Patient)
                  .WithMany(p => p.Consultations)
                  .HasForeignKey(c => c.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Doctor)
                  .WithMany(d => d.Consultations)
                  .HasForeignKey(c => c.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── TPH Staff ─────────────────────────────────────────────
        // Stratégie TPH : une seule table "Staff" avec colonne discriminante
        // EF Core gère automatiquement la colonne "Discriminator"
        modelBuilder.Entity<Staff>()
            .HasDiscriminator<string>("StaffType")
            .HasValue<Doctor>("Doctor")
            .HasValue<Nurse>("Nurse")
            .HasValue<AdminStaff>("AdminStaff");
    }
}