using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.Eventing;
using Ordering.Domain.Eventing.Events;
using Ordering.Persistance.Repositories;
using System.Text.Json;

namespace Ordering.Persistance.EventStreaming
{
    public class EventStreamingService : IEventStreamingService
    {
        private readonly IOutboxEventRepository _outboxEventRepository;
        public EventStreamingService(IOutboxEventRepository outboxEventRepository)
        {
            _outboxEventRepository = outboxEventRepository;
        }
        public async Task<IEnumerable<OutboxEvent>> GetPendingEvents(Guid transactionId)
        {
            List<OutboxEventEntity> entities = await _outboxEventRepository
                 .GetAll()
                 .Where(x => x.EventState == Domain.Eventing.EventState.Added && x.TransactionId == transactionId)
                 .ToListAsync();

            if (!entities.Any())
            {
                return [];
            }

            return entities.Select(x => JsonSerializer.Deserialize<OutboxEvent>(x.Content));
        }

        public Task PropageEvent(Guid eventId, EventState eventState)
        {
            OutboxEventEntity entity = _outboxEventRepository.GetAll().Where(x => x.Id == eventId).Single();
            switch (eventState)
            {
                case EventState.InProgress:
                case EventState.Completed:
                    {
                        entity.EventState = eventState;
                        break;
                    }
                case EventState.Failed:
                    {
                        entity.EventState = EventState.Added;
                        entity.RetryCount += 1;
                        break;
                    }
            }

            return _outboxEventRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public Task SaveEventAsync(OutboxEvent @event, IDbContextTransaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            OutboxEventEntity outboxEventEntity = new OutboxEventEntity(@event, transaction.TransactionId);

            _outboxEventRepository.Add(outboxEventEntity);

            return _outboxEventRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
