using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Entities.Treatments;

namespace BookRight.DomainLib.Discounts;

public record PriceCalculatorInput(
    decimal NormalPrice,
    DateTime BookingDate,
    DateTime CustomerBirthdate,
    decimal CustomerBookingsLast12Months,
    int NumberOfBirthdayDiscountUsed,
    IReadOnlyList<Treatment> Treatments,
    IReadOnlyList<Campaign> ActiveCampaigns
    );