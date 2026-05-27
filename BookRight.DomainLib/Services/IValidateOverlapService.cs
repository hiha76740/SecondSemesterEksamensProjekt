using BookRight.DomainLib.Entities.Bookings;

namespace BookRight.DomainLib.Services
{
    /// <summary>
    /// Defines a service that validates whether a booking overlaps any bookings in a provided collection.
    /// </summary>
    /// <remarks>Implementations should detect overlapping time ranges and signal validation failures via
    /// exceptions or domain-specific results. Consider time zone handling, inclusive/exclusive boundaries for adjacent
    /// bookings, and thread-safety for concurrent use.</remarks>
    public interface IValidateOverlapService
    {
        /// <summary>
        /// Validates a booking against a collection of existing bookings.
        /// </summary>
        /// <remarks>Throws ArgumentNullException if booking or bookings is null. Throws
        /// InvalidOperationException if validation fails, for example due to overlapping bookings or other
        /// business-rule violations.</remarks>
        /// <param name="booking">The booking to validate.</param>
        /// <param name="bookings">A collection of existing bookings to validate against.</param>
        void Validate(Booking booking, IEnumerable<Booking> bookings);
    }
}
