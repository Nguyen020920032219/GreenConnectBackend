namespace GreenConnectPlatform.Business.Models.ScrapCategories;

public class ScrapCategoryModel
{
    public int ScrapCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }
}