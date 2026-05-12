using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Campaigns.DTOs;
using BookRight.FacadeLib.Commands.Campaigns.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Campaigns;

public class SetCampaignInActiveHandler(ICampaignRepository campaignRepository) : ISetCampaignInActiveHandler
{
    async Task ISetCampaignInActiveHandler.Handle(SetCampaignInActiveCommand command)
    {
        var campaign = await campaignRepository.GetByIdAsync(command.CampaignId)
            ?? throw new NotFoundException("Campaign not found");

        campaign.SetInaktive();

        await campaignRepository.SaveAsync();
    }
}
