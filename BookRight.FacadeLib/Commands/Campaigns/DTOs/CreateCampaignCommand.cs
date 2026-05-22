namespace BookRight.FacadeLib.Commands.Campaigns.DTOs;

public record CreateCampaignCommand(string Name, decimal DiscountPercentage, DateOnly StartDate, DateOnly EndDate, IReadOnlyList<Guid> AssignedTreatments);

