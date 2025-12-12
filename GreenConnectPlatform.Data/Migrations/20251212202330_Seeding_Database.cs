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
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 0, null, "7eb7d404-2b80-4073-b173-d3870c4fa3eb", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "admin@gc.com", true, "Admin System", false, null, "ADMIN@GC.COM", "0900000000", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0900000000", true, "be66ffbc-9531-47ac-811b-dc47debc02d5", "Active", false, null, "0900000000" },
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 0, "Individual", "3306a149-ba9f-4233-9eda-1d499743fd80", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "anhba@gc.com", true, "Anh Ba Ve Chai", false, null, "ANHBA@GC.COM", "0933333333", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0933333333", true, "70ad698d-3812-496b-a2e9-d6e392e8a60c", "Active", false, null, "0933333333" },
                    { new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 0, null, "49000c40-e66e-4a83-ae8e-fdc95914c428", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "chitu@gc.com", true, "Chị Tư Nội Trợ", false, null, "CHITU@GC.COM", "0922222222", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0922222222", true, "873bdfa3-3a5f-427e-9603-adcf62652c6b", "Active", false, null, "0922222222" },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), 0, "Business", "700a3db3-846e-4db5-98e1-71e918a49a99", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "vuaabc@gc.com", true, "Vựa Tái Chế ABC", false, null, "VUAABC@GC.COM", "0988888888", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0988888888", true, "9f6ead4f-1e92-4d0d-8d7b-e242a7af27f7", "Active", false, null, "0988888888" }
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
                columns: new[] { "ScrapCategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Giấy / Carton", null },
                    { 2, "Nhựa (Chai/Lọ)", null },
                    { 3, "Lon Nhôm", null },
                    { 4, "Sắt vụn", null },
                    { 5, "Đồ điện tử", null }
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
                values: new object[] { new Guid("8567ccdf-3d3e-4031-b61c-e3f9c254c45d"), "Vựa ABC đã hoàn thành đơn hàng.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001"), "Transaction", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo" },
                values: new object[] { new Guid("7b0feabd-b131-4878-8aa4-4aacacb8e5c6"), 200000m, "NCB", null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), "Mua Goi Pro", new Guid("a2222222-0000-0000-0000-000000000001"), "VNPay", "00", "Success", "ORD001", new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "VNP001" });

            migrationBuilder.InsertData(
                table: "PointHistories",
                columns: new[] { "PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId" },
                values: new object[,]
                {
                    { new Guid("c307140c-1bdf-4635-bf98-4785e727394a"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), -20, "Khiếu nại", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("f1f439c4-7f28-4503-bd0a-dae370165297"), new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 10, "Hoàn thành đơn", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId" },
                values: new object[,]
                {
                    { new Guid("221e84d7-021d-4480-ae0f-ef5b1db71034"), "Kho Quận 7, HCM", null, "CTY ABC", "0988888888", "970436", null, null, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.72 10.75)"), 5000, 2, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("2451a5c3-bb00-4fb0-ad5b-97ceddd2687e"), "Hẻm 456 Lê Văn Sỹ, Q3, HCM", null, null, null, null, null, "Male", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.68 10.78)"), 120, 1, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("a8380770-e159-4453-8627-7450b122a1f8"), "123 CMT8, Q3, HCM", null, "NGUYEN THI TU", "0922222222", "970422", null, "Female", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), 50, 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("afcac476-db96-4510-9325-a080b2d37e2e"), "Headquarter", null, null, null, null, null, null, null, 9999, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ReferencePrices",
                columns: new[] { "ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId" },
                values: new object[,]
                {
                    { new Guid("137d33ff-cc8b-405d-9e51-1213d9acd4ee"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 3000m, 1, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("240bad73-6d3f-484f-8b91-d6c9574141c4"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 15000m, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("9c9d382b-e074-4cc2-ab65-417d48bf8d7b"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 8000m, 4, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("b0e314c3-2250-45bc-8582-527252ee93cc"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5000m, 2, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ScrapPosts",
                columns: new[] { "ScrapPostId", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b0000001-0000-0000-0000-000000000001"), "123 CMT8", new DateTime(2025, 10, 8, 10, 0, 0, 0, DateTimeKind.Utc), "Lấy hết giúp em.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), true, "Completed", "Dọn kho Giấy & Nhựa", null },
                    { new Guid("b0000002-0000-0000-0000-000000000001"), "123 CMT8", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Ai tiện ghé lấy.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), false, "Open", "Bán 50 vỏ lon", null }
                });

            migrationBuilder.InsertData(
                table: "UserPackages",
                columns: new[] { "UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId" },
                values: new object[,]
                {
                    { new Guid("52b4241d-befa-41b6-a174-f290a045a676"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-0000-0000-0000-000000000001"), 499, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("6dc11236-4804-4a46-ad0d-ec60455b246e"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-0000-0000-0000-000000000001"), 5, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") }
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
                columns: new[] { "ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status" },
                values: new object[,]
                {
                    { 1, new Guid("b0000001-0000-0000-0000-000000000001"), "20kg", null, "Collected" },
                    { 2, new Guid("b0000001-0000-0000-0000-000000000001"), "2 bao", null, "Collected" },
                    { 3, new Guid("b0000002-0000-0000-0000-000000000001"), "50 lon", null, "Available" }
                });

            migrationBuilder.InsertData(
                table: "OfferDetail",
                columns: new[] { "OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit" },
                values: new object[,]
                {
                    { new Guid("01779f89-41c6-4d0b-a7d6-fba16425a676"), new Guid("f0000001-0000-0000-0000-000000000001"), 5000m, 2, "kg" },
                    { new Guid("6cb857b1-6969-48c5-aa93-3a8be294f11e"), new Guid("f0000001-0000-0000-0000-000000000001"), 3000m, 1, "kg" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleProposals",
                columns: new[] { "ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status" },
                values: new object[] { new Guid("24080ccd-b28c-4b94-9198-0e8f13cb01b4"), new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Ok chốt", "Accepted" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("70000001-0000-0000-0000-000000000001"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 12, 20, 23, 30, 174, DateTimeKind.Utc).AddTicks(8546), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), "Cash", new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Completed", 100000m, null },
                    { new Guid("70000002-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), null, null, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), "CanceledByUser", 0m, null }
                });

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "ChatRoomId", "CreatedAt", "TransactionId" },
                values: new object[] { new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Complaints",
                columns: new[] { "ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId" },
                values: new object[] { new Guid("92575e54-78d8-4aac-b54a-310f15f3d16a"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Hẹn không đến.", "Submitted", new Guid("70000002-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId" },
                values: new object[] { new Guid("b62594b7-a4d0-4111-8f39-8d3be95f01aa"), "Nhanh gọn lẹ.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "TransactionDetails",
                columns: new[] { "ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit" },
                values: new object[,]
                {
                    { 1, new Guid("70000001-0000-0000-0000-000000000001"), 45000m, 3000m, 15f, "kg" },
                    { 2, new Guid("70000001-0000-0000-0000-000000000001"), 55000m, 5000m, 11f, "kg" }
                });

            migrationBuilder.InsertData(
                table: "ChatParticipants",
                columns: new[] { "ChatRoomId", "UserId", "JoinedAt" },
                values: new object[,]
                {
                    { new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp" },
                values: new object[,]
                {
                    { new Guid("09806b11-7c7f-4f54-aa8e-7a1ca96e9841"), new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), "Chào chị, em tới rồi.", true, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e3854ef8-4b10-448c-b94b-81edae0b3c16"), new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), "Ok em.", true, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 10, 12, 1, 0, 0, DateTimeKind.Utc) }
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
                keyValues: new object[] { new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "ChatParticipants",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

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
                keyValue: new Guid("92575e54-78d8-4aac-b54a-310f15f3d16a"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: new Guid("b62594b7-a4d0-4111-8f39-8d3be95f01aa"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("09806b11-7c7f-4f54-aa8e-7a1ca96e9841"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("e3854ef8-4b10-448c-b94b-81edae0b3c16"));

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: new Guid("8567ccdf-3d3e-4031-b61c-e3f9c254c45d"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("01779f89-41c6-4d0b-a7d6-fba16425a676"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("6cb857b1-6969-48c5-aa93-3a8be294f11e"));

            migrationBuilder.DeleteData(
                table: "PaymentTransactions",
                keyColumn: "PaymentId",
                keyValue: new Guid("7b0feabd-b131-4878-8aa4-4aacacb8e5c6"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("c307140c-1bdf-4635-bf98-4785e727394a"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("f1f439c4-7f28-4503-bd0a-dae370165297"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("221e84d7-021d-4480-ae0f-ef5b1db71034"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("2451a5c3-bb00-4fb0-ad5b-97ceddd2687e"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("a8380770-e159-4453-8627-7450b122a1f8"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("afcac476-db96-4510-9325-a080b2d37e2e"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("137d33ff-cc8b-405d-9e51-1213d9acd4ee"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("240bad73-6d3f-484f-8b91-d6c9574141c4"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("9c9d382b-e074-4cc2-ab65-417d48bf8d7b"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("b0e314c3-2250-45bc-8582-527252ee93cc"));

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
                keyValue: new Guid("24080ccd-b28c-4b94-9198-0e8f13cb01b4"));

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { 1, new Guid("b0000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { 2, new Guid("b0000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { 3, new Guid("b0000002-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { 1, new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { 2, new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("52b4241d-befa-41b6-a174-f290a045a676"));

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("6dc11236-4804-4a46-ad0d-ec60455b246e"));

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
                keyValue: new Guid("dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65"));

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
                keyColumn: "ScrapCategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScrapPosts",
                keyColumn: "ScrapPostId",
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
                keyColumn: "ScrapPostId",
                keyValue: new Guid("b0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"));
        }
    }
}
