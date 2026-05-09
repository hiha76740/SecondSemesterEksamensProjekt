using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Clinics;

public class ChangeClinicInfoHandler(IClinicRepository clinicRepository) : IChangeClinicInfoHandler
{
    async Task IChangeClinicInfoHandler.Handle(ChangeClinicInfoCommand command)
    {
        bool changesMade = false;

        var clinic = await clinicRepository.GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException("Clinic not found");

        var address = new Address(command.Street, command.PostalCode, command.City);
        var openingHours = new OpeningHours(command.OpenHour, command.CloseHour);

        if (clinic.Address != address)
        {
            clinic.ChangeAddress(address);
            changesMade = true;
        }

        if (clinic.TreatmentRoomLimit != command.TreatmentRoomLimit)
        {
            clinic.ChangeTreatmentRoomLimit(command.TreatmentRoomLimit);
            changesMade = true;
        }

        if (clinic.OpeningHours != openingHours)
        {
            clinic.ChangeOpeningHours(openingHours);
            changesMade = true;
        }

        if (changesMade == true)
        {
            await clinicRepository.SaveAsync();
        }
    }
}
