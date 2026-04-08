namespace PosApi.Models;

public class CashbackRequest
{
    public int Id { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public CashbackStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum CashbackStatus
{
    Pending,
    Approved,
    Rejected
}
