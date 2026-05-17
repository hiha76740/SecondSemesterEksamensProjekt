
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

        if (command.PreferredTherapistId.HasValue)
        {
            Guid preferredTherapistId = command.PreferredTherapistId.Value;

            _ = await therapistRepository.GetByIdAsync(preferredTherapistId)
                ?? throw new NotFoundException("Therapist could not be found");
        }

        var address = new Address(command.Street, command.PostalCode, command.City);
        var email = new Email(command.EmailAddress);
        var phoneNumber = new PhoneNumber(command.PhoneNumber);


        if (customer.FirstName != command.FirstName)
        {
            customer.ChangeFirstName(command.FirstName);
            changesMade = true;
        }

        if (customer.LastName != command.LastName)
        {
            customer.ChangeLastName(command.LastName);
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

        if (customer.PreferredTherapistId != command.PreferredTherapistId)
        {
            customer.ChangePreferredTherapist(command.PreferredTherapistId);
            changesMade = true;
        }


        if (changesMade == true)
            await customerRepository.SaveAsync();
    }
}
