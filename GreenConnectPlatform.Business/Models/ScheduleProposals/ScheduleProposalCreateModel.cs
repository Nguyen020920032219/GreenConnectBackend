using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.ScheduleProposals;

public class ScheduleProposalCreateModel
{
    [Required(ErrorMessage = "ProposedTime is required")]
    public DateTime ProposedTime { get; set; }

    public string? ResponseMessage { get; set; }
}