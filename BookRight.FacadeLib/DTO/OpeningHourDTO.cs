namespace BookRight.FacadeLib.DTO;

public record OpeningHourDTO(string Weekday, TimeOnly? OpeningTime, TimeOnly? ClosingTime, bool IsClosed);
