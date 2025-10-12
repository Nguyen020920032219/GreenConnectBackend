using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GreenConnectPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:complaint_status", "submitted,in_review,resolved,dismissed")
                .Annotation("Npgsql:Enum:item_status", "available,booked,collected")
                .Annotation("Npgsql:Enum:offer_status", "pending,accepted,rejected,canceled")
                .Annotation("Npgsql:Enum:post_status", "open,partially_booked,fully_booked,completed,canceled")
                .Annotation("Npgsql:Enum:transaction_status", "scheduled,in_progress,completed,canceled_by_system,canceled_by_user")
                .Annotation("Npgsql:Enum:user_status", "pending_verification,active,inactive,blocked");

            migrationBuilder.CreateTable(
                name: "RewardItems",
                columns: table => new
                {
                    RewardItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PointsCost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardItems", x => x.RewardItemId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapCategories",
                columns: table => new
                {
                    ScrapCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapCategories", x => x.ScrapCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    RewardPoint = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScrapPosts",
                columns: table => new
                {
                    ScrapPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AvailableTimeRange = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapPosts", x => x.ScrapPostId);
                    table.ForeignKey(
                        name: "FK_ScrapPosts_Users_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRewardRedemptions",
                columns: table => new
                {
                    RedemptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RedemptionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RewardItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewardRedemptions", x => x.RedemptionId);
                    table.ForeignKey(
                        name: "FK_UserRewardRedemptions_RewardItems_RewardItemId",
                        column: x => x.RewardItemId,
                        principalTable: "RewardItems",
                        principalColumn: "RewardItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRewardRedemptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionOffers",
                columns: table => new
                {
                    OfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScrapPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScrapCollectorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionOffers", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_CollectionOffers_ScrapPosts_ScrapPostId",
                        column: x => x.ScrapPostId,
                        principalTable: "ScrapPosts",
                        principalColumn: "ScrapPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionOffers_Users_ScrapCollectorId",
                        column: x => x.ScrapCollectorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScrapPostDetails",
                columns: table => new
                {
                    ScrapPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScrapCategoryId = table.Column<int>(type: "integer", nullable: false),
                    AmountDescription = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapPostDetails", x => new { x.ScrapPostId, x.ScrapCategoryId });
                    table.ForeignKey(
                        name: "FK_ScrapPostDetails_ScrapCategories_ScrapCategoryId",
                        column: x => x.ScrapCategoryId,
                        principalTable: "ScrapCategories",
                        principalColumn: "ScrapCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScrapPostDetails_ScrapPosts_ScrapPostId",
                        column: x => x.ScrapPostId,
                        principalTable: "ScrapPosts",
                        principalColumn: "ScrapPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CheckInSelfieUrl = table.Column<string>(type: "text", nullable: true),
                    FinalWeight = table.Column<float>(type: "real", nullable: true),
                    FinalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScrapCollectorId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_CollectionOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "CollectionOffers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_ScrapCollectorId",
                        column: x => x.ScrapCollectorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionOfferDetails",
                columns: table => new
                {
                    OfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScrapPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScrapCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionOfferDetails", x => new { x.OfferId, x.ScrapPostId, x.ScrapCategoryId });
                    table.ForeignKey(
                        name: "FK_CollectionOfferDetails_CollectionOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "CollectionOffers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionOfferDetails_ScrapPostDetails_ScrapPostId_ScrapCa~",
                        columns: x => new { x.ScrapPostId, x.ScrapCategoryId },
                        principalTable: "ScrapPostDetails",
                        principalColumns: new[] { "ScrapPostId", "ScrapCategoryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    ChatRoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.ChatRoomId);
                    table.ForeignKey(
                        name: "FK_ChatRooms_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    ComplaintId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    EvidenceUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComplainantId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccusedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.ComplaintId);
                    table.ForeignKey(
                        name: "FK_Complaints_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_Users_AccusedId",
                        column: x => x.AccusedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complaints_Users_ComplainantId",
                        column: x => x.ComplainantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RevieweeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_RevieweeId",
                        column: x => x.RevieweeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatParticipants",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatRoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatParticipants", x => new { x.UserId, x.ChatRoomId });
                    table.ForeignKey(
                        name: "FK_ChatParticipants_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "ChatRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChatRoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "ChatRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RewardItems",
                columns: new[] { "RewardItemId", "Description", "ItemName", "PointsCost" },
                values: new object[,]
                {
                    { 1, null, "Khung viền Avatar 'Chiến binh Tái chế'", 100 },
                    { 2, null, "Voucher Giảm giá 10%", 500 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"), null, "Admin", "ADMIN" },
                    { new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"), null, "ScrapCollector", "SCRAPCOLLECTOR" },
                    { new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), null, "Household", "HOUSEHOLD" }
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
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff"), 0, "8a780245-f0c6-4c0f-a99c-a3034f02d437", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Admin GreenConnect", false, null, null, "0900000000", null, "0900000000", true, null, 1, false, "0900000000" },
                    { new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 0, "df101f36-638a-4a05-8e9d-ca1fa7ea43d5", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Anh Ba Ve Chai", false, null, null, "0911111111", null, "0911111111", true, null, 1, false, "0911111111" },
                    { new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 0, "1e46f029-b862-4c0b-b1d6-bb7a36b3384c", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Chị Tư Bán Ve Chai", false, null, null, "0922222222", null, "0922222222", true, null, 1, false, "0922222222" },
                    { new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122"), 0, "00bc157a-9db5-487a-9bc6-835946d78475", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, false, "Gia đình Bác Năm", false, null, null, "0933333333", null, "0933333333", true, null, 1, false, "0933333333" }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AvatarUrl", "DateOfBirth", "Gender", "RewardPoint", "UserId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "123 Admin St, District 1, HCMC", null, null, null, 0, new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "456 Collector Rd, Binh Thanh, HCMC", null, null, "Male", 0, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "789 Household Ave, District 3, HCMC", null, null, "Female", 0, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "101 Household Way, Phu Nhuan, HCMC", null, null, null, 0, new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122") }
                });

            migrationBuilder.InsertData(
                table: "ScrapPosts",
                columns: new[] { "ScrapPostId", "Address", "AvailableTimeRange", "CreatedAt", "Description", "HouseholdId", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "789 Household Ave, District 3, HCMC", "Chiều nay (14h-16h)", new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), "Khoảng 1 bao lớn, đã gom sạch sẽ.", new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), 2, "Dọn nhà bếp, có chai nhựa và lon" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "101 Household Way, Phu Nhuan, HCMC", "Mai sáng (9h-11h)", new DateTime(2025, 10, 11, 10, 0, 0, 0, DateTimeKind.Utc), "Khoảng 3kg giấy A4 cũ, vài thùng carton.", new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122"), 2, "Giấy vụn và carton" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("8dd3637c-72a3-4a25-99d2-a7d1bce85542"), new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff") },
                    { new Guid("d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84"), new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00") },
                    { new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011") },
                    { new Guid("f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c"), new Guid("d4e5f6a1-b2c3-0011-2233-ddeeff001122") }
                });

            migrationBuilder.InsertData(
                table: "CollectionOffers",
                columns: new[] { "OfferId", "CreatedAt", "ProposedPrice", "ScrapCollectorId", "ScrapPostId", "Status" },
                values: new object[] { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), 50000m, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new Guid("20000000-0000-0000-0000-000000000001"), 1 });

            migrationBuilder.InsertData(
                table: "ScrapPostDetails",
                columns: new[] { "ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status" },
                values: new object[,]
                {
                    { 3, new Guid("20000000-0000-0000-0000-000000000001"), "Khoảng 1 bao lớn", null, 0 },
                    { 4, new Guid("20000000-0000-0000-0000-000000000001"), "Khoảng 50 lon", null, 0 },
                    { 1, new Guid("20000000-0000-0000-0000-000000000002"), "3kg giấy", null, 0 },
                    { 2, new Guid("20000000-0000-0000-0000-000000000002"), "5 thùng", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "CheckInSelfieUrl", "CheckInTime", "CreatedAt", "FinalPrice", "FinalWeight", "HouseholdId", "OfferId", "ScheduledTime", "ScrapCollectorId", "Status" },
                values: new object[] { new Guid("40000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 50000m, null, new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("30000000-0000-0000-0000-000000000001"), null, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), 2 });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), "Thu gom nhanh, thân thiện!", new DateTime(2025, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5, new Guid("b2c3d4e5-f6a1-8899-0011-bbccddeeff00"), new Guid("c3d4e5f6-a1b2-9900-1122-ccddeeff0011"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.CreateIndex(
                name: "IX_ChatParticipants_ChatRoomId",
                table: "ChatParticipants",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_TransactionId",
                table: "ChatRooms",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionOfferDetails_ScrapPostId_ScrapCategoryId",
                table: "CollectionOfferDetails",
                columns: new[] { "ScrapPostId", "ScrapCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionOffers_ScrapCollectorId",
                table: "CollectionOffers",
                column: "ScrapCollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionOffers_ScrapPostId",
                table: "CollectionOffers",
                column: "ScrapPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_AccusedId",
                table: "Complaints",
                column: "AccusedId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplainantId",
                table: "Complaints",
                column: "ComplainantId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_TransactionId",
                table: "Complaints",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_RevieweeId",
                table: "Feedbacks",
                column: "RevieweeId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReviewerId",
                table: "Feedbacks",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TransactionId",
                table: "Feedbacks",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_messages_chatroom",
                table: "Messages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScrapPostDetails_ScrapCategoryId",
                table: "ScrapPostDetails",
                column: "ScrapCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapPosts_HouseholdId",
                table: "ScrapPosts",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapPosts_Status",
                table: "ScrapPosts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_HouseholdId",
                table: "Transactions",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OfferId",
                table: "Transactions",
                column: "OfferId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ScrapCollectorId",
                table: "Transactions",
                column: "ScrapCollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRewardRedemptions_RewardItemId",
                table: "UserRewardRedemptions",
                column: "RewardItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRewardRedemptions_UserId",
                table: "UserRewardRedemptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatParticipants");

            migrationBuilder.DropTable(
                name: "CollectionOfferDetails");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRewardRedemptions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "ScrapPostDetails");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "RewardItems");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ScrapCategories");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "CollectionOffers");

            migrationBuilder.DropTable(
                name: "ScrapPosts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
