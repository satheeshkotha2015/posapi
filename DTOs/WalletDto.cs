namespace PosApi.DTOs;

public class WalletResponseDto
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class WalletOperationDto
{
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
