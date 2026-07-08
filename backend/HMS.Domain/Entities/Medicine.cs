namespace HMS.Domain.Entities;

public class Medicine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public string ActiveIngredient { get; set; } = string.Empty;

    public string Manufacturer { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int StockQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;
}