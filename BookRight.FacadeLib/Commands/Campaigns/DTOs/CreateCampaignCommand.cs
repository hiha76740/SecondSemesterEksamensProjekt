namespace BookRight.FacadeLib.Commands.Campaigns.DTOs;

public record CreateCampaignCommand(string Name, int DiscountProcentage, DateOnly StartDate, DateOnly EndDate, Guid TreatmentId);

