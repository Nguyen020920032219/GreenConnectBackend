using System.ComponentModel.DataAnnotations;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Models.Users;

public class SubmitEkycRequest
{
    [Required] public BuyerType BuyerType { get; set; }

    [Required] public IFormFile FrontImage { get; set; } = null!;
}