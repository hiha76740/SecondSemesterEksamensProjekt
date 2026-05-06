using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IChangeTreatmentHandler
{
    Task Handle(ChangeTreatmentCommand command);
}