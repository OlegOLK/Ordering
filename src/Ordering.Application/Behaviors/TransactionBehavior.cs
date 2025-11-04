using Cortex.Mediator.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Eventing.Services;
using Ordering.Persistance;

namespace Ordering.Application.Behaviors
{
    /// <summary>
    /// TransactionBehavior
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="Cortex.Mediator.Commands.ICommandPipelineBehavior&lt;TRequest, TResponse&gt;" />
    public class TransactionBehavior<TRequest, TResponse>
        : ICommandPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly IDbContext _dbContext;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;


        /// <summary>
        /// The event publishing service
        /// </summary>
        private readonly IEventPublishingService _eventPublishingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionBehavior{TRequest, TResponse}" /> class.
        /// </summary>
        /// <param name="eventPublishingService">The event publishing service.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public TransactionBehavior(
            IEventPublishingService eventPublishingService,
            IDbContext dbContext,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _eventPublishingService = eventPublishingService;
            _dbContext = dbContext;
            _logger = logger;
        }
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="next">The next.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<TResponse?> Handle(TRequest command, CommandHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? response = default;

            IExecutionStrategy executionStrategy = _dbContext.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await _dbContext.UnitOfWork.BeginTransactionAsync();
                using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                {
                    response = await next();
                    await _dbContext.UnitOfWork.CommitTransactionAsync(transaction);
                    transactionId = transaction.TransactionId;
                }

                await _eventPublishingService.PropagateEvent(transactionId);
            });

            return response;
        }
    }
}
