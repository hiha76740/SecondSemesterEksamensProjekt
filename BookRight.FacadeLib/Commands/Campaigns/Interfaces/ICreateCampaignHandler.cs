using BookRight.FacadeLib.Commands.Campaigns.DTOs;

namespace BookRight.FacadeLib.Commands.Campaigns.Interfaces;

public interface ICreateCampaignHandler
{
    Task Handle(CreateCampaignCommand command);
}
