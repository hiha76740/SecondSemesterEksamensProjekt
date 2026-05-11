using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Customers;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Customers;

public class CustomerQueryHandlerIMPL(BookRightDbContext db) : ICustomerQueries
{
    async Task<CustomerDTO?> ICustomerQueries.GetByIdAsync(Guid Id)
    {
        return await db.Customers
            .AsNoTracking()
            .Select(c => new CustomerDTO(
                c.Id,
                c.Firstname,
                c.Lastname,
                c.Birthdate,
                c.Note,
                c.Address.Street,
                c.Address.PostalCode,
                c.Address.City,
                c.Email.EmailAddress,
                c.PhoneNumber.Number,
                c.PrefferedTherapist))
            .FirstOrDefaultAsync();
    }

    Task<IReadOnlyList<CustomerDTO>> ICustomerQueries.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    // TODO: Consider also adding the name of PrefferedTherapist to CustomerDTO and finish the implementation.
}
