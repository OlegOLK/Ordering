using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Eventing;
using Ordering.Domain.Messaging.Messages;
using Ordering.Persistance.EventStreaming;
using Ordering.Persistance.Repositories;
using Ordering.Processing.Exceptions;
using Ordering.Processing.Processors;

namespace Ordering.Processing.Consumers
{
    public class CreateOrderMessageConsumer : IConsumer<CreateOrderMessage>
    {
        private readonly IEnumerable<IProcessor> _processors;
        private readonly ILogger<CreateOrderMessageConsumer> _logger;
        private readonly IOutboxEventRepository _outboxEventRepository;
        public CreateOrderMessageConsumer(
            ILogger<CreateOrderMessageConsumer> logger,
            IEnumerable<IProcessor> processors,
            IOutboxEventRepository outboxEventRepository)
        {
            _logger = logger;
            _processors = processors;
            _outboxEventRepository = outboxEventRepository;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessage> context)
        {
            ProcessingContext ctx = new ProcessingContext
            {
                OrderId = context.Message.OrderId,
                TransactionId = context.Message.Id
            };
            HashSet<IProcessor> completedSteps = new HashSet<IProcessor>();
            string error = string.Empty;
            EventState state = EventState.Completed;
            int retry = 0;
            foreach (IProcessor processor in _processors.OrderBy(x => x.Order))
            {
                try
                {
                    await processor.Process(ctx);
                    completedSteps.Add(processor);
                }
                catch (ProcessingException ex)
                {
                    _logger.LogError(ex, "Rolling back proccessing.");
                    foreach (IProcessor item in completedSteps)
                    {
                        await item.Compensate(ctx);
                    }
                    state = EventState.Failed;
                    error = ex.Message;
                    retry++;
                }
            }

            OutboxEventEntity eventEntity = _outboxEventRepository.GetAll().Where(x => x.Id == context.Message.EventId).Single();
            eventEntity.ErrorMessage = error;
            eventEntity.RetryCount += retry;
            eventEntity.EventState = state;

            await _outboxEventRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
