using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.ScheduleProposals;

public class ScheduleProposalCreateModel
{
    [Required(ErrorMessage = "ProposedTime là bắt buộc")]
    public DateTime ProposedTime { get; set; }

    public string? ResponseMessage { get; set; }
}