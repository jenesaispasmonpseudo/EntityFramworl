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

    // Modifier un patient
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

        await _context.SaveChangesAsync();
        return patient;
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

    // Rechercher par nom (insensible à la casse)
    public async Task<List<Patient>> SearchByNameAsync(string name)
    {
        return await _context.Patients
            .Where(p => p.LastName.ToLower().Contains(name.ToLower())
                     || p.FirstName.ToLower().Contains(name.ToLower()))
            .AsNoTracking()
            .ToListAsync();
    }

    // Lister tous les patients par ordre alphabétique avec pagination
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
}