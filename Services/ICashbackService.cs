using PosApi.DTOs;

namespace PosApi.Services;

public interface ICashbackService
{
    Task<CashbackResponseDto> RequestCashbackAsync(CashbackRequestDto request);
}
