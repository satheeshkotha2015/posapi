namespace PosApi.DTOs;

public class PurchaseRequestDto
{
    public decimal Amount { get; set; }
    public string CustomerId { get; set; } = string.Empty;
}

public class PurchaseResponseDto
{
    public bool Success { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Message { get; set; } = string.Empty;
}
