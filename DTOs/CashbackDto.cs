namespace PosApi.DTOs;

public class CashbackRequestDto
{
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class CashbackResponseDto
{
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}
