using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IRemoveParticipantHandler
{
    Task Handle(RemoveParticipantCommand command);
}
