namespace BookRight.FacadeLib.DTO;

public record ClinicDTO(
    string Name,
    int TreatmentRoomLimit,
    DateTime OpeningTime,
    DateTime CloseingTime,
    string Street,
    string PostalCode,
    string City
    );
