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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '786bf9d9-51c3-4fb6-a8f8-c4dd84b12563',
        TIMESTAMP '2025-10-10T10:00:00', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM',
        '0900000000', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE,
        '02824a24-a664-444d-9a44-e92ceb8ee21a', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '4ada4827-d9d7-4c69-8546-559f74cd2c82',
        TIMESTAMP '2025-10-10T10:00:00', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM',
        '0933333333', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE,
        '9673f2c8-53bb-4cd6-acc0-1059bd3c2f4e', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, '30271cf5-639b-486a-b924-1a4557eee666',
        TIMESTAMP '2025-10-10T10:00:00', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM',
        '0922222222', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE,
        '183eb312-9ef5-40eb-8be2-b8e7d9175395', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', 'd5949604-86a6-42e8-a1ea-6d8e47b5bfd5',
        TIMESTAMP '2025-10-10T10:00:00', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM',
        '0988888888', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE,
        '43f8e0b9-dece-450f-b2cc-6e6e6ed9b11b', 'Active', FALSE, NULL, '0988888888');

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

INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy",
                                          "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes",
                                          "Status", "SubmittedAt")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', TIMESTAMP '1990-01-01T00:00:00', 'NGUYEN VAN BA', '079090000001',
        'Cục Cảnh sát QLHC về TTXH', TIMESTAMP '2020-05-10T00:00:00', 'TP.HCM', NULL, NULL, NULL, 'PendingReview',
        TIMESTAMP '2025-10-10T10:00:00');
INSERT INTO "CollectorVerificationInfos" ("UserId", "DateOfBirth", "FullnameOnId", "IdentityNumber", "IssuedBy",
                                          "IssuedDate", "PlaceOfOrigin", "ReviewedAt", "ReviewerId", "ReviewerNotes",
                                          "Status", "SubmittedAt")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', NULL, 'CONG TY TNHH VUA VE CHAI ABC', '0312345678',
        'Sở Kế hoạch và Đầu tư TP.HCM', TIMESTAMP '2018-10-20T00:00:00', 'TP.HCM', TIMESTAMP '2025-10-01T10:00:00',
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 'Giấy phép kinh doanh hợp lệ.', 'Approved',
        TIMESTAMP '2025-09-30T10:00:00');

INSERT INTO "Notifications" ("NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId")
VALUES ('23bae82a-a7d5-4a28-a15e-787f65955a96', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMP '2025-10-10T13:00:00',
        '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo",
                                   "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId",
                                   "VnpTransactionNo")
VALUES ('46e40dba-f734-4161-8c7d-9f73a09cc39f', 200000.0, 'NCB', NULL, TIMESTAMP '2025-10-05T10:00:00', 'Mua Goi Pro',
        'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('534d0017-bd7a-420a-8ac5-5a988821c571', TIMESTAMP '2025-10-10T13:00:00', 10, 'Hoàn thành đơn',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('5da6d760-60a9-4a45-8e1e-c27f3012ac83', TIMESTAMP '2025-10-05T10:00:00', -20, 'Khiếu nại',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('25833e81-5d0a-4d79-a477-bfe71aa3a8ea', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422',
        NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('3a52da4b-9465-420c-9865-9061a77c8651', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL,
        GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('6d44205f-7c2c-4297-b0d2-0b6e73c44808', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('ab384c63-d28b-4315-95c5-c2f55909b60e', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male',
        GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('00d13a5d-451a-40c3-8747-c6102fea8256', TIMESTAMP '2025-10-10T10:00:00', 1000.0,
        '33333333-3333-3333-3333-333333333333', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('a28081b5-59cf-43eb-9795-600b493c507f', TIMESTAMP '2025-10-10T10:00:00', 3000.0,
        '11111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('db88dcc1-be3e-4a1e-a0f1-bf55b0665ada', TIMESTAMP '2025-10-10T10:00:00', 5000.0,
        '22222222-2222-2222-2222-222222222222', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll",
                          "Status", "Title", "UpdatedAt", "UserId")
VALUES ('b0000001-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-08T10:00:00', 'Lấy hết giúp em.',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TRUE, 3,
        'Dọn kho Giấy & Nhựa', NULL, NULL);

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "Title", "UpdatedAt",
                          "UserId")
VALUES ('b0000002-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-10T10:00:00', 'Ai tiện ghé lấy.',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 'Bán 50 vỏ lon', NULL,
        NULL);

INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('67b31caf-52cb-46c8-9a6a-a9318026edc1', TIMESTAMP '2025-10-10T10:00:00', NULL,
        'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('84e3cc1b-ad93-4010-a0f0-900de45254a4', TIMESTAMP '2025-10-10T10:00:00', TIMESTAMP '2025-11-09T10:00:00',
        'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "UserRewardRedemptions" ("RedemptionDate", "RewardItemId", "UserId")
VALUES (TIMESTAMP '2025-10-10T11:00:00', 2, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "CollectionOffers" ("CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status",
                                "TimeSlotId")
VALUES ('f0000001-0000-0000-0000-000000000001', TIMESTAMP '2025-10-09T10:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        'b0000001-0000-0000-0000-000000000001', 'Accepted', NULL);

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Quantity",
                                "ScrapCategoryId1", "Status", "Unit")
VALUES ('11111111-1111-1111-1111-111111111111', 'b0000001-0000-0000-0000-000000000001', '20kg', NULL, 0.0, NULL, 2,
        'kg');
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Quantity",
                                "ScrapCategoryId1", "Status", "Unit")
VALUES ('22222222-2222-2222-2222-222222222222', 'b0000001-0000-0000-0000-000000000001', '2 bao', NULL, 0.0, NULL, 2,
        'kg');

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "Quantity",
                                "ScrapCategoryId1", "Unit")
VALUES ('33333333-3333-3333-3333-333333333333', 'b0000002-0000-0000-0000-000000000001', '50 lon', NULL, 0.0, NULL,
        'kg');

INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('7ca8d261-9d92-4220-8d92-ece3b628e9d7', 'f0000001-0000-0000-0000-000000000001', 3000.0,
        '11111111-1111-1111-1111-111111111111', 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('8671d90c-1910-4c02-b16c-1b36777d0258', 'f0000001-0000-0000-0000-000000000001', 5000.0,
        '22222222-2222-2222-2222-222222222222', 'kg');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TimeSlotId", "TotalAmount",
                            "UpdatedAt", "UserId", "UserId1")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)',
        TIMESTAMP '2025-10-10T12:00:00', TIMESTAMP '2025-12-21T15:21:12.871019', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        'f0000001-0000-0000-0000-000000000001', 'Cash', TIMESTAMP '2025-10-10T12:00:00',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', NULL, 100000.0, NULL, NULL, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TimeSlotId", "TotalAmount",
                            "UpdatedAt", "UserId", "UserId1")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMP '2025-10-05T10:00:00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL,
        'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', NULL, 0.0, NULL, NULL, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('3af52111-7fa3-454a-b555-167cda22afdb', TIMESTAMP '2025-10-09T10:00:00',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status",
                          "TransactionId")
VALUES ('b3779725-f849-4982-882d-a76eb1e93feb', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-05T10:00:00', NULL, 'Hẹn không đến.', 'Submitted',
        '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('f4e4dafc-89e9-458c-a472-c28c7d5b278f', 'Nhanh gọn lẹ.', TIMESTAMP '2025-10-10T13:00:00', 5,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity",
                                  "ScrapCategoryId1", "Unit")
VALUES ('11111111-1111-1111-1111-111111111111', '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, NULL,
        'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity",
                                  "ScrapCategoryId1", "Unit")
VALUES ('22222222-2222-2222-2222-222222222222', '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, NULL,
        'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('3af52111-7fa3-454a-b555-167cda22afdb', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        TIMESTAMP '2025-10-09T10:00:00');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('3af52111-7fa3-454a-b555-167cda22afdb', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        TIMESTAMP '2025-10-09T10:00:00');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('07a5bf16-97d5-4a99-bde4-607c61e57279', '3af52111-7fa3-454a-b555-167cda22afdb', 'Ok em.', TRUE,
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-10T12:01:00');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('fdad2164-ad4b-4a8c-8fa0-2d746ed5bb70', '3af52111-7fa3-454a-b555-167cda22afdb', 'Chào chị, em tới rồi.', TRUE,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-10T12:00:00');

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
VALUES ('20251221152113_Seeding_Database', '9.0.9');

COMMIT;

