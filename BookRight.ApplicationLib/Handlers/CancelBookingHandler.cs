using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Handlers;

namespace BookRight.ApplicationLib.Handlers;

public class CancelBookingHandler : ICancelBookingHandler
{
    private readonly IBookingRepository _bookingRepository;

    public CancelBookingHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(CancelBookingCommand command)
    {
        if (command.BookingId == Guid.Empty)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("BookingId cannot be empty.");

        var booking = await _bookingRepository.GetByIdAsync(command.BookingId);

        if (booking is null)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("Booking could not be found.");

        booking.Cancel();

        await _bookingRepository.Save();
    }
}