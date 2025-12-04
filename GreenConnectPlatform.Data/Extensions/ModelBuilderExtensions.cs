using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        // 1. ĐỊNH NGHĨA ID CỐ ĐỊNH (Đã sửa lại các GUID chứa ký tự không hợp lệ)
        var seedDate = new DateTime(2025, 10, 10, 10, 0, 0, DateTimeKind.Utc);
        var passwordHash =
            "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A=="; // Admin@123

        // Roles
        var adminRoleId = new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542");
        var householdRoleId = new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c");
        var individualRoleId = new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84");
        var businessRoleId = new Guid("e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a");

        // Users
        var adminId = new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff");
        var householdId = new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"); // Chị Tư
        var individualId = new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"); // Anh Ba
        var businessId = new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"); // Vựa ABC

        // --- A. ROLES & USERS ---
        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<Guid> { Id = householdRoleId, Name = "Household", NormalizedName = "HOUSEHOLD" },
            new IdentityRole<Guid>
                { Id = individualRoleId, Name = "IndividualCollector", NormalizedName = "INDIVIDUALCOLLECTOR" },
            new IdentityRole<Guid>
                { Id = businessRoleId, Name = "BusinessCollector", NormalizedName = "BUSINESSCOLLECTOR" }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = adminId, UserName = "0900000000", NormalizedUserName = "0900000000", Email = "admin@gc.com",
                NormalizedEmail = "ADMIN@GC.COM", PhoneNumber = "0900000000", PhoneNumberConfirmed = true,
                EmailConfirmed = true, PasswordHash = passwordHash, FullName = "Admin System",
                Status = UserStatus.Active, CreatedAt = seedDate, SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = householdId, UserName = "0922222222", NormalizedUserName = "0922222222", Email = "chitu@gc.com",
                NormalizedEmail = "CHITU@GC.COM", PhoneNumber = "0922222222", PhoneNumberConfirmed = true,
                EmailConfirmed = true, PasswordHash = passwordHash, FullName = "Chị Tư Nội Trợ",
                Status = UserStatus.Active, CreatedAt = seedDate, SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = individualId, UserName = "0933333333", NormalizedUserName = "0933333333", Email = "anhba@gc.com",
                NormalizedEmail = "ANHBA@GC.COM", PhoneNumber = "0933333333", PhoneNumberConfirmed = true,
                EmailConfirmed = true, PasswordHash = passwordHash, FullName = "Anh Ba Ve Chai",
                Status = UserStatus.Active, BuyerType = BuyerType.Individual, CreatedAt = seedDate,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = businessId, UserName = "0988888888", NormalizedUserName = "0988888888", Email = "vuaabc@gc.com",
                NormalizedEmail = "VUAABC@GC.COM", PhoneNumber = "0988888888", PhoneNumberConfirmed = true,
                EmailConfirmed = true, PasswordHash = passwordHash, FullName = "Vựa Tái Chế ABC",
                Status = UserStatus.Active, BuyerType = BuyerType.Business, CreatedAt = seedDate,
                SecurityStamp = Guid.NewGuid().ToString()
            }
        );

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId },
            new IdentityUserRole<Guid> { UserId = householdId, RoleId = householdRoleId },
            new IdentityUserRole<Guid> { UserId = individualId, RoleId = individualRoleId },
            new IdentityUserRole<Guid> { UserId = businessId, RoleId = businessRoleId }
        );

        // Profiles
        modelBuilder.Entity<Profile>().HasData(
            new Profile
            {
                ProfileId = Guid.NewGuid(), UserId = adminId, Address = "Headquarter", PointBalance = 9999, RankId = 3
            },
            new Profile
            {
                ProfileId = Guid.NewGuid(), UserId = householdId, Address = "123 CMT8, Q3, HCM", Gender = Gender.Female,
                PointBalance = 50, RankId = 1, BankCode = "970422", BankAccountNumber = "0922222222",
                BankAccountName = "NGUYEN THI TU", Location = new Point(106.690, 10.777) { SRID = 4326 }
            },
            new Profile
            {
                ProfileId = Guid.NewGuid(), UserId = individualId, Address = "Hẻm 456 Lê Văn Sỹ, Q3, HCM",
                Gender = Gender.Male, PointBalance = 120, RankId = 1,
                Location = new Point(106.680, 10.780) { SRID = 4326 }
            },
            new Profile
            {
                ProfileId = Guid.NewGuid(), UserId = businessId, Address = "Kho Quận 7, HCM", PointBalance = 5000,
                RankId = 2, BankCode = "970436", BankAccountNumber = "0988888888", BankAccountName = "CTY ABC",
                Location = new Point(106.720, 10.750) { SRID = 4326 }
            }
        );

        // --- B. MASTER DATA ---
        modelBuilder.Entity<Rank>().HasData(
            new Rank { RankId = 1, Name = "Mới", MinPoints = 0, BadgeImageUrl = "bronze.png" },
            new Rank { RankId = 2, Name = "Bạc", MinPoints = 1000, BadgeImageUrl = "silver.png" },
            new Rank { RankId = 3, Name = "Vàng", MinPoints = 5000, BadgeImageUrl = "gold.png" }
        );

        modelBuilder.Entity<ScrapCategory>().HasData(
            new ScrapCategory { ScrapCategoryId = 1, CategoryName = "Giấy / Carton" },
            new ScrapCategory { ScrapCategoryId = 2, CategoryName = "Nhựa (Chai/Lọ)" },
            new ScrapCategory { ScrapCategoryId = 3, CategoryName = "Lon Nhôm" },
            new ScrapCategory { ScrapCategoryId = 4, CategoryName = "Sắt vụn" },
            new ScrapCategory { ScrapCategoryId = 5, CategoryName = "Đồ điện tử" }
        );

        modelBuilder.Entity<ReferencePrice>().HasData(
            new ReferencePrice
            {
                ReferencePriceId = Guid.NewGuid(), ScrapCategoryId = 1, PricePerKg = 3000, UpdatedByAdminId = adminId,
                LastUpdated = seedDate
            },
            new ReferencePrice
            {
                ReferencePriceId = Guid.NewGuid(), ScrapCategoryId = 2, PricePerKg = 5000, UpdatedByAdminId = adminId,
                LastUpdated = seedDate
            },
            new ReferencePrice
            {
                ReferencePriceId = Guid.NewGuid(), ScrapCategoryId = 3, PricePerKg = 15000, UpdatedByAdminId = adminId,
                LastUpdated = seedDate
            },
            new ReferencePrice
            {
                ReferencePriceId = Guid.NewGuid(), ScrapCategoryId = 4, PricePerKg = 8000, UpdatedByAdminId = adminId,
                LastUpdated = seedDate
            }
        );

        // [FIXED] Sửa ID bắt đầu bằng 'a' (hợp lệ) thay vì 'p'
        var freePackId = new Guid("a1111111-0000-0000-0000-000000000001");
        var proPackId = new Guid("a2222222-0000-0000-0000-000000000001");

        modelBuilder.Entity<PaymentPackage>().HasData(
            new PaymentPackage
            {
                PackageId = freePackId, Name = "Gói Free", Description = "5 lượt/tuần", Price = 0, ConnectionAmount = 5,
                PackageType = PackageType.Freemium, IsActive = true
            },
            new PaymentPackage
            {
                PackageId = proPackId, Name = "Gói Pro", Description = "500 lượt/tháng", Price = 200000,
                ConnectionAmount = 500, PackageType = PackageType.Paid, IsActive = true
            }
        );
        
                modelBuilder.Entity<RewardItem>().HasData(
            // 1. Gói Credit lẻ
            new RewardItem 
            { 
                RewardItemId = 1, 
                ItemName = "1 Lượt Kết Nối", 
                Description = "Đổi ngay 1 lượt xem SĐT để liên hệ chủ bài đăng.", 
                PointsCost = 100, 
                Type = "Credit", 
                Value = "1", 
                ImageUrl = "https://firebasestorage.googleapis.com/.../icon_credit_1.png" 
            },
            
            // 2. Combo Credit (Tiết kiệm 10%)
            new RewardItem 
            { 
                RewardItemId = 2, 
                ItemName = "Combo 5 Lượt", 
                Description = "Gói tiết kiệm. Phù hợp cho người thu gom thường xuyên.", 
                PointsCost = 450, 
                Type = "Credit", 
                Value = "5", 
                ImageUrl = "https://firebasestorage.googleapis.com/.../icon_credit_5.png" 
            },
            
            // 3. Combo Credit lớn (Tiết kiệm 20%)
            new RewardItem 
            { 
                RewardItemId = 3, 
                ItemName = "Combo 10 Lượt", 
                Description = "Gói sỉ siêu hời. Thoải mái kết nối.", 
                PointsCost = 800, 
                Type = "Credit", 
                Value = "10", 
                ImageUrl = "https://firebasestorage.googleapis.com/.../icon_credit_10.png" 
            },

            // 4. Gói dùng thử VIP (Trải nghiệm tính năng Pro)
            // Value format: "PackageId|Days" -> ID gói Pro và số ngày sử dụng (1 ngày)
            new RewardItem 
            { 
                RewardItemId = 4, 
                ItemName = "Dùng thử VIP 1 Ngày", 
                Description = "Mở khóa không giới hạn lượt xem và tính năng Pro trong 24h.", 
                PointsCost = 2000, 
                Type = "Package", 
                Value = $"{proPackId}|1", // Link tới gói Pro đã seed ở trên
                ImageUrl = "https://firebasestorage.googleapis.com/.../icon_vip_day.png" 
            }
        );

        // --- C. POSTS & OFFERS & TRANSACTIONS ---

        // Post 1: Full-lot
        var post1Id = new Guid("b0000001-0000-0000-0000-000000000001");
        modelBuilder.Entity<ScrapPost>().HasData(
            new ScrapPost
            {
                ScrapPostId = post1Id, HouseholdId = householdId, Title = "Dọn kho Giấy & Nhựa",
                Description = "Lấy hết giúp em.", Address = "123 CMT8", Status = PostStatus.Completed,
                MustTakeAll = true, Location = new Point(106.690, 10.777) { SRID = 4326 },
                CreatedAt = seedDate.AddDays(-2)
            }
        );
        modelBuilder.Entity<ScrapPostDetail>().HasData(
            new ScrapPostDetail
            {
                ScrapPostId = post1Id, ScrapCategoryId = 1, AmountDescription = "20kg",
                Status = PostDetailStatus.Collected
            },
            new ScrapPostDetail
            {
                ScrapPostId = post1Id, ScrapCategoryId = 2, AmountDescription = "2 bao",
                Status = PostDetailStatus.Collected
            }
        );

        // Offer
        var offer1Id = new Guid("f0000001-0000-0000-0000-000000000001");
        modelBuilder.Entity<CollectionOffer>().HasData(
            new CollectionOffer
            {
                CollectionOfferId = offer1Id, ScrapPostId = post1Id, ScrapCollectorId = businessId,
                Status = OfferStatus.Accepted, CreatedAt = seedDate.AddDays(-1)
            }
        );
        modelBuilder.Entity<OfferDetail>().HasData(
            new OfferDetail
            {
                OfferDetailId = Guid.NewGuid(), CollectionOfferId = offer1Id, ScrapCategoryId = 1, PricePerUnit = 3000,
                Unit = "kg"
            },
            new OfferDetail
            {
                OfferDetailId = Guid.NewGuid(), CollectionOfferId = offer1Id, ScrapCategoryId = 2, PricePerUnit = 5000,
                Unit = "kg"
            }
        );

        // Transaction [FIXED] Sửa ID bắt đầu bằng '7' thay vì 't'
        var trans1Id = new Guid("70000001-0000-0000-0000-000000000001");

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                TransactionId = trans1Id, HouseholdId = householdId, ScrapCollectorId = businessId, OfferId = offer1Id,
                Status = TransactionStatus.Completed, PaymentMethod = TransactionPaymentMethod.Cash,
                TotalAmount = 100000,
                ScheduledTime = seedDate.AddHours(2), CheckInTime = seedDate.AddHours(2),
                CheckInLocation = new Point(106.690, 10.777) { SRID = 4326 }
            }
        );
        modelBuilder.Entity<TransactionDetail>().HasData(
            new TransactionDetail
            {
                TransactionId = trans1Id, ScrapCategoryId = 1, PricePerUnit = 3000, Unit = "kg", Quantity = 15,
                FinalPrice = 45000
            },
            new TransactionDetail
            {
                TransactionId = trans1Id, ScrapCategoryId = 2, PricePerUnit = 5000, Unit = "kg", Quantity = 11,
                FinalPrice = 55000
            }
        );

        // Post 2: Open
        var post2Id = new Guid("b0000002-0000-0000-0000-000000000001");
        modelBuilder.Entity<ScrapPost>().HasData(
            new ScrapPost
            {
                ScrapPostId = post2Id, HouseholdId = householdId, Title = "Bán 50 vỏ lon",
                Description = "Ai tiện ghé lấy.", Address = "123 CMT8", Status = PostStatus.Open,
                MustTakeAll = false, Location = new Point(106.690, 10.777) { SRID = 4326 }, CreatedAt = seedDate
            }
        );
        modelBuilder.Entity<ScrapPostDetail>().HasData(
            new ScrapPostDetail
            {
                ScrapPostId = post2Id, ScrapCategoryId = 3, AmountDescription = "50 lon",
                Status = PostDetailStatus.Available
            }
        );

        // --- D. COMMUNICATION ---
        modelBuilder.Entity<ScheduleProposal>().HasData(
            new ScheduleProposal
            {
                ScheduleProposalId = Guid.NewGuid(), CollectionOfferId = offer1Id, ProposerId = businessId,
                ProposedTime = seedDate.AddHours(2), Status = ProposalStatus.Accepted, ResponseMessage = "Ok chốt",
                CreatedAt = seedDate.AddDays(-1)
            }
        );

        var chatRoomId = Guid.NewGuid();
        modelBuilder.Entity<ChatRoom>().HasData(
            new ChatRoom { ChatRoomId = chatRoomId, TransactionId = trans1Id, CreatedAt = seedDate.AddDays(-1) }
        );

        modelBuilder.Entity<ChatParticipant>().HasData(
            new ChatParticipant { ChatRoomId = chatRoomId, UserId = householdId, JoinedAt = seedDate.AddDays(-1) },
            new ChatParticipant { ChatRoomId = chatRoomId, UserId = businessId, JoinedAt = seedDate.AddDays(-1) }
        );

        modelBuilder.Entity<Message>().HasData(
            new Message
            {
                MessageId = Guid.NewGuid(), ChatRoomId = chatRoomId, SenderId = businessId,
                Content = "Chào chị, em tới rồi.", Timestamp = seedDate.AddHours(2), IsRead = true
            },
            new Message
            {
                MessageId = Guid.NewGuid(), ChatRoomId = chatRoomId, SenderId = householdId, Content = "Ok em.",
                Timestamp = seedDate.AddHours(2).AddMinutes(1), IsRead = true
            }
        );

        modelBuilder.Entity<Notification>().HasData(
            new Notification
            {
                NotificationId = Guid.NewGuid(), RecipientId = householdId,
                Content = "Vựa ABC đã hoàn thành đơn hàng.", IsRead = false,
                EntityType = "Transaction", EntityId = trans1Id, CreatedAt = seedDate.AddHours(3)
            }
        );

        // --- E. ADVANCED ---
        modelBuilder.Entity<CollectorVerificationInfo>().HasData(
            new CollectorVerificationInfo
            {
                UserId = individualId, Status = VerificationStatus.PendingReview, DocumentFrontUrl = "front.jpg",
                DocumentBackUrl = "back.jpg", SubmittedAt = seedDate
            },
            new CollectorVerificationInfo
            {
                UserId = businessId, Status = VerificationStatus.Approved, DocumentFrontUrl = "license.jpg",
                SubmittedAt = seedDate.AddDays(-10), ReviewerId = adminId, ReviewedAt = seedDate.AddDays(-9)
            }
        );

        modelBuilder.Entity<UserPackage>().HasData(
            new UserPackage
            {
                UserPackageId = Guid.NewGuid(), UserId = individualId, PackageId = freePackId,
                ActivationDate = seedDate, RemainingConnections = 5
            },
            new UserPackage
            {
                UserPackageId = Guid.NewGuid(), UserId = businessId, PackageId = proPackId, ActivationDate = seedDate,
                ExpirationDate = seedDate.AddDays(30), RemainingConnections = 499
            }
        );

        modelBuilder.Entity<PaymentTransaction>().HasData(
            new PaymentTransaction
            {
                PaymentId = Guid.NewGuid(), UserId = businessId, PackageId = proPackId, Amount = 200000,
                PaymentGateway = "VNPay", Status = PaymentStatus.Success, TransactionRef = "ORD001",
                VnpTransactionNo = "VNP001", BankCode = "NCB", ResponseCode = "00", OrderInfo = "Mua Goi Pro",
                CreatedAt = seedDate.AddDays(-5)
            }
        );

        modelBuilder.Entity<Feedback>().HasData(
            new Feedback
            {
                FeedbackId = Guid.NewGuid(), TransactionId = trans1Id, ReviewerId = householdId,
                RevieweeId = businessId,
                Rate = 5, Comment = "Nhanh gọn lẹ.", CreatedAt = seedDate.AddHours(3)
            }
        );

        // [FIXED] Transaction lỗi cho Complaint (Sửa ID bắt đầu bằng 7)
        var transErrorId = new Guid("70000002-0000-0000-0000-000000000002");
        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                TransactionId = transErrorId, HouseholdId = householdId, ScrapCollectorId = individualId,
                OfferId = offer1Id,
                Status = TransactionStatus.CanceledByUser, CreatedAt = seedDate.AddDays(-5)
            }
        );

        modelBuilder.Entity<Complaint>().HasData(
            new Complaint
            {
                ComplaintId = Guid.NewGuid(), TransactionId = transErrorId, ComplainantId = householdId,
                AccusedId = individualId,
                Reason = "Hẹn không đến.", Status = ComplaintStatus.Submitted, CreatedAt = seedDate.AddDays(-5)
            }
        );

        modelBuilder.Entity<PointHistory>().HasData(
            new PointHistory
            {
                PointHistoryId = Guid.NewGuid(), UserId = householdId, PointChange = 10, Reason = "Hoàn thành đơn",
                CreatedAt = seedDate.AddHours(3)
            },
            new PointHistory
            {
                PointHistoryId = Guid.NewGuid(), UserId = householdId, PointChange = -20, Reason = "Khiếu nại",
                CreatedAt = seedDate.AddDays(-5)
            }
        );

        modelBuilder.Entity<UserRewardRedemption>().HasData(
            new UserRewardRedemption { UserId = householdId, RewardItemId = 2, RedemptionDate = seedDate.AddHours(1) }
        );
    }
}