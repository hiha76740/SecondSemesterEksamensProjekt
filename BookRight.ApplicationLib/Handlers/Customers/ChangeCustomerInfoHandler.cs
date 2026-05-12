
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.ApplicationLib.Handlers.Customers;

public class ChangeCustomerInfoHandler(ICustomerRepository customerRepository, ITherapistRepository therapistRepository) : IChangeCustomerInfoHandler
{
    async Task IChangeCustomerInfoHandler.Handle(ChangeCustomerInfoCommand command)
    {
        bool changesMade = false;

        var customer = await customerRepository.GetByIdAsync(command.CustomerId)
            ?? throw new NotFoundException("Customer could not be found");

        if (command.PreferredTherapist.HasValue)
        {
            Guid preferredTherapist = command.PreferredTherapist.Value;

            _ = await therapistRepository.GetByIdAsync(preferredTherapist)
                ?? throw new NotFoundException("Therapist could not be found");
        }

        var address = new Address(command.Street, command.PostalCode, command.City);
        var email = new Email(command.EmailAddress);
        var phoneNumber = new PhoneNumber(command.PhoneNumber);


        if (customer.Firstname != command.Firstname)
        {
            customer.ChangeFirstname(command.Firstname);
            changesMade = true;
        }

        if (customer.Lastname != command.Lastname)
        {
            customer.ChangeLastname(command.Lastname);
            changesMade = true;
        }

        if (customer.Address != address)
        {
            customer.ChangeAddress(address);
            changesMade = true;
        }

        if (customer.Email != email)
        {
            customer.ChangeEmail(email);
            changesMade = true;
        }

        if (customer.PhoneNumber != phoneNumber)
        {
            customer.ChangePhoneNumber(phoneNumber);
            changesMade = true;
        }

        if (customer.Note != command.Note)
        {
            customer.ChangeNote(command.Note);
            changesMade = true;
        }

        if (customer.PreferredTherapist != command.PreferredTherapist)
        {
            customer.ChangePreferredTherapist(command.PreferredTherapist);
            changesMade = true;
        }


        if (changesMade == true)
            await customerRepository.SaveAsync();
    }
}
