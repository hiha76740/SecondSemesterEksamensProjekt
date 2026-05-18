namespace BookRight.FacadeLib.DTO;

public record OpeningHourDTO(
    string WeekDay,
    TimeOnly? OpeningTime,
    TimeOnly? ClosingTime,
    bool IsClosed);
