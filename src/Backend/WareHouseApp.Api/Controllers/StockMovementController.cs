using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class StockMovementsController(IStockMovementService stockMovementService) : ControllerBase
{
    /// <summary>
    /// Queries the stock movement history for a specific product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product for which to query movement history.</param>
    /// <returns>The list of stock movements for the specified product, ordered from most recent to oldest.</returns>
    [HttpGet("product/{id:int}")]
    [ProducesResponseType<IEnumerable<StockMovementDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StockMovementDto>>> GetProductHistory(int id)
    {
        var history = await stockMovementService.GetProductHistoryAsync(id);
        return Ok(history);
    }
}