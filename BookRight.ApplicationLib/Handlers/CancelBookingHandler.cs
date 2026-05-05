using BookRight.ApplicationLib.Repositories;

namespace BookRight.ApplicationLib.Handlers;

public class CancelBookingHandler
{
    private readonly IBookingRepository _bookingRepository;

    public CancelBookingHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task HandleAsync(Guid bookingId)
    {
        if (bookingId == Guid.Empty)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("BookingId cannot be empty.");

        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking is null)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("Booking was not found.");

        booking.Cancel();

        await _bookingRepository.Save();
    }
}