using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;

namespace F1_Fantasy_liga.Tests;

public class CircuitsApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithCircuitList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Circuit { Id = 2, Name = "Monza", Country = "Italy", City = "Monza", Length = 5.793, NumberOfLaps = 53, IsDeleted = false });

        var response = await client.GetAsync("/api/circuits");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var circuits = await response.Content.ReadFromJsonAsync<List<CircuitDTO>>();
        Assert.NotNull(circuits);
        Assert.Equal(2, circuits!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredCircuits()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false },
            new Circuit { Id = 2, Name = "Monza", Country = "Italy", City = "Monza", Length = 5.793, NumberOfLaps = 53, IsDeleted = false });

        var response = await client.GetAsync("/api/circuits?search=Monaco");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var circuits = await response.Content.ReadFromJsonAsync<List<CircuitDTO>>();
        Assert.NotNull(circuits);
        Assert.Single(circuits!);
        Assert.Equal("Monaco", circuits![0].Name);
    }

    [Fact]
    public async Task GetById_ReturnsCircuit_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false });

        var response = await client.GetAsync("/api/circuits/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var circuit = await response.Content.ReadFromJsonAsync<CircuitDTO>();
        Assert.NotNull(circuit);
        Assert.Equal("Monaco", circuit!.Name);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/circuits/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/circuits/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/circuits", new CircuitCreateDTO
        {
            Name = "Bahrain International Circuit",
            Country = "Bahrain",
            City = "Sakhir",
            Length = 5.412,
            NumberOfLaps = 57
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var circuit = await response.Content.ReadFromJsonAsync<CircuitDTO>();
        Assert.NotNull(circuit);
        Assert.Equal("Bahrain International Circuit", circuit!.Name);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/circuits", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/circuits/1", new CircuitUpdateDTO
        {
            Name = "Monaco Grand Prix Circuit",
            Country = "Monaco",
            City = "Monte Carlo",
            Length = 3.337,
            NumberOfLaps = 78
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/circuits/99999", new CircuitUpdateDTO
        {
            Name = "Monaco Grand Prix Circuit",
            Country = "Monaco",
            City = "Monte Carlo",
            Length = 3.337,
            NumberOfLaps = 78
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new Circuit { Id = 1, Name = "Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78, IsDeleted = false });

        var response = await client.DeleteAsync("/api/circuits/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/circuits/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}