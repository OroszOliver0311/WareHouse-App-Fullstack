using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{

    [HttpGet("dashboard")]
    public async Task<ActionResult<IEnumerable<ProductDashboardDto>>> GetDashboard()
    {
        var data = await productService.GetDashboardAsync();
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailDto>> GetDetails(int id)
    {
        var product = await productService.GetProductDetailAsync(id);
        return Ok(product);
    }
}