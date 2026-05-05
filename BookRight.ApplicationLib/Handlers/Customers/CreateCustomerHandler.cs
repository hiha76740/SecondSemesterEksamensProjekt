
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Handlers;

namespace BookRight.ApplicationLib.Handlers.Customers;

public class CreateCustomerHandler : ICreateCustomerHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITherapistRepository _therapistRepository;

    public CreateCustomerHandler(
        ICustomerRepository customerRepo,
        ITherapistRepository therapistRepo)
    {
        _customerRepository = customerRepo;
        _therapistRepository = therapistRepo;
    }
}
