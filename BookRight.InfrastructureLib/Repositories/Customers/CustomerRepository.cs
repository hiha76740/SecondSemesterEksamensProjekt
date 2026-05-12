using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;

namespace BookRight.InfrastructureLib.Repositories.Customers;

public class CustomerRepository(BookRightDbContext db) : ICustomerRepository
{
    async Task<Customer?> ICustomerRepository.GetByIdAsync(Guid id)
    {
       return await db.Customers.FindAsync(id); 
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
