using Ordering.Domain.Messaging.Messages;

namespace Ordering.Domain.Messaging.Services;

/// <summary>
/// IMessageService
/// </summary>
public interface IMessageService<TMessage> where TMessage : BaseMessage
{
    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    Task PublishAsync(TMessage message);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    Task ConsumeAsync(TMessage message);
}
