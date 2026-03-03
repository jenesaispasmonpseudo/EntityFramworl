using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Data.Validations;

public class PastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
            return date.Date < DateTime.Today;

        return false;
    }
}