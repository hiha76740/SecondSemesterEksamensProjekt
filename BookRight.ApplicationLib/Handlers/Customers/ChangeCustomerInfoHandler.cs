
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Customers;

public class ChangeCustomerInfoHandler(ICustomerRepository customerRepository, ITherapistRepository therapistRepository) : IChangeCustomerInfoHandler
{
    async Task IChangeCustomerInfoHandler.Handle(ChangeCustomerInfoCommand command)
    {

    }
}
