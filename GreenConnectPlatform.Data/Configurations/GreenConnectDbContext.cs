using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Configurations;

public class GreenConnectDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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

            entity.HasOne(d => d.ScrapPost)
                .WithMany(p => p.CollectionOffers)
                .HasForeignKey(d => d.ScrapPostId)
                .HasConstraintName("CollectionOffers_ScrapPostId_fkey");

            entity.HasOne(d => d.ScrapCollector)
                .WithMany(p => p.CollectionOffers)
                .HasForeignKey(d => d.ScrapCollectorId)
                .HasConstraintName("CollectionOffers_ScrapCollectorId_fkey");

            entity.HasMany(o => o.ScheduleProposals)
                .WithOne(p => p.Offer)
                .HasForeignKey(p => p.CollectionOfferId)
                .OnDelete(DeleteBehavior.Cascade);
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
            entity.Property(e => e.Gender)
                .HasConversion<string>();
            entity.Property(e => e.Location).HasColumnType("geometry(Point,4326)");
            entity.Property(e => e.PointBalance).HasDefaultValue(200);
            entity.HasOne(d => d.Rank)
                .WithMany(p => p.Profiles)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.Restrict);
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

            entity.Property(e => e.ScheduleProposalId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.HasOne(d => d.Proposer).WithMany(p => p.ScheduleProposals)
                .HasForeignKey(d => d.ProposerId)
                .HasConstraintName("ScheduleProposals_ProposerId_fkey");
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

            entity.Property(e => e.CheckInLocation).HasColumnType("geometry(Point,4326)");

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

            entity.Property(e => e.PricePerUnit).HasPrecision(18, 2);
            entity.Property(e => e.Unit).HasMaxLength(10).HasDefaultValue("kg");
            entity.Property(e => e.Quantity);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);
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

        modelBuilder.Entity<PaymentPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<UserPackage>(entity =>
        {
            entity.HasKey(e => e.UserPackageId);
            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
            entity.HasOne(d => d.Package).WithMany().HasForeignKey(d => d.PackageId);
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
            entity.HasOne(d => d.Package).WithMany().HasForeignKey(d => d.PackageId).IsRequired(false);
        });

        modelBuilder.Entity<ReferencePrice>(entity =>
        {
            entity.HasKey(e => e.ReferencePriceId);
            entity.Property(e => e.PricePerKg).HasPrecision(18, 2);
            entity.HasOne(d => d.ScrapCategory).WithMany().HasForeignKey(d => d.ScrapCategoryId);
            entity.HasOne(d => d.UpdatedByAdmin).WithMany().HasForeignKey(d => d.UpdatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PointHistory>(entity =>
        {
            entity.HasKey(e => e.PointHistoryId);
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.MinPoints).IsUnique();
        });

        modelBuilder.Entity<OfferDetail>(entity =>
        {
            entity.HasKey(e => e.OfferDetailId);
            entity.Property(e => e.PricePerUnit).HasPrecision(18, 2);
            entity.Property(e => e.Unit).HasMaxLength(10).HasDefaultValue("kg");

            entity.HasOne(d => d.CollectionOffer)
                .WithMany(p => p.OfferDetails)
                .HasForeignKey(d => d.CollectionOfferId);

            entity.HasOne(d => d.ScrapCategory)
                .WithMany()
                .HasForeignKey(d => d.ScrapCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #region Data Seeding

        var seedDate = new DateTime(2025, 10, 10, 10, 0, 0, DateTimeKind.Utc);

        var adminRoleId = new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542");
        var householdRoleId = new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c");
        var individualRoleId = new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84");
        var businessRoleId = new Guid("e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a");

        var adminUserId = new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff");
        var householdUserId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011");
        var buyerIndiId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00");
        var buyerBussId = new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef");

        modelBuilder.Entity<ScrapCategory>().HasData(
            new ScrapCategory { ScrapCategoryId = 1, CategoryName = "Giấy vụn" },
            new ScrapCategory { ScrapCategoryId = 2, CategoryName = "Thùng carton" },
            new ScrapCategory { ScrapCategoryId = 3, CategoryName = "Chai nhựa (PET)" },
            new ScrapCategory { ScrapCategoryId = 4, CategoryName = "Lon nhôm" },
            new ScrapCategory { ScrapCategoryId = 5, CategoryName = "Sắt vụn" },
            new ScrapCategory { ScrapCategoryId = 6, CategoryName = "Đồ điện tử cũ" }
        );

        modelBuilder.Entity<RewardItem>().HasData(
            new RewardItem { RewardItemId = 1, ItemName = "Khung viền Avatar 'Chiến binh Tái chế'", PointsCost = 100 },
            new RewardItem { RewardItemId = 2, ItemName = "Voucher Giảm giá 10% (fake)", PointsCost = 500 }
        );

        modelBuilder.Entity<Rank>().HasData(
            new Rank { RankId = 1, Name = "Bronze", MinPoints = 0, BadgeImageUrl = "/images/badges/bronze.png" },
            new Rank { RankId = 2, Name = "Silver", MinPoints = 5000, BadgeImageUrl = "/images/badges/silver.png" },
            new Rank { RankId = 3, Name = "Gold", MinPoints = 20000, BadgeImageUrl = "/images/badges/gold.png" }
        );

        var packageFreemiumId = new Guid("f0000001-0000-0000-0000-000000000001");
        var packagePaidId = new Guid("f0000002-0000-0000-0000-000000000002");

        modelBuilder.Entity<PaymentPackage>().HasData(
            new PaymentPackage
            {
                PackageId = packageFreemiumId,
                Name = "Gói Dùng Thử",
                Description = "Gói miễn phí cho người dùng mới trải nghiệm.",
                Price = 0,
                ConnectionAmount = 5,
                IsActive = true,
                PackageType = PackageType.Freemium
            },
            new PaymentPackage
            {
                PackageId = packagePaidId,
                Name = "Gói Chuyên Nghiệp (1 Tháng)",
                Description = "Kết nối không giới hạn trong 30 ngày.",
                Price = 99000,
                ConnectionAmount = null,
                IsActive = true,
                PackageType = PackageType.Paid
            }
        );

        modelBuilder.Entity<ReferencePrice>().HasData(
            new ReferencePrice
            {
                ReferencePriceId = new Guid("11111111-0000-0000-0000-000000000001"),
                ScrapCategoryId = 3,
                PricePerKg = 3000,
                LastUpdated = seedDate,
                UpdatedByAdminId = adminUserId
            },
            new ReferencePrice
            {
                ReferencePriceId = new Guid("11111111-0000-0000-0000-000000000002"),
                ScrapCategoryId = 4,
                PricePerKg = 25000,
                LastUpdated = seedDate,
                UpdatedByAdminId = adminUserId
            }
        );

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<Guid> { Id = householdRoleId, Name = "Household", NormalizedName = "HOUSEHOLD" },
            new IdentityRole<Guid>
                { Id = individualRoleId, Name = "IndividualCollector", NormalizedName = "INDIVIDUALCOLLECTOR" },
            new IdentityRole<Guid>
                { Id = businessRoleId, Name = "BusinessCollector", NormalizedName = "BUSINESSCOLLECTOR" }
        );

        modelBuilder.Entity<User>().HasData(
            //Email: admin@gmail.com,  Password: Admin@123
            new User
            {
                Id = adminUserId, UserName = "0900000000", Email = "admin@gmail.com", NormalizedUserName = "0900000000",
                PhoneNumber = "0900000000", PhoneNumberConfirmed = true, NormalizedEmail = "ADMIN@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==",
                FullName = "Admin GreenConnect", Status = UserStatus.Active, CreatedAt = seedDate,
                BuyerType = null
            },
            new User
            {
                Id = buyerIndiId, UserName = "0911111111", NormalizedUserName = "0911111111",
                PhoneNumber = "0911111111", PhoneNumberConfirmed = true,
                FullName = "Anh Ba Ve Chai", Status = UserStatus.PendingVerification,
                CreatedAt = seedDate,
                BuyerType = BuyerType.Individual,
                Email = "collector@gmail.com", NormalizedEmail = "COLLECTOR@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A=="
            },
            new User
            {
                Id = householdUserId, UserName = "0922222222", NormalizedUserName = "0922222222",
                PhoneNumber = "0922222222", PhoneNumberConfirmed = true,
                FullName = "Chị Tư Bán Ve Chai", Status = UserStatus.Active,
                CreatedAt = seedDate,
                BuyerType = null,
                Email = "household@gmail.com", NormalizedEmail = "HOUSEHOLD@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A=="
            },
            new User
            {
                Id = buyerBussId, UserName = "0988888888", NormalizedUserName = "0988888888",
                PhoneNumber = "0988888888", PhoneNumberConfirmed = true,
                FullName = "Vựa Ve Chai ABC", Status = UserStatus.Active,
                CreatedAt = seedDate,
                BuyerType = BuyerType.Business,
                Email = "scrapyard@gmail.com", NormalizedEmail = "SCRAPYARD@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A=="
            }
        );

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = adminUserId, RoleId = adminRoleId },
            new IdentityUserRole<Guid> { UserId = householdUserId, RoleId = householdRoleId },
            new IdentityUserRole<Guid> { UserId = buyerIndiId, RoleId = individualRoleId },
            new IdentityUserRole<Guid> { UserId = buyerBussId, RoleId = businessRoleId }
        );

        modelBuilder.Entity<Profile>().HasData(
            new Profile
            {
                ProfileId = new Guid("22222222-0000-0000-0000-000000000001"), UserId = adminUserId,
                Address = "123 Admin St, District 1, HCMC",
                PointBalance = 200, RankId = 1
            },
            new Profile
            {
                ProfileId = new Guid("22222222-0000-0000-0000-000000000002"), UserId = buyerIndiId,
                Address = "456 Collector Rd, Binh Thanh, HCMC", Gender = Gender.Male,
                PointBalance = 200, RankId = 1
            },
            new Profile
            {
                ProfileId = new Guid("22222222-0000-0000-0000-000000000003"), UserId = householdUserId,
                Address = "789 Household Ave, District 3, HCMC", Gender = Gender.Female,
                PointBalance = 200, RankId = 1
            },
            new Profile
            {
                ProfileId = new Guid("22222222-0000-0000-0000-000000000004"), UserId = buyerBussId,
                Address = "100 Vua Ve Chai St, District 10, HCMC",
                PointBalance = 6000, RankId = 2
            }
        );

        modelBuilder.Entity<CollectorVerificationInfo>().HasData(
            new CollectorVerificationInfo
            {
                UserId = buyerIndiId,
                Status = VerificationStatus.PendingReview,
                DocumentFrontUrl = "https://example.com/images/anh_ba_cccd_truoc.png",
                DocumentBackUrl = "https://example.com/images/anh_ba_cccd_sau.png",
                SubmittedAt = seedDate
            },
            new CollectorVerificationInfo
            {
                UserId = buyerBussId,
                Status = VerificationStatus.Approved,
                DocumentFrontUrl = "https://example.com/images/vua_abc_gpkd.png",
                SubmittedAt = seedDate.AddDays(-1),
                ReviewerId = adminUserId,
                ReviewedAt = seedDate,
                ReviewerNotes = "Vựa uy tín, đã xác nhận."
            }
        );

        var samplePostId = new Guid("20000000-0000-0000-0000-000000000001");
        var sampleOfferId = new Guid("30000000-0000-0000-0000-000000000001");
        var sampleTxId = new Guid("40000000-0000-0000-0000-000000000001");
        var sampleChatRoomId = new Guid("50000000-0000-0000-0000-000000000001");

        modelBuilder.Entity<ScrapPost>().HasData(
            new ScrapPost
            {
                ScrapPostId = samplePostId,
                Title = "Dọn nhà bếp, có chai nhựa và lon",
                Description = "Khoảng 1 bao lớn, đã gom sạch sẽ.",
                Address = "789 Household Ave, District 3, HCMC",
                AvailableTimeRange = "Chiều nay (14h-16h)",
                Status = PostStatus.Completed,
                CreatedAt = seedDate.AddDays(-1),
                HouseholdId = householdUserId
            }
        );

        modelBuilder.Entity<ScrapPostDetail>().HasData(
            new ScrapPostDetail
            {
                ScrapPostId = samplePostId, ScrapCategoryId = 3, AmountDescription = "Khoảng 1 bao lớn",
                Status = PostDetailStatus.Collected
            },
            new ScrapPostDetail
            {
                ScrapPostId = samplePostId, ScrapCategoryId = 4, AmountDescription = "Khoảng 50 lon",
                Status = PostDetailStatus.Collected
            }
        );

        modelBuilder.Entity<CollectionOffer>().HasData(
            new CollectionOffer
            {
                CollectionOfferId = sampleOfferId,
                ScrapPostId = samplePostId,
                ScrapCollectorId = buyerBussId,
                Status = OfferStatus.Accepted,
                CreatedAt = seedDate.AddDays(-1)
            }
        );

        modelBuilder.Entity<OfferDetail>().HasData(
            new OfferDetail
            {
                OfferDetailId = new Guid("33333333-0000-0000-0000-000000000001"),
                CollectionOfferId = sampleOfferId,
                ScrapCategoryId = 3,
                PricePerUnit = 3000,
                Unit = "kg"
            },
            new OfferDetail
            {
                OfferDetailId = new Guid("33333333-0000-0000-0000-000000000002"),
                CollectionOfferId = sampleOfferId,
                ScrapCategoryId = 4,
                PricePerUnit = 25000,
                Unit = "kg"
            }
        );

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                TransactionId = sampleTxId,
                OfferId = sampleOfferId,
                HouseholdId = householdUserId,
                ScrapCollectorId = buyerBussId,
                Status = TransactionStatus.Completed,
                ScheduledTime = seedDate.AddHours(4),
                CheckInTime = seedDate.AddHours(4),
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<TransactionDetail>().HasData(
            new TransactionDetail
            {
                TransactionId = sampleTxId,
                ScrapCategoryId = 3,
                PricePerUnit = 3000,
                Unit = "kg",
                Quantity = 5.0f,
                FinalPrice = 15000
            },
            new TransactionDetail
            {
                TransactionId = sampleTxId,
                ScrapCategoryId = 4,
                PricePerUnit = 25000,
                Unit = "kg",
                Quantity = 1.4f,
                FinalPrice = 35000
            }
        );

        modelBuilder.Entity<Feedback>().HasData(
            new Feedback
            {
                FeedbackId = new Guid("44444444-0000-0000-0000-000000000001"),
                TransactionId = sampleTxId,
                ReviewerId = householdUserId,
                RevieweeId = buyerBussId,
                Rate = 5,
                Comment = "Vựa thu gom nhanh, cân đo chính xác!",
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<ChatRoom>().HasData(
            new ChatRoom { ChatRoomId = sampleChatRoomId, TransactionId = sampleTxId, CreatedAt = seedDate }
        );
        modelBuilder.Entity<ChatParticipant>().HasData(
            new ChatParticipant { ChatRoomId = sampleChatRoomId, UserId = householdUserId },
            new ChatParticipant { ChatRoomId = sampleChatRoomId, UserId = buyerBussId }
        );
        modelBuilder.Entity<Message>().HasData(
            new Message
            {
                MessageId = new Guid("88888888-0000-0000-0000-000000000001"), ChatRoomId = sampleChatRoomId,
                SenderId = buyerBussId,
                Content = "Chào chị, em là bên Vựa ABC, em qua thu gom theo lịch hẹn nhé.",
                Timestamp = seedDate.AddHours(3)
            },
            new Message
            {
                MessageId = new Guid("88888888-0000-0000-0000-000000000002"), ChatRoomId = sampleChatRoomId,
                SenderId = householdUserId,
                Content = "OK bạn, mình ở nhà.",
                Timestamp = seedDate.AddHours(3).AddMinutes(1)
            }
        );

        modelBuilder.Entity<PointHistory>().HasData(
            new PointHistory
            {
                PointHistoryId = new Guid("55555555-0000-0000-0000-000000000001"),
                UserId = householdUserId,
                PointChange = 50,
                Reason = $"Completed Transaction #{sampleTxId.ToString().Substring(0, 8)}",
                CreatedAt = seedDate
            },
            new PointHistory
            {
                PointHistoryId = new Guid("55555555-0000-0000-0000-000000000002"),
                UserId = buyerBussId,
                PointChange = 50,
                Reason = $"Completed Transaction #{sampleTxId.ToString().Substring(0, 8)}",
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<UserPackage>().HasData(
            new UserPackage
            {
                UserPackageId = new Guid("66666666-0000-0000-0000-000000000001"),
                UserId = buyerIndiId,
                PackageId = packageFreemiumId,
                ActivationDate = seedDate,
                RemainingConnections = 5
            },
            new UserPackage
            {
                UserPackageId = new Guid("66666666-0000-0000-0000-000000000002"),
                UserId = buyerBussId,
                PackageId = packagePaidId,
                ActivationDate = seedDate,
                ExpirationDate = seedDate.AddDays(30),
                RemainingConnections = null
            }
        );

        modelBuilder.Entity<PaymentTransaction>().HasData(
            new PaymentTransaction
            {
                PaymentId = new Guid("77777777-0000-0000-0000-000000000001"),
                UserId = buyerBussId,
                PackageId = packagePaidId,
                Amount = 99000,
                PaymentGateway = "VNPay",
                TransactionCode = "VNP123456",
                Status = PaymentStatus.Success,
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<UserRewardRedemption>().HasData(
            new UserRewardRedemption
            {
                UserId = householdUserId,
                RewardItemId = 1,
                RedemptionDate = seedDate.AddHours(1)
            }
        );
        modelBuilder.Entity<PointHistory>().HasData(
            new PointHistory
            {
                PointHistoryId = new Guid("99999999-0000-0000-0000-000000000001"),
                UserId = householdUserId,
                PointChange = -100,
                Reason = "Redeemed 'Khung viền Avatar'",
                CreatedAt = seedDate.AddHours(1)
            }
        );

        var errorTxId = new Guid("40000000-0000-0000-0000-000000000002");
        var errorOfferId = new Guid("30000000-0000-0000-0000-000000000002");
        var errorPostId = new Guid("20000000-0000-0000-0000-000000000003");

        modelBuilder.Entity<ScrapPost>().HasData(new ScrapPost
        {
            ScrapPostId = errorPostId, Title = "Đồ điện tử cũ", Address = "456 Collector Rd, Binh Thanh, HCMC",
            Status = PostStatus.Completed, CreatedAt = seedDate, HouseholdId = householdUserId
        });
        modelBuilder.Entity<ScrapPostDetail>().HasData(new ScrapPostDetail
        {
            ScrapPostId = errorPostId, ScrapCategoryId = 6, AmountDescription = "1 cái TV hỏng",
            Status = PostDetailStatus.Collected
        });
        modelBuilder.Entity<CollectionOffer>().HasData(new CollectionOffer
        {
            CollectionOfferId = errorOfferId, ScrapPostId = errorPostId, ScrapCollectorId = buyerBussId,
            Status = OfferStatus.Accepted, CreatedAt = seedDate
        });
        modelBuilder.Entity<OfferDetail>().HasData(new OfferDetail
        {
            OfferDetailId = new Guid("11111111-0000-0000-0000-000000000001"), CollectionOfferId = errorOfferId,
            ScrapCategoryId = 6,
            PricePerUnit = 50000, Unit = "cái"
        });
        modelBuilder.Entity<Transaction>().HasData(new Transaction
        {
            TransactionId = errorTxId, OfferId = errorOfferId, HouseholdId = householdUserId,
            ScrapCollectorId = buyerBussId, Status = TransactionStatus.Completed, CreatedAt = seedDate
        });
        modelBuilder.Entity<TransactionDetail>().HasData(new TransactionDetail
        {
            TransactionId = errorTxId, ScrapCategoryId = 6, PricePerUnit = 50000, Unit = "cái", Quantity = 1,
            FinalPrice = 50000
        });
        modelBuilder.Entity<Complaint>().HasData(
            new Complaint
            {
                ComplaintId = new Guid("22222222-0000-0000-0000-000000000001"),
                TransactionId = errorTxId,
                ComplainantId = householdUserId,
                AccusedId = buyerBussId,
                Reason = "Vựa tới nơi trả giá ép giá, không đúng thỏa thuận!",
                Status = ComplaintStatus.Submitted,
                CreatedAt = seedDate.AddHours(2)
            }
        );
        modelBuilder.Entity<PointHistory>().HasData(
            new PointHistory
            {
                PointHistoryId = new Guid("33333333-0000-0000-0000-000000000001"),
                UserId = householdUserId,
                PointChange = -100,
                Reason = "Submitted Complaint",
                CreatedAt = seedDate.AddHours(2)
            }
        );

        modelBuilder.Entity<Notification>().HasData(
            new Notification
            {
                NotificationId = new Guid("12121212-0000-0000-0000-000000000001"),
                RecipientId = householdUserId,
                Content = "Vựa Ve Chai ABC đã gửi đề nghị thu gom cho bài đăng 'Dọn nhà bếp...'",
                IsRead = false,
                EntityType = "Offer",
                EntityId = sampleOfferId,
                CreatedAt = seedDate.AddDays(-1)
            }
        );

        modelBuilder.Entity<ScheduleProposal>().HasData(
            new ScheduleProposal
            {
                ScheduleProposalId = new Guid("13131313-0000-0000-0000-000000000001"),
                CollectionOfferId = sampleOfferId, ProposerId = buyerBussId,
                ProposedTime = seedDate.AddHours(4),
                Status = ProposalStatus.Accepted,
                CreatedAt = seedDate.AddDays(-1)
            }
        );

        #endregion
    }
}