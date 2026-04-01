using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();






Console.WriteLine("===========F1 Fantasy League===========");

// ================= CONSTRUCTORS =================
var redbull = new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", FoundedDate = new DateTime(2005, 1, 1)};
 
var ferrari = new Constructor { Id = 2, Name = "Scuderia Ferrari", Nationality = "Italian", FoundedDate = new DateTime(1950, 5, 13)};
 
var mercedes = new Constructor { Id = 3, Name = "Mercedes-AMG Petronas", Nationality = "German", FoundedDate = new DateTime(1954, 6, 20)};

// ================= DRIVERS =================
var verstappen = new Driver { Id = 1, Name = "Max",     Surname = "Verstappen", Number = 1,  Price = 33.5m, Points = 331, ConstructorId = 1, Constructor = redbull  };
var perez      = new Driver { Id = 2, Name = "Sergio",  Surname = "Perez",      Number = 11, Price = 18.0m, Points = 152, ConstructorId = 1, Constructor = redbull  };
var leclerc    = new Driver { Id = 3, Name = "Charles", Surname = "Leclerc",    Number = 16, Price = 26.5m, Points = 209, ConstructorId = 2, Constructor = ferrari  };
var sainz      = new Driver { Id = 4, Name = "Carlos",  Surname = "Sainz",      Number = 55, Price = 23.0m, Points = 184, ConstructorId = 2, Constructor = ferrari  };
var hamilton   = new Driver { Id = 5, Name = "Lewis",   Surname = "Hamilton",   Number = 44, Price = 28.0m, Points = 190, ConstructorId = 3, Constructor = mercedes };
var russell    = new Driver { Id = 6, Name = "George",  Surname = "Russell",    Number = 63, Price = 21.0m, Points = 164, ConstructorId = 3, Constructor = mercedes };

redbull.Drivers.AddRange(new[]  { verstappen, perez });
ferrari.Drivers.AddRange(new[]  { leclerc, sainz });
mercedes.Drivers.AddRange(new[] { hamilton, russell });

// ================= CIRCUITS =================
var bahrain = new Circuit { Id = 1, Name = "Bahrain International Circuit", Country = "Bahrain", City = "Sakhir",      Length = 5.412, NumberOfLaps = 57 };
var monaco  = new Circuit { Id = 2, Name = "Circuit de Monaco",             Country = "Monaco",  City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78 };
var monza   = new Circuit { Id = 3, Name = "Autodromo Nazionale Monza",     Country = "Italy",   City = "Monza",       Length = 5.793, NumberOfLaps = 53 };

// ================= RACE =================
var bahrainGP = new Race { Id = 1, Name = "Bahrain Grand Prix", RaceDate = new DateTime(2024, 3,  2), CircuitId = 1, Circuit = bahrain };
var monacoGP  = new Race { Id = 2, Name = "Monaco Grand Prix",  RaceDate = new DateTime(2024, 5, 26), CircuitId = 2, Circuit = monaco  };
var italianGP = new Race { Id = 3, Name = "Italian Grand Prix", RaceDate = new DateTime(2024, 9,  1), CircuitId = 3, Circuit = monza   };

// ================= RACE RESULTS =================
// Bahrain GP
var rr1 = new RaceResult { Id = 1,  FinishedPosition = 1, ScoredPoints = (int)RaceResultPoints.First, DriverStatus = DriverStatus.Finished, DriverId = 1, Driver = verstappen, RaceId = 1, Race = bahrainGP };
var rr2 = new RaceResult { Id = 2,  FinishedPosition = 2, ScoredPoints = (int)RaceResultPoints.Second, DriverStatus = DriverStatus.Finished, DriverId = 4, Driver = sainz,      RaceId = 1, Race = bahrainGP };
var rr3 = new RaceResult { Id = 3,  FinishedPosition = 3, ScoredPoints = (int)RaceResultPoints.Third, DriverStatus = DriverStatus.Finished, DriverId = 3, Driver = leclerc,    RaceId = 1, Race = bahrainGP };
var rr4 = new RaceResult { Id = 4,  FinishedPosition = 4, ScoredPoints = (int)RaceResultPoints.Fourth, DriverStatus = DriverStatus.Finished, DriverId = 5, Driver = hamilton,   RaceId = 1, Race = bahrainGP };
var rr5 = new RaceResult { Id = 5,  FinishedPosition = 0, ScoredPoints = (int)RaceResultPoints.OutOfPoints, DriverStatus = DriverStatus.DNF,      DriverId = 2, Driver = perez,      RaceId = 1, Race = bahrainGP };
 
// Monaco GP
var rr6 = new RaceResult { Id = 6,  FinishedPosition = 1, ScoredPoints = (int)RaceResultPoints.First, DriverStatus = DriverStatus.Finished, DriverId = 3, Driver = leclerc,    RaceId = 2, Race = monacoGP };
var rr7 = new RaceResult { Id = 7,  FinishedPosition = 2, ScoredPoints = (int)RaceResultPoints.Second, DriverStatus = DriverStatus.Finished, DriverId = 1, Driver = verstappen, RaceId = 2, Race = monacoGP };
var rr8 = new RaceResult { Id = 8,  FinishedPosition = 3, ScoredPoints = (int)RaceResultPoints.Third, DriverStatus = DriverStatus.Finished, DriverId = 5, Driver = hamilton,   RaceId = 2, Race = monacoGP };
var rr9 = new RaceResult { Id = 9,  FinishedPosition = 0, ScoredPoints = (int)RaceResultPoints.OutOfPoints, DriverStatus = DriverStatus.DSQ,      DriverId = 6, Driver = russell,    RaceId = 2, Race = monacoGP };
 
// Italian GP
var rr10 = new RaceResult { Id = 10, FinishedPosition = 1, ScoredPoints = (int)RaceResultPoints.First, DriverStatus = DriverStatus.Finished, DriverId = 3, Driver = leclerc,    RaceId = 3, Race = italianGP };
var rr11 = new RaceResult { Id = 11, FinishedPosition = 2, ScoredPoints = (int)RaceResultPoints.Second, DriverStatus = DriverStatus.Finished, DriverId = 5, Driver = hamilton,   RaceId = 3, Race = italianGP };
var rr12 = new RaceResult { Id = 12, FinishedPosition = 3, ScoredPoints = (int)RaceResultPoints.Third, DriverStatus = DriverStatus.Finished, DriverId = 6, Driver = russell,    RaceId = 3, Race = italianGP };
var rr13 = new RaceResult { Id = 13, FinishedPosition = 0, ScoredPoints = (int)RaceResultPoints.OutOfPoints, DriverStatus = DriverStatus.DNF,      DriverId = 1, Driver = verstappen, RaceId = 3, Race = italianGP };
 
bahrainGP.RaceResults.AddRange(new[] { rr1, rr2, rr3, rr4, rr5 });
monacoGP.RaceResults.AddRange(new[]  { rr6, rr7, rr8, rr9 });
italianGP.RaceResults.AddRange(new[] { rr10, rr11, rr12, rr13 });

verstappen.RaceResults.AddRange(new[] { rr1, rr7, rr13 });
perez.RaceResults.Add(rr5);
leclerc.RaceResults.AddRange(new[] { rr3, rr6, rr10 });
sainz.RaceResults.Add(rr2);
hamilton.RaceResults.AddRange(new[] { rr4, rr8, rr11 });
russell.RaceResults.AddRange(new[] { rr9, rr12 });

// ================= USERS =================
var user1 = new User { Id = 1, Name = "Marko", Surname = "Horvat", Email = "marko@email.com", PasswordHash = "hash1", Role = Role.Admin };
var user2 = new User { Id = 2, Name = "Ivana", Surname = "Zec",    Email = "ivana@email.com", PasswordHash = "hash2", Role = Role.User  };
var user3 = new User { Id = 3, Name = "Pero",  Surname = "Kovač",  Email = "pero@email.com",  PasswordHash = "hash3", Role = Role.User  };

// ================= FANTASY LEAGUES =================
var liga1 = new FantasyLeague
{
    Id = 1,
    Name = "Prijatelji Liga 2024",
    Description = "Privatna fantasy liga",
    StartDate = new DateTime(2024, 3, 1),
    EndDate = new DateTime(2024, 11, 30),
    LeagueType = LeagueType.Private
};
 
var liga2 = new FantasyLeague
{
    Id = 2,
    Name = "Javna Liga 2024",
    Description = "Otvorena liga za sve",
    StartDate = new DateTime(2024, 3, 1),
    EndDate = new DateTime(2024, 11, 30),
    LeagueType = LeagueType.Public
};
 
var liga3 = new FantasyLeague
{
    Id = 3,
    Name = "Elitna Liga 2024",
    Description = "Liga za napredne igrače",
    StartDate = new DateTime(2024, 3, 1),
    EndDate = new DateTime(2024, 11, 30),
    LeagueType = LeagueType.Private
};

// ================= FANTASY TEAMS =================
var team1 = new FantasyTeam { Id = 1, Name = "Speed Demons",        Budget = 88.5m, Points = 420, UserId = 1, User = user1, ConstructorId = 2, Constructor = ferrari,  FantasyLeagueId = 1, FantasyLeague = liga1 };
var team2 = new FantasyTeam { Id = 2, Name = "Tifosi Forza",        Budget = 71.0m, Points = 385, UserId = 2, User = user2, ConstructorId = 3, Constructor = mercedes, FantasyLeagueId = 1, FantasyLeague = liga1 };
var team3 = new FantasyTeam { Id = 3, Name = "Verstappen Fan Club", Budget = 80.0m, Points = 398, UserId = 3, User = user3, ConstructorId = 1, Constructor = redbull,  FantasyLeagueId = 2, FantasyLeague = liga2 };

var team4 = new FantasyTeam { Id = 4, Name = "One Man Wolf Pack", Budget = 88.5m, Points = 410, UserId = 1, User = user1, ConstructorId = 2, Constructor = ferrari, FantasyLeagueId = 3, FantasyLeague = liga3 };
var team5 = new FantasyTeam { Id = 5, Name = "Forza England", Budget = 71.0m, Points = 350, UserId = 2, User = user2, ConstructorId = 3, Constructor = mercedes, FantasyLeagueId = 3, FantasyLeague = liga3 };
var team6 = new FantasyTeam { Id = 6, Name = "LH44", Budget = 80.0m, Points = 380, UserId = 3, User = user3, ConstructorId = 1, Constructor = redbull, FantasyLeagueId = 3, FantasyLeague = liga3 };


team1.Drivers.AddRange(new[] { verstappen, leclerc, hamilton });
team2.Drivers.AddRange(new[] { leclerc, sainz, russell });
team3.Drivers.AddRange(new[] { verstappen, perez, hamilton });

team4.Drivers.AddRange(new[] { sainz, leclerc, hamilton });
team5.Drivers.AddRange(new[] { leclerc, verstappen, hamilton });
team6.Drivers.AddRange(new[] { sainz, russell, hamilton });

liga1.FantasyTeams.AddRange(new[] { team1, team2 });
liga2.FantasyTeams.Add(team3);
liga3.FantasyTeams.AddRange(new[] { team4, team5, team6 });

user1.FantasyTeams.AddRange(new[] { team1, team4 });
user2.FantasyTeams.AddRange(new[] { team2, team5 });
user3.FantasyTeams.AddRange(new[] { team3, team6 });

// ================= LINQ UPITI =================

var allDrivers      = new List<Driver>      { verstappen, perez, leclerc, sainz, hamilton, russell };
var allResults      = new List<RaceResult> { rr1, rr2, rr3, rr4, rr5, rr6, rr7, rr8, rr9, rr10, rr11, rr12, rr13 };
var allLeagues      = new List<FantasyLeague> { liga1, liga2, liga3 };


var driverus = allDrivers
    .Where(d => d.Price<25)
    .Select(d=> new { d.Name, d.Points, d.Price})
    .OrderByDescending(d=>d.Price)
    .ToList();
foreach(var driverius in driverus)
    Console.WriteLine($"Ime {driverius.Name} Bodovi {driverius.Points} Cijena {driverius.Price}");


var driverAnalysis = allDrivers
    .OrderByDescending(d => d.Points)
    .Select(d => new
    {
        Ime = $"{d.Name} {d.Surname}",
        d.Points,
        Tim = d.Constructor.Name,
        Skup = d.Price > 25 ? "DA" : "NE"
    });

Console.WriteLine("1. Vozači (poredak + skupi):");
foreach (var d in driverAnalysis)
    Console.WriteLine($"   {d.Ime} ({d.Tim}) - {d.Points} bodova | Skup: {d.Skup}");


var raceSummary = allResults
    .GroupBy(r => r.Race.Name)
    .Select(g => new
    {
        Utrka = g.Key,
        Pobjednik = g
            .Where(r => r.DriverStatus == DriverStatus.Finished)
            .OrderBy(r => r.FinishedPosition)
            .FirstOrDefault()?.Driver.Surname ?? "N/A",
        Incidenti = g.Count(r => r.DriverStatus != DriverStatus.Finished)
    });

Console.WriteLine("\n2. Sažetak utrka:");
foreach (var r in raceSummary)
    Console.WriteLine($"   {r.Utrka} -> Pobjednik: {r.Pobjednik} | Incidenti: {r.Incidenti}");


var constructorStandings = allResults
    .Where(r => r.DriverStatus == DriverStatus.Finished)
    .GroupBy(r => r.Driver.Constructor.Name)
    .Select(g => new
    {
        Konstruktor = g.Key,
        Bodovi = g.Sum(r => r.ScoredPoints)
    })
    .OrderByDescending(x => x.Bodovi);

Console.WriteLine("\n3. Poredak konstruktora:");
foreach (var c in constructorStandings)
    Console.WriteLine($"   {c.Konstruktor}: {c.Bodovi} bodova");


var leagueOverview = allLeagues
    .Select(l => new
    {
        Liga = l.Name,
        Tip = l.LeagueType.ToString(),
        BrojTimova = l.FantasyTeams.Count,
        NajboljiTim = l.FantasyTeams
            .OrderByDescending(t => t.Points)
            .FirstOrDefault()?.Name ?? "N/A"
    });

Console.WriteLine("\n4. Pregled liga:");
foreach (var l in leagueOverview)
    Console.WriteLine($"   {l.Liga} ({l.Tip}) - {l.BrojTimova} timova | Najbolji: {l.NajboljiTim}");


Console.WriteLine("\n========== KRAJ ==========");









// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
