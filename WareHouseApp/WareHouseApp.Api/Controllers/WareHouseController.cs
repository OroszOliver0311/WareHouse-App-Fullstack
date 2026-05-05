using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions; // Ez kell a saját kivételed miatt!
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WareHousesController(IWareHouseService wareHouseService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WareHouseDto>>> GetAll()
    {
        var warehouses = await wareHouseService.GetAllWareHousesAsync();
        return Ok(warehouses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WareHouseDto>> GetById(int id)
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
    public async Task<ActionResult<WareHouseDto>> Create([FromBody] CreateWareHouseDto dto)
    {
        var newWarehouse = await wareHouseService.CreateWareHouseAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newWarehouse.Id }, newWarehouse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WareHouseDto>> Update(int id, [FromBody] CreateWareHouseDto dto)
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
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