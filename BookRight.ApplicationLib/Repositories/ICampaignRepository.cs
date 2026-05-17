using BookRight.DomainLib.Entities.Campaigns;

namespace BookRight.ApplicationLib.Repositories
{
    public interface ICampaignRepository
    {
        Task AddAsync(Campaign campaign);
        Task<IReadOnlyList<Campaign>> GetActiveCampaignsAsync();
        Task<Campaign?> GetByIdAsync(Guid campaignId);
        Task SaveAsync();
    }
}