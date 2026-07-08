using HMS.Domain.Enums;

namespace HMS.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime AppointmentDate { get; set; } 
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public string Reason { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public Consultation? Consultation { get; set; }
}