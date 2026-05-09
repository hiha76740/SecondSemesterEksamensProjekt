namespace BookRight.FacadeLib.Commands.Clinics.DTOs;

public record ChangeClinicInfoCommand(Guid ClinicId, string Street, string PostalCode, string City, int TreatmentRoomLimit, DateTime OpenHour, DateTime CloseHour);
