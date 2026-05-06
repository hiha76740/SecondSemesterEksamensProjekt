using BookRight.FacadeLib.Booking.DTOs;

namespace BookRight.FacadeLib.Booking.Interfaces;

public interface IRegisterNoShowHandler
{
    Task Handle(RegisterNoShowCommand command);
}