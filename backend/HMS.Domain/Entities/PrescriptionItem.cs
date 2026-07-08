namespace HMS.Domain.Entities;

public class PrescriptionItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrescriptionId { get; set; }
    public Guid MedicineId { get; set; }

    public int Quantity { get; set; } 
    public int DurationDays { get; set; } 
    public string Instruction { get; set; } = string.Empty;

    public Prescription? Prescription { get; set; }
    public Medicine? Medicine { get; set; }
}