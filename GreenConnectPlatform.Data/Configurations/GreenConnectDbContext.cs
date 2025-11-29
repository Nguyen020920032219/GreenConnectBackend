using System.Reflection;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Configurations;

public class GreenConnectDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public GreenConnectDbContext(DbContextOptions<GreenConnectDbContext> options) : base(options)
    {
    }

    public virtual DbSet<ChatParticipant> ChatParticipants { get; set; }
    public virtual DbSet<ChatRoom> ChatRooms { get; set; }
    public virtual DbSet<CollectionOffer> CollectionOffers { get; set; }
    public virtual DbSet<CollectorVerificationInfo> CollectorVerificationInfos { get; set; }
    public virtual DbSet<Complaint> Complaints { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<RewardItem> RewardItems { get; set; }
    public virtual DbSet<ScheduleProposal> ScheduleProposals { get; set; }
    public virtual DbSet<ScrapCategory> ScrapCategories { get; set; }
    public virtual DbSet<ScrapPost> ScrapPosts { get; set; }
    public virtual DbSet<ScrapPostDetail> ScrapPostDetails { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }
    public virtual DbSet<UserRewardRedemption> UserRewardRedemptions { get; set; }
    public virtual DbSet<PaymentPackage> PaymentPackages { get; set; }
    public virtual DbSet<UserPackage> UserPackages { get; set; }
    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public virtual DbSet<ReferencePrice> ReferencePrices { get; set; }
    public virtual DbSet<PointHistory> PointHistories { get; set; }
    public virtual DbSet<Rank> Ranks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.HasPostgresExtension("unaccent");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Seed();
    }
}