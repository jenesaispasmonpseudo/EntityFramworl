using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Data.Entities;

public class Consultation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le patient est obligatoire.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Le médecin est obligatoire.")]
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "La date est obligatoire.")]
    public DateTime Date { get; set; }

    [Required]
    public ConsultationStatus Status { get; set; } = ConsultationStatus.Planned;

    [StringLength(500)]
    public string? Notes { get; set; }

    // Navigation
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}