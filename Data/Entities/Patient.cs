using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalManagement.Data.Validations;

namespace HospitalManagement.Data.Entities;

public class Patient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le numéro de dossier est obligatoire.")]
    [StringLength(20)]
    public string FileNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    [StringLength(100, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(100, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de naissance est obligatoire.")]
    [DataType(DataType.Date)]
    [PastDate(ErrorMessage = "La date de naissance doit être dans le passé.")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le téléphone est obligatoire.")]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'adresse est obligatoire.")]
    public Address Address { get; set; } = new();
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}