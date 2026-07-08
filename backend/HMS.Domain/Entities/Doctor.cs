namespace HMS.Domain.Entities;

public class Doctor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } 

    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty; 
    public string Specialty { get; set; } = string.Empty; 
    public string PhoneNumber { get; set; } = string.Empty;
    public int ExperienceYears { get; set; } 
    public string Biography { get; set; } = string.Empty; 

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<DoctorLeave> Leaves { get; set; } = new List<DoctorLeave>();
}