using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;

namespace F1_Fantasy_liga.Tests;

public class RacesApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithRaceList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new Race { Id = 2, Name = "Italian Grand Prix", RaceDate = new DateTime(2024, 9, 1), CircuitId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/races");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var races = await response.Content.ReadFromJsonAsync<List<RaceDTO>>();
        Assert.NotNull(races);
        Assert.Equal(2, races!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredRaces()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new Race { Id = 2, Name = "Italian Grand Prix", RaceDate = new DateTime(2024, 9, 1), CircuitId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/races?search=Monaco");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var races = await response.Content.ReadFromJsonAsync<List<RaceDTO>>();
        Assert.NotNull(races);
        Assert.Single(races!);
        Assert.Equal("Monaco Grand Prix", races![0].Name);
    }

    [Fact]
    public async Task GetById_ReturnsRace_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/races/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var race = await response.Content.ReadFromJsonAsync<RaceDTO>();
        Assert.NotNull(race);
        Assert.Equal("Monaco Grand Prix", race!.Name);
        Assert.NotNull(race.Circuit);
        Assert.Equal(1, race.Circuit!.Id);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/races/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/races/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false });

        var response = await client.PostAsJsonAsync("/api/races", new RaceCreateDTO
        {
            Name = "Monaco Grand Prix",
            RaceDate = new DateTime(2024, 5, 26),
            CircuitId = 1
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var race = await response.Content.ReadFromJsonAsync<RaceDTO>();
        Assert.NotNull(race);
        Assert.Equal("Monaco Grand Prix", race!.Name);
        Assert.NotNull(race.Circuit);
        Assert.Equal(1, race.Circuit!.Id);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/races", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/races/1", new RaceUpdateDTO
        {
            Name = "Monaco Grand Prix Updated",
            RaceDate = new DateTime(2024, 5, 26),
            CircuitId = 1
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/races/99999", new RaceUpdateDTO
        {
            Name = "Monaco Grand Prix Updated",
            RaceDate = new DateTime(2024, 5, 26),
            CircuitId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false });

        var response = await client.DeleteAsync("/api/races/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/races/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}