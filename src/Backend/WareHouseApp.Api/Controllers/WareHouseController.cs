using Microsoft.AspNetCore.Mvc;
using WareHouse_App.Entities;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class WareHousesController(IWareHouseService wareHouseService) : ControllerBase
{
    /// <summary>
    /// Lekérdezi az összes raktár adatait. Ez a művelet nem igényel paramétereket, és egy listát ad vissza a raktárakról, amelyek tartalmazzák az azonosítójukat, nevüket és helyüket. Ez a művelet hasznos lehet a raktárak áttekintéséhez vagy egy legördülő menü feltöltéséhez, ahol a felhasználó kiválaszthatja a kívánt raktárat.
    /// </summary>
    /// <returns> Az összes raktár listája </returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<WareHouseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WareHouseDto>>> GetAllWareHouses()
    {
        var warehouses = await wareHouseService.GetAllWareHousesAsync();
        return Ok(warehouses);
    }
    /// <summary>
    /// Lekérdezi a megadott azonosítójú raktár adatait.
    /// </summary>
    /// <param name="id"> A raktár egyéni azonosítója </param>
    /// <returns> A keresett raktár adatai </returns>
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
    /// Új raktárat hoz létre a megadott adatok alapján. Ez a művelet egy CreateWareHouseDto objektumot vár a kérés törzsében, amely tartalmazza a raktár nevét és helyét. A művelet sikeres végrehajtása után a létrehozott raktár adatait tartalmazó WareHouseDto objektumot ad vissza, valamint a HTTP 201 Created státuszkódot és a helyét a Location fejlécben.
    /// </summary>
    /// <param name="dto"> A létrehozandó raktár adatai </param>
    /// <returns> A létrehozott raktár adatai </returns>
    [HttpPost]
    [ProducesResponseType<WareHouseDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<WareHouseDto>> CreateWareHouse(CreateWareHouseDto dto)
    {
        var newWarehouse = await wareHouseService.CreateWareHouseAsync(dto);
        return CreatedAtAction(nameof(GetWareHouseById), new { id = newWarehouse.Id }, newWarehouse);
    }
    /// <summary>
    /// Frissíti a megadott azonosítójú raktárat a megadott adatok alapján.
    /// </summary>
    /// <param name="id"> A raktár egyéni azonosítója </param>
    /// <param name="dto"> A frissítendő raktár adatai </param>
    /// <returns> A frissített raktár adatai </returns>
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
    /// Törli a megadott azonosítójú raktárat.
    /// </summary>
    /// <param name="id"> A raktár egyéni azonosítója </param>
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
}