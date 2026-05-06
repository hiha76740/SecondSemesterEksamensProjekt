using BookRight.FacadeLib.Booking.DTOs;

namespace BookRight.FacadeLib.Booking.Interfaces;

public interface IChangeTherapistHandler
{
    Task Handle(ChangeTherapistCommand command);
}