using Microsoft.AspNetCore.Mvc;
using Ordering.API.Models.Input;
using Ordering.API.Models.Output;
using Ordering.Domain.Services;

namespace Ordering.API.Controllers;

/// <summary>
/// OrdersController
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<OrdersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Posts the order.
    /// </summary>
    /// <param name="orderingService">The ordering service.</param>
    /// <param name="order">The order.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResult> PostOrder(
        [FromServices] IOrderingService orderingService,
        [FromBody] OrderInput order,
        CancellationToken cancellationToken)
    {
        int orderId = await orderingService.CreateOrder(order.ToDomainModel(), cancellationToken);

        if (orderId >= 0)
        {
            _logger.LogInformation("PostOrder finished with status - success.");
            return TypedResults.Ok(Response<int>.Ok(orderId));
        }

        _logger.LogWarning("PostOrder finished with status - fail.");
        return TypedResults.BadRequest();
    }
}
