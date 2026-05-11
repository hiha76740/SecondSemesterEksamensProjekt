namespace BookRight.DomainLib.Discounts;

public record PriceCalculatorInput(
    decimal NormalPrice,
    DateTime BookingDate,
    DateTime CustomerBirthdate,
    decimal CustomerBookingsLast12Months,
    int NumberOfBirthdayDiscountUsed
    );