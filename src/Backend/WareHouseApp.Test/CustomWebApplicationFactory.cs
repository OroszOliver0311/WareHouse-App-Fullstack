using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WareHouseApp.Dal;

namespace WareHouseApp.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;
    public JsonSerializerOptions SerializerOptions { get; }

    public CustomWebApplicationFactory()
    {
        JsonSerializerOptions jso = new(JsonSerializerDefaults.Web);
        jso.Converters.Add(new JsonStringEnumConverter());
        SerializerOptions = jso;
    }
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
  
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddScoped(sp => new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .UseApplicationServiceProvider(sp)
                .Options);
        });

        var host = base.CreateHost(builder);


        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>()
            .Database.EnsureCreated();

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Close();
        _connection?.Dispose();
    }
}
