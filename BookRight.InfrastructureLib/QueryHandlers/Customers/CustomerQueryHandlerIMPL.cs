using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Customers;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Customers;

public class CustomerQueryHandlerIMPL(BookRightDbContext db) : ICustomerQueries
{
    Task<CustomerDTO?> ICustomerQueries.GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    Task<IReadOnlyList<CustomerDTO>> ICustomerQueries.GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
