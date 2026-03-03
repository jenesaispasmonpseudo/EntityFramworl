using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Data.Entities;

public class Doctor : Staff
{
    // Id, FirstName, LastName, HireDate, Salary → hérités de Staff

    [Required(ErrorMessage = "La spécialité est obligatoire.")]
    [StringLength(100)]
    public string Specialty { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le numéro de licence est obligatoire.")]
    [StringLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    [Required]
    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public ICollection<Consultation> Consultations { get; set; } = [];
}