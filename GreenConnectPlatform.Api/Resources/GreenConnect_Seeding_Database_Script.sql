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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '9e973a08-46bc-4f86-914a-fb5d31b3ac08',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM',
        '0900000000', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE,
        '7e0de5c2-2a83-4730-8df8-9a34377d753a', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '5faaa3cf-f604-47ae-ab09-106a1e1046e8',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM',
        '0933333333', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE,
        '5111c008-028c-4724-8ba8-fbb5a5684a24', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, 'd3e670e1-e669-4400-86d3-23b0421f8baf',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM',
        '0922222222', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE,
        '311cb207-e89c-4094-af75-04f34d004a26', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '7a9b8462-fa7d-4114-a0fa-981f6e8959c0',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM',
        '0988888888', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE,
        'a3086c36-089d-4161-b110-f468a746562f', 'Active', FALSE, NULL, '0988888888');

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

INSERT INTO "CollectorVerificationInfos" ("UserId", "DocumentBackUrl", "DocumentFrontUrl", "ReviewedAt", "ReviewerId",
                                          "ReviewerNotes", "Status", "SubmittedAt")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'back.jpg', 'front.jpg', NULL, NULL, NULL, 'PendingReview',
        TIMESTAMPTZ '2025-10-10T10:00:00Z');
INSERT INTO "CollectorVerificationInfos" ("UserId", "DocumentBackUrl", "DocumentFrontUrl", "ReviewedAt", "ReviewerId",
                                          "ReviewerNotes", "Status", "SubmittedAt")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', NULL, 'license.jpg', TIMESTAMPTZ '2025-10-01T10:00:00Z',
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff', NULL, 'Approved', TIMESTAMPTZ '2025-09-30T10:00:00Z');

INSERT INTO "Notifications" ("NotificationId", "Content", "CreatedAt", "EntityId", "EntityType", "RecipientId")
VALUES ('44cb32e4-16da-4853-a2df-95c487d61cd7', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMPTZ '2025-10-10T13:00:00Z',
        '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo",
                                   "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId",
                                   "VnpTransactionNo")
VALUES ('a8909d9e-9113-42d1-bc22-5360180e632b', 200000.0, 'NCB', NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('4cf6bbbc-1332-4876-b705-95aa43f52c3a', TIMESTAMPTZ '2025-10-05T10:00:00Z', -20, 'Khiếu nại',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('7ac402ab-f0ba-4a4f-9623-9761e7fa162e', TIMESTAMPTZ '2025-10-10T13:00:00Z', 10, 'Hoàn thành đơn',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('5af913b5-6f87-488c-b0ec-7a34bc9dca6b', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('984825fb-8e1d-42d9-b18e-f0c10fbed485', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male',
        GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('ae90516c-bf6b-4c3c-b9b6-e662ba414812', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422',
        NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('f1a2ca59-0a02-4ffa-b35f-5a880351004b', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL,
        GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('0a629df4-ed47-48e3-804f-d94f11228c45', TIMESTAMPTZ '2025-10-10T10:00:00Z', 15000.0, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('199b8d36-35fd-4e08-bb66-6b8179a868d2', TIMESTAMPTZ '2025-10-10T10:00:00Z', 5000.0, 2,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('4977f38e-b859-4ce8-bc09-30af002e4fae', TIMESTAMPTZ '2025-10-10T10:00:00Z', 3000.0, 1,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('bad9450b-ee58-45d4-954f-8166f540bee7', TIMESTAMPTZ '2025-10-10T10:00:00Z', 8000.0, 4,
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
VALUES ('07b2bd88-53c9-46fd-afe1-6afa165799c6', TIMESTAMPTZ '2025-10-10T10:00:00Z', NULL,
        'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('6ca8eb1e-6ba5-4ee0-99b8-fad863081aee', TIMESTAMPTZ '2025-10-10T10:00:00Z', TIMESTAMPTZ '2025-11-09T10:00:00Z',
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
VALUES ('c52875f4-70ad-4cc3-8c6b-2ef8a4d6205e', 'f0000001-0000-0000-0000-000000000001', 5000.0, 2, 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('ee5adde4-88de-4334-b6eb-00c01536aad3', 'f0000001-0000-0000-0000-000000000001', 3000.0, 1, 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId",
                                 "ResponseMessage", "Status")
VALUES ('2bb38ccc-db21-49cc-ac67-9bbd0c2aa217', 'f0000001-0000-0000-0000-000000000001',
        TIMESTAMPTZ '2025-10-09T10:00:00Z', TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', TIMESTAMPTZ '2025-12-03T18:25:33.125597Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL,
        'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('98f753e3-4bd6-4030-9f2c-f7f219c580a8', TIMESTAMPTZ '2025-10-09T10:00:00Z',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status",
                          "TransactionId")
VALUES ('300392cc-e1d0-41fe-8c9d-692cb505f506', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-05T10:00:00Z', NULL, 'Hẹn không đến.', 'Submitted',
        '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('1e93bb8f-61bf-420e-82ea-14983375e7d4', 'Nhanh gọn lẹ.', TIMESTAMPTZ '2025-10-10T13:00:00Z', 5,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (1, '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (2, '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('98f753e3-4bd6-4030-9f2c-f7f219c580a8', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('98f753e3-4bd6-4030-9f2c-f7f219c580a8', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('675158cb-649c-4912-aa3b-f8d6f2707bc0', '98f753e3-4bd6-4030-9f2c-f7f219c580a8', 'Chào chị, em tới rồi.', TRUE,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMPTZ '2025-10-10T12:00:00Z');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('b9997598-322d-4c9e-b176-858f4c135a41', '98f753e3-4bd6-4030-9f2c-f7f219c580a8', 'Ok em.', TRUE,
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
VALUES ('20251203182533_Seeding_Database', '9.0.9');

COMMIT;

