using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Repositories.Customers;

public class CustomerRepository(BookRightDbContext db) : ICustomerRepository
{
    async Task<Customer?> ICustomerRepository.GetByIdAsync(Guid id)
    { 
       return await db.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    async Task ICustomerRepository.AddAsync(Customer customer)
    {
        await db.Customers.AddAsync(customer);
    }

    async Task ICustomerRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
