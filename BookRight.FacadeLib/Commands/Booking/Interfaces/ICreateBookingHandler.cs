using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface ICreateBookingHandler
{
    Task Handle(CreateBookingCommand command);
}