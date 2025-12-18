using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GreenConnectPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seeding_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"), null, "Admin", "ADMIN" },
                    { new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"), null, "IndividualCollector", "INDIVIDUALCOLLECTOR" },
                    { new Guid("e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a"), null, "BusinessCollector", "BUSINESSCOLLECTOR" },
                    { new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), null, "Household", "HOUSEHOLD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 0, null, "86d73b85-3891-4ca6-9e35-abbc678d3e74", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "admin@gc.com", true, "Admin System", false, null, "ADMIN@GC.COM", "0900000000", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0900000000", true, "2129e108-fc0b-4bd7-829a-218552913ff3", "Active", false, null, "0900000000" },
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 0, "Individual", "622266f8-b7bd-4076-beb2-efe377b5ac67", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "anhba@gc.com", true, "Anh Ba Ve Chai", false, null, "ANHBA@GC.COM", "0933333333", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0933333333", true, "2caa764a-dabd-44cb-bf11-164965ffe542", "Active", false, null, "0933333333" },
                    { new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 0, null, "c183da79-9e79-48c8-b13f-9cce47006f62", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "chitu@gc.com", true, "Chị Tư Nội Trợ", false, null, "CHITU@GC.COM", "0922222222", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0922222222", true, "29afac11-a7b6-484f-a23c-142ae07d180d", "Active", false, null, "0922222222" },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), 0, "Business", "726210bf-75a5-4436-afd7-d7b55c566280", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "vuaabc@gc.com", true, "Vựa Tái Chế ABC", false, null, "VUAABC@GC.COM", "0988888888", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0988888888", true, "60c26f6f-82c6-4041-bf20-30a0c2d81c09", "Active", false, null, "0988888888" }
                });

            migrationBuilder.InsertData(
                table: "PaymentPackages",
                columns: new[] { "PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType", "Price" },
                values: new object[,]
                {
                    { new Guid("a1111111-0000-0000-0000-000000000001"), 5, "5 lượt/tuần", true, "Gói Free", "Freemium", 0m },
                    { new Guid("a2222222-0000-0000-0000-000000000001"), 500, "500 lượt/tháng", true, "Gói Pro", "Paid", 200000m }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "RankId", "BadgeImageUrl", "MinPoints", "Name" },
                values: new object[,]
                {
                    { 1, "bronze.png", 0, "Mới" },
                    { 2, "silver.png", 1000, "Bạc" },
                    { 3, "gold.png", 5000, "Vàng" }
                });

            migrationBuilder.InsertData(
                table: "RewardItems",
                columns: new[] { "RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value" },
                values: new object[,]
                {
                    { 1, "Đổi ngay 1 lượt xem SĐT để liên hệ chủ bài đăng.", "https://firebasestorage.googleapis.com/.../icon_credit_1.png", "1 Lượt Kết Nối", 100, "Credit", "1" },
                    { 2, "Gói tiết kiệm. Phù hợp cho người thu gom thường xuyên.", "https://firebasestorage.googleapis.com/.../icon_credit_5.png", "Combo 5 Lượt", 450, "Credit", "5" },
                    { 3, "Gói sỉ siêu hời. Thoải mái kết nối.", "https://firebasestorage.googleapis.com/.../icon_credit_10.png", "Combo 10 Lượt", 800, "Credit", "10" },
                    { 4, "Mở khóa không giới hạn lượt xem và tính năng Pro trong 24h.", "https://firebasestorage.googleapis.com/.../icon_vip_day.png", "Dùng thử VIP 1 Ngày", 2000, "Package", "a2222222-0000-0000-0000-000000000001|1" }
                });

            migrationBuilder.InsertData(
                table: "ScrapCategories",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Giấy vụn" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Nhựa" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") }
                });

            migrationBuilder.InsertData(
                table: "CollectorVerificationInfos",
                columns: new[] { "UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy", "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes", "Status", "SubmittedAt" },
                values: new object[,]
                {
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "NGUYEN VAN BA", "079090000001", "Cục Cảnh sát QLHC về TTXH", new DateTime(2020, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "TP.HCM", null, null, null, "PendingReview", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), null, "CONG TY TNHH VUA VE CHAI ABC", "0312345678", "Sở Kế hoạch và Đầu tư TP.HCM", new DateTime(2018, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), "TP.HCM", new DateTime(2025, 10, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), "Giấy phép kinh doanh hợp lệ.", "Approved", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId" },
                values: new object[] { new Guid("c2213c00-475c-4819-a090-cdc0b55a5875"), "Vựa ABC đã hoàn thành đơn hàng.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001"), "Transaction", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo" },
                values: new object[] { new Guid("bd2c4e8f-0bef-4c60-8647-7388f0d8aee6"), 200000m, "NCB", null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), "Mua Goi Pro", new Guid("a2222222-0000-0000-0000-000000000001"), "VNPay", "00", "Success", "ORD001", new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "VNP001" });

            migrationBuilder.InsertData(
                table: "PointHistories",
                columns: new[] { "PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId" },
                values: new object[,]
                {
                    { new Guid("1099913a-3bf6-4702-b9a1-b42fcc4f8863"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), -20, "Khiếu nại", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("7e5629ae-37f6-40fb-aca1-9cd8a5606212"), new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 10, "Hoàn thành đơn", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId" },
                values: new object[,]
                {
                    { new Guid("1588a46d-e038-4731-a870-681faba79dca"), "Hẻm 456 Lê Văn Sỹ, Q3, HCM", null, null, null, null, null, "Male", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.68 10.78)"), 120, 1, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("67c26633-7c6d-48c9-bfcd-59c559042ce8"), "Kho Quận 7, HCM", null, "CTY ABC", "0988888888", "970436", null, null, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.72 10.75)"), 5000, 2, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("8931ab5c-dab3-4143-9795-33ed73548bfe"), "123 CMT8, Q3, HCM", null, "NGUYEN THI TU", "0922222222", "970422", null, "Female", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), 50, 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("b8ca0afb-58b8-434e-b3aa-3d48728fb586"), "Headquarter", null, null, null, null, null, null, null, 9999, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ReferencePrices",
                columns: new[] { "ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId" },
                values: new object[,]
                {
                    { new Guid("a17a5fe0-cce0-4fff-a5b1-5729606bceef"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 3000m, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("d9625346-0dc9-45e1-b86e-2465fe1fd460"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5000m, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("db5333ec-1c0f-4131-9d4b-b98bd71bba02"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 1000m, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ScrapPosts",
                columns: new[] { "Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("b0000001-0000-0000-0000-000000000001"), "123 CMT8", new DateTime(2025, 10, 8, 10, 0, 0, 0, DateTimeKind.Utc), "Lấy hết giúp em.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), true, 3, "Dọn kho Giấy & Nhựa", null, null });

            migrationBuilder.InsertData(
                table: "ScrapPosts",
                columns: new[] { "Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("b0000002-0000-0000-0000-000000000001"), "123 CMT8", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Ai tiện ghé lấy.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), "Bán 50 vỏ lon", null, null });

            migrationBuilder.InsertData(
                table: "UserPackages",
                columns: new[] { "UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId" },
                values: new object[,]
                {
                    { new Guid("55a45ac4-7c17-4086-b38f-91ea0cf74a56"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-0000-0000-0000-000000000001"), 499, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("6c766bee-ddd7-43ed-acf6-5e08f767c9a2"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-0000-0000-0000-000000000001"), 5, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") }
                });

            migrationBuilder.InsertData(
                table: "UserRewardRedemptions",
                columns: new[] { "RedemptionDate", "RewardItemId", "UserId" },
                values: new object[] { new DateTime(2025, 10, 10, 11, 0, 0, 0, DateTimeKind.Utc), 2, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "CollectionOffers",
                columns: new[] { "CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status" },
                values: new object[] { new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("b0000001-0000-0000-0000-000000000001"), "Accepted" });

            migrationBuilder.InsertData(
                table: "ScrapPostDetails",
                columns: new[] { "ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1", "Status" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("b0000001-0000-0000-0000-000000000001"), "20kg", null, null, 2 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("b0000001-0000-0000-0000-000000000001"), "2 bao", null, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "ScrapPostDetails",
                columns: new[] { "ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("b0000002-0000-0000-0000-000000000001"), "50 lon", null, null });

            migrationBuilder.InsertData(
                table: "OfferDetail",
                columns: new[] { "OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit" },
                values: new object[,]
                {
                    { new Guid("27182d4e-1ca0-49f5-9482-f767966b3ba9"), new Guid("f0000001-0000-0000-0000-000000000001"), 3000m, new Guid("11111111-1111-1111-1111-111111111111"), "kg" },
                    { new Guid("b8ba387a-b2c3-4e31-a26c-766e8ce6a22a"), new Guid("f0000001-0000-0000-0000-000000000001"), 5000m, new Guid("22222222-2222-2222-2222-222222222222"), "kg" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleProposals",
                columns: new[] { "ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status" },
                values: new object[] { new Guid("f00d2454-e965-48c7-9eed-252717f64606"), new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Ok chốt", "Accepted" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("70000001-0000-0000-0000-000000000001"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 18, 10, 7, 6, 588, DateTimeKind.Utc).AddTicks(4854), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), "Cash", new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Completed", 100000m, null },
                    { new Guid("70000002-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), null, null, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), "CanceledByUser", 0m, null }
                });

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "ChatRoomId", "CreatedAt", "TransactionId" },
                values: new object[] { new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Complaints",
                columns: new[] { "ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId" },
                values: new object[] { new Guid("2d81459c-de4f-4923-96f0-5b1388dc1b6d"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Hẹn không đến.", "Submitted", new Guid("70000002-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId" },
                values: new object[] { new Guid("b0ded13b-d244-4432-bc6f-02a39964e397"), "Nhanh gọn lẹ.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "TransactionDetails",
                columns: new[] { "ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("70000001-0000-0000-0000-000000000001"), 45000m, 3000m, 15f, null, "kg" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("70000001-0000-0000-0000-000000000001"), 55000m, 5000m, 11f, null, "kg" }
                });

            migrationBuilder.InsertData(
                table: "ChatParticipants",
                columns: new[] { "ChatRoomId", "UserId", "JoinedAt" },
                values: new object[,]
                {
                    { new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp" },
                values: new object[,]
                {
                    { new Guid("26d89cd4-a0c0-47fe-b7b6-a7060544a456"), new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), "Ok em.", true, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 10, 12, 1, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f4a22bcb-359e-4540-a49b-b2972e4c15fc"), new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), "Chào chị, em tới rồi.", true, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

            migrationBuilder.DeleteData(
                table: "ChatParticipants",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "ChatParticipants",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

            migrationBuilder.DeleteData(
                table: "CollectorVerificationInfos",
                keyColumn: "UserId",
                keyValue: new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "CollectorVerificationInfos",
                keyColumn: "UserId",
                keyValue: new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"));

            migrationBuilder.DeleteData(
                table: "Complaints",
                keyColumn: "ComplaintId",
                keyValue: new Guid("2d81459c-de4f-4923-96f0-5b1388dc1b6d"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: new Guid("b0ded13b-d244-4432-bc6f-02a39964e397"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("26d89cd4-a0c0-47fe-b7b6-a7060544a456"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("f4a22bcb-359e-4540-a49b-b2972e4c15fc"));

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: new Guid("c2213c00-475c-4819-a090-cdc0b55a5875"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("27182d4e-1ca0-49f5-9482-f767966b3ba9"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("b8ba387a-b2c3-4e31-a26c-766e8ce6a22a"));

            migrationBuilder.DeleteData(
                table: "PaymentTransactions",
                keyColumn: "PaymentId",
                keyValue: new Guid("bd2c4e8f-0bef-4c60-8647-7388f0d8aee6"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("1099913a-3bf6-4702-b9a1-b42fcc4f8863"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("7e5629ae-37f6-40fb-aca1-9cd8a5606212"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("1588a46d-e038-4731-a870-681faba79dca"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("67c26633-7c6d-48c9-bfcd-59c559042ce8"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("8931ab5c-dab3-4143-9795-33ed73548bfe"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("b8ca0afb-58b8-434e-b3aa-3d48728fb586"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("a17a5fe0-cce0-4fff-a5b1-5729606bceef"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("d9625346-0dc9-45e1-b86e-2465fe1fd460"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("db5333ec-1c0f-4131-9d4b-b98bd71bba02"));

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScheduleProposals",
                keyColumn: "ScheduleProposalId",
                keyValue: new Guid("f00d2454-e965-48c7-9eed-252717f64606"));

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("b0000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("b0000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("b0000002-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("55a45ac4-7c17-4086-b38f-91ea0cf74a56"));

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("6c766bee-ddd7-43ed-acf6-5e08f767c9a2"));

            migrationBuilder.DeleteData(
                table: "UserRewardRedemptions",
                keyColumns: new[] { "RedemptionDate", "RewardItemId", "UserId" },
                keyValues: new object[] { new DateTime(2025, 10, 10, 11, 0, 0, 0, DateTimeKind.Utc), 2, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"));

            migrationBuilder.DeleteData(
                table: "ChatRooms",
                keyColumn: "ChatRoomId",
                keyValue: new Guid("f46cfc4f-e950-4a62-a5b7-ace86d35b738"));

            migrationBuilder.DeleteData(
                table: "PaymentPackages",
                keyColumn: "PackageId",
                keyValue: new Guid("a1111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PaymentPackages",
                keyColumn: "PackageId",
                keyValue: new Guid("a2222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "RankId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "RankId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "RankId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "ScrapPosts",
                keyColumn: "Id",
                keyValue: new Guid("b0000002-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("70000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("70000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CollectionOffers",
                keyColumn: "CollectionOfferId",
                keyValue: new Guid("f0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"));

            migrationBuilder.DeleteData(
                table: "ScrapPosts",
                keyColumn: "Id",
                keyValue: new Guid("b0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"));
        }
    }
}
