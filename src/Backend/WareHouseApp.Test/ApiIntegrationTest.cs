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
    public async Task GetV2Test_ReturnsOk_StringMessage()
    {

        var response = await _client.GetAsync("/api/warehouses/v2-test");


        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var message = await response.Content.ReadAsStringAsync();
        message.Should().Contain("V2");
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
        createdProduct!.Id.Should().BeGreaterThan(0); 
        createdProduct.Name.Should().Be(newProductDto.Name);
    }


    [Fact]
    public async Task GetProductDetails_ReturnsNotFound_WhenIdDoesNotExist()
    {
    
        var response = await _client.GetAsync("/api/products/99999");

  
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
}