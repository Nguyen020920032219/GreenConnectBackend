using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Entities;

public class ScrapPostDetail
{
    public string? AmountDescription { get; set; }
    public string? ImageUrl { get; set; }
    public ItemStatus Status { get; set; } = ItemStatus.Available;
    public Guid ScrapPostId { get; set; }
    public int ScrapCategoryId { get; set; }
    public virtual ScrapPost ScrapPost { get; set; } = null!;
    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
    public virtual ICollection<CollectionOffer> Offers { get; set; } = new List<CollectionOffer>();
}