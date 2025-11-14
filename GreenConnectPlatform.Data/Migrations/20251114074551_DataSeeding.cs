using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GreenConnectPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
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
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 0, null, "b59df1b5-8308-41f7-9688-dbca0327cb50", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", false, "Admin GreenConnect", false, null, null, "0900000000", null, null, "AQAAAAIAAYagAAAAEDmdqqVjPEcGnYT3hugAntBzyIN2dWsejrKbiqciN/LzWhifHV6aiV58TKe3h5J5Hg==", "0900000000", true, null, 1, false, null, "0900000000" },
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 0, 0, "790c3a0e-58dc-4be6-86a7-95076b94b888", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Anh Ba Ve Chai", false, null, null, "0911111111", null, null, null, "0911111111", true, null, 0, false, null, "0911111111" },
                    { new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 0, null, "71f5f4f4-1087-4ae6-af76-37875c78bc38", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Chị Tư Bán Ve Chai", false, null, null, "0922222222", null, null, null, "0922222222", true, null, 1, false, null, "0922222222" },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), 0, 1, "63040614-b36a-4108-b857-e2aaf75cfa34", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Vựa Ve Chai ABC", false, null, null, "0988888888", null, null, null, "0988888888", true, null, 1, false, null, "0988888888" }
                });

            migrationBuilder.InsertData(
                table: "PaymentPackages",
                columns: new[] { "PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType", "Price" },
                values: new object[,]
                {
                    { new Guid("f0000001-0000-0000-0000-000000000001"), 5, "Gói miễn phí cho người dùng mới trải nghiệm.", true, "Gói Dùng Thử", 0, 0m },
                    { new Guid("f0000002-0000-0000-0000-000000000002"), null, "Kết nối không giới hạn trong 30 ngày.", true, "Gói Chuyên Nghiệp (1 Tháng)", 1, 99000m }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "RankId", "BadgeImageUrl", "MinPoints", "Name" },
                values: new object[,]
                {
                    { 1, "/images/badges/bronze.png", 0, "Bronze" },
                    { 2, "/images/badges/silver.png", 5000, "Silver" },
                    { 3, "/images/badges/gold.png", 20000, "Gold" }
                });

            migrationBuilder.InsertData(
                table: "RewardItems",
                columns: new[] { "RewardItemId", "Description", "ItemName", "PointsCost" },
                values: new object[,]
                {
                    { 1, null, "Khung viền Avatar 'Chiến binh Tái chế'", 100 },
                    { 2, null, "Voucher Giảm giá 10% (fake)", 500 }
                });

            migrationBuilder.InsertData(
                table: "ScrapCategories",
                columns: new[] { "ScrapCategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Giấy vụn", null },
                    { 2, "Thùng carton", null },
                    { 3, "Chai nhựa (PET)", null },
                    { 4, "Lon nhôm", null },
                    { 5, "Sắt vụn", null },
                    { 6, "Đồ điện tử cũ", null }
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
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), "https://example.com/images/anh_ba_cccd_sau.png", "https://example.com/images/anh_ba_cccd_truoc.png", null, null, null, "PendingReview", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), null, "https://example.com/images/vua_abc_gpkd.png", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), "Vựa uy tín, đã xác nhận.", "Approved", new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId" },
                values: new object[] { new Guid("12121212-0000-0000-0000-000000000001"), "Vựa Ve Chai ABC đã gửi đề nghị thu gom cho bài đăng 'Dọn nhà bếp...'", new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "Offer", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "PaymentId", "Amount", "CreatedAt", "PackageId", "PaymentGateway", "Status", "TransactionCode", "UserId" },
                values: new object[] { new Guid("77777777-0000-0000-0000-000000000001"), 99000m, new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000002-0000-0000-0000-000000000002"), "VNPay", 1, "VNP123456", new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

            migrationBuilder.InsertData(
                table: "PointHistories",
                columns: new[] { "PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId" },
                values: new object[,]
                {
                    { new Guid("33333333-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), -100, "Submitted Complaint", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("55555555-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 50, "Completed Transaction #40000000", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("55555555-0000-0000-0000-000000000002"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 50, "Completed Transaction #40000000", new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") },
                    { new Guid("99999999-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 11, 0, 0, 0, DateTimeKind.Utc), -100, "Redeemed 'Khung viền Avatar'", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AvatarUrl", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000001"), "123 Admin St, District 1, HCMC", null, null, null, null, 200, 1, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("22222222-0000-0000-0000-000000000002"), "456 Collector Rd, Binh Thanh, HCMC", null, null, "Male", null, 200, 1, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("22222222-0000-0000-0000-000000000003"), "789 Household Ave, District 3, HCMC", null, null, "Female", null, 200, 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("22222222-0000-0000-0000-000000000004"), "100 Vua Ve Chai St, District 10, HCMC", null, null, null, null, 6000, 2, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") }
                });

            migrationBuilder.InsertData(
                table: "ReferencePrices",
                columns: new[] { "ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 3000m, 3, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 25000m, 4, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") }
                });

            migrationBuilder.InsertData(
                table: "ScrapPosts",
                columns: new[] { "ScrapPostId", "Address", "AvailableTimeRange", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "789 Household Ave, District 3, HCMC", "Chiều nay (14h-16h)", new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), "Khoảng 1 bao lớn, đã gom sạch sẽ.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), null, false, 3, "Dọn nhà bếp, có chai nhựa và lon", null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "456 Collector Rd, Binh Thanh, HCMC", null, new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), null, false, 3, "Đồ điện tử cũ", null }
                });

            migrationBuilder.InsertData(
                table: "UserPackages",
                columns: new[] { "UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId" },
                values: new object[,]
                {
                    { new Guid("66666666-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("f0000001-0000-0000-0000-000000000001"), 5, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("66666666-0000-0000-0000-000000000002"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000002-0000-0000-0000-000000000002"), null, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") }
                });

            migrationBuilder.InsertData(
                table: "UserRewardRedemptions",
                columns: new[] { "RedemptionDate", "RewardItemId", "UserId" },
                values: new object[] { new DateTime(2025, 10, 10, 11, 0, 0, 0, DateTimeKind.Utc), 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.InsertData(
                table: "CollectionOffers",
                columns: new[] { "CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("20000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("20000000-0000-0000-0000-000000000003"), 1 }
                });

            migrationBuilder.InsertData(
                table: "ScrapPostDetails",
                columns: new[] { "ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status" },
                values: new object[,]
                {
                    { 3, new Guid("20000000-0000-0000-0000-000000000001"), "Khoảng 1 bao lớn", null, "Collected" },
                    { 4, new Guid("20000000-0000-0000-0000-000000000001"), "Khoảng 50 lon", null, "Collected" },
                    { 6, new Guid("20000000-0000-0000-0000-000000000003"), "1 cái TV hỏng", null, "Collected" }
                });

            migrationBuilder.InsertData(
                table: "OfferDetail",
                columns: new[] { "OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000002"), 50000m, 6, "cái" },
                    { new Guid("33333333-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), 3000m, 3, "kg" },
                    { new Guid("33333333-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000001"), 25000m, 4, "kg" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleProposals",
                columns: new[] { "ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status" },
                values: new object[] { new Guid("13131313-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), null, "Accepted" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "ScheduledTime", "ScrapCollectorId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Completed", null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), null, new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("30000000-0000-0000-0000-000000000002"), null, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), "Completed", null }
                });

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "ChatRoomId", "CreatedAt", "TransactionId" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Complaints",
                columns: new[] { "ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId" },
                values: new object[] { new Guid("22222222-0000-0000-0000-000000000001"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, "Vựa tới nơi trả giá ép giá, không đúng thỏa thuận!", "Submitted", new Guid("40000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId" },
                values: new object[] { new Guid("44444444-0000-0000-0000-000000000001"), "Vựa thu gom nhanh, cân đo chính xác!", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "TransactionDetails",
                columns: new[] { "ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit" },
                values: new object[,]
                {
                    { 3, new Guid("40000000-0000-0000-0000-000000000001"), 15000m, 3000m, 5f, "kg" },
                    { 4, new Guid("40000000-0000-0000-0000-000000000001"), 35000m, 25000m, 1.4f, "kg" },
                    { 6, new Guid("40000000-0000-0000-0000-000000000002"), 50000m, 50000m, 1f, "cái" }
                });

            migrationBuilder.InsertData(
                table: "ChatParticipants",
                columns: new[] { "ChatRoomId", "UserId" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp" },
                values: new object[,]
                {
                    { new Guid("88888888-0000-0000-0000-000000000001"), new Guid("50000000-0000-0000-0000-000000000001"), "Chào chị, em là bên Vựa ABC, em qua thu gom theo lịch hẹn nhé.", false, new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"), new DateTime(2025, 10, 10, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("88888888-0000-0000-0000-000000000002"), new Guid("50000000-0000-0000-0000-000000000001"), "OK bạn, mình ở nhà.", false, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new DateTime(2025, 10, 10, 13, 1, 0, 0, DateTimeKind.Utc) }
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
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

            migrationBuilder.DeleteData(
                table: "ChatParticipants",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef") });

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
                keyValue: new Guid("22222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("88888888-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: new Guid("88888888-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: new Guid("12121212-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("33333333-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "OfferDetail",
                keyColumn: "OfferDetailId",
                keyValue: new Guid("33333333-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PaymentTransactions",
                keyColumn: "PaymentId",
                keyValue: new Guid("77777777-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("33333333-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PointHistories",
                keyColumn: "PointHistoryId",
                keyValue: new Guid("99999999-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("22222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("22222222-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("22222222-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: new Guid("22222222-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "RankId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ReferencePrices",
                keyColumn: "ReferencePriceId",
                keyValue: new Guid("11111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScheduleProposals",
                keyColumn: "ScheduleProposalId",
                keyValue: new Guid("13131313-0000-0000-0000-000000000001"));

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
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { 3, new Guid("20000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { 4, new Guid("20000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "ScrapPostDetails",
                keyColumns: new[] { "ScrapCategoryId", "ScrapPostId" },
                keyValues: new object[] { 6, new Guid("20000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { 3, new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { 4, new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "TransactionDetails",
                keyColumns: new[] { "ScrapCategoryId", "TransactionId" },
                keyValues: new object[] { 6, new Guid("40000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("66666666-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "UserPackages",
                keyColumn: "UserPackageId",
                keyValue: new Guid("66666666-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "UserRewardRedemptions",
                keyColumns: new[] { "RedemptionDate", "RewardItemId", "UserId" },
                keyValues: new object[] { new DateTime(2025, 10, 10, 11, 0, 0, 0, DateTimeKind.Utc), 1, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") });

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
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "ChatRooms",
                keyColumn: "ChatRoomId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PaymentPackages",
                keyColumn: "PackageId",
                keyValue: new Guid("f0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PaymentPackages",
                keyColumn: "PackageId",
                keyValue: new Guid("f0000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "RankId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "RankId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RewardItems",
                keyColumn: "RewardItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScrapCategories",
                keyColumn: "ScrapCategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "CollectionOffers",
                keyColumn: "CollectionOfferId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CollectionOffers",
                keyColumn: "CollectionOfferId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ScrapPosts",
                keyColumn: "ScrapPostId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef"));

            migrationBuilder.DeleteData(
                table: "ScrapPosts",
                keyColumn: "ScrapPostId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"));
        }
    }
}
