using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Handlers;

public interface IRegisterNoShowHandler
{
    Task Handle(RegisterNoShowCommand command);
}