namespace BookRight.DomainLib.Exceptions;

/// <summary>
/// Exception thrown when a requested resource or entity cannot be found.
/// </summary>
/// <remarks>Does not add behavior beyond Exception; use to represent a not-found condition (for example, to map
/// to HTTP 404) and provide a descriptive message identifying the missing resource.</remarks>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of NotFoundException with the specified error message.
    /// </summary>
    /// <remarks>The message is passed to the base Exception class and becomes the exception's Message
    /// property.</remarks>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public NotFoundException(string message) : base(message) { }
}
