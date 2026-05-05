using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Handlers;

public interface ICancelBookingHandler
{
    Task Handle(CancelBookingCommand command);
}