using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IChangeTimeHandler
{
    Task Handle(ChangeTimeCommand command);
}