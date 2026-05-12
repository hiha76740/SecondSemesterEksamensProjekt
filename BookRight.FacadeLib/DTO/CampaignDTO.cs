namespace BookRight.FacadeLib.DTO;

public record CampaignDTO(string Name,decimal DiscountProcentage,DateOnly CampaignStart, DateOnly CampaignEnd, Guid TreatmentId, string Status);
