using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenConnectPlatform.Data.Entities;

public class ScrapCategory
{
    public int ScrapCategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public virtual ICollection<ScrapPostDetail> ScrapPostDetails { get; set; } = new List<ScrapPostDetail>();
}