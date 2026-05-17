using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Campaigns.DTOs;
using BookRight.FacadeLib.Commands.Campaigns.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Campaigns;

public class CreateCampaignHandler(ICampaignRepository campaignRepository, ITreatmentRepository treatmentRepository) : ICreateCampaignHandler
{
    async Task ICreateCampaignHandler.Handle(CreateCampaignCommand command)
    {
        foreach (var treatmentId in command.AssignedTreatments)
        {
            _ = await treatmentRepository.GetByIdAsync(treatmentId)
           ?? throw new NotFoundException($"Treatment with id {treatmentId} not found");
        }

        var campaignPeriod = new CampaignPeriod(command.StartDate, command.EndDate);

        var campaign = Campaign.Create(command.Name, command.DiscountPercentage, campaignPeriod, command.AssignedTreatments);

        await campaignRepository.AddAsync(campaign);
        await campaignRepository.SaveAsync();
    }
}
