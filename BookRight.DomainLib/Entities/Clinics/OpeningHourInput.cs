using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Clinics;

public record OpeningHourInput(Weekdays Weekday, TimeOnly? Open, TimeOnly? Close, bool IsClosed);
