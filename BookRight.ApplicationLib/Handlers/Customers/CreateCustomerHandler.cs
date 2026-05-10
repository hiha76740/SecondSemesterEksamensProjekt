
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Customers;

public class CreateCustomerHandler(ICustomerRepository customerRepository, ITherapistRepository therapistRepository) : ICreateCustomerHandler
{
    async Task ICreateCustomerHandler.Handle(CreateCustomerCommand command)
    {
        
        if (command.PreferredTherapist.HasValue == true)
        {
            Guid preferredTherapist = command.PreferredTherapist.Value;
            _ = await therapistRepository.GetByIdAsync(preferredTherapist)
                ?? throw new NotFoundException("Therapist could not be found");
        }

        var address = new Address(command.Street, command.PostalCode, command.City);
        var email = new Email(command.EmailAddress);
        var phoneNumber = new PhoneNumber(command.PhoneNumber);

        var customer = Customer.Create(command.Firstname, command.Lastname, command.Birthdate, address, email, phoneNumber, command.Note, command.PreferredTherapist);

        await customerRepository.AddAsync(customer);
        await customerRepository.SaveAsync();
    }
}
