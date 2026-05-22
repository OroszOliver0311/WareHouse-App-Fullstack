using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WareHouseApp.Api.Controllers;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Exceptions;
using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Tests;

public class ControllerUnitTests
{
    [Fact]
    public async Task GetAllWareHouses_ReturnsOk_WithListOfWareHouses()
    {
        var mockService = new Mock<IWareHouseService>();
        var fakeData = new List<WareHouseDto>
        {
            new WareHouseDto { Id = 1, Name = "Test Warehouse", Location = "Pécs" }
        };
        mockService.Setup(s => s.GetAllWareHousesAsync()).ReturnsAsync(fakeData);

        var controller = new WareHousesController(mockService.Object);

        var result = await controller.GetAllWareHouses();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<WareHouseDto>>().Subject;
        returnValue.Should().HaveCount(1);
        returnValue.First().Name.Should().Be("Test Warehouse");
    }

    [Fact]
    public async Task GetWareHouseById_ReturnsOk_WhenWareHouseExists()
    {
        var mockService = new Mock<IWareHouseService>();
        var fakeDto = new WareHouseDto { Id = 5, Name = "Test Warehouse", Location = "Debrecen" };
        mockService.Setup(s => s.GetWareHouseByIdAsync(5)).ReturnsAsync(fakeDto);

        var controller = new WareHousesController(mockService.Object);

        var result = await controller.GetWareHouseById(5);


        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDto = okResult.Value.Should().BeOfType<WareHouseDto>().Subject;
        returnedDto.Id.Should().Be(5);
    }

    [Fact]
    public async Task GetWareHouseById_ReturnsNotFound_WhenEntityDoesNotExist()
    {

        var mockService = new Mock<IWareHouseService>();
        mockService.Setup(s => s.GetWareHouseByIdAsync(99))
                   .ThrowsAsync(new EntityNotFoundException("WareHouse", 99));

        var controller = new WareHousesController(mockService.Object);

        Func<Task> action = async () => await controller.GetWareHouseById(99);

        await action.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAtAction_WithCreatedProduct()
    {

        var mockService = new Mock<IProductService>();
        var createDto = new CreateProductDto { Name = "Mouse", SKU = "123", UnitPrice = 1000 };
        var createdDto = new ProductDetailDto { Id = 1, Name = "Mouse", SKU = "123", UnitPrice = 1000 };

        mockService.Setup(s => s.CreateProductAsync(createDto)).ReturnsAsync(createdDto);
        var controller = new ProductsController(mockService.Object);


        var result = await controller.CreateProduct(createDto);


        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(ProductsController.GetDetails));
        createdResult.RouteValues!["id"].Should().Be(1); 
        createdResult.Value.Should().BeEquivalentTo(createdDto);
    }


    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenSuccessful()
    {

        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.DeleteProductAsync(1)).Returns(Task.CompletedTask);
        var controller = new ProductsController(mockService.Object);


        var result = await controller.DeleteProduct(1);


        result.Should().BeOfType<NoContentResult>();

        mockService.Verify(s => s.DeleteProductAsync(1), Times.Once);
    }
}