
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Customers;

public class CreateCustomerHandler : ICreateCustomerHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITherapistRepository _therapistRepository;

    public CreateCustomerHandler(
        ICustomerRepository customerRepo,
        ITherapistRepository therapistRepo)
    {
        _customerRepository = customerRepo;
        _therapistRepository = therapistRepo;
    }

    async Task ICreateCustomerHandler.Handle(CreateCustomerCommand command)
    {
        
        if (command.TherapistId.HasValue)
        {
            Guid therapistId = command.TherapistId.Value;
            _ = await _therapistRepository.GetByIdAsync(therapistId)
                ?? throw new NotFoundException("Therapist could not be found");
        }

        var address = new Address(command.Street, command.PostalCode, command.City);
        var email = new Email(command.EmailAddress);
        var phoneNumber = new PhoneNumber(command.PhoneNumber);

        var customer = Customer.Create(command.Firstname, command.Lastname, command.Birthdate, address, email, phoneNumber, command.Note, command.TherapistId);

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveAsync();
        
    }
}
