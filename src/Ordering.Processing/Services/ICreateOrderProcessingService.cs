using Ordering.Domain.Messaging.Messages;

namespace Ordering.Processing.Services;

/// <summary>
/// ICreateOrderProcessingService
/// </summary>
public interface ICreateOrderProcessingService
{
    /// <summary>
    /// Processes the message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    Task ProcessMessage(CreateOrderMessage message);
}
