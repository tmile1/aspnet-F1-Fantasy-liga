using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Tests;

public class FantasyLeaguesApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithLeagueList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyLeague { Id = 2, Name = "Public League", Description = "Public", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Public, IsDeleted = false });

        var response = await client.GetAsync("/api/fantasy-leagues");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var leagues = await response.Content.ReadFromJsonAsync<List<FantasyLeagueDTO>>();
        Assert.NotNull(leagues);
        Assert.Equal(2, leagues!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredLeagues()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyLeague { Id = 2, Name = "Public League", Description = "Public", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Public, IsDeleted = false });

        var response = await client.GetAsync("/api/fantasy-leagues?search=Private");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var leagues = await response.Content.ReadFromJsonAsync<List<FantasyLeagueDTO>>();
        Assert.NotNull(leagues);
        Assert.Single(leagues!);
        Assert.Equal("Private League", leagues![0].Name);
    }

    [Fact]
    public async Task GetById_ReturnsLeague_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false });

        var response = await client.GetAsync("/api/fantasy-leagues/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var league = await response.Content.ReadFromJsonAsync<FantasyLeagueDTO>();
        Assert.NotNull(league);
        Assert.Equal("Private League", league!.Name);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/fantasy-leagues/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/fantasy-leagues/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/fantasy-leagues", new FantasyLeagueCreateDTO
        {
            Name = "Spring League",
            Description = "Season opener",
            StartDate = new DateTime(2024, 3, 1),
            EndDate = new DateTime(2024, 11, 30),
            LeagueType = LeagueType.Public
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var league = await response.Content.ReadFromJsonAsync<FantasyLeagueDTO>();
        Assert.NotNull(league);
        Assert.Equal("Spring League", league!.Name);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/fantasy-leagues", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/fantasy-leagues/1", new FantasyLeagueUpdateDTO
        {
            Name = "Private League Updated",
            Description = "Updated",
            StartDate = new DateTime(2024, 3, 1),
            EndDate = new DateTime(2024, 11, 30),
            LeagueType = LeagueType.Private
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/fantasy-leagues/99999", new FantasyLeagueUpdateDTO
        {
            Name = "Private League Updated",
            Description = "Updated",
            StartDate = new DateTime(2024, 3, 1),
            EndDate = new DateTime(2024, 11, 30),
            LeagueType = LeagueType.Private
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false });

        var response = await client.DeleteAsync("/api/fantasy-leagues/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/fantasy-leagues/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}