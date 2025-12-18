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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '2c0a7b9b-544a-4803-9600-7a66bf5f7ab1', TIMESTAMP '2025-10-10T10:00:00', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM', '0900000000', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE, 'd461522e-a71b-47e7-9da0-90481c3c4009', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '08a2b067-1144-4c77-8765-80ac9d6fb7bb', TIMESTAMP '2025-10-10T10:00:00', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM', '0933333333', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE, '2ecaa9da-b0f0-4f08-814f-7905668b5754', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, '8c1cc2b6-7949-47c9-8d45-bb1c098b2017', TIMESTAMP '2025-10-10T10:00:00', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM', '0922222222', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE, '3fd68c43-a862-4064-a4be-447c19a94c6a', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '08cadce4-32b9-46fb-af1a-3e0519d2a267', TIMESTAMP '2025-10-10T10:00:00', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM', '0988888888', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE, '548eb8db-7a57-4a6e-b62e-db712f11e5e6', 'Active', FALSE, NULL, '0988888888');

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
VALUES ('33333333-3333-3333-3333-333333333333', NULL, 'Lon Nhôm');

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
VALUES ('8cafcf2a-6a7b-4b8d-bafa-00a0b48fec6f', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMP '2025-10-10T13:00:00', '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo")
VALUES ('ce9ecacb-3024-4ada-811e-9144805fcbf1', 200000.0, 'NCB', NULL, TIMESTAMP '2025-10-05T10:00:00', 'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('4e652fdb-e2d2-4c09-ab65-5e395acd36f8', TIMESTAMP '2025-10-05T10:00:00', -20, 'Khiếu nại', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('e66d2554-2dc4-4865-ba3b-6346fb8f6929', TIMESTAMP '2025-10-10T13:00:00', 10, 'Hoàn thành đơn', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('018a9a4a-8a9a-45c9-b90a-d6a4091ec6d9', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male', GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('04bf3108-9f3e-4a88-9080-5f24c5e91fa1', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('1e455517-ea52-4658-8c4b-8d4527aa8c11', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL, GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('49a4bc8d-8d4a-40cf-84bd-f95e69a73afe', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422', NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('087ce033-5432-4f87-bf3e-24a52c76287b', TIMESTAMP '2025-10-10T10:00:00', 1000.0, '33333333-3333-3333-3333-333333333333', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('5ffb7bb8-8246-465e-959c-e5cc6860469b', TIMESTAMP '2025-10-10T10:00:00', 3000.0, '11111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('f435b299-35bd-4778-8e4a-519749905d98', TIMESTAMP '2025-10-10T10:00:00', 5000.0, '22222222-2222-2222-2222-222222222222', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt", "UserId")
VALUES ('b0000001-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-08T10:00:00', 'Lấy hết giúp em.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TRUE, 3, 'Dọn kho Giấy & Nhựa', NULL, NULL);

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "Title", "UpdatedAt", "UserId")
VALUES ('b0000002-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-10T10:00:00', 'Ai tiện ghé lấy.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 'Bán 50 vỏ lon', NULL, NULL);

INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('b2e8bf31-e78a-48c4-b1a7-d4d6439e5962', TIMESTAMP '2025-10-10T10:00:00', NULL, 'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('bed8cee1-aa1e-4af6-9bdf-56416b2d4187', TIMESTAMP '2025-10-10T10:00:00', TIMESTAMP '2025-11-09T10:00:00', 'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "UserRewardRedemptions" ("RedemptionDate", "RewardItemId", "UserId")
VALUES (TIMESTAMP '2025-10-10T11:00:00', 2, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "CollectionOffers" ("CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status")
VALUES ('f0000001-0000-0000-0000-000000000001', TIMESTAMP '2025-10-09T10:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'b0000001-0000-0000-0000-000000000001', 'Accepted');

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1", "Status")
VALUES ('11111111-1111-1111-1111-111111111111', 'b0000001-0000-0000-0000-000000000001', '20kg', NULL, NULL, 2);
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1", "Status")
VALUES ('22222222-2222-2222-2222-222222222222', 'b0000001-0000-0000-0000-000000000001', '2 bao', NULL, NULL, 2);

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1")
VALUES ('33333333-3333-3333-3333-333333333333', 'b0000002-0000-0000-0000-000000000001', '50 lon', NULL, NULL);

INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('2b9e6d15-b854-4913-ba85-e1a450ae747f', 'f0000001-0000-0000-0000-000000000001', 3000.0, '11111111-1111-1111-1111-111111111111', 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('a5623305-1383-4e66-812d-4faac7f1010a', 'f0000001-0000-0000-0000-000000000001', 5000.0, '22222222-2222-2222-2222-222222222222', 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status")
VALUES ('4747a0d3-a5a2-4d32-91a1-0ccdb1ceec9a', 'f0000001-0000-0000-0000-000000000001', TIMESTAMP '2025-10-09T10:00:00', TIMESTAMP '2025-10-10T12:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TIMESTAMP '2025-10-10T12:00:00', TIMESTAMP '2025-12-18T15:30:04.257929', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash', TIMESTAMP '2025-10-10T12:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMP '2025-10-05T10:00:00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('071c802b-cc1f-4919-b661-47ce8817bf11', TIMESTAMP '2025-10-09T10:00:00', '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId")
VALUES ('f8315c8f-4d93-49ef-b892-db65621fa5de', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-05T10:00:00', NULL, 'Hẹn không đến.', 'Submitted', '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('290c172e-e5fe-41fd-afaa-d6253f20f8c0', 'Nhanh gọn lẹ.', TIMESTAMP '2025-10-10T13:00:00', 5, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('11111111-1111-1111-1111-111111111111', '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, NULL, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('22222222-2222-2222-2222-222222222222', '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, NULL, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('071c802b-cc1f-4919-b661-47ce8817bf11', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-09T10:00:00');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('071c802b-cc1f-4919-b661-47ce8817bf11', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-09T10:00:00');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('22c42174-d894-4438-bb58-312802f5c2f6', '071c802b-cc1f-4919-b661-47ce8817bf11', 'Ok em.', TRUE, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-10T12:01:00');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('e603e1e5-9185-4a2c-bbf1-0ff9f51b5bf1', '071c802b-cc1f-4919-b661-47ce8817bf11', 'Chào chị, em tới rồi.', TRUE, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-10T12:00:00');

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
VALUES ('20251218153004_Seeding_Database', '9.0.9');

COMMIT;

