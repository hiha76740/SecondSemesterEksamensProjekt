using BookRight.DomainLib.Entities.Bookings;

namespace BookRight.DomainLib.Services
{
    public interface IValidateOverlapService
    {
        void Validate(Booking booking, IEnumerable<Booking> bookings);
    }
}
