using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class ChangeTherapistInfoHandler(ITherapistRepository therapistRepository) : IChangeTherapistInfoHandler
{
    async Task IChangeTherapistInfoHandler.Handle(ChangeTherapistInfoCommand command)
    {
        bool changesMade = false;

        var therapist = await therapistRepository.GetByIdAsync(command.TherapistId)
          ?? throw new NotFoundException("Therapist could not be found");

        var address = new Address(
          command.Street,
          command.PostalCode,
          command.City);

        var email = new Email(
          command.EmailAddress);

        var phoneNumber = new PhoneNumber(
          command.PhoneNumber);
    }
}