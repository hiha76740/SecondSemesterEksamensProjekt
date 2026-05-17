namespace BookRight.FacadeLib.DTO;

public record ClinicDTO(
    Guid Id,
    string Name,
    int TreatmentRoomLimit,
    IReadOnlyList<OpeningHourDTO> OpeningHours,
    string Street,
    string PostalCode,
    string City
    );
