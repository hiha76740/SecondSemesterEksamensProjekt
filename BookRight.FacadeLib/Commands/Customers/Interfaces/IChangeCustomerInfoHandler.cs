using BookRight.FacadeLib.Commands.Customers.DTOs;

namespace BookRight.FacadeLib.Commands.Customers.Interfaces;

public interface IChangeCustomerInfoHandler
{
    Task Handle(ChangeCustomerInfoCommand command);
}
