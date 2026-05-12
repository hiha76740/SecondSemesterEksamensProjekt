using BookRight.FacadeLib.Commands.Campaigns.DTOs;

namespace BookRight.FacadeLib.Commands.Campaigns.Interfaces;

public interface ISetCampaignInActiveHandler
{
    Task Handle(SetCampaignInActiveCommand command);
}
