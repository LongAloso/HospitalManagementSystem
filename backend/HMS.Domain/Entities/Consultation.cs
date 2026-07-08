namespace HMS.Domain.Entities;

public class Consultation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; } 
    public Guid DoctorId { get; set; }
    
    public string BloodPressure { get; set; } = string.Empty; 
    public decimal Temperature { get; set; } 
    public decimal Weight { get; set; } 
    public decimal Height { get; set; } 
    public int HeartRate { get; set; } 

    public string Symptoms { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty; 

    public string Icd10Code { get; set; } = string.Empty; 
    public string Icd10Name { get; set; } = string.Empty; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Appointment? Appointment { get; set; }
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public Prescription? Prescription { get; set; }
}