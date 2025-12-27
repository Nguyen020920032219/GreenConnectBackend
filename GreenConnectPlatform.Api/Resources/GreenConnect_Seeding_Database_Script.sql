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
VALUES ('a1b2c3d4-e5f6-7788-9900-aabbccddeeff', 0, NULL, '3a1fa80a-346b-496c-856a-e43392b683ad', TIMESTAMP '2025-10-10T10:00:00', 'admin@gc.com', TRUE, 'Admin System', FALSE, NULL, 'ADMIN@GC.COM', '0900000000', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0900000000', TRUE, '105b7e50-a5eb-4350-b573-b7d7e2e7da6e', 'Active', FALSE, NULL, '0900000000');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 0, 'Individual', '19d03e8b-5f47-49a8-ac37-af5d69ac7bd0', TIMESTAMP '2025-10-10T10:00:00', 'anhba@gc.com', TRUE, 'Anh Ba Ve Chai', FALSE, NULL, 'ANHBA@GC.COM', '0933333333', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0933333333', TRUE, '1ff84b6f-403c-46cd-b76d-8fdb74e7bb73', 'Active', FALSE, NULL, '0933333333');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 0, NULL, 'd8045c3f-6082-42f6-b139-c71e43ab6b30', TIMESTAMP '2025-10-10T10:00:00', 'chitu@gc.com', TRUE, 'Chị Tư Nội Trợ', FALSE, NULL, 'CHITU@GC.COM', '0922222222', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0922222222', TRUE, '581db0d0-c2e5-458b-a870-3bf193b0a49e', 'Active', FALSE, NULL, '0922222222');
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "BuyerType", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiredAt", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName")
VALUES ('e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 0, 'Business', 'b4ed21e9-a66e-4c99-89f4-514e33648b27', TIMESTAMP '2025-10-10T10:00:00', 'vuaabc@gc.com', TRUE, 'Vựa Tái Chế ABC', FALSE, NULL, 'VUAABC@GC.COM', '0988888888', NULL, NULL, 'AQAAAAIAAYagAAAAELSd8S1/ERD0+I4lEAStNTTw/VOGuVVH8vE3SL92wYldv4i4XV78koh+GJ3GpdR05A==', '0988888888', TRUE, '39d1902a-8dce-4a87-b3bf-340201d39d05', 'Active', FALSE, NULL, '0988888888');

INSERT INTO "PaymentPackages" ("PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType", "Price")
VALUES ('a1111111-0000-0000-0000-000000000001', 5, '5 credit', TRUE, 'Gói Free', 'Freemium', 0.0);
INSERT INTO "PaymentPackages" ("PackageId", "ConnectionAmount", "Description", "IsActive", "Name", "PackageType", "Price")
VALUES ('a2222222-0000-0000-0000-000000000001', 500, '500 credit', TRUE, 'Gói Pro', 'Paid', 200000.0);

INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (1, 'bronze.png', 0, 'Mới');
INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (2, 'silver.png', 1000, 'Bạc');
INSERT INTO "Ranks" ("RankId", "BadgeImageUrl", "MinPoints", "Name")
VALUES (3, 'gold.png', 5000, 'Vàng');

INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (1, '10 credit.', 'https://firebasestorage.googleapis.com/.../icon_credit_1.png', '1 Lượt Kết Nối', 100, 'Credit', '10');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (2, 'Gói tiết kiệm. Phù hợp cho người thu gom thường xuyên.', 'https://firebasestorage.googleapis.com/.../icon_credit_5.png', 'Combo 5 Lượt', 400, 'Credit', '50');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (3, 'Gói sỉ siêu hời. Thoải mái kết nối.', 'https://firebasestorage.googleapis.com/.../icon_credit_10.png', 'Combo 10 Lượt', 750, 'Credit', '100');
INSERT INTO "RewardItems" ("RewardItemId", "Description", "ImageUrl", "ItemName", "PointsCost", "Type", "Value")
VALUES (4, 'Gói sỉ siêu hời. Thoải mái kết nối.', 'https://firebasestorage.googleapis.com/.../icon_vip_day.png', 'Combo 20 Lượt', 1300, 'Package', '200');

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
VALUES ('9e9c009b-d815-4d9e-b933-adaa1d820cf8', 'Vựa ABC đã hoàn thành đơn hàng.', TIMESTAMP '2025-10-10T13:00:00', '70000001-0000-0000-0000-000000000001', 'Transaction', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "PaymentTransactions" ("PaymentId", "Amount", "BankCode", "ClientIpAddress", "CreatedAt", "OrderInfo", "PackageId", "PaymentGateway", "ResponseCode", "Status", "TransactionRef", "UserId", "VnpTransactionNo")
VALUES ('37103350-1020-4004-9bf7-183172d149fc', 200000.0, 'NCB', NULL, TIMESTAMP '2025-10-05T10:00:00', 'Mua Goi Pro', 'a2222222-0000-0000-0000-000000000001', 'VNPay', '00', 'Success', 'ORD001', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'VNP001');

INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('439eb002-872a-491f-b932-94a1b025cd22', TIMESTAMP '2025-10-10T13:00:00', 10, 'Hoàn thành đơn', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "PointHistories" ("PointHistoryId", "CreatedAt", "PointChange", "Reason", "UserId")
VALUES ('e0664397-ea26-4742-aa29-e8b563f77785', TIMESTAMP '2025-10-05T10:00:00', -20, 'Khiếu nại', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');

INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('2099e919-5274-498a-9ee3-0f82effb6971', 'Hẻm 456 Lê Văn Sỹ, Q3, HCM', NULL, NULL, NULL, NULL, NULL, 'Male', GEOMETRY 'SRID=4326;POINT (106.68 10.78)', 120, 1, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('4e6dacdb-dce1-4ff6-a819-0772448428b6', '123 CMT8, Q3, HCM', NULL, 'NGUYEN THI TU', '0922222222', '970422', NULL, 'Female', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 50, 1, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('8adb5b2b-5f5b-43ce-a846-fa20d2df13bc', 'Headquarter', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9999, 3, 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "Profiles" ("ProfileId", "Address", "AvatarUrl", "BankAccountName", "BankAccountNumber", "BankCode", "DateOfBirth", "Gender", "Location", "PointBalance", "RankId", "UserId")
VALUES ('b5210c76-d2ee-4934-a501-a113eca9e71b', 'Kho Quận 7, HCM', NULL, 'CTY ABC', '0988888888', '970436', NULL, NULL, GEOMETRY 'SRID=4326;POINT (106.72 10.75)', 5000, 2, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');

INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('19c66a17-aaea-462a-aee7-bb2464f917e9', TIMESTAMP '2025-10-10T10:00:00', 1000.0, '33333333-3333-3333-3333-333333333333', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('2a52feca-a44c-42dc-b841-42760209ccd9', TIMESTAMP '2025-10-10T10:00:00', 3000.0, '11111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');
INSERT INTO "ReferencePrices" ("ReferencePriceId", "LastUpdated", "PricePerKg", "ScrapCategoryId", "UpdatedByAdminId")
VALUES ('45546a6f-0075-41f4-9ce7-6e82d6c0b496', TIMESTAMP '2025-10-10T10:00:00', 5000.0, '22222222-2222-2222-2222-222222222222', 'a1b2c3d4-e5f6-7788-9900-aabbccddeeff');

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "MustTakeAll", "Status", "Title", "UpdatedAt", "UserId")
VALUES ('b0000001-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-08T10:00:00', 'Lấy hết giúp em.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TRUE, 3, 'Dọn kho Giấy & Nhựa', NULL, NULL);

INSERT INTO "ScrapPosts" ("Id", "Address", "CreatedAt", "Description", "HouseholdId", "Location", "Title", "UpdatedAt", "UserId")
VALUES ('b0000002-0000-0000-0000-000000000001', '123 CMT8', TIMESTAMP '2025-10-10T10:00:00', 'Ai tiện ghé lấy.', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', 'Bán 50 vỏ lon', NULL, NULL);

INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('1c7aea86-6c29-4f0d-96f2-c3c5ad1bc089', TIMESTAMP '2025-10-10T10:00:00', TIMESTAMP '2025-11-09T10:00:00', 'a2222222-0000-0000-0000-000000000001', 499, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef');
INSERT INTO "UserPackages" ("UserPackageId", "ActivationDate", "ExpirationDate", "PackageId", "RemainingConnections", "UserId")
VALUES ('a8daaf34-1620-4b70-a788-4ca27ad7f68f', TIMESTAMP '2025-10-10T10:00:00', NULL, 'a1111111-0000-0000-0000-000000000001', 5, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00');

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
VALUES ('43114e15-0aad-4740-b035-72dce1765fa5', 'f0000001-0000-0000-0000-000000000001', 5000.0, '22222222-2222-2222-2222-222222222222', 'kg');
INSERT INTO "OfferDetail" ("OfferDetailId", "CollectionOfferId", "PricePerUnit", "ScrapCategoryId", "Unit")
VALUES ('7d65736a-d527-4077-baf3-45bce32eeb4a', 'f0000001-0000-0000-0000-000000000001', 3000.0, '11111111-1111-1111-1111-111111111111', 'kg');

INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TimeSlotId", "TotalAmount", "UpdatedAt", "UserId", "UserId1")
VALUES ('70000001-0000-0000-0000-000000000001', GEOMETRY 'SRID=4326;POINT (106.69 10.777)', TIMESTAMP '2025-10-10T12:00:00', TIMESTAMP '2025-12-27T18:40:51.595791', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', 'Cash', TIMESTAMP '2025-10-10T12:00:00', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'Completed', NULL, 100000.0, NULL, NULL, NULL);
INSERT INTO "Transactions" ("TransactionId", "CheckInLocation", "CheckInTime", "CreatedAt", "HouseholdId", "OfferId", "PaymentMethod", "ScheduledTime", "ScrapCollectorId", "Status", "TimeSlotId", "TotalAmount", "UpdatedAt", "UserId", "UserId1")
VALUES ('70000002-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMP '2025-10-05T10:00:00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', 'f0000001-0000-0000-0000-000000000001', NULL, NULL, 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'CanceledByUser', NULL, 0.0, NULL, NULL, NULL);

INSERT INTO "ChatRooms" ("ChatRoomId", "CreatedAt", "TransactionId")
VALUES ('4e4b5ff7-84e8-431a-b3a0-2ed067c22e62', TIMESTAMP '2025-10-09T10:00:00', '70000001-0000-0000-0000-000000000001');

INSERT INTO "Complaints" ("ComplaintId", "AccusedId", "ComplainantId", "CreatedAt", "EvidenceUrl", "Reason", "Status", "TransactionId")
VALUES ('71f6b085-a158-4413-ae74-775abc4f9360', 'b2c3d4e5-f6a1-8899-0011-bbccddeeff00', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-05T10:00:00', NULL, 'Hẹn không đến.', 'Submitted', '70000002-0000-0000-0000-000000000002');

INSERT INTO "Feedbacks" ("FeedbackId", "Comment", "CreatedAt", "Rate", "RevieweeId", "ReviewerId", "TransactionId")
VALUES ('dd2179fe-0301-489e-9993-e5436587995c', 'Nhanh gọn lẹ.', TIMESTAMP '2025-10-10T13:00:00', 5, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', '70000001-0000-0000-0000-000000000001');

INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('11111111-1111-1111-1111-111111111111', '70000001-0000-0000-0000-000000000001', 45000.0, 3000.0, 15, NULL, 'kg');
INSERT INTO "TransactionDetails" ("ScrapCategoryId", "TransactionId", "FinalPrice", "PricePerUnit", "Quantity", "ScrapCategoryId1", "Unit")
VALUES ('22222222-2222-2222-2222-222222222222', '70000001-0000-0000-0000-000000000001', 55000.0, 5000.0, 11, NULL, 'kg');

INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('4e4b5ff7-84e8-431a-b3a0-2ed067c22e62', 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-09T10:00:00');
INSERT INTO "ChatParticipants" ("ChatRoomId", "UserId", "JoinedAt")
VALUES ('4e4b5ff7-84e8-431a-b3a0-2ed067c22e62', 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-09T10:00:00');

INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('65f8493c-a5dd-4b14-8e48-e0751a139cee', '4e4b5ff7-84e8-431a-b3a0-2ed067c22e62', 'Ok em.', TRUE, 'c3d4e5f6-a1b2-9900-1122-ccddeeff0011', TIMESTAMP '2025-10-10T12:01:00');
INSERT INTO "Messages" ("MessageId", "ChatRoomId", "Content", "IsRead", "SenderId", "Timestamp")
VALUES ('a6194fc1-f9da-4920-b1f4-fc1e25e1f886', '4e4b5ff7-84e8-431a-b3a0-2ed067c22e62', 'Chào chị, em tới rồi.', TRUE, 'e6a1b2c3-d4e5-f6a7-8899-0011bbccdeef', TIMESTAMP '2025-10-10T12:00:00');

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
VALUES ('20251227184052_Seeding_Database', '9.0.9');

COMMIT;

