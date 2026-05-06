using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IChangeTherapistHandler
{
    Task Handle(ChangeTherapistCommand command);
}