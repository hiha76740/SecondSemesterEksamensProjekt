using BookRight.FacadeLib.Booking.DTOs;

namespace BookRight.FacadeLib.Booking.Interfaces;

public interface IChangeTimeHandler
{
    Task Handle(ChangeTimeCommand command);
}