namespace GreenConnectPlatform.Business.Models.ScrapCategories;

public class ScrapCategoryModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }
}