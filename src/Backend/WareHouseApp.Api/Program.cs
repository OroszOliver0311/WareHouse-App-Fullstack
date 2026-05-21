using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using WareHouseApp.Bll.Dtos;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Bll.Services;
using WareHouseApp.Bll.Services.AutoMapperServices;
using WareHouseApp.Bll.Services.LINQServices;
using WareHouseApp.Dal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Verziókezelés beállítása URL beégetés nélkül

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); 
    options.AssumeDefaultVersionWhenUnspecified = true; 
    options.ReportApiVersions = true; 

    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"), 
        new HeaderApiVersionReader("X-Api-Version")     
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// V1 Dokumentáció generálása
builder.Services.AddOpenApiDocument(options =>
{
    options.DocumentName = "v1";
    options.ApiGroupNames = new[] { "v1" };
    options.PostProcess = document =>
    {
        document.Info.Title = "WareHouseApp API - Verzió 1.0";
        document.Info.Version = "v1.0";
    };
});

builder.Services.AddOpenApiDocument(options =>
{
    options.DocumentName = "v2";
    options.ApiGroupNames = new[] { "v2" };
    options.PostProcess = document =>
    {
        document.Info.Title = "WareHouseApp API - Verzió 2.0 (Új funkciók)";
        document.Info.Version = "v2.0";
    };
});



builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductService, ProductServiceAutoMapper>();
builder.Services.AddScoped<IInventoryService, InventoryServiceLINQ>();
builder.Services.AddScoped<IWareHouseService, WareHouseServiceAutoMapper>();
builder.Services.AddScoped<IStockMovementService, StockMovementServiceAutoMapper>();

builder.Services.AddAutoMapper(a => a.AddProfile<MappingProfile>());
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAngular");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
