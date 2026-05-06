using BookRight.FacadeLib.Booking.DTOs;

namespace BookRight.FacadeLib.Booking.Interfaces;

public interface IChangeTreatmentHandler
{
    Task Handle(ChangeTreatmentCommand command);
}