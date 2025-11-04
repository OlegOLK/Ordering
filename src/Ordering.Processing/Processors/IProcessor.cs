namespace Ordering.Processing.Processors
{
    public interface IProcessor
    {
        int Order { get; }
        Task Process(ProcessingContext ctx, CancellationToken cancellationToken = default);
        Task Compensate(ProcessingContext ctx, CancellationToken cancellationToken = default);
    }
}
