using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Handlers;

public interface IAddCertificationTypeHandler
{
    Task Handle(AddCertificationTypeCommand command);
}