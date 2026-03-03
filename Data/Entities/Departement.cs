using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Data.Entities;

public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom du département est obligatoire.")]
    [StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La localisation est obligatoire.")]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    public int? HeadDoctorId { get; set; }
    public Doctor? HeadDoctor { get; set; }

    // Feature 3 : hiérarchie de départements (auto-référence)
    // null = département racine (ex: "Cardiologie")
    // non null = sous-département (ex: "Cardiologie pédiatrique")
    public int? ParentDepartmentId { get; set; }
    public Department? ParentDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}