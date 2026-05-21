using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Test;
using Xunit;

namespace WareHouseApp.Tests
{
    public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _appFactory;
        private readonly Faker<CreateProductDto> _dtoFaker;

        public ProductIntegrationTests(CustomWebApplicationFactory appFactory)
        {
            _appFactory = appFactory;

            _dtoFaker = new Faker<CreateProductDto>()
                .RuleFor(p => p.Name, f => f.Commerce.Product())
                .RuleFor(p => p.SKU, f => f.Random.AlphaNumeric(6).ToUpper())
                .RuleFor(p => p.UnitPrice, f => f.Random.Int(100, 10000));
        }

        [Fact]
        public async Task Post_Should_Succeed_With_Created()
        {
            // Arrange 
            var client = _appFactory.CreateClient();
            var dto = _dtoFaker.Generate();

            // Act
            var response = await client.PostAsJsonAsync("/api/Products", dto);
            var resultProduct = await response.Content.ReadFromJsonAsync<ProductDetailDto>();

            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            resultProduct.Should().NotBeNull();
            resultProduct!.Name.Should().Be(dto.Name);
            resultProduct.SKU.Should().Be(dto.SKU);
            resultProduct.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetDashboard_Should_Return_Ok_And_List()
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Products/dashboard");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var list = await response.Content.ReadFromJsonAsync<List<ProductDashboardDto>>();
            list.Should().NotBeNull();
        }
        [Fact]
        public async Task GetById_Should_Return_Ok_When_Product_Exists()
        {
            //Arrange
            var client = _appFactory.CreateClient();
            var dto = _dtoFaker.Generate();
            var postResponse = await client.PostAsJsonAsync("/api/Products", dto);
            var created = await postResponse.Content.ReadFromJsonAsync<ProductDetailDto>();

            // Act
            var response = await client.GetAsync($"/api/Products/{created!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var product = await response.Content.ReadFromJsonAsync<ProductDetailDto>();
            product!.Id.Should().Be(created.Id);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Product_Does_Not_Exist()
        {
            // Act
            var client = _appFactory.CreateClient();
            var response = await client.GetAsync("/api/Products/99999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Should_Remove_Product()
        {
            // Arrange
            var client = _appFactory.CreateClient();
            var dto = _dtoFaker.Generate();
            var postResponse = await client.PostAsJsonAsync("/api/Products", dto);
            var created = await postResponse.Content.ReadFromJsonAsync<ProductDetailDto>();

            // Act
            var deleteResponse = await client.DeleteAsync($"/api/Products/{created!.Id}");
            var getResponse = await client.GetAsync($"/api/Products/{created.Id}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Put_Should_Update_Product()
        {
            // Arrange
            var client = _appFactory.CreateClient();
            var dto = _dtoFaker.Generate();
            var postResponse = await client.PostAsJsonAsync("/api/Products", dto);
            var created = await postResponse.Content.ReadFromJsonAsync<ProductDetailDto>();

            created!.Name = "Updated Name";

            // Act
            var putResponse = await client.PutAsJsonAsync($"/api/Products/{created.Id}", created);

            // Assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "Product name is required.")]
        [InlineData(null, "Product name is required.")]
        public async Task Should_Fail_When_Name_Is_Invalid(string name, string expectedError)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            var dto = _dtoFaker.RuleFor(x => x.Name, name).Generate();

            // Act
            var response = await client.PostAsJsonAsync("/api/Products", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }






























    }
}