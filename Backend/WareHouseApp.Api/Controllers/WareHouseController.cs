using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WareHousesController(IWareHouseService wareHouseService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WareHouseDto>>> GetAllWareHouses()
    {
        var warehouses = await wareHouseService.GetAllWareHousesAsync();
        return Ok(warehouses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WareHouseDto>> GetWareHouseById(int id)
    {
        try
        {
            var warehouse = await wareHouseService.GetWareHouseByIdAsync(id);
            return Ok(warehouse);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<WareHouseDto>> CreateWareHouse(CreateWareHouseDto dto)
    {
        var newWarehouse = await wareHouseService.CreateWareHouseAsync(dto);
        return CreatedAtAction(nameof(GetWareHouseById), new { id = newWarehouse.Id }, newWarehouse);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WareHouseDto>> UpdateWareHouse(int id, CreateWareHouseDto dto)
    {
        try
        {
            var updatedWarehouse = await wareHouseService.UpdateWareHouseAsync(id, dto);
            return Ok(updatedWarehouse);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteWareHouse(int id)
    {
        try
        {
            await wareHouseService.DeleteWareHouseAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}