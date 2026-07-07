using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Tests;

public class RaceResultsApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithRaceResultList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = false },
            new RaceResult { Id = 2, FinishedPosition = 2, ScoredPoints = 18, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/race-results");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var results = await response.Content.ReadFromJsonAsync<List<RaceResultDTO>>();
        Assert.NotNull(results);
        Assert.Equal(2, results!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredRaceResults()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new Race { Id = 2, Name = "Italian Grand Prix", RaceDate = new DateTime(2024, 9, 1), CircuitId = 1, IsDeleted = false },
            new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = false },
            new RaceResult { Id = 2, FinishedPosition = 2, ScoredPoints = 18, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 2, IsDeleted = false });

        var response = await client.GetAsync("/api/race-results?search=Monaco");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var results = await response.Content.ReadFromJsonAsync<List<RaceResultDTO>>();
        Assert.NotNull(results);
        Assert.Single(results!);
        Assert.Equal("Monaco Grand Prix", results![0].Race!.Name);
    }

    [Fact]
    public async Task GetById_ReturnsRaceResult_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/race-results/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<RaceResultDTO>();
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal(25, result.ScoredPoints);
        Assert.NotNull(result.Driver);
        Assert.NotNull(result.Race);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/race-results/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/race-results/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false });

        var response = await client.PostAsJsonAsync("/api/race-results", new RaceResultCreateDTO
        {
            FinishedPosition = 1,
            DriverId = 1,
            RaceId = 1,
            DriverStatus = DriverStatus.Finished
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<RaceResultDTO>();
        Assert.NotNull(result);
        Assert.Equal(25, result!.ScoredPoints);
        Assert.Equal(DriverStatus.Finished, result.DriverStatus);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/race-results", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new RaceResult { Id = 1, FinishedPosition = 2, ScoredPoints = 18, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/race-results/1", new RaceResultUpdateDTO
        {
            FinishedPosition = 1,
            DriverId = 1,
            RaceId = 1,
            DriverStatus = DriverStatus.Finished
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/race-results/99999", new RaceResultUpdateDTO
        {
            FinishedPosition = 1,
            DriverId = 1,
            RaceId = 1,
            DriverStatus = DriverStatus.Finished
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Constructor { Id = 1, Name = "Mercedes-AMG Petronas", IsDeleted = false },
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Driver { Id = 1, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28m, ConstructorId = 1, IsDeleted = false },
            new Race { Id = 1, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 1, IsDeleted = false },
            new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1, IsDeleted = false });

        var response = await client.DeleteAsync("/api/race-results/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/race-results/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}