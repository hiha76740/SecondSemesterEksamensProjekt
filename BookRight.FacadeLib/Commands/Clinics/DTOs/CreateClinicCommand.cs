using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Commands.Clinics.DTOs;

public record CreateClinicCommand(string Name, string Street, string PostalCode, string City, int TreatmentRoomLimit, List<OpeningHourDTO> OpeningHours);
