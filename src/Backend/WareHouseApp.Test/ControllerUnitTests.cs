using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WareHouseApp.Api.Controllers;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Dtos.Encoding;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Tests;

public class ControllerUnitTests
{
    private readonly Mock<IWareHouseService> _mockWareHouseService;
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IIdEncoder> _mockEncoder;

    private readonly WareHousesController _wareHousesController;
    private readonly ProductsController _productsController;
    public ControllerUnitTests()
    {
        _mockWareHouseService = new Mock<IWareHouseService>();
        _mockProductService = new Mock<IProductService>();
        _mockEncoder = new Mock<IIdEncoder>();

        _wareHousesController = new WareHousesController(_mockWareHouseService.Object, _mockEncoder.Object);
        _productsController = new ProductsController(_mockProductService.Object, _mockEncoder.Object);
    }

    [Fact]
    public async Task GetAllWareHouses_ReturnsOk_WithListOfWareHouses()
    {
        var fakeData = new List<WareHouseDto>
        {
            new WareHouseDto { Id = "1", Name = "Test Warehouse", Location = "Pécs" }
        };
        _mockWareHouseService.Setup(s => s.GetAllWareHousesAsync()).ReturnsAsync(fakeData);

        var result = await _wareHousesController.GetAllWareHouses();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<WareHouseDto>>().Subject;
        returnValue.Should().HaveCount(1);
        returnValue.First().Name.Should().Be("Test Warehouse");
    }

    [Fact]
    public async Task GetWareHouseById_ReturnsOk_WhenWareHouseExists()
    {
        
        string encodedId = "encoded-5";
        _mockEncoder.Setup(e => e.Decode(encodedId)).Returns(5);

        var fakeDto = new WareHouseDto { Id = encodedId, Name = "Test Warehouse", Location = "Debrecen" };
        _mockWareHouseService.Setup(s => s.GetWareHouseByIdAsync(5)).ReturnsAsync(fakeDto);

        var controller = new WareHousesController(_mockWareHouseService.Object, _mockEncoder.Object);

        var result = await controller.GetWareHouseById(encodedId);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDto = okResult.Value.Should().BeOfType<WareHouseDto>().Subject;
        returnedDto.Id.Should().Be(encodedId);
    }

    [Fact]
    public async Task GetWareHouseById_ReturnsNotFound_WhenEntityDoesNotExist()
    {


        string encodedId = "encoded-99";
        _mockEncoder.Setup(e => e.Decode(encodedId)).Returns(99);

        _mockWareHouseService.Setup(s => s.GetWareHouseByIdAsync(99))
                   .ThrowsAsync(new EntityNotFoundException("WareHouse", 99));

        var controller = new WareHousesController(_mockWareHouseService.Object, _mockEncoder.Object);

        Func<Task> action = async () => await controller.GetWareHouseById(encodedId);

        await action.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAtAction_WithCreatedProduct()
    {


        var createDto = new CreateProductDto { Name = "Mouse", SKU = "123", UnitPrice = 1000 };
        var createdDto = new ProductDetailDto { Id = "new-id-1", Name = "Mouse", SKU = "123", UnitPrice = 1000 };

        _mockProductService.Setup(s => s.CreateProductAsync(createDto)).ReturnsAsync(createdDto);
        var controller = new ProductsController(_mockProductService.Object, _mockEncoder.Object);

        var result = await controller.CreateProduct(createDto);

        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(ProductsController.GetDetails));

        createdResult.RouteValues!["id"].Should().Be("new-id-1");
        createdResult.Value.Should().BeEquivalentTo(createdDto);
    }


    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenSuccessful()
    {

        string encodedId = "delete-me-1";
        _mockEncoder.Setup(e => e.Decode(encodedId)).Returns(1);

        _mockProductService.Setup(s => s.DeleteProductAsync(1)).Returns(Task.CompletedTask);
        var controller = new ProductsController(_mockProductService.Object, _mockEncoder.Object);

        var result = await controller.DeleteProduct(encodedId);

        result.Should().BeOfType<NoContentResult>();
        _mockProductService.Verify(s => s.DeleteProductAsync(1), Times.Once);
    }
}