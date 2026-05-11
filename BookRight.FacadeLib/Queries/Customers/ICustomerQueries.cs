using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.Customers;

public interface ICustomerQueries
{
    Task<CustomerDTO?> GetByIdAsync(Guid Id);
    Task<IReadOnlyList<CustomerDTO>> GetAllAsync();
}
