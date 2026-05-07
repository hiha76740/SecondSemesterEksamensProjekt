using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Handlers;

public interface IAddCertificationTypeHandler
{
    Task Handle(AddCertificationTypeCommand command);
}