using System.Net;
using System.Net.Http.Json;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Tests;

public class FantasyTeamsApiTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithTeamList()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false },
            new FantasyTeam { Id = 2, Name = "Tifosi Forza", Budget = 71m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/fantasy-teams");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var teams = await response.Content.ReadFromJsonAsync<List<FantasyTeamDTO>>();
        Assert.NotNull(teams);
        Assert.Equal(2, teams!.Count);
    }

    [Fact]
    public async Task GetAll_WithSearch_ReturnsFilteredTeams()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false },
            new FantasyTeam { Id = 2, Name = "Tifosi Forza", Budget = 71m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/fantasy-teams?search=Tifosi");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var teams = await response.Content.ReadFromJsonAsync<List<FantasyTeamDTO>>();
        Assert.NotNull(teams);
        Assert.Single(teams!);
        Assert.Equal("Tifosi Forza", teams![0].Name);
    }

    [Fact]
    public async Task GetById_ReturnsTeam_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false });

        var response = await client.GetAsync("/api/fantasy-teams/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var team = await response.Content.ReadFromJsonAsync<FantasyTeamDTO>();
        Assert.NotNull(team);
        Assert.Equal("Speed Demons", team!.Name);
        Assert.NotNull(team.User);
        Assert.NotNull(team.Constructor);
        Assert.NotNull(team.FantasyLeague);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/fantasy-teams/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenSoftDeleted()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = true, DeletedAt = DateTime.UtcNow });

        var response = await client.GetAsync("/api/fantasy-teams/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false });

        var response = await client.PostAsJsonAsync("/api/fantasy-teams", new FantasyTeamCreateDTO
        {
            Name = "Speed Demons",
            Budget = 88.5m,
            UserId = "1",
            ConstructorId = 1,
            FantasyLeagueId = 1
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var team = await response.Content.ReadFromJsonAsync<FantasyTeamDTO>();
        Assert.NotNull(team);
        Assert.Equal("Speed Demons", team!.Name);
        Assert.NotNull(team.User);
        Assert.Equal("1", team.User!.Id);
        Assert.NotNull(team.Constructor);
        Assert.NotNull(team.FantasyLeague);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/fantasy-teams", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false });

        var response = await client.PutAsJsonAsync("/api/fantasy-teams/1", new FantasyTeamUpdateDTO
        {
            Name = "Speed Demons Updated",
            Budget = 90m,
            UserId = "1",
            ConstructorId = 1,
            FantasyLeagueId = 1
        });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/fantasy-teams/99999", new FantasyTeamUpdateDTO
        {
            Name = "Speed Demons Updated",
            Budget = 90m,
            UserId = "1",
            ConstructorId = 1,
            FantasyLeagueId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        await ApiTestDataSeeder.SeedAsync(factory.Services,
            new AppUser { Id = "1", UserName = "marko@email.com", NormalizedUserName = "MARKO@EMAIL.COM", Email = "marko@email.com", NormalizedEmail = "MARKO@EMAIL.COM", Name = "Marko", Surname = "Horvat", SecurityStamp = Guid.NewGuid().ToString("D"), ConcurrencyStamp = Guid.NewGuid().ToString("D"), IsDeleted = false },
            new Constructor { Id = 1, Name = "Red Bull Racing", IsDeleted = false },
            new FantasyLeague { Id = 1, Name = "Private League", Description = "Private", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private, IsDeleted = false },
            new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = "1", ConstructorId = 1, FantasyLeagueId = 1, IsDeleted = false });

        var response = await client.DeleteAsync("/api/fantasy-teams/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/fantasy-teams/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}