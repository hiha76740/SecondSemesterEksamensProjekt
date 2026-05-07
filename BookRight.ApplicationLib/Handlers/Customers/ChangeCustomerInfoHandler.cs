
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

        customer.ChangeFirstname(command.Firstname);
        customer.ChangeLastname(command.Lastname);
        customer.ChangeAddress(address);
        customer.ChangeEmail(email);
        customer.ChangePhoneNumber(phoneNumber);
        customer.ChangeNote(command.Note);
        customer.ChangePreferredTherapist(command.PreferredTherapist);

        await customerRepository.SaveAsync();
    }
}
