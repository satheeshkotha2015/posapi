using PosApi.DTOs;

namespace PosApi.Services;

public interface IPromotionService
{
    Task<PromotionResponseDto> ApplyPromotionAsync(PromotionRequestDto request);
}
