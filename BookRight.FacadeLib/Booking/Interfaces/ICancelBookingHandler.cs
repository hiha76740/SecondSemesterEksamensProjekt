using BookRight.FacadeLib.Booking.DTOs;

namespace BookRight.FacadeLib.Booking.Interfaces;

public interface ICancelBookingHandler
{
    Task Handle(CancelBookingCommand command);
}