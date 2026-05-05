using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockMovementsController(IStockMovementService stockMovementService) : ControllerBase
{
    [HttpGet("product/{id:int}")]
    public async Task<ActionResult<IEnumerable<StockMovementDto>>> GetProductHistory(int id)
    {
        var history = await stockMovementService.GetProductHistoryAsync(id);
        return Ok(history);
    }
}