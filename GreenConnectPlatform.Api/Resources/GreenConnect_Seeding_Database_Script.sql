START TRANSACTION;
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('8dd3637c-72a3-4a25-99d2-a7d1bce85542', NULL, 'Admin', 'ADMIN');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84', NULL, 'IndividualCollector', 'INDIVIDUALCOLLECTOR');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a', NULL, 'BusinessCollector', 'BUSINESSCOLLECTOR');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c', NULL, 'Household', 'HOUSEHOLD');

INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '6ed52439-554a-49c4-9d3c-a87c9ce42701', TIMESTAMP '2025-10-10T10:00:00', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM', '0900000000', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE, '61300619-6f14-4481-8687-e7e2529105b9', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '217d409e-559d-4916-86a3-1948cec508ba', TIMESTAMP '2025-10-10T10:00:00', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM', '0933333333', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE, '3217bfb7-0f8a-4c4c-8925-822959c86053', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, '8cced3d4-fd94-42d8-b8f9-56dd959af89d', TIMESTAMP '2025-10-10T10:00:00', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM', '0922222222', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE, '228afaa1-3c0d-4bec-a1c0-b24c2c4f2958', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', 'a97f4259-132c-4637-a7d2-2dc9e664bbb7', TIMESTAMP '2025-10-10T10:00:00', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM', '0988888888', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE, '755b954b-e8c7-47d5-9207-fc63b624557a', 'Active', FALSE, NULL, '0988888888');

INSERT INTO "PaymentPackages" ("PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType", "Price")
VALUES ('a1111111-0000-0000-0000-000000000001', 5, '5 lượt/tuần', TRUE, 'Gói Free', 'Freemium', 0.0);
INSERT INTO "PaymentPackages" ("PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType", "Price")
VALUES ('a2222222-0000-0000-0000-000000000001', 500, '500 lượt/tháng', TRUE, 'Gói Pro', 'Paid', 200000.0);

INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (1, 'bronze.png', 0, 'Mới');
INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (2, 'silver.png', 1000, 'Bạc');
INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (3, 'gold.png', 5000, 'Vàng');

INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (1, 'Đổi ngay 1 lượt xem SĐT để liên hệ chủ bài đăng.', 'https://firebasestorage.googleapis.com/.../icon_credit_1.png', '1 Lượt Kết Nối', 100, 'Credit', '1');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (2, 'Gói tiết kiệm. Phù hợp cho người thu gom thường xuyên.', 'https://firebasestorage.googleapis.com/.../icon_credit_5.png', 'Combo 5 Lượt', 450, 'Credit', '5');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (3, 'Gói sỉ siêu hời. Thoải mái kết nối.', 'https://firebasestorage.googleapis.com/.../icon_credit_10.png', 'Combo 10 Lượt', 800, 'Credit', '10');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (4, 'Mở khóa không giới hạn lượt xem và tính năng Pro trong 24h.', 'https://firebasestorage.googleapis.com/.../icon_vip_day.png', 'Dùng thử VIP 1 Ngày', 2000, 'Package', 'a2222222-0000-0000-0000-000000000001|1');

INSERT INTO "ScrapCategories" ("Id", "ImageUrl", "Name")
VALUES ('11111111-1111-1111-1111-111111111111', NULL, 'Giấy vụn');
INSERT INTO "ScrapCategories" ("Id", "ImageUrl", "Name")
VALUES ('22222222-2222-2222-2222-222222222222', NULL, 'Nhựa');
INSERT INTO "ScrapCategories" ("Id", "ImageUrl", "Name")
VALUES ('33333333-3333-3333-3333-333333333333', NULL, 'Lon');

INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('8dd3637c-72a3-4a25-99d2-a7d1bce85542', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy", "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes", "Status", "SubmittedAt")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', TIMESTAMP '1990-01-01T00:00:00', 'NGUYEN VAN BA', '079090000001', 'Cục Cảnh sát QLHC về TTXH', TIMESTAMP '2020-05-10T00:00:00', 'TP.HCM', NULL, NULL, NULL, 'PendingReview', TIMESTAMP '2025-10-10T10:00:00');
INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy", "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes", "Status", "SubmittedAt")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', NULL, 'CONG TY TNHH VUA VE CHAI ABC', '0312345678', 'Sở Kế hoạch và Đầu tư TP.HCM', TIMESTAMP '2018-10-20T00:00:00', 'TP.HCM', TIMESTAMP '2025-10-01T10:00:00', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 'Giấy phép kinh doanh hợp lệ.', 'Approved', TIMESTAMP '2025-09-30T10:00:00');

INSERT INTO "Notifications" ("NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId")
VALUES ('0012bbdb-0ce7-40d0-aea9-b2dc47ec4e80', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMP '2025-10-10T13:00:00', '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo")
VALUES ('04e897f6-95e9-4322-a5f9-9a1a41f13cde', 200000.0, 'NCB', NULL, TIMESTAMP '2025-10-05T10:00:00', 'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('793bb10b-de6c-4924-b4ac-39df746905af', TIMESTAMP '2025-10-05T10:00:00', -20, 'Khiếu nại', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('df1633f5-4d16-4f61-a4ea-cb2749785ca5', TIMESTAMP '2025-10-10T13:00:00', 10, 'Hoàn thành đơn', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('0feb7d69-31ac-49ee-90b4-205049034047', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('288f410d-faa4-4d71-9e02-cf6b44b47fb0', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male', GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('c30120a8-a361-42d8-a2f7-c07097338a78', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL, GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('c8012a9b-d13f-4901-aac9-6d051a12f47d', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422', NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('04ee52d3-f7a8-4d07-b767-0d82120e9f97', TIMESTAMP '2025-10-10T10:00:00', 3000.0, '11111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('44b9079a-4079-423e-a5e6-b60a08f35f8d', TIMESTAMP '2025-10-10T10:00:00', 5000.0, '22222222-2222-2222-2222-222222222222', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('989ceae0-34d6-4b3d-aa31-dad44e2c909a', TIMESTAMP '2025-10-10T10:00:00', 1000.0, '33333333-3333-3333-3333-333333333333', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt", "UserId")
VALUES ('b0000001-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-08T10:00:00', 'Lấy hết giúp em.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TRUE, 3, 'Dọn kho Giấy & Nhựa', NULL, NULL);

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "Title", "UpdatedAt", "UserId")
VALUES ('b0000002-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-10T10:00:00', 'Ai tiện ghé lấy.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 'Bán 50 vỏ lon', NULL, NULL);

INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('5eeac114-85ec-4504-b441-3351be0703f0', TIMESTAMP '2025-10-10T10:00:00', NULL, 'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('c0c47c6d-3dcd-473d-9824-801050ad5199', TIMESTAMP '2025-10-10T10:00:00', TIMESTAMP '2025-11-09T10:00:00', 'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "UserRewardRedemptions" ("RedemptionDate", "RewardItemId", "UserId")
VALUES (TIMESTAMP '2025-10-10T11:00:00', 2, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "CollectionOffers" ("CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status", "TimeSlotId")
VALUES ('f0000001-0000-0000-0000-000000000001', TIMESTAMP '2025-10-09T10:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'b0000001-0000-0000-0000-000000000001', 'Accepted', NULL);

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Quantity", "ScrapCategoryId1", "Status", "Unit")
VALUES ('11111111-1111-1111-1111-111111111111', 'b0000001-0000-0000-0000-000000000001', '20kg', NULL, 0.0, NULL, 2, 'kg');
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Quantity", "ScrapCategoryId1", "Status", "Unit")
VALUES ('22222222-2222-2222-2222-222222222222', 'b0000001-0000-0000-0000-000000000001', '2 bao', NULL, 0.0, NULL, 2, 'kg');

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('33333333-3333-3333-3333-333333333333', 'b0000002-0000-0000-0000-000000000001', '50 lon', NULL, 0.0, NULL, 'kg');

INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('2b1b6d14-a526-4ba0-a4ae-9a80cec13f2e', 'f0000001-0000-0000-0000-000000000001', 3000.0, '11111111-1111-1111-1111-111111111111', 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('d29edbb9-36aa-4072-a0fc-727d83409a85', 'f0000001-0000-0000-0000-000000000001', 5000.0, '22222222-2222-2222-2222-222222222222', 'kg');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TIMESTAMP '2025-10-10T12:00:00', TIMESTAMP '2025-12-20T20:19:19.062727', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash', TIMESTAMP '2025-10-10T12:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMP '2025-10-05T10:00:00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('cbe44683-5fec-4fb5-b0c1-1dba27e4189b', TIMESTAMP '2025-10-09T10:00:00', '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId")
VALUES ('40e9cf8d-5d44-4612-aeb5-4fb0cd57933e', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-05T10:00:00', NULL, 'Hẹn không đến.', 'Submitted', '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('fbe97709-81be-470a-9869-7cc4f11e64eb', 'Nhanh gọn lẹ.', TIMESTAMP '2025-10-10T13:00:00', 5, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('11111111-1111-1111-1111-111111111111', '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, NULL, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('22222222-2222-2222-2222-222222222222', '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, NULL, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('cbe44683-5fec-4fb5-b0c1-1dba27e4189b', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-09T10:00:00');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('cbe44683-5fec-4fb5-b0c1-1dba27e4189b', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-09T10:00:00');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('28f47733-f2d9-4428-860e-89fc20c4f246', 'cbe44683-5fec-4fb5-b0c1-1dba27e4189b', 'Ok em.', TRUE, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-10T12:01:00');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('efb31a4a-2756-468e-a6fe-77ff317fd43f', 'cbe44683-5fec-4fb5-b0c1-1dba27e4189b', 'Chào chị, em tới rồi.', TRUE, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-10T12:00:00');

SELECT setval(
    pg_get_serial_sequence('"Ranks"', 'RankId'),
    GREATEST(
        (SELECT MAX("RankId") FROM "Ranks") + 1,
        nextval(pg_get_serial_sequence('"Ranks"', 'RankId'))),
    false);
SELECT setval(
    pg_get_serial_sequence('"RewardItems"', 'RewardItemId'),
    GREATEST(
        (SELECT MAX("RewardItemId") FROM "RewardItems") + 1,
        nextval(pg_get_serial_sequence('"RewardItems"', 'RewardItemId'))),
    false);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251220201919_Seeding_Database', '9.0.9');

COMMIT;

