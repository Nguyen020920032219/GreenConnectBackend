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
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 0, null, "fdf08af5-9ce5-4d6e-920e-8b41e88d1c6b", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "admin@gc.com", true, "Admin System", false, null, "ADMIN@GC.COM", "0900000000", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0900000000", true, "97b2279c-b636-4bee-974d-d2e2d466179a", "Active", false, null, "0900000000" },
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 0, "Individual", "5958c94f-4c83-4d40-be0f-9f4e0c891d06", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "anhba@gc.com", true, "Anh Ba Ve Chai", false, null, "ANHBA@GC.COM", "0933333333", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0933333333", true, "245da196-cbea-4e34-a751-47131b212434", "Active", false, null, "0933333333" },
                    { new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 0, null, "6d330eb9-648b-4e4e-865d-14888c664ac8", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "chitu@gc.com", true, "Chị Tư Nội Trợ", false, null, "CHITU@GC.COM", "0922222222", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0922222222", true, "ef80a1d1-1d7f-45f3-8069-4003e7a7fafc", "Active", false, null, "0922222222" },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), 0, "Business", "873b01a7-9d5c-464a-984a-a8eca544e4bb", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "vuaabc@gc.com", true, "Vựa Tái Chế ABC", false, null, "VUAABC@GC.COM", "0988888888", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0988888888", true, "c77d1192-4dd4-41e2-837e-dc9d79248431", "Active", false, null, "0988888888" }
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
                values: new object[] { new Guid("a491f60c-b552-4e2f-9e0f-5591679d9586"), "Vựa ABC đã hoàn thành đơn hàng.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001"), "Transaction", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo" },
                values: new object[] { new Guid("fa532179-a2ff-4b81-99a5-65990f0c74e8"), 200000m, "NCB", null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), "Mua Goi Pro", new Guid("a2222222-0000-0000-0000-000000000001"), "VNPay", "00", "Success", "ORD001", new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "VNP001" });

            migrationBuilder.InsertData(
                table: "PointHistories",
                columns: new[] { "PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId" },
                values: new object[,]
                {
                    { new Guid("05e0131e-6254-4401-a01a-62704e8a97ea"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), -20, "Khiếu nại", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("c6e421bb-9aae-494a-a3a4-f26c10c06d5b"), new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 10, "Hoàn thành đơn", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId" },
                values: new object[,]
                {
                    { new Guid("03a7eae2-9222-423c-92bf-2a6c393df922"), "Kho Quận 7, HCM", null, "CTY ABC", "0988888888", "970436", null, null, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.72 10.75)"), 5000, 2, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("5bc5a760-22be-4c69-a2ac-3ad751ded537"), "Hẻm 456 Lê Văn Sỹ, Q3, HCM", null, null, null, null, null, "Male", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.68 10.78)"), 120, 1, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("735f3054-5e66-4d6c-acee-3ff798776cfc"), "123 CMT8, Q3, HCM", null, "NGUYEN THI TU", "0922222222", "970422", null, "Female", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), 50, 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("b6b12589-a56b-4eea-8395-3301b573d365"), "Headquarter", null, null, null, null, null, null, null, 9999, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ReferencePrices",
                columns: new[] { "ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId" },
                values: new object[,]
                {
                    { new Guid("33312e81-bb29-4714-b9e9-868260820fb7"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 3000m, 1, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("3dc8324c-fc0c-4ed8-a2e8-ffe899b4ea87"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 8000m, 4, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("9cb2c26f-d837-4466-ac59-d8d3b098fb6b"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5000m, 2, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("ed0fdf46-0ae8-46df-b506-c307d3b190de"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 15000m, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ScrapPosts",
                columns: new[] { "ScrapPostId", "Address", "AvailableTimeRange", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b0000001-0000-0000-0000-000000000001"), "123 CMT8", null, new DateTime(2025, 10, 8, 10, 0, 0, 0, DateTimeKind.Utc), "Lấy hết giúp em.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), true, "Completed", "Dọn kho Giấy & Nhựa", null },
                    { new Guid("b0000002-0000-0000-0000-000000000001"), "123 CMT8", null, new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Ai tiện ghé lấy.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), false, "Open", "Bán 50 vỏ lon", null }
                });

            migrationBuilder.InsertData(
                table: "UserPackages",
                columns: new[] { "UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId" },
                values: new object[,]
                {
                    { new Guid("8d813450-e585-4ad2-8cae-5ba6fcfe1f58"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-0000-0000-0000-000000000001"), 5, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("f1162c19-279f-4328-b57b-c969a3680438"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-0000-0000-0000-000000000001"), 499, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") }
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
                    { new Guid("5750de15-7d2a-4843-a19d-38a51e20c50b"), new Guid("f0000001-0000-0000-0000-000000000001"), 5000m, 2, "kg" },
                    { new Guid("bf87ccb4-5354-4922-8e1a-5a42fc0c0bc6"), new Guid("f0000001-0000-0000-0000-000000000001"), 3000m, 1, "kg" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleProposals",
                columns: new[] { "ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status" },
                values: new object[] { new Guid("6d5e86d0-a338-417d-84d9-91167cb57df6"), new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Ok chốt", "Accepted" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("70000001-0000-0000-0000-000000000001"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 5, 18, 21, 46, 298, DateTimeKind.Utc).AddTicks(9468), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), "Cash", new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Completed", 100000m, null },
                    { new Guid("70000002-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), null, null, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), "CanceledByUser", 0m, null }
                });

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "ChatRoomId", "CreatedAt", "TransactionId" },
                values: new object[] { new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Complaints",
                columns: new[] { "ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId" },
                values: new object[] { new Guid("ca1d610f-1c1a-435e-a9e4-e1370cfa0bff"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Hẹn không đến.", "Submitted", new Guid("70000002-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId" },
                values: new object[] { new Guid("13996aef-c86c-422a-aa3b-eece73b13513"), "Nhanh gọn lẹ.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("70000001-0000-0000-0000-000000000001") });

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
                    { new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp" },
                values: new object[,]
                {
                    { new Guid("1766e59d-a31b-4063-84aa-de32c33dd06d"), new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), "Chào chị, em tới rồi.", true, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3e460c78-f259-4d80-b300-8b228c3354b5"), new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), "Ok em.", true, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 10, 12, 1, 0, 0, DateTimeKind.Utc) }
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
                keyValues: new object[] { new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "ChatParticipants",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

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
                keyValue: new Guid("ca1d610f-1c1a-435e-a9e4-e1370cfa0bff"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: new Guid("13996aef-c86c-422a-aa3b-eece73b13513"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("1766e59d-a31b-4063-84aa-de32c33dd06d"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("3e460c78-f259-4d80-b300-8b228c3354b5"));

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: new Guid("a491f60c-b552-4e2f-9e0f-5591679d9586"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("5750de15-7d2a-4843-a19d-38a51e20c50b"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("bf87ccb4-5354-4922-8e1a-5a42fc0c0bc6"));

            migrationBuilder.DeleteData(
                table: "PaymentTransactions",
                keyColumn: "PaymentId",
                keyValue: new Guid("fa532179-a2ff-4b81-99a5-65990f0c74e8"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("05e0131e-6254-4401-a01a-62704e8a97ea"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("c6e421bb-9aae-494a-a3a4-f26c10c06d5b"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("03a7eae2-9222-423c-92bf-2a6c393df922"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("5bc5a760-22be-4c69-a2ac-3ad751ded537"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("735f3054-5e66-4d6c-acee-3ff798776cfc"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("b6b12589-a56b-4eea-8395-3301b573d365"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("33312e81-bb29-4714-b9e9-868260820fb7"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("3dc8324c-fc0c-4ed8-a2e8-ffe899b4ea87"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("9cb2c26f-d837-4466-ac59-d8d3b098fb6b"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("ed0fdf46-0ae8-46df-b506-c307d3b190de"));

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
                keyValue: new Guid("6d5e86d0-a338-417d-84d9-91167cb57df6"));

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
                keyValue: new Guid("8d813450-e585-4ad2-8cae-5ba6fcfe1f58"));

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("f1162c19-279f-4328-b57b-c969a3680438"));

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
                keyValue: new Guid("2d68c33d-bd2e-4010-8aa1-1862301f5bde"));

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
