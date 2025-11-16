using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Data.Entities;
using Profile = AutoMapper.Profile;

namespace GreenConnectPlatform.Business.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region ScrapPost, ScrapPostDetail

        CreateMap<ScrapPost, ScrapPostModel>();
        CreateMap<ScrapPostCreateModel, ScrapPost>().ForMember(dest => dest.Location, opt => opt.Ignore());
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

        #region CollectionOffer, OfferDetail

        CreateMap<CollectionOffer, CollectionOfferModel>();
        CreateMap<CollectionOffer, CollectionOfferOveralForCollectorModel>();
        CreateMap<CollectionOffer, CollectionOfferOveralForHouseModel>();
        CreateMap<CollectionOfferCreateModel, CollectionOffer>()
            .ForMember(
                dest => dest.ScheduleProposals,
                opt => opt.MapFrom(src =>
                    new List<ScheduleProposalCreateModel> { src.ScheduleProposal }
                )
            );
        CreateMap<OfferDetailCreateModel, OfferDetail>();
        CreateMap<OfferDetail, OfferDetailModel>();
        CreateMap<OfferDetailUpdateModel, OfferDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        #endregion

        #region ScheduleProposal

        CreateMap<ScheduleProposalCreateModel, ScheduleProposal>();
        CreateMap<ScheduleProposal, ScheduleProposalModel>();

        #endregion

        #region ScrapCategory

        CreateMap<ScrapCategory, ScrapCategoryModel>();

        #endregion

        #region Transaction, TransactionDetail

        CreateMap<Transaction, TransactionModel>();
        CreateMap<Transaction, TransactionOveralModel>();
        CreateMap<TransactionDetail, TransactionDetailModel>();
        CreateMap<TransactionDetailCreateModel, TransactionDetail>();
        CreateMap<TransactionDetailUpdateModel, TransactionDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        ;

        #endregion
    }
}