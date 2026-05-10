using BookRight.FacadeLib.Commands.Customers.DTOs;

namespace BookRight.FacadeLib.Commands.Customers.Interfaces;

public interface ICreateCustomerHandler
{
    Task Handle(CreateCustomerCommand command);
}
