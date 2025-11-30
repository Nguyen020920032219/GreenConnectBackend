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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, 'c7d0243c-7740-4359-a073-a13759b9404f',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM',
        '0900000000', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE,
        'b76d0f47-5f7a-4ee5-8a59-49c130179032', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '0c4ed74a-7920-4062-93b7-5e4d3d226b60',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM',
        '0933333333', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE,
        '14635d31-5e7d-4d73-b91c-aba53e9a7952', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, '5722a1db-e8c4-42cb-be05-49179048e73b',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM',
        '0922222222', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE,
        '06d006c0-b230-49a9-a766-b933203c9af2', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '0c3c54ef-d0a9-4280-b5ac-039928cea3d7',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM',
        '0988888888', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE,
        '15bb95ed-1a7c-4bee-9b7f-06fb6b6ca7c4', 'Active', FALSE, NULL, '0988888888');

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

INSERT INTO "RewardItems" ("RewardItemId", "Description", "ItemName", "PointsCost")
VALUES (1, 'Đổi voucher mua sắm', 'Voucher GotIt 50k', 500);
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ItemName", "PointsCost")
VALUES (2, 'Trang trí profile', 'Khung Avatar Xanh', 100);

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
VALUES ('b07d6276-695d-4c13-b5e0-94712ac95baa', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMPTZ '2025-10-10T13:00:00Z',
        '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo",
                                   "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId",
                                   "VnpTransactionNo")
VALUES ('7a9790a2-b965-4a24-ab6f-8968bb0be7ea', 200000.0, 'NCB', NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('c2e7549d-f7e6-4ccf-9203-2369743c202f', TIMESTAMPTZ '2025-10-10T13:00:00Z', 10, 'Hoàn thành đơn',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('d196bb47-79a2-4790-a65f-39a4d5d27573', TIMESTAMPTZ '2025-10-05T10:00:00Z', -20, 'Khiếu nại',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('7dfa4eb6-177b-4491-bbf6-b20beae98da2', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL,
        GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('89366c99-3a7d-458f-9ed3-df7886cdfbf4', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male',
        GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('b903db31-26d8-41bd-b9fa-9f477f86da03', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('c5eb844d-c009-4c2e-8a84-4a3a1b5ff683', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422',
        NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('160e9f19-f3de-496c-a928-ae582abc3ecb', TIMESTAMPTZ '2025-10-10T10:00:00Z', 15000.0, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('2207b7f4-62a6-495d-b92f-22724396142d', TIMESTAMPTZ '2025-10-10T10:00:00Z', 3000.0, 1,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('4b0077e9-8849-46b0-b887-896180e8c631', TIMESTAMPTZ '2025-10-10T10:00:00Z', 8000.0, 4,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('ced43f80-df59-4078-ad1f-178ccf937f32', TIMESTAMPTZ '2025-10-10T10:00:00Z', 5000.0, 2,
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
VALUES ('aafb6661-ba0c-4ecb-b3c8-7e8f6cb29cd0', TIMESTAMPTZ '2025-10-10T10:00:00Z', TIMESTAMPTZ '2025-11-09T10:00:00Z',
        'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('e1d086c3-5df5-434d-a10f-dd6ef8ed270a', TIMESTAMPTZ '2025-10-10T10:00:00Z', NULL,
        'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');

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
VALUES ('7a09bb88-d0df-426d-9fd8-18a81d83ce79', 'f0000001-0000-0000-0000-000000000001', 5000.0, 2, 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('a2494771-50b6-45ab-9b72-cfca041a68c3', 'f0000001-0000-0000-0000-000000000001', 3000.0, 1, 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId",
                                 "ResponseMessage", "Status")
VALUES ('a37a5243-4cfc-4800-a6f0-1f1438cf68e7', 'f0000001-0000-0000-0000-000000000001',
        TIMESTAMPTZ '2025-10-09T10:00:00Z', TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', TIMESTAMPTZ '2025-11-30T15:31:28.251778Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL,
        'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('77901ba0-b52f-4eba-bbe1-6a6a531f7ecc', TIMESTAMPTZ '2025-10-09T10:00:00Z',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status",
                          "TransactionId")
VALUES ('e5d7aae4-0c99-4824-ae26-6e6aab99e728', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-05T10:00:00Z', NULL, 'Hẹn không đến.', 'Submitted',
        '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('92b1fe1f-310b-4f90-9e93-09db44d3f7c0', 'Nhanh gọn lẹ.', TIMESTAMPTZ '2025-10-10T13:00:00Z', 5,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (1, '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (2, '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('77901ba0-b52f-4eba-bbe1-6a6a531f7ecc', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('77901ba0-b52f-4eba-bbe1-6a6a531f7ecc', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('99dd3a9d-e1e9-4c97-9339-ae86a36c6f8e', '77901ba0-b52f-4eba-bbe1-6a6a531f7ecc', 'Chào chị, em tới rồi.', TRUE,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMPTZ '2025-10-10T12:00:00Z');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('a136f1f8-ce9c-4877-a46d-afedaccd7126', '77901ba0-b52f-4eba-bbe1-6a6a531f7ecc', 'Ok em.', TRUE,
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
VALUES ('20251130153128_Seeding_Database', '9.0.9');

COMMIT;

