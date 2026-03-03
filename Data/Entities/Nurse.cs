using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Data.Entities;

public class Nurse : Staff
{
    // Id, FirstName, LastName, HireDate, Salary → hérités de Staff

    [Required(ErrorMessage = "Le service est obligatoire.")]
    [StringLength(100)]
    public string Service { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le grade est obligatoire.")]
    [StringLength(50)]
    public string Grade { get; set; } = string.Empty;
}