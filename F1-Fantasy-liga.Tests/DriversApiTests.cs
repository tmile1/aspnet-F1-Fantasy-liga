using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace F1_Fantasy_liga.Tests;

public class DriversApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithDriverList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Driver { Id = 101, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Driver { Id = 102, Name = "George", Surname = "Russell", Number = 63, Price = 21m, ConstructorId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/drivers");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var drivers = await response.Content.ReadFromJsonAsync<List<DriverDTO>>();
        Assert.NotNull(drivers);
        Assert.Equal(2, drivers!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredDrivers()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Driver { Id = 101, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Driver { Id = 102, Name = "George", Surname = "Russell", Number = 63, Price = 21m, ConstructorId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/drivers?search=hamilton");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var drivers = await response.Content.ReadFromJsonAsync<List<DriverDTO>>();
        Assert.NotNull(drivers);
        Assert.Single(drivers!);
        Assert.Equal("Hamilton", drivers![0].Surname);
    }

    [Fact]
    public async Task GetById_ReturnsDriver_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Driver { Id = 123, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/drivers/123");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var driver = await response.Content.ReadFromJsonAsync<DriverDTO>();
        Assert.NotNull(driver);
        Assert.Equal(123, driver!.Id);
        Assert.Equal("Lewis", driver.Name);
        Assert.Equal("Hamilton", driver.Surname);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/drivers/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Driver { Id = 123, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/drivers/123");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false });

        var response = await client.PostAsJsonAsync("/api/drivers", new DriverCreateDTO
        {
            Name = "Lewis",
            Surname = "Hamilton",
            Number = 44,
            Price = 28m,
            ConstructorId = 1
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdDriver = await response.Content.ReadFromJsonAsync<DriverDTO>();
        Assert.NotNull(createdDriver);
        Assert.Equal("Lewis", createdDriver!.Name);
        Assert.Equal("Hamilton", createdDriver.Surname);
        Assert.Equal(44, createdDriver.Number);
        Assert.Equal(28m, createdDriver.Price);
        Assert.NotNull(createdDriver.Constructor);
        Assert.Equal(1, createdDriver.Constructor!.Id);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/drivers", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Driver { Id = 123, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/drivers/123", new DriverUpdateDTO
        {
            Name = "Lewis",
            Surname = "Hamilton",
            Number = 44,
            Price = 29m,
            ConstructorId = 1
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/drivers/99999", new DriverUpdateDTO
        {
            Name = "Lewis",
            Surname = "Hamilton",
            Number = 44,
            Price = 29m,
            ConstructorId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Driver { Id = 123, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false });

        var response = await client.DeleteAsync("/api/drivers/123");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/drivers/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static async Task SeedAsync(IServiceProvider services, params object[] entities)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<F1DbContext>();

        await db.Database.EnsureCreatedAsync();

        foreach (var entity in entities)
        {
            switch (entity)
            {
                case Constructor constructor:
                    db.Constructors.Add(constructor);
                    break;
                case Driver driver:
                    db.Drivers.Add(driver);
                    break;
            }
        }

        await db.SaveChangesAsync();
    }
}