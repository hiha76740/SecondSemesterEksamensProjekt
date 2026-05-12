using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;

namespace BookRight.InfrastructureLib.Repositories.Customers;

public class CustomerRepository(BookRightDbContext db) : ICustomerRepository
{
    async Task<Customer?> ICustomerRepository.GetByIdAsync(Guid id)
    {
       return await db.Customers.FindAsync(id); 
    }
    async Task ICustomerRepository.AddAsync(Customer customer)
    {
        await db.Customers.AddAsync(customer);
    }

    Task ICustomerRepository.SaveAsync()
    {
        throw new NotImplementedException();
    }
}
