using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    /// <summary>
    /// Updates the inventory for a specific product in a specific warehouse. 
    /// If the inventory record does not exist, it will be created. 
    /// If it already exists, the quantity will be updated to the new value provided.
    /// </summary>
    /// <param name="upsertItem">The inventory item data to upsert.</param>
    [HttpPost("upsert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpsertInventory(InventoryItemDto upsertItem)
    {
        await inventoryService.UpsertInventoryAsync(upsertItem);
        return Ok();
    }
}