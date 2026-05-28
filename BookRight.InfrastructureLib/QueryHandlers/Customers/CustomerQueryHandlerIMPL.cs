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
            .Where(c => c.Id == Id)
            .Select(c => new CustomerDTO(
                c.Id,
                c.FirstName,
                c.LastName,
                c.BirthDate,
                c.Note,
                c.Address.Street,
                c.Address.PostalCode,
                c.Address.City,
                c.Email.EmailAddress,
                c.PhoneNumber.Number,
                c.PreferredTherapistId))
            .FirstOrDefaultAsync();
    }

    async Task<IReadOnlyList<CustomerDTO>> ICustomerQueries.GetAllAsync()
    {
        return await db.Customers
            .AsNoTracking()
            .Select(c => new CustomerDTO(
                c.Id,
                c.FirstName,
                c.LastName,
                c.BirthDate,
                c.Note,
                c.Address.Street,
                c.Address.PostalCode,
                c.Address.City,
                c.Email.EmailAddress,
                c.PhoneNumber.Number,
                c.PreferredTherapistId))
            .ToListAsync();
    }

    async Task<IReadOnlyList<CustomerDTO>> ICustomerQueries.SearchCustomerAsync(string filter)
    {
        return await db.Customers
           .AsNoTracking()
           .Where(c =>
           c.FirstName.Contains(filter) ||
           c.LastName.Contains(filter) ||
           c.Email.EmailAddress.Contains(filter) ||
           c.PhoneNumber.Number.Contains(filter))
           .Select(c => new CustomerDTO(
               c.Id,
               c.FirstName,
               c.LastName,
               c.BirthDate,
               c.Note,
               c.Address.Street,
               c.Address.PostalCode,
               c.Address.City,
               c.Email.EmailAddress,
               c.PhoneNumber.Number,
               c.PreferredTherapistId))
           .ToListAsync();
    }

    async Task<CustomerDTO?> ICustomerQueries.GetByPhoneNumberAsync(string phoneNumber)
    {
        return await db.Customers
         .AsNoTracking()
         .Where(c => c.PhoneNumber.Number == phoneNumber)
         .Select(c => new CustomerDTO(
             c.Id,
             c.FirstName,
             c.LastName,
             c.BirthDate,
             c.Note,
             c.Address.Street,
             c.Address.PostalCode,
             c.Address.City,
             c.Email.EmailAddress,
             c.PhoneNumber.Number,
             c.PreferredTherapistId))
         .FirstOrDefaultAsync();
    }
}
