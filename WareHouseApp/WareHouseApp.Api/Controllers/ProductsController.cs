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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDetailDto>> GetDetails(int id)
    {
        var product = await productService.GetProductDetailAsync(id);
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<ProductDetailDto>> CreateProduct(CreateProductDto createdproduct)
    {
        var newProduct = await productService.CreateProductAsync(createdproduct);
        return CreatedAtAction(nameof(GetDetails), new { id = newProduct.Id }, newProduct);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDetailDto>> UpdateProduct(int id,UpdateProductDto dto)
    {
        var updatedProduct = await productService.UpdateProductAsync(id, dto);
        return Ok(updatedProduct);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        await productService.DeleteProductAsync(id);
        return NoContent();
    }
}