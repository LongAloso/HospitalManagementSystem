namespace HMS.Domain.Entities;

public class PatientAllergy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string Allergen { get; set; } = string.Empty; // Tác nhân gây dị ứng (VD: Penicillin)
    public string Severity { get; set; } = string.Empty; // Mức độ: Nhẹ, Trung bình, Nặng
}

