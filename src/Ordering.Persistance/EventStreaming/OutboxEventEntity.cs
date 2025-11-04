using Ordering.Domain.Eventing;
using Ordering.Domain.Eventing.Events;
using Ordering.Domain.Models;
using System.Text.Json;

namespace Ordering.Persistance.EventStreaming
{
    public class OutboxEventEntity
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }

        private OutboxEventEntity()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxEventEntity"/> class.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        public OutboxEventEntity(OutboxEvent @event, Guid transactionId)
        {
            Id = @event.Id;
            TransactionId = transactionId;
            EventState = EventState.Added;
            RetryCount = 0;
            CreatedOn = DateTime.UtcNow;
            Content = JsonSerializer.Serialize(@event);
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; private set; }


        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; private set; }

        /// <summary>
        /// Gets or sets the state of the event.
        /// </summary>
        /// <value>
        /// The state of the event.
        /// </value>
        public EventState EventState { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public DateTime CreatedOn { get; set; }
    }
}
