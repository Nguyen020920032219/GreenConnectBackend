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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '7eb7d404-2b80-4073-b173-d3870c4fa3eb', TIMESTAMPTZ '2025-10-10T10:00:00Z', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM', '0900000000', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE, 'be66ffbc-9531-47ac-811b-dc47debc02d5', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '3306a149-ba9f-4233-9eda-1d499743fd80', TIMESTAMPTZ '2025-10-10T10:00:00Z', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM', '0933333333', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE, '70ad698d-3812-496b-a2e9-d6e392e8a60c', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, '49000c40-e66e-4a83-ae8e-fdc95914c428', TIMESTAMPTZ '2025-10-10T10:00:00Z', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM', '0922222222', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE, '873bdfa3-3a5f-427e-9603-adcf62652c6b', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '700a3db3-846e-4db5-98e1-71e918a49a99', TIMESTAMPTZ '2025-10-10T10:00:00Z', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM', '0988888888', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE, '9f6ead4f-1e92-4d0d-8d7b-e242a7af27f7', 'Active', FALSE, NULL, '0988888888');

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

INSERT INTO "ScrapCategories" ("ScrapCategoryId", "CategoryName", "Description")
VALUES (1, 'Giấy / Carton', NULL);
INSERT INTO "ScrapCategories" ("ScrapCategoryId", "CategoryName", "Description")
VALUES (2, 'Nhựa (Chai/Lọ)', NULL);
INSERT INTO "ScrapCategories" ("ScrapCategoryId", "CategoryName", "Description")
VALUES (3, 'Lon Nhôm', NULL);
INSERT INTO "ScrapCategories" ("ScrapCategoryId", "CategoryName", "Description")
VALUES (4, 'Sắt vụn', NULL);
INSERT INTO "ScrapCategories" ("ScrapCategoryId", "CategoryName", "Description")
VALUES (5, 'Đồ điện tử', NULL);

INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('8dd3637c-72a3-4a25-99d2-a7d1bce85542', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "AspNetUserRoles" ("RoleId", "UserId")
VALUES ('e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy", "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes", "Status", "SubmittedAt")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', TIMESTAMPTZ '1990-01-01T00:00:00Z', 'NGUYEN VAN BA', '079090000001', 'Cục Cảnh sát QLHC về TTXH', TIMESTAMPTZ '2020-05-10T00:00:00Z', 'TP.HCM', NULL, NULL, NULL, 'PendingReview', TIMESTAMPTZ '2025-10-10T10:00:00Z');
INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy", "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes", "Status", "SubmittedAt")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', NULL, 'CONG TY TNHH VUA VE CHAI ABC', '0312345678', 'Sở Kế hoạch và Đầu tư TP.HCM', TIMESTAMPTZ '2018-10-20T00:00:00Z', 'TP.HCM', TIMESTAMPTZ '2025-10-01T10:00:00Z', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 'Giấy phép kinh doanh hợp lệ.', 'Approved', TIMESTAMPTZ '2025-09-30T10:00:00Z');

INSERT INTO "Notifications" ("NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId")
VALUES ('8567ccdf-3d3e-4031-b61c-e3f9c254c45d', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMPTZ '2025-10-10T13:00:00Z', '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo")
VALUES ('7b0feabd-b131-4878-8aa4-4aacacb8e5c6', 200000.0, 'NCB', NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z', 'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('c307140c-1bdf-4635-bf98-4785e727394a', TIMESTAMPTZ '2025-10-05T10:00:00Z', -20, 'Khiếu nại', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('f1f439c4-7f28-4503-bd0a-dae370165297', TIMESTAMPTZ '2025-10-10T13:00:00Z', 10, 'Hoàn thành đơn', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('221e84d7-021d-4480-ae0f-ef5b1db71034', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL, GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('2451a5c3-bb00-4fb0-ad5b-97ceddd2687e', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male', GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('a8380770-e159-4453-8627-7450b122a1f8', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422', NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('afcac476-db96-4510-9325-a080b2d37e2e', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('137d33ff-cc8b-405d-9e51-1213d9acd4ee', TIMESTAMPTZ '2025-10-10T10:00:00Z', 3000.0, 1, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('240bad73-6d3f-484f-8b91-d6c9574141c4', TIMESTAMPTZ '2025-10-10T10:00:00Z', 15000.0, 3, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('9c9d382b-e074-4cc2-ab65-417d48bf8d7b', TIMESTAMPTZ '2025-10-10T10:00:00Z', 8000.0, 4, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('b0e314c3-2250-45bc-8582-527252ee93cc', TIMESTAMPTZ '2025-10-10T10:00:00Z', 5000.0, 2, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ScrapPosts" ("ScrapPostId", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt")
VALUES ('b0000001-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMPTZ '2025-10-08T10:00:00Z', 'Lấy hết giúp em.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TRUE, 'Completed', 'Dọn kho Giấy & Nhựa', NULL);
INSERT INTO "ScrapPosts" ("ScrapPostId", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt")
VALUES ('b0000002-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMPTZ '2025-10-10T10:00:00Z', 'Ai tiện ghé lấy.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', FALSE, 'Open', 'Bán 50 vỏ lon', NULL);

INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('52b4241d-befa-41b6-a174-f290a045a676', TIMESTAMPTZ '2025-10-10T10:00:00Z', TIMESTAMPTZ '2025-11-09T10:00:00Z', 'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('6dc11236-4804-4a46-ad0d-ec60455b246e', TIMESTAMPTZ '2025-10-10T10:00:00Z', NULL, 'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');

INSERT INTO "UserRewardRedemptions" ("RedemptionDate", "RewardItemId", "UserId")
VALUES (TIMESTAMPTZ '2025-10-10T11:00:00Z', 2, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "CollectionOffers" ("CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status")
VALUES ('f0000001-0000-0000-0000-000000000001', TIMESTAMPTZ '2025-10-09T10:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'b0000001-0000-0000-0000-000000000001', 'Accepted');

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status")
VALUES (1, 'b0000001-0000-0000-0000-000000000001', '20kg', NULL, 'Collected');
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status")
VALUES (2, 'b0000001-0000-0000-0000-000000000001', '2 bao', NULL, 'Collected');
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status")
VALUES (3, 'b0000002-0000-0000-0000-000000000001', '50 lon', NULL, 'Available');

INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('01779f89-41c6-4d0b-a7d6-fba16425a676', 'f0000001-0000-0000-0000-000000000001', 5000.0, 2, 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('6cb857b1-6969-48c5-aa93-3a8be294f11e', 'f0000001-0000-0000-0000-000000000001', 3000.0, 1, 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId", "ResponseMessage", "Status")
VALUES ('24080ccd-b28c-4b94-9198-0e8f13cb01b4', 'f0000001-0000-0000-0000-000000000001', TIMESTAMPTZ '2025-10-09T10:00:00Z', TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TIMESTAMPTZ '2025-10-10T12:00:00Z', TIMESTAMPTZ '2025-12-12T20:23:30.174854Z', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash', TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65', TIMESTAMPTZ '2025-10-09T10:00:00Z', '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId")
VALUES ('92575e54-78d8-4aac-b54a-310f15f3d16a', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-05T10:00:00Z', NULL, 'Hẹn không đến.', 'Submitted', '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('b62594b7-a4d0-4111-8f39-8d3be95f01aa', 'Nhanh gọn lẹ.', TIMESTAMPTZ '2025-10-10T13:00:00Z', 5, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (1, '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (2, '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-09T10:00:00Z');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMPTZ '2025-10-09T10:00:00Z');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('09806b11-7c7f-4f54-aa8e-7a1ca96e9841', 'dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65', 'Chào chị, em tới rồi.', TRUE, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMPTZ '2025-10-10T12:00:00Z');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('e3854ef8-4b10-448c-b94b-81edae0b3c16', 'dd6c578c-d8bf-4ff4-8fd2-3d41b531ee65', 'Ok em.', TRUE, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-10T12:01:00Z');

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
SELECT setval(
    pg_get_serial_sequence('"ScrapCategories"', 'ScrapCategoryId'),
    GREATEST(
        (SELECT MAX("ScrapCategoryId") FROM "ScrapCategories") + 1,
        nextval(pg_get_serial_sequence('"ScrapCategories"', 'ScrapCategoryId'))),
    false);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251212202330_Seeding_Database', '9.0.9');

COMMIT;

