namespace Ordering.Processing.Exceptions;

/// <summary>
/// ProcessingException
/// </summary>
/// <seealso cref="System.Exception" />
public class ProcessingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessingException"/> class.
    /// </summary>
    public ProcessingException()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessingException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProcessingException(string message)
        : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessingException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public ProcessingException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
