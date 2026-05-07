using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface IAddClinicToTherapistHandler
{
    Task Handle(AddClinicToTherapistCommand command);
}