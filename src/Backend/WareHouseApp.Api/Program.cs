using Microsoft.EntityFrameworkCore;
using WareHouseApp.Bll.Interfaces;
using WareHouseApp.Bll.Services.LINQServices;
using WareHouseApp.Dal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductService, ProductServiceLINQ>();
builder.Services.AddScoped<IInventoryService, InventoryServiceLINQ>();
builder.Services.AddScoped<IWareHouseService, WareHouseServiceLINQ>();
builder.Services.AddScoped<IStockMovementService, StockMovementServiceLINQ>();
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
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
