using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
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


        foreach (var oh in command.OpeningHours)
        {
            bool exsists = Enum.TryParse<Weekdays>(oh.Weekday, out var weekday);

            if (exsists == false)
                throw new NotFoundException($"Weekday {oh.Weekday} was not found");


            var changed = clinic.OpeningHours
                .Any(x => x.Weekday == weekday &&
                x.OpeningTime != oh.OpeningTime ||
                x.CloseingTime != oh.ClosingTime);


            if (changed == true)
            {
                var id = clinic.OpeningHours.Where(x => x.Weekday == weekday).Select(x => x.Id).FirstOrDefault();

                var opningHourInput = new OpeningHourInput(weekday, oh.OpeningTime, oh.ClosingTime, oh.IsClosed);

                clinic.ChangeOpeningHour(id, opningHourInput);
                changesMade = true;
            }
        }

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

        if (changesMade == true)
        {
            await clinicRepository.SaveAsync();
        }
    }
}
