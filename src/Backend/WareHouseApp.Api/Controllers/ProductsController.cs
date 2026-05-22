using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Dtos.Encoding;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Api.Controllers;


[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class ProductsController(IProductService productService, IIdEncoder idEncoder) : ControllerBase
{

    /// <summary>
    /// Retrieves aggregated dashboard data for products (basic statistics, total inventory).
    /// </summary>
    /// <returns>List of product data required to display the dashboard.</returns>
    [HttpGet("dashboard")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType<IEnumerable<ProductDashboardDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDashboardDto>>> GetDashboard()
    {
        var data = await productService.GetDashboardAsync();
        return Ok(data);
    }

    /// <summary>
    /// Retrieves paginated dashboard data for products, allowing clients to specify page number and size for efficient data retrieval.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <returns></returns>
    [HttpGet("dashboard")]
    [MapToApiVersion("2.0")] 
    [ProducesResponseType<PagedResponse<ProductDashboardDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ProductDashboardDto>>> GetDashboard(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var allProducts = await productService.GetDashboardAsync();
        var totalCount = allProducts.Count();

        var pagedItems = allProducts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var response = new PagedResponse<ProductDashboardDto>(pagedItems, pageNumber, pageSize, totalCount);
        return Ok(response);
    }


    /// <summary>
    /// Retrieves detailed information for a specific product by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>Detailed information of the product.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<ProductDetailDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDetailDto>> GetDetails(string id)
    {
        var realId = idEncoder.Decode(id);
        var product = await productService.GetProductDetailAsync(realId);
        return Ok(product);
    }
    
    /// <summary>
    /// Creates a new product in the system.
    /// </summary>
    /// <param name="createdproduct">The data of the product to create.</param>
    /// <returns>The newly created product's detailed data and the route to access it.</returns>
    [HttpPost]
    [ProducesResponseType<ProductDetailDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductDetailDto>> CreateProduct(CreateProductDto createdproduct)
    {
        var newProduct = await productService.CreateProductAsync(createdproduct);
        return CreatedAtAction(nameof(GetDetails), new { id = newProduct.Id }, newProduct);
    }

    /// <summary>
    /// Updates an existing product's data by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to update.</param>
    /// <param name="dto">The updated product data.</param>
    /// <returns>Detailed data of the updated product.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType<ProductDetailDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDetailDto>> UpdateProduct(string id,CreateProductDto dto)
    {
        var realId = idEncoder.Decode(id);
        var updatedProduct = await productService.UpdateProductAsync(realId, dto);
        return Ok(updatedProduct);
    }
    
    /// <summary>
    /// Deletes a product from the system by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        var realId = idEncoder.Decode(id);
        await productService.DeleteProductAsync(realId);
        return NoContent();
    }
}