using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GreenConnectPlatform.Data.Configurations;

public class GreenConnectDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ScrapCategory> ScrapCategories { get; set; }
    public DbSet<ScrapPost> ScrapPosts { get; set; }
    public DbSet<ScrapPostDetail> ScrapPostDetails { get; set; }
    public DbSet<CollectionOffer> CollectionOffers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<RewardItem> RewardItems { get; set; }
    public DbSet<UserRewardRedemption> UserRewardRedemptions { get; set; }

    public GreenConnectDbContext(DbContextOptions<GreenConnectDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Tắt cảnh báo model dynamic nếu cần (chỉ nên bật trong dev)
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region PostgreSQL ENUM Mapping

        if (Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
        {
            builder.HasPostgresEnum<UserStatus>();
            builder.HasPostgresEnum<PostStatus>();
            builder.HasPostgresEnum<ItemStatus>();
            builder.HasPostgresEnum<OfferStatus>();
            builder.HasPostgresEnum<TransactionStatus>();
            builder.HasPostgresEnum<ComplaintStatus>();
        }

        #endregion

        #region Identity Table Renaming

        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        #endregion

        #region Entity Configurations

        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FullName).HasMaxLength(100);
            entity.HasIndex(u => u.PhoneNumber).IsUnique();
        });

        builder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasKey(p => p.ProfileId);
            entity.Property(p => p.Address).HasMaxLength(255);
            entity.Property(p => p.Gender).HasMaxLength(10);
            entity.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ScrapCategory>(entity =>
        {
            entity.ToTable("ScrapCategories");
            entity.HasKey(sc => sc.ScrapCategoryId);
            entity.Property(sc => sc.CategoryName).HasMaxLength(100).IsRequired();
        });

        builder.Entity<ScrapPost>(entity =>
        {
            entity.ToTable("ScrapPosts");
            entity.HasKey(p => p.ScrapPostId);
            entity.Property(p => p.Title).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Address).HasMaxLength(255).IsRequired();
            entity.Property(p => p.AvailableTimeRange).HasMaxLength(100);
            entity.HasIndex(p => p.Status);
            entity.HasOne(p => p.Household)
                .WithMany(u => u.CreatedScrapPosts)
                .HasForeignKey(p => p.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ScrapPostDetail>(entity =>
        {
            entity.ToTable("ScrapPostDetails");
            entity.HasKey(d => new { d.ScrapPostId, d.ScrapCategoryId });
            entity.Property(d => d.AmountDescription).HasMaxLength(100);
        });

        builder.Entity<CollectionOffer>(entity =>
        {
            entity.ToTable("CollectionOffers");
            entity.HasKey(o => o.OfferId);
            entity.Property(o => o.ProposedPrice).HasColumnType("decimal(18, 2)").IsRequired();
            entity.HasMany(o => o.OfferedItems)
                .WithMany(d => d.Offers)
                .UsingEntity("CollectionOfferDetails",
                    l => l.HasOne(typeof(ScrapPostDetail)).WithMany().HasForeignKey("ScrapPostId", "ScrapCategoryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    r => r.HasOne(typeof(CollectionOffer)).WithMany().HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade));
        });

        builder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(t => t.TransactionId);
            entity.Property(t => t.FinalPrice).HasColumnType("decimal(18, 2)");
            entity.HasIndex(t => t.HouseholdId);
            entity.HasIndex(t => t.ScrapCollectorId);
            entity.HasOne(t => t.Offer)
                .WithOne(o => o.Transaction)
                .HasForeignKey<Transaction>(t => t.OfferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.Household)
                .WithMany(u => u.TransactionsAsHousehold)
                .HasForeignKey(t => t.HouseholdId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.ScrapCollector)
                .WithMany(u => u.TransactionsAsCollector)
                .HasForeignKey(t => t.ScrapCollectorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedbacks");
            entity.HasKey(f => f.FeedbackId);
            entity.Property(f => f.Rate).IsRequired();
            entity.HasOne(f => f.Transaction)
                .WithOne(t => t.Feedback)
                .HasForeignKey<Feedback>(f => f.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(f => f.Reviewer)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(f => f.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(f => f.Reviewee)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(f => f.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Complaint>(entity =>
        {
            entity.ToTable("Complaints");
            entity.HasKey(c => c.ComplaintId);
            entity.Property(c => c.Reason).IsRequired();
            entity.HasOne(c => c.Complainant)
                .WithMany(u => u.ComplaintsFiled)
                .HasForeignKey(c => c.ComplainantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.Accused)
                .WithMany(u => u.ComplaintsAgainst)
                .HasForeignKey(c => c.AccusedId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.Transaction)
                .WithMany(t => t.Complaints)
                .HasForeignKey(c => c.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ChatRoom>(entity =>
        {
            entity.ToTable("ChatRooms");
            entity.HasKey(cr => cr.ChatRoomId);
            entity.HasOne(cr => cr.Transaction)
                .WithOne(t => t.ChatRoom)
                .HasForeignKey<ChatRoom>(cr => cr.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages");
            entity.HasKey(m => m.MessageId);
            entity.Property(m => m.Content).IsRequired();
            entity.HasIndex(m => m.ChatRoomId);
            entity.HasOne(m => m.ChatRoom)
                .WithMany(cr => cr.Messages)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ChatParticipant>(entity =>
        {
            entity.ToTable("ChatParticipants");
            entity.HasKey(cp => new { cp.UserId, cp.ChatRoomId });
            entity.HasOne(cp => cp.User)
                .WithMany(u => u.ChatRooms)
                .HasForeignKey(cp => cp.UserId);
            entity.HasOne(cp => cp.ChatRoom)
                .WithMany(cr => cr.Participants)
                .HasForeignKey(cp => cp.ChatRoomId);
        });

        builder.Entity<RewardItem>(entity =>
        {
            entity.ToTable("RewardItems");
            entity.HasKey(ri => ri.RewardItemId);
            entity.Property(ri => ri.ItemName).HasMaxLength(150).IsRequired();
            entity.Property(ri => ri.PointsCost).IsRequired();
        });

        builder.Entity<UserRewardRedemption>(entity =>
        {
            entity.ToTable("UserRewardRedemptions");
            entity.HasKey(urr => urr.RedemptionId);
            entity.HasOne(urr => urr.User)
                .WithMany(u => u.RewardRedemptions)
                .HasForeignKey(urr => urr.UserId);
            entity.HasOne(urr => urr.RewardItem)
                .WithMany(ri => ri.Redemptions)
                .HasForeignKey(urr => urr.RewardItemId);
        });

        #endregion

        #region Data Seeding

        builder.Entity<ScrapCategory>().HasData(
            new ScrapCategory { ScrapCategoryId = 1, CategoryName = "Giấy vụn" },
            new ScrapCategory { ScrapCategoryId = 2, CategoryName = "Thùng carton" },
            new ScrapCategory { ScrapCategoryId = 3, CategoryName = "Chai nhựa (PET)" },
            new ScrapCategory { ScrapCategoryId = 4, CategoryName = "Lon nhôm" },
            new ScrapCategory { ScrapCategoryId = 5, CategoryName = "Sắt vụn" },
            new ScrapCategory { ScrapCategoryId = 6, CategoryName = "Đồ điện tử cũ" }
        );

        builder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
                { Id = new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"), Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<Guid>
            {
                Id = new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"), Name = "ScrapCollector",
                NormalizedName = "SCRAPCOLLECTOR"
            },
            new IdentityRole<Guid>
            {
                Id = new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), Name = "Household", NormalizedName = "HOUSEHOLD"
            }
        );

        var seedDate = new DateTime(2025, 10, 10, 10, 0, 0, DateTimeKind.Utc);

        builder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), UserName = "0900000000",
                NormalizedUserName = "0900000000", PhoneNumber = "0900000000", PhoneNumberConfirmed = true,
                FullName = "Admin GreenConnect", Status = UserStatus.Active, CreatedAt = seedDate
            },
            new User
            {
                Id = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), UserName = "0911111111",
                NormalizedUserName = "0911111111", PhoneNumber = "0911111111", PhoneNumberConfirmed = true,
                FullName = "Anh Ba Ve Chai", Status = UserStatus.Active, CreatedAt = seedDate
            },
            new User
            {
                Id = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), UserName = "0922222222",
                NormalizedUserName = "0922222222", PhoneNumber = "0922222222", PhoneNumberConfirmed = true,
                FullName = "Chị Tư Bán Ve Chai", Status = UserStatus.Active, CreatedAt = seedDate
            },
            new User
            {
                Id = new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122"), UserName = "0933333333",
                NormalizedUserName = "0933333333", PhoneNumber = "0933333333", PhoneNumberConfirmed = true,
                FullName = "Gia đình Bác Năm", Status = UserStatus.Active, CreatedAt = seedDate
            }
        );

        builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                UserId = new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"),
                RoleId = new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542")
            },
            new IdentityUserRole<Guid>
            {
                UserId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"),
                RoleId = new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84")
            },
            new IdentityUserRole<Guid>
            {
                UserId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"),
                RoleId = new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c")
            },
            new IdentityUserRole<Guid>
            {
                UserId = new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122"),
                RoleId = new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c")
            }
        );

        builder.Entity<Profile>().HasData(
            new Profile
            {
                ProfileId = new Guid("10000000-0000-0000-0000-000000000001"),
                UserId = new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), Address = "123 Admin St, District 1, HCMC"
            },
            new Profile
            {
                ProfileId = new Guid("10000000-0000-0000-0000-000000000002"),
                UserId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"),
                Address = "456 Collector Rd, Binh Thanh, HCMC", Gender = "Male"
            },
            new Profile
            {
                ProfileId = new Guid("10000000-0000-0000-0000-000000000003"),
                UserId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"),
                Address = "789 Household Ave, District 3, HCMC", Gender = "Female"
            },
            new Profile
            {
                ProfileId = new Guid("10000000-0000-0000-0000-000000000004"),
                UserId = new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122"),
                Address = "101 Household Way, Phu Nhuan, HCMC"
            }
        );

        builder.Entity<RewardItem>().HasData(
            new RewardItem { RewardItemId = 1, ItemName = "Khung viền Avatar 'Chiến binh Tái chế'", PointsCost = 100 },
            new RewardItem { RewardItemId = 2, ItemName = "Voucher Giảm giá 10%", PointsCost = 500 }
        );

        builder.Entity<ScrapPost>().HasData(
            new ScrapPost
            {
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000001"),
                Title = "Dọn nhà bếp, có chai nhựa và lon",
                Description = "Khoảng 1 bao lớn, đã gom sạch sẽ.",
                Address = "789 Household Ave, District 3, HCMC",
                AvailableTimeRange = "Chiều nay (14h-16h)",
                Status = PostStatus.FullyBooked,
                CreatedAt = new DateTime(2025, 10, 9, 10, 0, 0, DateTimeKind.Utc),
                HouseholdId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011")
            },
            new ScrapPost
            {
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000002"),
                Title = "Giấy vụn và carton",
                Description = "Khoảng 3kg giấy A4 cũ, vài thùng carton.",
                Address = "101 Household Way, Phu Nhuan, HCMC",
                AvailableTimeRange = "Mai sáng (9h-11h)",
                Status = PostStatus.FullyBooked,
                CreatedAt = new DateTime(2025, 10, 11, 10, 0, 0, DateTimeKind.Utc),
                HouseholdId = new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122")
            }
        );

        builder.Entity<ScrapPostDetail>().HasData(
            new ScrapPostDetail
            {
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000001"),
                ScrapCategoryId = 3,
                AmountDescription = "Khoảng 1 bao lớn"
            },
            new ScrapPostDetail
            {
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000001"),
                ScrapCategoryId = 4,
                AmountDescription = "Khoảng 50 lon"
            },
            new ScrapPostDetail
            {
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000002"),
                ScrapCategoryId = 1,
                AmountDescription = "3kg giấy"
            },
            new ScrapPostDetail
            {
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000002"),
                ScrapCategoryId = 2,
                AmountDescription = "5 thùng"
            }
        );

        builder.Entity<CollectionOffer>().HasData(
            new CollectionOffer
            {
                OfferId = new Guid("30000000-0000-0000-0000-000000000001"),
                ScrapPostId = new Guid("20000000-0000-0000-0000-000000000001"),
                ScrapCollectorId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"),
                ProposedPrice = 50000,
                Status = OfferStatus.Accepted,
                CreatedAt = new DateTime(2025, 10, 9, 10, 0, 0, DateTimeKind.Utc)
            }
        );

        builder.Entity<Transaction>().HasData(
            new Transaction
            {
                TransactionId = new Guid("40000000-0000-0000-0000-000000000001"),
                OfferId = new Guid("30000000-0000-0000-0000-000000000001"),
                HouseholdId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"),
                ScrapCollectorId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"),
                FinalPrice = 50000,
                Status = TransactionStatus.Completed,
                CreatedAt = new DateTime(2025, 10, 10, 10, 0, 0, DateTimeKind.Utc)
            }
        );

        builder.Entity<Feedback>().HasData(
            new Feedback
            {
                FeedbackId = new Guid("50000000-0000-0000-0000-000000000001"),
                TransactionId = new Guid("40000000-0000-0000-0000-000000000001"),
                ReviewerId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"),
                RevieweeId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"),
                Rate = 5,
                Comment = "Thu gom nhanh, thân thiện!",
                CreatedAt = new DateTime(2025, 10, 10, 10, 0, 0, DateTimeKind.Utc)
            }
        );

        #endregion
    }
}