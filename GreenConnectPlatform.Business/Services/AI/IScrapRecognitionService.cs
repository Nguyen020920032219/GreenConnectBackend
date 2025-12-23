using GreenConnectPlatform.Business.Models.AI;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.AI;

public interface IScrapRecognitionService
{
    Task<ScrapPostAiSuggestion> AnalyzeImageAsync(IFormFile imageFile, Guid userId);
}