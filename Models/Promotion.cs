namespace PosApi.Models;

public class Promotion
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal MinAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool IsActive { get; set; }
}
