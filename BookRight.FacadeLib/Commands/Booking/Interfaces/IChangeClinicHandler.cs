using BookRight.FacadeLib.Commands.Booking.DTOs;

namespace BookRight.FacadeLib.Commands.Booking.Interfaces;

public interface IChangeClinicHandler
{
    Task Handle(ChangeClinicCommand command);
}