namespace BookRight.FacadeLib.DTO;

public record CampaignDTO(string Name,decimal DiscountProcentage,DateOnly CampaignStart, DateOnly CampaignEnd, IReadOnlyList<Guid> AssignedTreatments, string Status);
