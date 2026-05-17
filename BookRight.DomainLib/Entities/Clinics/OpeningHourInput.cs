using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Clinics;

public record OpeningHourInput(WeekDays WeekDay, TimeOnly? Open, TimeOnly? Close, bool IsClosed);
