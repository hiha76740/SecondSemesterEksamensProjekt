using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface IAddCertificationTypeHandler
{
    Task Handle(AddCertificationTypeCommand command);
}