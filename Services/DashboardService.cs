using HospitalManagement.Data;
using HospitalManagement.Data.Entities;
using HospitalManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    // ── Fiche patient ─────────────────────────────────────────
    // Eager Loading avec Include/ThenInclude pour éviter le N+1 :
    // sans Include, EF ferait 1 requête pour le patient + 1 par consultation
    // → avec Include, tout est chargé en UNE seule requête SQL
    public async Task<PatientDetailDto?> GetPatientDetailAsync(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Consultations)
                .ThenInclude(c => c.Doctor)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient is null) return null;

        // Projection manuelle vers le DTO
        // On ne retourne que les données nécessaires pour la vue
        return new PatientDetailDto
        {
            Id = patient.Id,
            FileNumber = patient.FileNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Consultations = patient.Consultations.Select(c => new ConsultationDetailDto
            {
                Id = c.Id,
                Date = c.Date,
                Status = c.Status.ToString(),
                Notes = c.Notes,
                DoctorFullName = $"{c.Doctor.FirstName} {c.Doctor.LastName}",
                DoctorSpecialty = c.Doctor.Specialty
            }).ToList()
        };
    }

    // ── Planning médecin ──────────────────────────────────────
    // Projection directe avec Select → plus performant car EF traduit
    // directement en SQL sans charger les entités entières en mémoire
    public async Task<DoctorPlanningDto?> GetDoctorPlanningAsync(int doctorId)
    {
        return await _context.Doctors
            .Where(d => d.Id == doctorId)
            .Select(d => new DoctorPlanningDto
            {
                Id = d.Id,
                FullName = $"{d.FirstName} {d.LastName}",
                Specialty = d.Specialty,
                DepartmentName = d.Department.Name,
                UpcomingConsultations = d.Consultations
                    .Where(c => c.Date >= DateTime.Now
                             && c.Status == ConsultationStatus.Planned)
                    .OrderBy(c => c.Date)
                    .Select(c => new UpcomingConsultationDto
                    {
                        Id = c.Id,
                        Date = c.Date,
                        Status = c.Status.ToString(),
                        PatientFullName = $"{c.Patient.FirstName} {c.Patient.LastName}",
                        Notes = c.Notes
                    }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    // ── Statistiques département ──────────────────────────────
    // Projection avec Select + agrégation Count directement en SQL
    // Pertinent ici car on n'a besoin que de compteurs,
    // pas des entités Doctor ou Consultation complètes
    public async Task<List<DepartmentStatsDto>> GetDepartmentStatsAsync()
    {
        return await _context.Departments
            .Select(dep => new DepartmentStatsDto
            {
                Id = dep.Id,
                Name = dep.Name,
                Location = dep.Location,
                DoctorCount = dep.Doctors.Count(),
                ConsultationCount = dep.Doctors
                    .SelectMany(d => d.Consultations)
                    .Count()
            })
            .AsNoTracking()
            .ToListAsync();
    }
}