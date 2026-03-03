using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Data.Entities;

// Classe abstraite — stratégie TPH (Table Per Hierarchy)
// Une seule table "Staff" avec une colonne discriminante "StaffType"
// Avantage : pas de jointures, requêtes simples et performantes
public abstract class Staff
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    [StringLength(100, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(100, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date d'embauche est obligatoire.")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "Le salaire est obligatoire.")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Salary { get; set; }
}