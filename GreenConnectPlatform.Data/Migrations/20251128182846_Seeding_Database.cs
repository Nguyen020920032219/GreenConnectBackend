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
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 0, null, "556e2670-ca01-47f2-9ddf-909986cb1302", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "admin@gc.com", true, "Admin System", false, null, "ADMIN@GC.COM", "0900000000", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0900000000", true, "c6fec58f-881e-40f1-84a0-2e62ec964c0a", "Active", false, null, "0900000000" },
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 0, "Individual", "914a7446-3263-4bd1-b7f8-1034d03fe571", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "anhba@gc.com", true, "Anh Ba Ve Chai", false, null, "ANHBA@GC.COM", "0933333333", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0933333333", true, "d9f8bace-59bd-4fa4-8be8-48912f3b495b", "Active", false, null, "0933333333" },
                    { new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 0, null, "a0853657-0360-4f05-be43-62d7b275f9d0", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "chitu@gc.com", true, "Chị Tư Nội Trợ", false, null, "CHITU@GC.COM", "0922222222", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0922222222", true, "f82d9187-0826-4d9b-91b0-6517f58505ac", "Active", false, null, "0922222222" },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), 0, "Business", "4344b6d2-b241-44fa-be78-15719c3d2b58", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "vuaabc@gc.com", true, "Vựa Tái Chế ABC", false, null, "VUAABC@GC.COM", "0988888888", null, null, "AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==", "0988888888", true, "2ef016c3-9a02-44e7-ac39-8a6d515d1697", "Active", false, null, "0988888888" }
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
                columns: new[] { "RewardItemId", "Description", "ItemName", "PointsCost" },
                values: new object[,]
                {
                    { 1, "Đổi voucher mua sắm", "Voucher GotIt 50k", 500 },
                    { 2, "Trang trí profile", "Khung Avatar Xanh", 100 }
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
                columns: new[] { "UserId", "DocumentBackUrl", "DocumentFrontUrl", "ReviewedAt", "ReviewerId", "ReviewerNotes", "Status", "SubmittedAt" },
                values: new object[,]
                {
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), "back.jpg", "front.jpg", null, null, null, "PendingReview", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), null, "license.jpg", new DateTime(2025, 10, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), null, "Approved", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId" },
                values: new object[] { new Guid("67d3a3d9-6f3d-425a-9206-abb0da701338"), "Vựa ABC đã hoàn thành đơn hàng.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001"), "Transaction", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo" },
                values: new object[] { new Guid("0c0f117d-9314-4b4f-a0ec-8d37e8f6776a"), 200000m, "NCB", null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), "Mua Goi Pro", new Guid("a2222222-0000-0000-0000-000000000001"), "VNPay", "00", "Success", "ORD001", new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "VNP001" });

            migrationBuilder.InsertData(
                table: "PointHistories",
                columns: new[] { "PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId" },
                values: new object[,]
                {
                    { new Guid("7c24f3e1-d3dc-42a3-aec1-5f2d402f5017"), new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 10, "Hoàn thành đơn", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("f40bcb2c-ee0c-4d29-9c38-f847bb44c763"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), -20, "Khiếu nại", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId" },
                values: new object[,]
                {
                    { new Guid("3702da1a-b469-4bea-98d6-4c16cc278cb6"), "Headquarter", null, null, null, null, null, null, null, 9999, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("9195079b-0b88-49a6-8589-cd6876e6060c"), "Kho Quận 7, HCM", null, "CTY ABC", "0988888888", "970436", null, null, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.72 10.75)"), 5000, 2, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("a30c93f0-c8b7-42cf-9ac2-2e2c6c93597c"), "Hẻm 456 Lê Văn Sỹ, Q3, HCM", null, null, null, null, null, "Male", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.68 10.78)"), 120, 1, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("d949b7d8-d409-47a8-83ab-57e4979ddb94"), "123 CMT8, Q3, HCM", null, "NGUYEN THI TU", "0922222222", "970422", null, "Female", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), 50, 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") }
                });

            migrationBuilder.InsertData(
                table: "ReferencePrices",
                columns: new[] { "ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId" },
                values: new object[,]
                {
                    { new Guid("5c729f0f-2cb9-416a-b7e8-46ff8b334cd5"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 3000m, 1, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("67e735f9-7524-494d-993a-6909ca744145"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 15000m, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("e2e5e70c-cd84-4ac0-b7fb-b2678276e2f2"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5000m, 2, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("ecda8dad-c98e-4505-bd66-8a2b19b0e750"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 8000m, 4, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
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
                    { new Guid("7d8dffe5-7b8e-44af-b202-c2aea3f1b656"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-0000-0000-0000-000000000001"), 499, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("ed1cf213-5e6b-4c7c-a88e-1b4592d2d2e9"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-0000-0000-0000-000000000001"), 5, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") }
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
                    { new Guid("2ee088ff-5f54-412e-8be5-34b6a243432a"), new Guid("f0000001-0000-0000-0000-000000000001"), 3000m, 1, "kg" },
                    { new Guid("b76d2d8a-b80f-4a42-82a0-d0bd7a3c41f0"), new Guid("f0000001-0000-0000-0000-000000000001"), 5000m, 2, "kg" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleProposals",
                columns: new[] { "ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status" },
                values: new object[] { new Guid("7f9385fc-2ca7-455e-a509-13b57d0ed4cb"), new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Ok chốt", "Accepted" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("70000001-0000-0000-0000-000000000001"), (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (106.69 10.777)"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 28, 18, 28, 46, 371, DateTimeKind.Utc).AddTicks(1232), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), "Cash", new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Completed", 100000m, null },
                    { new Guid("70000002-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("f0000001-0000-0000-0000-000000000001"), null, null, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), "CanceledByUser", 0m, null }
                });

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "ChatRoomId", "CreatedAt", "TransactionId" },
                values: new object[] { new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("70000001-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Complaints",
                columns: new[] { "ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId" },
                values: new object[] { new Guid("6e0ab11c-9708-482b-95b5-c324f0ff7bd7"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Hẹn không đến.", "Submitted", new Guid("70000002-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId" },
                values: new object[] { new Guid("097c877d-7931-4a60-8ee6-43e74e402134"), "Nhanh gọn lẹ.", new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("70000001-0000-0000-0000-000000000001") });

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
                    { new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp" },
                values: new object[,]
                {
                    { new Guid("4c0edb44-47e1-45b0-93c1-68d8dee3d28d"), new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), "Chào chị, em tới rồi.", true, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66afdc2b-95ec-405c-80c4-b70d159c7b5d"), new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), "Ok em.", true, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 10, 12, 1, 0, 0, DateTimeKind.Utc) }
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
                keyValues: new object[] { new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "ChatParticipants",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("63464d25-b55b-419b-a411-da4c45b636b6"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

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
                keyValue: new Guid("6e0ab11c-9708-482b-95b5-c324f0ff7bd7"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: new Guid("097c877d-7931-4a60-8ee6-43e74e402134"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("4c0edb44-47e1-45b0-93c1-68d8dee3d28d"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("66afdc2b-95ec-405c-80c4-b70d159c7b5d"));

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: new Guid("67d3a3d9-6f3d-425a-9206-abb0da701338"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("2ee088ff-5f54-412e-8be5-34b6a243432a"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("b76d2d8a-b80f-4a42-82a0-d0bd7a3c41f0"));

            migrationBuilder.DeleteData(
                table: "PaymentTransactions",
                keyColumn: "PaymentId",
                keyValue: new Guid("0c0f117d-9314-4b4f-a0ec-8d37e8f6776a"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("7c24f3e1-d3dc-42a3-aec1-5f2d402f5017"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("f40bcb2c-ee0c-4d29-9c38-f847bb44c763"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("3702da1a-b469-4bea-98d6-4c16cc278cb6"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("9195079b-0b88-49a6-8589-cd6876e6060c"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("a30c93f0-c8b7-42cf-9ac2-2e2c6c93597c"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("d949b7d8-d409-47a8-83ab-57e4979ddb94"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("5c729f0f-2cb9-416a-b7e8-46ff8b334cd5"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("67e735f9-7524-494d-993a-6909ca744145"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("e2e5e70c-cd84-4ac0-b7fb-b2678276e2f2"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("ecda8dad-c98e-4505-bd66-8a2b19b0e750"));

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScheduleProposals",
                keyColumn: "ScheduleProposalId",
                keyValue: new Guid("7f9385fc-2ca7-455e-a509-13b57d0ed4cb"));

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
                keyValue: new Guid("7d8dffe5-7b8e-44af-b202-c2aea3f1b656"));

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("ed1cf213-5e6b-4c7c-a88e-1b4592d2d2e9"));

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
                keyValue: new Guid("63464d25-b55b-419b-a411-da4c45b636b6"));

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
