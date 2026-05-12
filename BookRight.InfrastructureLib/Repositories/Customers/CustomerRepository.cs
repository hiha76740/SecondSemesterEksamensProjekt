using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;

namespace BookRight.InfrastructureLib.Repositories.Customers;

public class CustomerRepository(BookRightDbContext db) : ICustomerRepository
{
    Task<Customer?> ICustomerRepository.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task ICustomerRepository.AddAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    Task ICustomerRepository.SaveAsync()
    {
        throw new NotImplementedException();
    }
}
