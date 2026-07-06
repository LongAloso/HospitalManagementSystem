namespace HMS.Domain.Entities;

public class MedicalHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }

    public bool HasDiabetes { get; set; } // Tiểu đường
    public bool HasHypertension { get; set; } // Huyết áp cao
    public bool HasAsthma { get; set; } // Hen suyễn
    public bool HasCancerHistory { get; set; } // Tiền sử ung thư
    public string PreviousSurgeries { get; set; } = string.Empty; // Phẫu thuật trước đó
    public string FamilyDisease { get; set; } = string.Empty; // Bệnh di truyền gia đình
}