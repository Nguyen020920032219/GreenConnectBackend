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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '86d73b85-3891-4ca6-9e35-abbc678d3e74',
        TIMESTAMP '2025-10-10T10:00:00', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM',
        '0900000000', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE,
        '2129e108-fc0b-4bd7-829a-218552913ff3', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '622266f8-b7bd-4076-beb2-efe377b5ac67',
        TIMESTAMP '2025-10-10T10:00:00', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM',
        '0933333333', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE,
        '2caa764a-dabd-44cb-bf11-164965ffe542', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, 'c183da79-9e79-48c8-b13f-9cce47006f62',
        TIMESTAMP '2025-10-10T10:00:00', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM',
        '0922222222', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE,
        '29afac11-a7b6-484f-a23c-142ae07d180d', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '726210bf-75a5-4436-afd7-d7b55c566280',
        TIMESTAMP '2025-10-10T10:00:00', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM',
        '0988888888', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE,
        '60c26f6f-82c6-4041-bf20-30a0c2d81c09', 'Active', FALSE, NULL, '0988888888');

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
VALUES ('c2213c00-475c-4819-a090-cdc0b55a5875', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMP '2025-10-10T13:00:00',
        '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo",
                                   "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId",
                                   "VnpTransactionNo")
VALUES ('bd2c4e8f-0bef-4c60-8647-7388f0d8aee6', 200000.0, 'NCB', NULL, TIMESTAMP '2025-10-05T10:00:00', 'Mua Goi Pro',
        'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('1099913a-3bf6-4702-b9a1-b42fcc4f8863', TIMESTAMP '2025-10-05T10:00:00', -20, 'Khiếu nại',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('7e5629ae-37f6-40fb-aca1-9cd8a5606212', TIMESTAMP '2025-10-10T13:00:00', 10, 'Hoàn thành đơn',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('1588a46d-e038-4731-a870-681faba79dca', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male',
        GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('67c26633-7c6d-48c9-bfcd-59c559042ce8', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL,
        GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('8931ab5c-dab3-4143-9795-33ed73548bfe', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422',
        NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('b8ca0afb-58b8-434e-b3aa-3d48728fb586', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('a17a5fe0-cce0-4fff-a5b1-5729606bceef', TIMESTAMP '2025-10-10T10:00:00', 3000.0,
        '11111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('d9625346-0dc9-45e1-b86e-2465fe1fd460', TIMESTAMP '2025-10-10T10:00:00', 5000.0,
        '22222222-2222-2222-2222-222222222222', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('db5333ec-1c0f-4131-9d4b-b98bd71bba02', TIMESTAMP '2025-10-10T10:00:00', 1000.0,
        '33333333-3333-3333-3333-333333333333', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

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
VALUES ('55a45ac4-7c17-4086-b38f-91ea0cf74a56', TIMESTAMP '2025-10-10T10:00:00', TIMESTAMP '2025-11-09T10:00:00',
        'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('6c766bee-ddd7-43ed-acf6-5e08f767c9a2', TIMESTAMP '2025-10-10T10:00:00', NULL,
        'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');

INSERT INTO "UserRewardRedemptions" ("RedemptionDate", "RewardItemId", "UserId")
VALUES (TIMESTAMP '2025-10-10T11:00:00', 2, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "CollectionOffers" ("CollectionOfferId", "CreatedAt", "ScrapCollectorId", "ScrapPostId", "Status")
VALUES ('f0000001-0000-0000-0000-000000000001', TIMESTAMP '2025-10-09T10:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        'b0000001-0000-0000-0000-000000000001', 'Accepted');

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1",
                                "Status")
VALUES ('11111111-1111-1111-1111-111111111111', 'b0000001-0000-0000-0000-000000000001', '20kg', NULL, NULL, 2);
INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1",
                                "Status")
VALUES ('22222222-2222-2222-2222-222222222222', 'b0000001-0000-0000-0000-000000000001', '2 bao', NULL, NULL, 2);

INSERT INTO "ScrapPostDetails" ("ScrapCategoryId", "ScrapPostId", "AmountDescription", "ImageUrl", "ScrapCategoryId1")
VALUES ('33333333-3333-3333-3333-333333333333', 'b0000002-0000-0000-0000-000000000001', '50 lon', NULL, NULL);

INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('27182d4e-1ca0-49f5-9482-f767966b3ba9', 'f0000001-0000-0000-0000-000000000001', 3000.0,
        '11111111-1111-1111-1111-111111111111', 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('b8ba387a-b2c3-4e31-a26c-766e8ce6a22a', 'f0000001-0000-0000-0000-000000000001', 5000.0,
        '22222222-2222-2222-2222-222222222222', 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId",
                                 "ResponseMessage", "Status")
VALUES ('f00d2454-e965-48c7-9eed-252717f64606', 'f0000001-0000-0000-0000-000000000001', TIMESTAMP '2025-10-09T10:00:00',
        TIMESTAMP '2025-10-10T12:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)',
        TIMESTAMP '2025-10-10T12:00:00', TIMESTAMP '2025-12-18T10:07:06.588485', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        'f0000001-0000-0000-0000-000000000001', 'Cash', TIMESTAMP '2025-10-10T12:00:00',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMP '2025-10-05T10:00:00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL,
        'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('f46cfc4f-e950-4a62-a5b7-ace86d35b738', TIMESTAMP '2025-10-09T10:00:00',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status",
                          "TransactionId")
VALUES ('2d81459c-de4f-4923-96f0-5b1388dc1b6d', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-05T10:00:00', NULL, 'Hẹn không đến.', 'Submitted',
        '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('b0ded13b-d244-4432-bc6f-02a39964e397', 'Nhanh gọn lẹ.', TIMESTAMP '2025-10-10T13:00:00', 5,
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
VALUES ('f46cfc4f-e950-4a62-a5b7-ace86d35b738', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        TIMESTAMP '2025-10-09T10:00:00');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('f46cfc4f-e950-4a62-a5b7-ace86d35b738', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        TIMESTAMP '2025-10-09T10:00:00');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('26d89cd4-a0c0-47fe-b7b6-a7060544a456', 'f46cfc4f-e950-4a62-a5b7-ace86d35b738', 'Ok em.', TRUE,
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-10T12:01:00');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('f4a22bcb-359e-4540-a49b-b2972e4c15fc', 'f46cfc4f-e950-4a62-a5b7-ace86d35b738', 'Chào chị, em tới rồi.', TRUE,
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
VALUES ('20251218100707_Seeding_Database', '9.0.9');

COMMIT;

