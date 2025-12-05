START TRANSACTION;
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('8dd3637c-72a3-4a25-99d2-a7d1bce85542', NULL, 'Admin', 'ADMIN');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('d7d0c75c-9c3f-4e6b-9b7a-8f8d9a6c9e84', NULL, 'IndividualCollector', 'INDIVIDUALCOLLECTOR');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('e0a5a415-5a4e-4f6a-8b9a-1b2c3d4e5f6a', NULL, 'BusinessCollector', 'BUSINESSCOLLECTOR');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('f9e7c1b5-9c8f-4b1a-8c7d-6e5f4a3b2a1c', NULL, 'Household', 'HOUSEHOLD');

INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, 'fdf08af5-9ce5-4d6e-920e-8b41e88d1c6b',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM',
        '0900000000', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE,
        '97b2279c-b636-4bee-974d-d2e2d466179a', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '5958c94f-4c83-4d40-be0f-9f4e0c891d06',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM',
        '0933333333', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE,
        '245da196-cbea-4e34-a751-47131b212434', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, '6d330eb9-648b-4e4e-865d-14888c664ac8',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM',
        '0922222222', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE,
        'ef80a1d1-1d7f-45f3-8069-4003e7a7fafc', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '873b01a7-9d5c-464a-984a-a8eca544e4bb',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM',
        '0988888888', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE,
        'c77d1192-4dd4-41e2-837e-dc9d79248431', 'Active', FALSE, NULL, '0988888888');

INSERT INTO "PaymentPackages" ("PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType",
                               "Price")
VALUES ('a1111111-0000-0000-0000-000000000001', 5, '5 lượt/tuần', TRUE, 'Gói Free', 'Freemium', 0.0);
INSERT INTO "PaymentPackages" ("PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType",
                               "Price")
VALUES ('a2222222-0000-0000-0000-000000000001', 500, '500 lượt/tháng', TRUE, 'Gói Pro', 'Paid', 200000.0);

INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (1, 'bronze.png', 0, 'Mới');
INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (2, 'silver.png', 1000, 'Bạc');
INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (3, 'gold.png', 5000, 'Vàng');

INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (1, 'Đổi ngay 1 lượt xem SĐT để liên hệ chủ bài đăng.',
        'https://firebasestorage.googleapis.com/.../icon_credit_1.png', '1 Lượt Kết Nối', 100, 'Credit', '1');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (2, 'Gói tiết kiệm. Phù hợp cho người thu gom thường xuyên.',
        'https://firebasestorage.googleapis.com/.../icon_credit_5.png', 'Combo 5 Lượt', 450, 'Credit', '5');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (3, 'Gói sỉ siêu hời. Thoải mái kết nối.', 'https://firebasestorage.googleapis.com/.../icon_credit_10.png',
        'Combo 10 Lượt', 800, 'Credit', '10');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (4, 'Mở khóa không giới hạn lượt xem và tính năng Pro trong 24h.',
        'https://firebasestorage.googleapis.com/.../icon_vip_day.png', 'Dùng thử VIP 1 Ngày', 2000, 'Package',
        'a2222222-0000-0000-0000-000000000001|1');

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

INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy",
                                          "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes",
                                          "Status", "SubmittedAt")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', TIMESTAMPTZ '1990-01-01T00:00:00Z', 'NGUYEN VAN BA', '079090000001',
        'Cục Cảnh sát QLHC về TTXH', TIMESTAMPTZ '2020-05-10T00:00:00Z', 'TP.HCM', NULL, NULL, NULL, 'PendingReview',
        TIMESTAMPTZ '2025-10-10T10:00:00Z');
INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy",
                                          "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes",
                                          "Status", "SubmittedAt")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', NULL, 'CONG TY TNHH VUA VE CHAI ABC', '0312345678',
        'Sở Kế hoạch và Đầu tư TP.HCM', TIMESTAMPTZ '2018-10-20T00:00:00Z', 'TP.HCM',
        TIMESTAMPTZ '2025-10-01T10:00:00Z', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 'Giấy phép kinh doanh hợp lệ.',
        'Approved', TIMESTAMPTZ '2025-09-30T10:00:00Z');

INSERT INTO "Notifications" ("NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId")
VALUES ('a491f60c-b552-4e2f-9e0f-5591679d9586', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMPTZ '2025-10-10T13:00:00Z',
        '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo",
                                   "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId",
                                   "VnpTransactionNo")
VALUES ('fa532179-a2ff-4b81-99a5-65990f0c74e8', 200000.0, 'NCB', NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('05e0131e-6254-4401-a01a-62704e8a97ea', TIMESTAMPTZ '2025-10-05T10:00:00Z', -20, 'Khiếu nại',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('c6e421bb-9aae-494a-a3a4-f26c10c06d5b', TIMESTAMPTZ '2025-10-10T13:00:00Z', 10, 'Hoàn thành đơn',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('03a7eae2-9222-423c-92bf-2a6c393df922', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL,
        GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('5bc5a760-22be-4c69-a2ac-3ad751ded537', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male',
        GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('735f3054-5e66-4d6c-acee-3ff798776cfc', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422',
        NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('b6b12589-a56b-4eea-8395-3301b573d365', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('33312e81-bb29-4714-b9e9-868260820fb7', TIMESTAMPTZ '2025-10-10T10:00:00Z', 3000.0, 1,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('3dc8324c-fc0c-4ed8-a2e8-ffe899b4ea87', TIMESTAMPTZ '2025-10-10T10:00:00Z', 8000.0, 4,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('9cb2c26f-d837-4466-ac59-d8d3b098fb6b', TIMESTAMPTZ '2025-10-10T10:00:00Z', 5000.0, 2,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('ed0fdf46-0ae8-46df-b506-c307d3b190de', TIMESTAMPTZ '2025-10-10T10:00:00Z', 15000.0, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ScrapPosts" ("ScrapPostId", "Address", "AvailableTimeRange", "CreatedAt", "Description", "HouseholdId",
                          "Location", "MustTakeAll", "Status", "Title", "UpdatedAt")
VALUES ('b0000001-0000-0000-0000-000000000001', '123 CMT8', NULL, TIMESTAMPTZ '2025-10-08T10:00:00Z',
        'Lấy hết giúp em.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TRUE,
        'Completed', 'Dọn kho Giấy & Nhựa', NULL);
INSERT INTO "ScrapPosts" ("ScrapPostId", "Address", "AvailableTimeRange", "CreatedAt", "Description", "HouseholdId",
                          "Location", "MustTakeAll", "Status", "Title", "UpdatedAt")
VALUES ('b0000002-0000-0000-0000-000000000001', '123 CMT8', NULL, TIMESTAMPTZ '2025-10-10T10:00:00Z',
        'Ai tiện ghé lấy.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', FALSE,
        'Open', 'Bán 50 vỏ lon', NULL);

INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('8d813450-e585-4ad2-8cae-5ba6fcfe1f58', TIMESTAMPTZ '2025-10-10T10:00:00Z', NULL,
        'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('f1162c19-279f-4328-b57b-c969a3680438', TIMESTAMPTZ '2025-10-10T10:00:00Z', TIMESTAMPTZ '2025-11-09T10:00:00Z',
        'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "UserRewardRedemptions" ("RedemptionDate", "RewardItemId", "UserId")
VALUES (TIMESTAMPTZ '2025-10-10T11:00:00Z', 2, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "CollectionOffers" ("CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status")
VALUES ('f0000001-0000-0000-0000-000000000001', TIMESTAMPTZ '2025-10-09T10:00:00Z',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'b0000001-0000-0000-0000-000000000001', 'Accepted');

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status")
VALUES (1, 'b0000001-0000-0000-0000-000000000001', '20kg', NULL, 'Collected');
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status")
VALUES (2, 'b0000001-0000-0000-0000-000000000001', '2 bao', NULL, 'Collected');
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Status")
VALUES (3, 'b0000002-0000-0000-0000-000000000001', '50 lon', NULL, 'Available');

INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('5750de15-7d2a-4843-a19d-38a51e20c50b', 'f0000001-0000-0000-0000-000000000001', 5000.0, 2, 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('bf87ccb4-5354-4922-8e1a-5a42fc0c0bc6', 'f0000001-0000-0000-0000-000000000001', 3000.0, 1, 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId",
                                 "ResponseMessage", "Status")
VALUES ('6d5e86d0-a338-417d-84d9-91167cb57df6', 'f0000001-0000-0000-0000-000000000001',
        TIMESTAMPTZ '2025-10-09T10:00:00Z', TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', TIMESTAMPTZ '2025-12-05T18:21:46.298946Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL,
        'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('2d68c33d-bd2e-4010-8aa1-1862301f5bde', TIMESTAMPTZ '2025-10-09T10:00:00Z',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status",
                          "TransactionId")
VALUES ('ca1d610f-1c1a-435e-a9e4-e1370cfa0bff', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-05T10:00:00Z', NULL, 'Hẹn không đến.', 'Submitted',
        '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('13996aef-c86c-422a-aa3b-eece73b13513', 'Nhanh gọn lẹ.', TIMESTAMPTZ '2025-10-10T13:00:00Z', 5,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (1, '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (2, '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('2d68c33d-bd2e-4010-8aa1-1862301f5bde', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('2d68c33d-bd2e-4010-8aa1-1862301f5bde', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('1766e59d-a31b-4063-84aa-de32c33dd06d', '2d68c33d-bd2e-4010-8aa1-1862301f5bde', 'Chào chị, em tới rồi.', TRUE,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMPTZ '2025-10-10T12:00:00Z');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('3e460c78-f259-4d80-b300-8b228c3354b5', '2d68c33d-bd2e-4010-8aa1-1862301f5bde', 'Ok em.', TRUE,
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-10T12:01:00Z');

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
VALUES ('20251205182146_Seeding_Database', '9.0.9');

COMMIT;

