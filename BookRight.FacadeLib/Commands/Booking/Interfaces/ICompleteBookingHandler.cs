using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface ICompleteBookingHandler
{
    Task Handle(CompleteBookingCommand command);
}