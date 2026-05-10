using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IAddParticipantHandler
{
    Task Handle(AddParticipantCommand command);
}
