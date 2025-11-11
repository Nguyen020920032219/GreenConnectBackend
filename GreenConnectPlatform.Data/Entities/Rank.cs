namespace GreenConnectPlatform.Data.Entities;

public class Rank
{
    public int RankId { get; set; } 
    public string Name { get; set; } = null!;
    public int MinPoints { get; set; }
    public string? BadgeImageUrl { get; set; }
    
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}