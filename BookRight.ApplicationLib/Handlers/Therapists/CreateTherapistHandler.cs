using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class CreateTherapistHandler : ICreateTherapistHandler
{
    private readonly ITherapistRepository _therapistRepository;

    public CreateTherapistHandler(
        ITherapistRepository therapistRepository)
    {
        _therapistRepository = therapistRepository;
    }


    async Task ICreateTherapistHandler.Handle(
        CreateTherapistCommand command)
    {
        var address = new Address(
            command.Street,
            command.PostalCode,
            command.City);

        var email = new Email(
            command.EmailAddress);

        var phoneNumber = new PhoneNumber(
            command.PhoneNumber);


        var therapist = Therapist.Create(
            command.AuthorizationNumber,
            command.Name,
            command.HourlyRate,
            address,
            email,
            phoneNumber);


        await _therapistRepository.AddAsync(therapist);

        await _therapistRepository.SaveAsync();
    }
}