using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpPost("upsert")]
    public async Task<ActionResult> UpsertInventory(InventoryItemDto upsertItem)
    {
        await inventoryService.UpsertInventoryAsync(upsertItem);
        return Ok();
    }
}