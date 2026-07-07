using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.Extensions.DependencyInjection;

namespace F1_Fantasy_liga.Tests;

public static class ApiTestDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, params object[] entities)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<F1DbContext>();

        await db.Database.EnsureCreatedAsync();

        foreach (var entity in entities)
        {
            switch (entity)
            {
                case AppUser user:
                    db.Users.Add(user);
                    break;
                case Constructor constructor:
                    db.Constructors.Add(constructor);
                    break;
                case Circuit circuit:
                    db.Circuits.Add(circuit);
                    break;
                case Race race:
                    db.Races.Add(race);
                    break;
                case Driver driver:
                    db.Drivers.Add(driver);
                    break;
                case RaceResult raceResult:
                    db.RaceResults.Add(raceResult);
                    break;
                case FantasyLeague fantasyLeague:
                    db.FantasyLeagues.Add(fantasyLeague);
                    break;
                case FantasyTeam fantasyTeam:
                    db.FantasyTeams.Add(fantasyTeam);
                    break;
            }
        }

        await db.SaveChangesAsync();
    }
}