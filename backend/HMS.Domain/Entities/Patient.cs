namespace HMS.Domain.Entities;

public class Patient
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } // Khóa ngoại liên kết với bảng User
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CitizenId { get; set; } = string.Empty; // CCCD
    public string Address { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty; // A, B, AB, O
    public string EmergencyContact { get; set; } = string.Empty;

    // Navigation properties
    public MedicalHistory? MedicalHistory { get; set; }
    public ICollection<PatientAllergy> Allergies { get; set; } = new List<PatientAllergy>();
}