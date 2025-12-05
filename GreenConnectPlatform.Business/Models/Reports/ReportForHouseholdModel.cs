using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Reports;

public class ReportForHouseholdModel
{
    public int PointBalance { get; set; }
    public float EarnPointFromPosts { get; set; }
    public int TotalMyPosts { get; set; }
    public List<PostModel> PostModels { get; set; } = new();
}

public class PostModel
{
    public PostStatus PostStatus { get; set; }
    public int TotalPosts { get; set; }
}