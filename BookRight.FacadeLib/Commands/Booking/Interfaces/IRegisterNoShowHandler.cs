using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IRegisterNoShowHandler
{
    Task Handle(RegisterNoShowCommand command);
}