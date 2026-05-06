using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Handlers;

public interface ICreateCustomerHandler
{
    Task Handle(CreateCustomerCommand command);
}
