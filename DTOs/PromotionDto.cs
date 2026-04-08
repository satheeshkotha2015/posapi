namespace PosApi.DTOs;

public class PromotionRequestDto
{
    public string TransactionId { get; set; } = string.Empty;
    public int PromotionId { get; set; }
}

public class PromotionResponseDto
{
    public bool Applied { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Message { get; set; } = string.Empty;
}
