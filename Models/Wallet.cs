namespace PosApi.Models;

public class Wallet
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
