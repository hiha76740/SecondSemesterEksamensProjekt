using BookRight.FacadeLib.Commands.Pricing.DTOs;
using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Commands.Pricing.Interfaces;

public interface IPriceCalculatorService
{
    Task<PriceCalculatorDTO> CalculatePrice(PriceCalculatorCommand command);
}
