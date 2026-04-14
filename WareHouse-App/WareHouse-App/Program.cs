var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<WareHouse_App.Data.WarehouseDbContext>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WareHouse_App.Data.WarehouseDbContext>();
    WareHouse_App.Data.DbSeeder.Seed(context);
}



app.MapControllers();
app.Run();
