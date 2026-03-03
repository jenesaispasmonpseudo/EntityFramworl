using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data.Entities;

// Owned Entity : pas de table propre, les colonnes sont intégrées
// dans la table de l'entité propriétaire (Patient, Department...)
[Owned]
public class Address
{
    [Required(ErrorMessage = "La rue est obligatoire.")]
    [StringLength(200)]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ville est obligatoire.")]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le code postal est obligatoire.")]
    [StringLength(10)]
    public string ZipCode { get; set; } = string.Empty;

    [StringLength(100)]
    public string Country { get; set; } = "France";
}