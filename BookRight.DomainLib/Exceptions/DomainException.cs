namespace BookRight.DomainLib.Exceptions;

/// <summary>
/// Represents domain-level errors and business-rule violations.
/// </summary>
/// <remarks>Throw to indicate domain-specific validation failures or business-rule violations. Provide a
/// descriptive message when creating the exception.</remarks>
public class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DomainException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DomainException(string message) : base(message) { }
}
