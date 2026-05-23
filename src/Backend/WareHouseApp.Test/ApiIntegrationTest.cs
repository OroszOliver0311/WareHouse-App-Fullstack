using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Test;

namespace WareHouseApp.Tests;

public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task GetWareHouses_ReturnsOk_WithSeedData()
    {

        var response = await _client.GetAsync("/api/warehouses");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var warehouses = await response.Content.ReadFromJsonAsync<IEnumerable<WareHouseDto>>(_factory.SerializerOptions);
        warehouses.Should().NotBeNullOrEmpty();

        warehouses.Should().Contain(w => w.Name == "Budapesti Központi Raktár");
    }

    [Fact]
    public async Task UpdateWareHouse_ReturnsOk_WithUpdatedData()
    {
        var createDto = new CreateWareHouseDto { Name = "Old Warehouse", Location = "Miskolc" };
        var createResponse = await _client.PostAsJsonAsync("/api/warehouses", createDto, _factory.SerializerOptions);
        var createdWarehouse = await createResponse.Content.ReadFromJsonAsync<WareHouseDto>(_factory.SerializerOptions);
        var idToUpdate = createdWarehouse!.Id;

        var updateDto = new CreateWareHouseDto { Name = "Super New Warehouse", Location = "Eger" };

        var updateResponse = await _client.PutAsJsonAsync($"/api/warehouses/{idToUpdate}", updateDto, _factory.SerializerOptions);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedWarehouse = await updateResponse.Content.ReadFromJsonAsync<WareHouseDto>(_factory.SerializerOptions);
        updatedWarehouse.Should().NotBeNull();
        updatedWarehouse!.Name.Should().Be("Super New Warehouse");
        updatedWarehouse.Location.Should().Be("Eger");
    }


    [Fact]
    public async Task CreateProduct_ReturnsCreated_WithValidBogusData()
    {

        var faker = new Faker<CreateProductDto>("en")
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.SKU, f => f.Commerce.Ean13())
            .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(1000, 50000)));

        var newProductDto = faker.Generate();


        var response = await _client.PostAsJsonAsync("/api/products", newProductDto, _factory.SerializerOptions);


        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdProduct = await response.Content.ReadFromJsonAsync<ProductDetailDto>(_factory.SerializerOptions);
        createdProduct.Should().NotBeNull();
        createdProduct!.Id.Should().NotBeNullOrEmpty(); 
        createdProduct.Name.Should().Be(newProductDto.Name);
    }


    [Fact]
    public async Task GetProductDetails_ReturnsNotFound_WhenIdDoesNotExist()
    {
    
        var response = await _client.GetAsync("/api/products/invalidID12");

  
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ProblemDetails>(_factory.SerializerOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }


    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenProductExists()
    {
        var createDto = new CreateProductDto { Name = "Product To Delete", SKU = "DEL-123", UnitPrice = 1000 };
        var createResponse = await _client.PostAsJsonAsync("/api/products", createDto, _factory.SerializerOptions);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDetailDto>(_factory.SerializerOptions);

        var idToDelete = createdProduct!.Id;

        var deleteResponse = await _client.DeleteAsync($"/api/products/{idToDelete}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);


        var getResponse = await _client.GetAsync($"/api/products/{idToDelete}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
   
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateProduct_ReturnsBadRequest_WhenNameIsInvalid(string? invalidName)
    {
        var invalidDto = new CreateProductDto
        {
            Name = invalidName!,
            SKU = "TEST-SKU",
            UnitPrice = 1500
        };

        var response = await _client.PostAsJsonAsync("/api/products", invalidDto, _factory.SerializerOptions);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);


        var problemDetails = await response.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ValidationProblemDetails>(_factory.SerializerOptions);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
    }




}