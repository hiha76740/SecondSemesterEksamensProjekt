using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface IRemoveCertificationFromTherapistHandler
{
    Task Handle(RemoveCertificationTypeCommand command);
}