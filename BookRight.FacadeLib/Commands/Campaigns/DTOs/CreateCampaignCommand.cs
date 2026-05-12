namespace BookRight.FacadeLib.Commands.Campaigns.DTOs;

public record CreateCampaignCommand(string Name, int DiscountPercentage, DateOnly StartDate, DateOnly EndDate, IReadOnlyList<Guid> AssignedTreatments);

