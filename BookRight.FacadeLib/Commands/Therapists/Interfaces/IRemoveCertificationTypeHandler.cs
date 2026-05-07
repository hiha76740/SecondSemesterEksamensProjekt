using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface IRemoveCertificationTypeHandler
{
    Task Handle(RemoveCertificationTypeCommand command);
}