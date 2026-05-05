using BookRight.DomainLib.Entities.Customers;

namespace BookRight.ApplicationLib.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task AddAsync(Customer customer);
    Task SaveAsync();
}
