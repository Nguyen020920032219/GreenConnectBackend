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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '556e2670-ca01-47f2-9ddf-909986cb1302',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM',
        '0900000000', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE,
        'c6fec58f-881e-40f1-84a0-2e62ec964c0a', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '914a7446-3263-4bd1-b7f8-1034d03fe571',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM',
        '0933333333', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE,
        'd9f8bace-59bd-4fa4-8be8-48912f3b495b', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, 'a0853657-0360-4f05-be43-62d7b275f9d0',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM',
        '0922222222', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE,
        'f82d9187-0826-4d9b-91b0-6517f58505ac', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email",
                           "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail",
                           "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber",
                           "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt",
                           "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', '4344b6d2-b241-44fa-be78-15719c3d2b58',
        TIMESTAMPTZ '2025-10-10T10:00:00Z', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM',
        '0988888888', NULL, NULL,
        'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE,
        '2ef016c3-9a02-44e7-ac39-8a6d515d1697', 'Active', FALSE, NULL, '0988888888');

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
VALUES ('67d3a3d9-6f3d-425a-9206-abb0da701338', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMPTZ '2025-10-10T13:00:00Z',
        '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo",
                                   "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId",
                                   "VnpTransactionNo")
VALUES ('0c0f117d-9314-4b4f-a0ec-8d37e8f6776a', 200000.0, 'NCB', NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001',
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('7c24f3e1-d3dc-42a3-aec1-5f2d402f5017', TIMESTAMPTZ '2025-10-10T13:00:00Z', 10, 'Hoàn thành đơn',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('f40bcb2c-ee0c-4d29-9c38-f847bb44c763', TIMESTAMPTZ '2025-10-05T10:00:00Z', -20, 'Khiếu nại',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('3702da1a-b469-4bea-98d6-4c16cc278cb6', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('9195079b-0b88-49a6-8589-cd6876e6060c', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL,
        GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('a30c93f0-c8b7-42cf-9ac2-2e2c6c93597c', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male',
        GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode",
                        "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('d949b7d8-d409-47a8-83ab-57e4979ddb94', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422',
        NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('5c729f0f-2cb9-416a-b7e8-46ff8b334cd5', TIMESTAMPTZ '2025-10-10T10:00:00Z', 3000.0, 1,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('67e735f9-7524-494d-993a-6909ca744145', TIMESTAMPTZ '2025-10-10T10:00:00Z', 15000.0, 3,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('e2e5e70c-cd84-4ac0-b7fb-b2678276e2f2', TIMESTAMPTZ '2025-10-10T10:00:00Z', 5000.0, 2,
        'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('ecda8dad-c98e-4505-bd66-8a2b19b0e750', TIMESTAMPTZ '2025-10-10T10:00:00Z', 8000.0, 4,
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
VALUES ('7d8dffe5-7b8e-44af-b202-c2aea3f1b656', TIMESTAMPTZ '2025-10-10T10:00:00Z', TIMESTAMPTZ '2025-11-09T10:00:00Z',
        'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections",
                            "UserId")
VALUES ('ed1cf213-5e6b-4c7c-a88e-1b4592d2d2e9', TIMESTAMPTZ '2025-10-10T10:00:00Z', NULL,
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
VALUES ('2ee088ff-5f54-412e-8be5-34b6a243432a', 'f0000001-0000-0000-0000-000000000001', 3000.0, 1, 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('b76d2d8a-b80f-4a42-82a0-d0bd7a3c41f0', 'f0000001-0000-0000-0000-000000000001', 5000.0, 2, 'kg');

INSERT INTO "ScheduleProposals" ("ScheduleProposalId", "CollectionOfferId", "CreatedAt", "ProposedTime", "ProposerId",
                                 "ResponseMessage", "Status")
VALUES ('7f9385fc-2ca7-455e-a509-13b57d0ed4cb', 'f0000001-0000-0000-0000-000000000001',
        TIMESTAMPTZ '2025-10-09T10:00:00Z', TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        'Ok chốt', 'Accepted');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', TIMESTAMPTZ '2025-11-28T18:28:46.371123Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash',
        TIMESTAMPTZ '2025-10-10T12:00:00Z', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', 100000.0, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId",
                            "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TotalAmount", "UpdatedAt")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-05T10:00:00Z',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL,
        'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', 0.0, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('63464d25-b55b-419b-a411-da4c45b636b6', TIMESTAMPTZ '2025-10-09T10:00:00Z',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status",
                          "TransactionId")
VALUES ('6e0ab11c-9708-482b-95b5-c324f0ff7bd7', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00',
        'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMPTZ '2025-10-05T10:00:00Z', NULL, 'Hẹn không đến.', 'Submitted',
        '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('097c877d-7931-4a60-8ee6-43e74e402134', 'Nhanh gọn lẹ.', TIMESTAMPTZ '2025-10-10T13:00:00Z', 5,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (1, '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "Unit")
VALUES (2, '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('63464d25-b55b-419b-a411-da4c45b636b6', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('63464d25-b55b-419b-a411-da4c45b636b6', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef',
        TIMESTAMPTZ '2025-10-09T10:00:00Z');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('4c0edb44-47e1-45b0-93c1-68d8dee3d28d', '63464d25-b55b-419b-a411-da4c45b636b6', 'Chào chị, em tới rồi.', TRUE,
        'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMPTZ '2025-10-10T12:00:00Z');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('66afdc2b-95ec-405c-80c4-b70d159c7b5d', '63464d25-b55b-419b-a411-da4c45b636b6', 'Ok em.', TRUE,
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
VALUES ('20251128182846_Seeding_Database', '9.0.9');

COMMIT;

