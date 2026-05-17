using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Discounts.DiscountStrategies;

namespace BookRight.Web.DependencyInjections
{
    public static class DiscountStrategyDI
    {
        public static IServiceCollection AddDiscountStrategyDI(this IServiceCollection services)
        {
            services.AddTransient<IDiscountStrategy, BronzeLoyalityDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, SilverLoyalityDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, GoldLoyalityDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, BirthDateDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, CampaignDiscountStrategy>();

            return services;
        }
    }
}
