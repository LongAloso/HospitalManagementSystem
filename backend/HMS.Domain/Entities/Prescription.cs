namespace HMS.Domain.Entities;

public class Prescription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ConsultationId { get; set; } 
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }

    public string Notes { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Pending";

    public Consultation? Consultation { get; set; }
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}