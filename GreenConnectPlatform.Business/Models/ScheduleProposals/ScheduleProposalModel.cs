using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScheduleProposals;

public class ScheduleProposalModel
{
    public Guid ScheduleProposalId { get; set; }

    public Guid CollectionOfferId { get; set; }

    public DateTime ProposedTime { get; set; }

    public ProposalStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ResponseMessage { get; set; }
}