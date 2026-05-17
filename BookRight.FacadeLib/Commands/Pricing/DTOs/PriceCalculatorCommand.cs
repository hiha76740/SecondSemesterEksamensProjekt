namespace BookRight.FacadeLib.Commands.Pricing.DTOs;

public record PriceCalculatorCommand(Guid CustomerId, Guid TreatmentId, DateTime BookingDate);
