namespace HMS.Domain.Entities;

public class DoctorLeave
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DoctorId { get; set; } 

    public DateTime LeaveDate { get; set; } 
    public string Reason { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Doctor? Doctor { get; set; }
}