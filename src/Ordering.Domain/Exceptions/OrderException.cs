namespace Ordering.Domain.Exceptions;

/// <summary>
/// OrderException
/// </summary>
/// <seealso cref="Exception" />
public class OrderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderException"/> class.
    /// </summary>
    public OrderException()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public OrderException(string message)
        : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public OrderException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
