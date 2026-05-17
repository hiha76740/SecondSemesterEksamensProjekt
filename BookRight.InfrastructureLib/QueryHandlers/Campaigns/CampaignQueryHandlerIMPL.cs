using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Campaigns;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Campaigns;

public class CampaignQueryHandlerIMPL(BookRightDbContext db) : ICampaignQueries
{
    async Task<IReadOnlyList<CampaignDTO>> ICampaignQueries.GetAllAsync()
    {
        return await db.Campaigns
            .AsNoTracking()
            .Select(c => new CampaignDTO(
                c.Id,
                c.Name,
                c.DiscountPercentage,
                c.CampaignPeriod.From,
                c.CampaignPeriod.To,
                c.AssignedTreatments,
                c.Status.ToString()))
            .ToListAsync();
    }

    async Task<CampaignDTO?> ICampaignQueries.GetByIdAsync(Guid Id)
    {
        return await db.Campaigns
            .AsNoTracking()
            .Where(c => c.Id == Id)
            .Select(c => new CampaignDTO(
                c.Id,
                c.Name,
                c.DiscountPercentage,
                c.CampaignPeriod.From,
                c.CampaignPeriod.To,
                c.AssignedTreatments,
                c.Status.ToString()
                ))
            .FirstOrDefaultAsync();
    }
}
