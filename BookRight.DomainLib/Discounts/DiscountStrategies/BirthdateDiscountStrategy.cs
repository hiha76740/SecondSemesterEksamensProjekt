using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

/// <summary>
/// Applies a birthday-month discount when the booking month matches the customer’s birth month and the annual usage
/// limit has not been exceeded.
/// </summary>
/// <remarks>Uses a fixed 25% discount and allows one use per year by default. Implements IDiscountStrategy and
/// returns DiscountTypes.BirthdayMonth in the PriceCalculatorResult.</remarks>
public class BirthDateDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;
    private readonly int _maxUsePerYear;

    public BirthDateDiscountStrategy()
    {
        _discountPercentage = 25;
        _maxUsePerYear = 1;

    }

    public DiscountTypes DiscountTypes => DiscountTypes.BirthdayMonth;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.BookingDate.Month == input.CustomerBirthDate.Month &&
            input.NumberOfBirthdayDiscountUsed < _maxUsePerYear)
        {
            price = input.NormalPrice * (1 - _discountPercentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
