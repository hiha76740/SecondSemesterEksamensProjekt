using BookRight.FacadeLib.Commands.Clinics.DTOs;

namespace BookRight.FacadeLib.Commands.Clinics.Interfaces;

public interface ICreateClinicHandler
{
    Task Handle(CreateClinicCommand command);
}
