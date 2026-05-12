using BookRight.DomainLib.Entities.Campaigns;

namespace BookRight.DomainLib.Discounts;

public record PriceCalculatorInput(
    decimal NormalPrice,
    DateTime BookingDate,
    DateTime CustomerBirthdate,
    decimal CustomerBookingsLast12Months,
    int NumberOfBirthdayDiscountUsed,
    IReadOnlyList<Campaign> ActiveCampaigns
    );