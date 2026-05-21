using Microsoft.AspNetCore.Mvc;
using Moq;
using WareHouseApp.Api.Controllers;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;
using FluentAssertions;

namespace WareHouseApp.Tests
{
    public class ProductUnitTests
    {
        [Fact]
        public async Task GetDetails_WithExistingId_ReturnsOk()
        {
            var mockService = new Mock<IProductService>();
            var expectedDto = new ProductDetailDto { Id = 1, Name = "Mock Product" , SKU = "MOCK-123" };

            mockService.Setup(s => s.GetProductDetailAsync(1))
                       .ReturnsAsync(expectedDto);

            var controller = new ProductsController(mockService.Object);

            var result = await controller.GetDetails(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedDto);
        }
    }
}