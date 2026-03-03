using HospitalManagement.Data;
using HospitalManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Services;

public class ConsultationService
{
    private readonly AppDbContext _context;

    public ConsultationService(AppDbContext context)
    {
        _context = context;
    }

    // Planifier une consultation
    public async Task<Consultation> ScheduleAsync(Consultation consultation)
    {
        consultation.Status = ConsultationStatus.Planned;
        _context.Consultations.Add(consultation);
        await _context.SaveChangesAsync();
        return consultation;
    }

    // Modifier le statut
    public async Task<Consultation?> UpdateStatusAsync(int id, ConsultationStatus status)
    {
        var consultation = await _context.Consultations.FindAsync(id);
        if (consultation is null) return null;

        consultation.Status = status;
        await _context.SaveChangesAsync();
        return consultation;
    }

    // Annuler une consultation
    public async Task<bool> CancelAsync(int id)
    {
        var consultation = await _context.Consultations.FindAsync(id);
        if (consultation is null) return false;

        consultation.Status = ConsultationStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    // Consultations à venir pour un patient
    public async Task<List<Consultation>> GetUpcomingForPatientAsync(int patientId)
    {
        return await _context.Consultations
            .Where(c => c.PatientId == patientId
                     && c.Status == ConsultationStatus.Planned
                     && c.Date >= DateTime.Now)
            .Include(c => c.Doctor)
            .OrderBy(c => c.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    // Consultations du jour pour un médecin
    public async Task<List<Consultation>> GetTodayForDoctorAsync(int doctorId)
    {
        var today = DateTime.Today;
        return await _context.Consultations
            .Where(c => c.DoctorId == doctorId
                     && c.Date.Date == today
                     && c.Status != ConsultationStatus.Cancelled)
            .Include(c => c.Patient)
            .OrderBy(c => c.Date)
            .AsNoTracking()
            .ToListAsync();
    }
}