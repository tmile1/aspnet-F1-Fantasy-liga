using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Repositories
{
    public class MockDataStore
    {
        public List<Constructor> Constructors { get; }
        public List<Driver> Drivers { get; }
        public List<Circuit> Circuits { get; }
        public List<Race> Races { get; }
        public List<RaceResult> RaceResults { get; }
        public List<User> Users { get; }
        public List<FantasyLeague> FantasyLeagues { get; }
        public List<FantasyTeam> FantasyTeams { get; }

        public MockDataStore()
        {
            var redbull = new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", FoundedDate = new DateTime(2005, 3, 6) };
            var ferrari = new Constructor { Id = 2, Name = "Scuderia Ferrari", Nationality = "Italian", FoundedDate = new DateTime(1950, 5, 21) };
            var mercedes = new Constructor { Id = 3, Name = "Mercedes-AMG Petronas", Nationality = "German", FoundedDate = new DateTime(1954, 7, 4) };
            var mclaren = new Constructor { Id = 4, Name = "McLaren", Nationality = "British", FoundedDate = new DateTime(1963, 5, 22) };
            var astonMartin = new Constructor { Id = 5, Name = "Aston Martin", Nationality = "British", FoundedDate = new DateTime(2021, 3, 28) };
            var alpine = new Constructor { Id = 6, Name = "Alpine", Nationality = "French", FoundedDate = new DateTime(2021, 1, 1) };
            var williams = new Constructor { Id = 7, Name = "Williams", Nationality = "British", FoundedDate = new DateTime(1977, 5, 8) };
            var visarb = new Constructor { Id = 8, Name = "Visa Cash App RB", Nationality = "Italian", FoundedDate = new DateTime(2024, 2, 8) };
            var sauber = new Constructor { Id = 9, Name = "Kick Sauber", Nationality = "Swiss", FoundedDate = new DateTime(1993, 1, 1) };
            var haas = new Constructor { Id = 10, Name = "Haas F1 Team", Nationality = "American", FoundedDate = new DateTime(2016, 3, 20) };

            //var verstappen = new Driver { Id = 1, Name = "Max", Surname = "Verstappen", Number = 1, Price = 33.5m, Points = 331, ConstructorId = 1, Constructor = redbull };
            //var perez = new Driver { Id = 2, Name = "Sergio", Surname = "Perez", Number = 11, Price = 18.0m, Points = 152, ConstructorId = 1, Constructor = redbull };
            //var leclerc = new Driver { Id = 3, Name = "Charles", Surname = "Leclerc", Number = 16, Price = 26.5m, Points = 209, ConstructorId = 2, Constructor = ferrari };
            //var sainz = new Driver { Id = 4, Name = "Carlos", Surname = "Sainz", Number = 55, Price = 23.0m, Points = 184, ConstructorId = 2, Constructor = ferrari };
            //var hamilton = new Driver { Id = 5, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28.0m, Points = 190, ConstructorId = 3, Constructor = mercedes };
            //var russell = new Driver { Id = 6, Name = "George", Surname = "Russell", Number = 63, Price = 21.0m, Points = 164, ConstructorId = 3, Constructor = mercedes };
            //var norris = new Driver { Id = 7, Name = "Lando", Surname = "Norris", Number = 4, Price = 25.0m, Points = 205, ConstructorId = 4, Constructor = mclaren };
            //var piastri = new Driver { Id = 8, Name = "Oscar", Surname = "Piastri", Number = 81, Price = 21.5m, Points = 167, ConstructorId = 4, Constructor = mclaren };
            //var alonso = new Driver { Id = 9, Name = "Fernando", Surname = "Alonso", Number = 14, Price = 24.0m, Points = 198, ConstructorId = 5, Constructor = astonMartin };
            //var stroll = new Driver { Id = 10, Name = "Lance", Surname = "Stroll", Number = 18, Price = 16.0m, Points = 74, ConstructorId = 5, Constructor = astonMartin };
            //var ocon = new Driver { Id = 11, Name = "Esteban", Surname = "Ocon", Number = 31, Price = 17.5m, Points = 58, ConstructorId = 6, Constructor = alpine };
            //var gasly = new Driver { Id = 12, Name = "Pierre", Surname = "Gasly", Number = 10, Price = 18.0m, Points = 62, ConstructorId = 6, Constructor = alpine };
            //var albon = new Driver { Id = 13, Name = "Alexander", Surname = "Albon", Number = 23, Price = 15.5m, Points = 76, ConstructorId = 7, Constructor = williams };
            //var sargeant = new Driver { Id = 14, Name = "Logan", Surname = "Sargeant", Number = 2, Price = 12.0m, Points = 12, ConstructorId = 7, Constructor = williams };
            //var tsunoda = new Driver { Id = 15, Name = "Yuki", Surname = "Tsunoda", Number = 22, Price = 17.0m, Points = 64, ConstructorId = 8, Constructor = visarb };
            //var ricciardo = new Driver { Id = 16, Name = "Daniel", Surname = "Ricciardo", Number = 3, Price = 18.5m, Points = 30, ConstructorId = 8, Constructor = visarb };
            //var bottas = new Driver { Id = 17, Name = "Valtteri", Surname = "Bottas", Number = 77, Price = 16.5m, Points = 49, ConstructorId = 9, Constructor = sauber };
            //var zhou = new Driver { Id = 18, Name = "Guanyu", Surname = "Zhou", Number = 24, Price = 15.0m, Points = 32, ConstructorId = 9, Constructor = sauber };
            //var magnussen = new Driver { Id = 19, Name = "Kevin", Surname = "Magnussen", Number = 20, Price = 15.5m, Points = 25, ConstructorId = 10, Constructor = haas };
            //var hulkenberg = new Driver { Id = 20, Name = "Nico", Surname = "Hulkenberg", Number = 27, Price = 16.0m, Points = 41, ConstructorId = 10, Constructor = haas };

            //redbull.Drivers.AddRange(new[] { verstappen, perez });
            //ferrari.Drivers.AddRange(new[] { leclerc, sainz });
            //mercedes.Drivers.AddRange(new[] { hamilton, russell });
            //mclaren.Drivers.AddRange(new[] { norris, piastri });
            //astonMartin.Drivers.AddRange(new[] { alonso, stroll });
            //alpine.Drivers.AddRange(new[] { ocon, gasly });
            //williams.Drivers.AddRange(new[] { albon, sargeant });
            //visarb.Drivers.AddRange(new[] { tsunoda, ricciardo });
            //sauber.Drivers.AddRange(new[] { bottas, zhou });
            //haas.Drivers.AddRange(new[] { magnussen, hulkenberg });

            var bahrain = new Circuit { Id = 1, Name = "Bahrain International Circuit", Country = "Bahrain", City = "Sakhir", Length = 5.412, NumberOfLaps = 57 };
            var monaco = new Circuit { Id = 2, Name = "Circuit de Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78 };
            var monza = new Circuit { Id = 3, Name = "Autodromo Nazionale Monza", Country = "Italy", City = "Monza", Length = 5.793, NumberOfLaps = 53 };

            var bahrainGP = new Race { Id = 1, Name = "Bahrain Grand Prix", RaceDate = new DateTime(2024, 3, 2), CircuitId = 1, Circuit = bahrain };
            var monacoGP = new Race { Id = 2, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 2, Circuit = monaco };
            var italianGP = new Race { Id = 3, Name = "Italian Grand Prix", RaceDate = new DateTime(2024, 9, 1), CircuitId = 3, Circuit = monza };

            //var rr1 = new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = (int)RaceResultPoints.First, DriverStatus = DriverStatus.Finished, DriverId = 1, Driver = verstappen, RaceId = 1, Race = bahrainGP };
            //var rr2 = new RaceResult { Id = 2, FinishedPosition = 2, ScoredPoints = (int)RaceResultPoints.Second, DriverStatus = DriverStatus.Finished, DriverId = 4, Driver = sainz, RaceId = 1, Race = bahrainGP };
            //var rr3 = new RaceResult { Id = 3, FinishedPosition = 3, ScoredPoints = (int)RaceResultPoints.Third, DriverStatus = DriverStatus.Finished, DriverId = 3, Driver = leclerc, RaceId = 1, Race = bahrainGP };
            //var rr4 = new RaceResult { Id = 4, FinishedPosition = 4, ScoredPoints = (int)RaceResultPoints.Fourth, DriverStatus = DriverStatus.Finished, DriverId = 5, Driver = hamilton, RaceId = 1, Race = bahrainGP };
            //var rr5 = new RaceResult { Id = 5, FinishedPosition = 0, ScoredPoints = (int)RaceResultPoints.OutOfPoints, DriverStatus = DriverStatus.DNF, DriverId = 2, Driver = perez, RaceId = 1, Race = bahrainGP };

            //var rr6 = new RaceResult { Id = 6, FinishedPosition = 1, ScoredPoints = (int)RaceResultPoints.First, DriverStatus = DriverStatus.Finished, DriverId = 3, Driver = leclerc, RaceId = 2, Race = monacoGP };
            //var rr7 = new RaceResult { Id = 7, FinishedPosition = 2, ScoredPoints = (int)RaceResultPoints.Second, DriverStatus = DriverStatus.Finished, DriverId = 1, Driver = verstappen, RaceId = 2, Race = monacoGP };
            //var rr8 = new RaceResult { Id = 8, FinishedPosition = 3, ScoredPoints = (int)RaceResultPoints.Third, DriverStatus = DriverStatus.Finished, DriverId = 5, Driver = hamilton, RaceId = 2, Race = monacoGP };
            //var rr9 = new RaceResult { Id = 9, FinishedPosition = 0, ScoredPoints = (int)RaceResultPoints.OutOfPoints, DriverStatus = DriverStatus.DSQ, DriverId = 6, Driver = russell, RaceId = 2, Race = monacoGP };

            //var rr10 = new RaceResult { Id = 10, FinishedPosition = 1, ScoredPoints = (int)RaceResultPoints.First, DriverStatus = DriverStatus.Finished, DriverId = 3, Driver = leclerc, RaceId = 3, Race = italianGP };
            //var rr11 = new RaceResult { Id = 11, FinishedPosition = 2, ScoredPoints = (int)RaceResultPoints.Second, DriverStatus = DriverStatus.Finished, DriverId = 5, Driver = hamilton, RaceId = 3, Race = italianGP };
            //var rr12 = new RaceResult { Id = 12, FinishedPosition = 3, ScoredPoints = (int)RaceResultPoints.Third, DriverStatus = DriverStatus.Finished, DriverId = 6, Driver = russell, RaceId = 3, Race = italianGP };
            //var rr13 = new RaceResult { Id = 13, FinishedPosition = 0, ScoredPoints = (int)RaceResultPoints.OutOfPoints, DriverStatus = DriverStatus.DNF, DriverId = 1, Driver = verstappen, RaceId = 3, Race = italianGP };

            //bahrainGP.RaceResults.AddRange(new[] { rr1, rr2, rr3, rr4, rr5 });
            //monacoGP.RaceResults.AddRange(new[] { rr6, rr7, rr8, rr9 });
            //italianGP.RaceResults.AddRange(new[] { rr10, rr11, rr12, rr13 });

            //verstappen.RaceResults.AddRange(new[] { rr1, rr7, rr13 });
            //perez.RaceResults.Add(rr5);
            //leclerc.RaceResults.AddRange(new[] { rr3, rr6, rr10 });
            //sainz.RaceResults.Add(rr2);
            //hamilton.RaceResults.AddRange(new[] { rr4, rr8, rr11 });
            //russell.RaceResults.AddRange(new[] { rr9, rr12 });

            var user1 = new User { Id = 1, Name = "Marko", Surname = "Horvat", Email = "marko@email.com", PasswordHash = "hash1", Role = Role.Admin };
            var user2 = new User { Id = 2, Name = "Ivana", Surname = "Zec", Email = "ivana@email.com", PasswordHash = "hash2", Role = Role.User };
            var user3 = new User { Id = 3, Name = "Pero", Surname = "Kovač", Email = "pero@email.com", PasswordHash = "hash3", Role = Role.User };

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

            //var team1 = new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, Points = 420, UserId = 1, User = user1, ConstructorId = 2, Constructor = ferrari, FantasyLeagueId = 1, FantasyLeague = liga1 };
            //var team2 = new FantasyTeam { Id = 2, Name = "Tifosi Forza", Budget = 71.0m, Points = 385, UserId = 2, User = user2, ConstructorId = 3, Constructor = mercedes, FantasyLeagueId = 1, FantasyLeague = liga1 };
            //var team3 = new FantasyTeam { Id = 3, Name = "Verstappen Fan Club", Budget = 80.0m, Points = 398, UserId = 3, User = user3, ConstructorId = 1, Constructor = redbull, FantasyLeagueId = 2, FantasyLeague = liga2 };
            //var team4 = new FantasyTeam { Id = 4, Name = "One Man Wolf Pack", Budget = 88.5m, Points = 410, UserId = 1, User = user1, ConstructorId = 2, Constructor = ferrari, FantasyLeagueId = 3, FantasyLeague = liga3 };
            //var team5 = new FantasyTeam { Id = 5, Name = "Forza England", Budget = 71.0m, Points = 350, UserId = 2, User = user2, ConstructorId = 3, Constructor = mercedes, FantasyLeagueId = 3, FantasyLeague = liga3 };
            //var team6 = new FantasyTeam { Id = 6, Name = "LH44", Budget = 80.0m, Points = 380, UserId = 3, User = user3, ConstructorId = 1, Constructor = redbull, FantasyLeagueId = 3, FantasyLeague = liga3 };

            //team1.Drivers.AddRange(new[] { verstappen, leclerc, hamilton });
            //team2.Drivers.AddRange(new[] { leclerc, sainz, russell });
            //team3.Drivers.AddRange(new[] { verstappen, perez, hamilton });
            //team4.Drivers.AddRange(new[] { sainz, leclerc, hamilton });
            //team5.Drivers.AddRange(new[] { leclerc, verstappen, hamilton });
            //team6.Drivers.AddRange(new[] { sainz, russell, hamilton });

            //liga1.FantasyTeams.AddRange(new[] { team1, team2 });
            //liga2.FantasyTeams.Add(team3);
            //liga3.FantasyTeams.AddRange(new[] { team4, team5, team6 });

            //user1.FantasyTeams.AddRange(new[] { team1, team4 });
            //user2.FantasyTeams.AddRange(new[] { team2, team5 });
            //user3.FantasyTeams.AddRange(new[] { team3, team6 });

            Constructors = new List<Constructor> { redbull, ferrari, mercedes, mclaren, astonMartin, alpine, williams, visarb, sauber, haas };
            //Drivers = new List<Driver> { verstappen, perez, leclerc, sainz, hamilton, russell, norris, piastri, alonso, stroll, ocon, gasly, albon, sargeant, tsunoda, ricciardo, bottas, zhou, magnussen, hulkenberg };
            Circuits = new List<Circuit> { bahrain, monaco, monza };
            Races = new List<Race> { bahrainGP, monacoGP, italianGP };
            //RaceResults = new List<RaceResult> { rr1, rr2, rr3, rr4, rr5, rr6, rr7, rr8, rr9, rr10, rr11, rr12, rr13 };
            Users = new List<User> { user1, user2, user3 };
            FantasyLeagues = new List<FantasyLeague> { liga1, liga2, liga3 };
            //FantasyTeams = new List<FantasyTeam> { team1, team2, team3, team4, team5, team6 };
        }
    }
}