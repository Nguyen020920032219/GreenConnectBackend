using GreenConnectPlatform.Business.Models.VerificationInfos;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.AI;

public interface IEkycService
{
    Task<IdCardOcrResult> ExtractIdCardInfoAsync(IFormFile imageFile);
}