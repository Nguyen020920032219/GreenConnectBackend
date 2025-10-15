using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Configurations;

public partial class GreenConnectDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public GreenConnectDbContext()
    {
    }

    public GreenConnectDbContext(DbContextOptions<GreenConnectDbContext> options)
        : base(options)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<ChatParticipant>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ChatRoomId }).HasName("ChatParticipants_pkey");

            entity.Property(e => e.JoinedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.ChatRoomId)
                .HasConstraintName("ChatParticipants_ChatRoomId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("ChatParticipants_UserId_fkey");
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.ChatRoomId).HasName("ChatRooms_pkey");

            entity.HasIndex(e => e.TransactionId, "ChatRooms_TransactionId_key").IsUnique();

            entity.Property(e => e.ChatRoomId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Transaction).WithOne(p => p.ChatRoom)
                .HasForeignKey<ChatRoom>(d => d.TransactionId)
                .HasConstraintName("ChatRooms_TransactionId_fkey");
        });

        modelBuilder.Entity<CollectionOffer>(entity =>
        {
            entity.HasKey(e => e.CollectionOfferId).HasName("CollectionOffers_pkey");

            entity.Property(e => e.CollectionOfferId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.ProposedPrice).HasPrecision(18, 2);
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.ScrapCollector).WithMany(p => p.CollectionOffers)
                .HasForeignKey(d => d.ScrapCollectorId)
                .HasConstraintName("CollectionOffers_ScrapCollectorId_fkey");

            entity.HasOne(d => d.ScrapPost).WithMany(p => p.CollectionOffers)
                .HasForeignKey(d => d.ScrapPostId)
                .HasConstraintName("CollectionOffers_ScrapPostId_fkey");
        });

        modelBuilder.Entity<CollectorVerificationInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("CollectorVerificationInfos_pkey");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.User).WithOne(p => p.CollectorVerificationInfo)
                .HasForeignKey<CollectorVerificationInfo>(d => d.UserId)
                .HasConstraintName("CollectorVerificationInfos_UserId_fkey");

            entity.HasOne<User>(v => v.Reviewer)
                .WithMany()
                .HasForeignKey(v => v.ReviewerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.ComplaintId).HasName("Complaints_pkey");

            entity.Property(e => e.ComplaintId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.Accused).WithMany(p => p.ComplaintAccuseds)
                .HasForeignKey(d => d.AccusedId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Complaints_AccusedId_fkey");

            entity.HasOne(d => d.Complainant).WithMany(p => p.ComplaintComplainants)
                .HasForeignKey(d => d.ComplainantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Complaints_ComplainantId_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("Complaints_TransactionId_fkey");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("Feedbacks_pkey");

            entity.Property(e => e.FeedbackId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Reviewee).WithMany(p => p.FeedbackReviewees)
                .HasForeignKey(d => d.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Feedbacks_RevieweeId_fkey");

            entity.HasOne(d => d.Reviewer).WithMany(p => p.FeedbackReviewers)
                .HasForeignKey(d => d.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Feedbacks_ReviewerId_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("Feedbacks_TransactionId_fkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("Messages_pkey");

            entity.HasIndex(e => new { e.ChatRoomId, e.Timestamp }, "IX_Messages_ChatRoomId_Timestamp")
                .IsDescending(false, true);

            entity.Property(e => e.MessageId).ValueGeneratedNever();
            entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatRoomId)
                .HasConstraintName("Messages_ChatRoomId_fkey");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("Messages_SenderId_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("Notifications_pkey");

            entity.HasIndex(e => new { e.RecipientId, e.CreatedAt }, "IX_Notifications_RecipientId_CreatedAt")
                .IsDescending(false, true);

            entity.Property(e => e.NotificationId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.IsRead).HasDefaultValue(false);

            entity.HasOne(d => d.Recipient).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.RecipientId)
                .HasConstraintName("Notifications_RecipientId_fkey");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("Profiles_pkey");

            entity.HasIndex(e => e.Location, "IX_Profiles_Location").HasMethod("gist");

            entity.HasIndex(e => e.UserId, "Profiles_UserId_key").IsUnique();

            entity.Property(e => e.ProfileId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Location).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.RewardPoint).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithOne(p => p.Profile)
                .HasForeignKey<Profile>(d => d.UserId)
                .HasConstraintName("Profiles_UserId_fkey");
        });

        modelBuilder.Entity<RewardItem>(entity =>
        {
            entity.HasKey(e => e.RewardItemId).HasName("RewardItems_pkey");

            entity.Property(e => e.ItemName).HasMaxLength(150);
        });

        modelBuilder.Entity<ScheduleProposal>(entity =>
        {
            entity.HasKey(e => e.ScheduleProposalId).HasName("ScheduleProposals_pkey");

            entity.HasIndex(e => e.TransactionId, "IX_ScheduleProposals_TransactionId");

            entity.Property(e => e.ScheduleProposalId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.Proposer).WithMany(p => p.ScheduleProposals)
                .HasForeignKey(d => d.ProposerId)
                .HasConstraintName("ScheduleProposals_ProposerId_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.ScheduleProposals)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("ScheduleProposals_TransactionId_fkey");
        });

        modelBuilder.Entity<ScrapCategory>(entity =>
        {
            entity.HasKey(e => e.ScrapCategoryId).HasName("ScrapCategories_pkey");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<ScrapPost>(entity =>
        {
            entity.HasKey(e => e.ScrapPostId).HasName("ScrapPosts_pkey");

            entity.HasIndex(e => e.Location, "IX_ScrapPosts_Location").HasMethod("gist");

            entity.HasIndex(e => new { e.Status, e.HouseholdId }, "IX_ScrapPosts_Status_HouseholdId");

            entity.Property(e => e.ScrapPostId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AvailableTimeRange).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Location).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Household).WithMany(p => p.ScrapPosts)
                .HasForeignKey(d => d.HouseholdId)
                .HasConstraintName("ScrapPosts_HouseholdId_fkey");
        });

        modelBuilder.Entity<ScrapPostDetail>(entity =>
        {
            entity.HasKey(e => new { e.ScrapPostId, e.ScrapCategoryId }).HasName("ScrapPostDetails_pkey");

            entity.Property(e => e.AmountDescription).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.ScrapCategory).WithMany(p => p.ScrapPostDetails)
                .HasForeignKey(d => d.ScrapCategoryId)
                .HasConstraintName("ScrapPostDetails_ScrapCategoryId_fkey");

            entity.HasOne(d => d.ScrapPost).WithMany(p => p.ScrapPostDetails)
                .HasForeignKey(d => d.ScrapPostId)
                .HasConstraintName("ScrapPostDetails_ScrapPostId_fkey");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("Transactions_pkey");

            entity.HasIndex(e => new { e.Status, e.HouseholdId }, "IX_Transactions_Status_HouseholdId");

            entity.HasIndex(e => new { e.Status, e.ScrapCollectorId }, "IX_Transactions_Status_ScrapCollectorId");

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.Household).WithMany(p => p.TransactionHouseholds)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Transactions_HouseholdId_fkey");

            entity.HasOne(d => d.Offer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Transactions_OfferId_fkey");

            entity.HasOne(d => d.ScrapCollector).WithMany(p => p.TransactionScrapCollectors)
                .HasForeignKey(d => d.ScrapCollectorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Transactions_ScrapCollectorId_fkey");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => new { e.TransactionId, e.ScrapCategoryId }).HasName("TransactionDetails_pkey");

            entity.Property(e => e.ActualPrice).HasPrecision(18, 2);

            entity.HasOne(d => d.ScrapCategory).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.ScrapCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("TransactionDetails_ScrapCategoryId_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("TransactionDetails_TransactionId_fkey");
        });

        modelBuilder.Entity<UserRewardRedemption>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RewardItemId, e.RedemptionDate })
                .HasName("UserRewardRedemptions_pkey");

            entity.Property(e => e.RedemptionDate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.RewardItem).WithMany(p => p.UserRewardRedemptions)
                .HasForeignKey(d => d.RewardItemId)
                .HasConstraintName("UserRewardRedemptions_RewardItemId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRewardRedemptions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserRewardRedemptions_UserId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}