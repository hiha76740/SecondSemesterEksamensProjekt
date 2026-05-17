using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Enums;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Repositories.Campaigns;

public class CampaignRepository(BookRightDbContext db) : ICampaignRepository
{
    async Task ICampaignRepository.AddAsync(Campaign campaign)
    {
        await db.Campaigns.AddAsync(campaign);
    }

    async Task<IReadOnlyList<Campaign>> ICampaignRepository.GetActiveCampaignsAsync()
    {
        return await db.Campaigns
            .Where(c => c.Status == CampaignStatus.Active)
            .ToListAsync();
    }

    async Task<Campaign?> ICampaignRepository.GetByIdAsync(Guid campaignId)
    {
        return await db.Campaigns.FirstOrDefaultAsync(c => c.Id == campaignId);
    }

    async Task ICampaignRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
