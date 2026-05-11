using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class CampaignDiscountStrategy : IDiscountStrategy
{
    private readonly DateTime _campainStart;
    private readonly DateTime _campainEnd;
    private readonly decimal _discountProcentage;

    public CampaignDiscountStrategy(decimal discountProcentage, DateTime campainStart, DateTime campainEnd)
    {
        _campainStart = campainStart;
        _campainEnd = campainEnd;
        _discountProcentage = discountProcentage;
    }

    public DiscountTypes discountTypes => DiscountTypes.Campaign;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = 0;

        if (input.BookingDate >= _campainStart && input.BookingDate <= _campainEnd)
            price = input.NormalPrice * (1 - _discountProcentage / 100);

        return new PriceCalculatorResult(input.NormalPrice, price, discountTypes);
    }
}
