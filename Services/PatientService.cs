using HospitalManagement.Data;
using HospitalManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Services;

public class PatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }

    // Créer un patient
    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    // Modifier un patient avec gestion de la concurrence
    public async Task<Patient?> UpdateAsync(int id, Patient updated)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient is null) return null;

        patient.FirstName = updated.FirstName;
        patient.LastName = updated.LastName;
        patient.Email = updated.Email;
        patient.Phone = updated.Phone;
        patient.Address = updated.Address;
        patient.DateOfBirth = updated.DateOfBirth;

        try
        {
            await _context.SaveChangesAsync();
            return patient;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // On récupère les valeurs actuelles en base
            var entry = ex.Entries.Single();
            var databaseValues = await entry.GetDatabaseValuesAsync();

            if (databaseValues is null)
                // Le patient a été supprimé par un autre utilisateur
                throw new InvalidOperationException(
                    "Le patient a été supprimé par un autre utilisateur.");

            throw new DbUpdateConcurrencyException(
                "Le patient a été modifié par un autre utilisateur. " +
                "Veuillez recharger les données et réessayer.", ex);
        }
    }

    // Supprimer un patient
    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient is null) return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    // Recherche par nom — utilise l'index IX_Patient_LastName
    public async Task<List<Patient>> SearchByNameAsync(string name)
    {
        return await _context.Patients
            .Where(p => p.LastName.ToLower().Contains(name.ToLower())
                     || p.FirstName.ToLower().Contains(name.ToLower()))
            .AsNoTracking()
            .ToListAsync();
    }

    // Recherche par numéro de dossier — utilise l'index unique IX_Patient_FileNumber
    public async Task<Patient?> GetByFileNumberAsync(string fileNumber)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.FileNumber == fileNumber);
    }

    // Liste alphabétique avec pagination
    public async Task<List<Patient>> GetAllAlphabeticalAsync(int page = 1, int pageSize = 10)
    {
        return await _context.Patients
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    // Compter les patients par département
    public async Task<Dictionary<string, int>> CountPatientsByDepartmentAsync()
    {
        return await _context.Consultations
            .GroupBy(c => c.Doctor.Department.Name)
            .Select(g => new { Department = g.Key, Count = g.Select(c => c.PatientId).Distinct().Count() })
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Department, x => x.Count);
    }
}