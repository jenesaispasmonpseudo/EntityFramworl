using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Data.Entities;

public class AdminStaff : Staff
{
    // Id, FirstName, LastName, HireDate, Salary → hérités de Staff

    [Required(ErrorMessage = "La fonction est obligatoire.")]
    [StringLength(100)]
    public string Function { get; set; } = string.Empty;
}