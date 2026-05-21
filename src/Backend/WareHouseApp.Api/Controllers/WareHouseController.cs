using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class WareHousesController(IWareHouseService wareHouseService) : ControllerBase
{
    /// <summary>
    /// Retrieves data for all warehouses. This operation requires no parameters and returns a list of warehouses including their identifiers, names and locations. It can be used to get an overview of warehouses or to populate a dropdown where the user can select a warehouse.
    /// </summary>
    /// <returns>List of all warehouses</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<WareHouseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WareHouseDto>>> GetAllWareHouses()
    {
        var warehouses = await wareHouseService.GetAllWareHousesAsync();
        return Ok(warehouses);
    }
    /// <summary>
    /// Retrieves data for the warehouse with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the warehouse</param>
    /// <returns>The data of the requested warehouse</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType<WareHouseDto>(StatusCodes.Status200OK)]
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
    /// <summary>
    /// Creates a new warehouse using the provided data. This operation expects a CreateWareHouseDto in the request body containing the warehouse name and location. On success it returns a WareHouseDto with the created warehouse data, HTTP 201 Created status and the resource location in the Location header.
    /// </summary>
    /// <param name="dto">The data for the warehouse to create</param>
    /// <returns>The created warehouse data</returns>
    [HttpPost]
    [ProducesResponseType<WareHouseDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<WareHouseDto>> CreateWareHouse(CreateWareHouseDto dto)
    {
        var newWarehouse = await wareHouseService.CreateWareHouseAsync(dto);
        return CreatedAtAction(nameof(GetWareHouseById), new { id = newWarehouse.Id }, newWarehouse);
    }
    /// <summary>
    /// Updates the warehouse with the specified identifier using the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the warehouse</param>
    /// <param name="dto">The warehouse data to update</param>
    /// <returns>The updated warehouse data</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType<WareHouseDto>(StatusCodes.Status200OK)]
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
    /// <summary>
    /// Deletes the warehouse with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the warehouse</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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



    /// <summary>
    /// V2 Teszt: Egy új funkció, ami csak a 2.0-ás API verzióban létezik!
    /// </summary>
    [HttpGet("v2-test")]
    [MapToApiVersion("2.0")] // <--- Ez a varázsszó! Ettől csak a v2-es legördülőben fog megjelenni.
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public ActionResult<string> GetV2Test()
    {
        return Ok("Sikeresen meghívtad a V2-es API-t az URL megváltoztatása nélkül!");
    }


}