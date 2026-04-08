using PosApi.DTOs;
using PosApi.Models;

namespace PosApi.Services;

public interface IPurchaseService
{
    Task<PurchaseResponseDto> ProcessPurchaseAsync(PurchaseRequestDto request);
}
