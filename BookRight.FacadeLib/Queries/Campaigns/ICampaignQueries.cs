using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.Campaigns;

public interface ICampaignQueries
{
    Task<CampaignDTO?> GetByIdAsync(Guid Id);
    Task<IReadOnlyList<CampaignDTO>> GetAllAsync();
}
