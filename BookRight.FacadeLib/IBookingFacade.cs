using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib;

// Facade-interface: kontrakt mellem UI-laget og Application-laget.
// UI kalder Facade, og Facade sender request videre til den relevante use case.

public interface IBookingFacade
{
    Task CancelBookingAsync(CancelBookingRequest request);
}