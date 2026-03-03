namespace HospitalManagement.DTOs;

public class PatientDetailDto
{
    public int Id { get; set; }
    public string FileNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<ConsultationDetailDto> Consultations { get; set; } = new();
}

public class ConsultationDetailDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string DoctorFullName { get; set; } = string.Empty;
    public string DoctorSpecialty { get; set; } = string.Empty;
}