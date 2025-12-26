using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.CreditTransactionHistories;
using GreenConnectPlatform.Business.Models.Feedbacks;
using GreenConnectPlatform.Business.Models.Notifications;
using GreenConnectPlatform.Business.Models.PaymentPackages;
using GreenConnectPlatform.Business.Models.PaymentTransactions;
using GreenConnectPlatform.Business.Models.PointHistories;
using GreenConnectPlatform.Business.Models.RecurringScheduleDetails;
using GreenConnectPlatform.Business.Models.RecurringSchedules;
using GreenConnectPlatform.Business.Models.ReferencePrices;
using GreenConnectPlatform.Business.Models.RewardItems;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Data.Entities;
using Profile = AutoMapper.Profile;

namespace GreenConnectPlatform.Business.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region ScrapPost, ScrapPostDetail

        CreateMap<ScrapPost, ScrapPostModel>();
        CreateMap<ScrapPostCreateModel, ScrapPost>()
            .ForMember(dest => dest.Location, opt => opt.Ignore())
            .ForMember(dest => dest.TimeSlots, opt => opt.MapFrom(src => src.ScrapPostTimeSlots));
        ;
        CreateMap<ScrapPost, ScrapPostOverralModel>();
        CreateMap<ScrapPostDetailCreateModel, ScrapPostDetail>();
        CreateMap<ScrapPostDetail, ScrapPostDetailModel>();
        CreateMap<ScrapPostUpdateModel, ScrapPost>()
            .ForMember(dest => dest.Location, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ScrapPostDetailUpdateModel, ScrapPostDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        #endregion

        #region ScrapPostTimeSlot

        CreateMap<ScrapPostTimeSlot, ScrapPostTimeSlotModel>();
        CreateMap<ScrapPostTimeSlotCreateModel, ScrapPostTimeSlot>();

        #endregion

        #region CollectionOffer, OfferDetail

        CreateMap<CollectionOffer, CollectionOfferModel>();
        CreateMap<CollectionOffer, CollectionOfferOveralForCollectorModel>();
        CreateMap<CollectionOffer, CollectionOfferOveralForHouseModel>();
        CreateMap<CollectionOfferCreateModel, CollectionOffer>();
        CreateMap<OfferDetailCreateModel, OfferDetail>();
        CreateMap<OfferDetail, OfferDetailModel>();
        CreateMap<OfferDetailUpdateModel, OfferDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        #endregion

        #region ScrapCategory

        CreateMap<ScrapCategory, ScrapCategoryModel>();

        #endregion

        #region Transaction, TransactionDetail

        CreateMap<Transaction, TransactionModel>();
        CreateMap<Transaction, TransactionOveralModel>();
        CreateMap<Transaction, TransactionForPaymentModel>();
        CreateMap<TransactionDetail, TransactionDetailModel>();
        CreateMap<TransactionDetailCreateModel, TransactionDetail>();
        CreateMap<TransactionDetailUpdateModel, TransactionDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        ;

        #endregion

        #region User

        CreateMap<User, UserViewModel>();
        CreateMap<User, UserModel>();
        CreateMap<CollectorVerificationInfo, VerificationInfoModel>();
        CreateMap<CollectorVerificationInfo, VerificationInfoOveralModel>();

        #endregion

        #region PaymentPackage

        CreateMap<PaymentPackage, PaymentPackageModel>();
        CreateMap<PaymentPackage, PaymentPackageOverallModel>();
        CreateMap<PaymentPackageCreateModel, PaymentPackage>();
        CreateMap<PaymentPackageUpdateModel, PaymentPackage>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        #endregion

        #region ReferencePrice

        CreateMap<ReferencePrice, ReferencePriceModel>();

        #endregion

        #region Complaint

        CreateMap<Complaint, ComplaintModel>();
        CreateMap<ComplaintCreateModel, Complaint>();

        #endregion

        #region RewardItem

        CreateMap<RewardItem, RewardItemModel>();
        CreateMap<RewardItemCreateModel, RewardItem>();
        CreateMap<RewardItemUpdateModel, RewardItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<RewardItemCreateModel, RewardItem>();

        #endregion

        #region Feedback

        CreateMap<Feedback, FeedbackModel>();
        CreateMap<FeedbackCreateModel, Feedback>();

        #endregion

        #region PointHistory

        CreateMap<PointHistory, PointHistoryModel>();

        #endregion

        #region Notification

        CreateMap<Notification, NotificationModel>();

        #endregion

        #region CreditTransactionHistoryService

        CreateMap<CreditTransactionHistory, CreditTransactionHistoryModel>();

        #endregion

        #region PaymentTransaction

        CreateMap<PaymentTransaction, PaymentTransactionModel>();

        #endregion

        #region RecurringSchedule, RecurringScheduleDetail

        CreateMap<RecurringSchedule, RecurringScheduleModel>();
        CreateMap<RecurringSchedule, RecurringScheduleOverallModel>();
        CreateMap<RecurringScheduleCreateModel, RecurringSchedule>()
            .ForMember(dest => dest.Location, opt => opt.Ignore());
        CreateMap<RecurringScheduleUpdateModel, RecurringSchedule>()
            .ForMember(dest => dest.Location, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<RecurringScheduleDetail, RecurringScheduleDetailModel>();
        CreateMap<RecurringScheduleDetailCreateModel, RecurringScheduleDetail>();

        #endregion
    }
}