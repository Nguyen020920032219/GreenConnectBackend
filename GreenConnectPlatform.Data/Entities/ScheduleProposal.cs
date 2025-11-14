using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class ScheduleProposal
{
    public Guid ScheduleProposalId { get; set; }

    public Guid CollectionOfferId { get; set; }

    public Guid ProposerId { get; set; }

    public DateTime ProposedTime { get; set; }

    public ProposalStatus Status { get; set; } = ProposalStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ResponseMessage { get; set; }

    public virtual User Proposer { get; set; } = null!;

    public virtual CollectionOffer Offer { get; set; } = null!;
}