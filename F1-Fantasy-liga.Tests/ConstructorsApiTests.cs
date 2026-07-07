using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;

namespace F1_Fantasy_liga.Tests;

public class ConstructorsApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithConstructorList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", IsDeleted = false },
            new Constructor { Id = 2, Name = "Scuderia Ferrari", Nationality = "Italian", IsDeleted = false });

        var response = await client.GetAsync("/api/constructors");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var constructors = await response.Content.ReadFromJsonAsync<List<ConstructorDTO>>();
        Assert.NotNull(constructors);
        Assert.Equal(2, constructors!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredConstructors()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", IsDeleted = false },
            new Constructor { Id = 2, Name = "Scuderia Ferrari", Nationality = "Italian", IsDeleted = false });

        var response = await client.GetAsync("/api/constructors?search=Red Bull");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var constructors = await response.Content.ReadFromJsonAsync<List<ConstructorDTO>>();
        Assert.NotNull(constructors);
        Assert.Single(constructors!);
        Assert.Equal("Red Bull Racing", constructors![0].Name);
    }

    [Fact]
    public async Task GetById_ReturnsConstructor_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", IsDeleted = false });

        var response = await client.GetAsync("/api/constructors/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var constructor = await response.Content.ReadFromJsonAsync<ConstructorDTO>();
        Assert.NotNull(constructor);
        Assert.Equal(1, constructor!.Id);
        Assert.Equal("Red Bull Racing", constructor.Name);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/constructors/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/constructors/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/constructors", new ConstructorCreateDTO
        {
            Name = "McLaren",
            Nationality = "British",
            FoundedDate = new DateTime(1963, 5, 22)
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var constructor = await response.Content.ReadFromJsonAsync<ConstructorDTO>();
        Assert.NotNull(constructor);
        Assert.Equal("McLaren", constructor!.Name);
        Assert.Equal("British", constructor.Nationality);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/constructors", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/constructors/1", new ConstructorUpdateDTO
        {
            Name = "Red Bull Racing",
            Nationality = "Austrian",
            FoundedDate = new DateTime(2005, 3, 6)
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/constructors/99999", new ConstructorUpdateDTO
        {
            Name = "Red Bull Racing",
            Nationality = "Austrian",
            FoundedDate = new DateTime(2005, 3, 6)
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", IsDeleted = false });

        var response = await client.DeleteAsync("/api/constructors/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/constructors/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}