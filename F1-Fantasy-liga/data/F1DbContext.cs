using Microsoft.EntityFrameworkCore;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Data 
{
    public class F1DbContext : DbContext
    {
        public F1DbContext(DbContextOptions<F1DbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Constructor> Constructors { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<RaceResult> RaceResults { get; set; }
        public DbSet<Circuit> Circuits { get; set; }
        public DbSet<FantasyTeam> FantasyTeams { get; set; }
        public DbSet<FantasyLeague> FantasyLeagues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Constructors
            modelBuilder.Entity<Constructor>().HasData(
                new Constructor { Id = 1, Name = "Red Bull Racing", Nationality = "Austrian", FoundedDate = new DateTime(2005, 3, 6) },
                new Constructor { Id = 2, Name = "Scuderia Ferrari", Nationality = "Italian", FoundedDate = new DateTime(1950, 5, 21) },
                new Constructor { Id = 3, Name = "Mercedes-AMG Petronas", Nationality = "German", FoundedDate = new DateTime(1954, 7, 4) },
                new Constructor { Id = 4, Name = "McLaren", Nationality = "British", FoundedDate = new DateTime(1963, 5, 22) },
                new Constructor { Id = 5, Name = "Aston Martin", Nationality = "British", FoundedDate = new DateTime(2021, 3, 28) },
                new Constructor { Id = 6, Name = "Alpine", Nationality = "French", FoundedDate = new DateTime(2021, 1, 1) },
                new Constructor { Id = 7, Name = "Williams", Nationality = "British", FoundedDate = new DateTime(1977, 5, 8) },
                new Constructor { Id = 8, Name = "Visa Cash App RB", Nationality = "Italian", FoundedDate = new DateTime(2024, 2, 8) },
                new Constructor { Id = 9, Name = "Kick Sauber", Nationality = "Swiss", FoundedDate = new DateTime(1993, 1, 1) },
                new Constructor { Id = 10, Name = "Haas F1 Team", Nationality = "American", FoundedDate = new DateTime(2016, 3, 20) }
            );

            // Circuits
            modelBuilder.Entity<Circuit>().HasData(
                new Circuit { Id = 1, Name = "Bahrain International Circuit", Country = "Bahrain", City = "Sakhir", Length = 5.412, NumberOfLaps = 57 },
                new Circuit { Id = 2, Name = "Circuit de Monaco", Country = "Monaco", City = "Monte Carlo", Length = 3.337, NumberOfLaps = 78 },
                new Circuit { Id = 3, Name = "Autodromo Nazionale Monza", Country = "Italy", City = "Monza", Length = 5.793, NumberOfLaps = 53 }
            );

            // Races
            modelBuilder.Entity<Race>().HasData(
                new Race { Id = 1, Name = "Bahrain Grand Prix", RaceDate = new DateTime(2024, 3, 2), CircuitId = 1 },
                new Race { Id = 2, Name = "Monaco Grand Prix", RaceDate = new DateTime(2024, 5, 26), CircuitId = 2 },
                new Race { Id = 3, Name = "Italian Grand Prix", RaceDate = new DateTime(2024, 9, 1), CircuitId = 3 }
            );

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Marko", Surname = "Horvat", Email = "marko@email.com", PasswordHash = "hash1", Role = Role.Admin },
                new User { Id = 2, Name = "Ivana", Surname = "Zec", Email = "ivana@email.com", PasswordHash = "hash2", Role = Role.User },
                new User { Id = 3, Name = "Pero", Surname = "Kovač", Email = "pero@email.com", PasswordHash = "hash3", Role = Role.User }
            );

            // FantasyLeagues
            modelBuilder.Entity<FantasyLeague>().HasData(
                new FantasyLeague { Id = 1, Name = "Prijatelji Liga 2024", Description = "Privatna fantasy liga", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private },
                new FantasyLeague { Id = 2, Name = "Javna Liga 2024", Description = "Otvorena liga za sve", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Public },
                new FantasyLeague { Id = 3, Name = "Elitna Liga 2024", Description = "Liga za napredne igrače", StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 11, 30), LeagueType = LeagueType.Private }
            );

            // Drivers (bez Points jer je NotMapped)
            modelBuilder.Entity<Driver>().HasData(
                new Driver { Id = 1, Name = "Max", Surname = "Verstappen", Number = 1, Price = 33.5m, ConstructorId = 1 },
                new Driver { Id = 2, Name = "Sergio", Surname = "Perez", Number = 11, Price = 18.0m, ConstructorId = 1 },
                new Driver { Id = 3, Name = "Charles", Surname = "Leclerc", Number = 16, Price = 26.5m, ConstructorId = 2 },
                new Driver { Id = 4, Name = "Carlos", Surname = "Sainz", Number = 55, Price = 23.0m, ConstructorId = 2 },
                new Driver { Id = 5, Name = "Lewis", Surname = "Hamilton", Number = 44, Price = 28.0m, ConstructorId = 3 },
                new Driver { Id = 6, Name = "George", Surname = "Russell", Number = 63, Price = 21.0m, ConstructorId = 3 },
                new Driver { Id = 7, Name = "Lando", Surname = "Norris", Number = 4, Price = 25.0m, ConstructorId = 4 },
                new Driver { Id = 8, Name = "Oscar", Surname = "Piastri", Number = 81, Price = 21.5m, ConstructorId = 4 },
                new Driver { Id = 9, Name = "Fernando", Surname = "Alonso", Number = 14, Price = 24.0m, ConstructorId = 5 },
                new Driver { Id = 10, Name = "Lance", Surname = "Stroll", Number = 18, Price = 16.0m, ConstructorId = 5 },
                new Driver { Id = 11, Name = "Esteban", Surname = "Ocon", Number = 31, Price = 17.5m, ConstructorId = 6 },
                new Driver { Id = 12, Name = "Pierre", Surname = "Gasly", Number = 10, Price = 18.0m, ConstructorId = 6 },
                new Driver { Id = 13, Name = "Alexander", Surname = "Albon", Number = 23, Price = 15.5m, ConstructorId = 7 },
                new Driver { Id = 14, Name = "Logan", Surname = "Sargeant", Number = 2, Price = 12.0m, ConstructorId = 7 },
                new Driver { Id = 15, Name = "Yuki", Surname = "Tsunoda", Number = 22, Price = 17.0m, ConstructorId = 8 },
                new Driver { Id = 16, Name = "Daniel", Surname = "Ricciardo", Number = 3, Price = 18.5m, ConstructorId = 8 },
                new Driver { Id = 17, Name = "Valtteri", Surname = "Bottas", Number = 77, Price = 16.5m, ConstructorId = 9 },
                new Driver { Id = 18, Name = "Guanyu", Surname = "Zhou", Number = 24, Price = 15.0m, ConstructorId = 9 },
                new Driver { Id = 19, Name = "Kevin", Surname = "Magnussen", Number = 20, Price = 15.5m, ConstructorId = 10 },
                new Driver { Id = 20, Name = "Nico", Surname = "Hulkenberg", Number = 27, Price = 16.0m, ConstructorId = 10 }
            );

            // RaceResults
            modelBuilder.Entity<RaceResult>().HasData(
                new RaceResult { Id = 1, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 1 },
                new RaceResult { Id = 2, FinishedPosition = 2, ScoredPoints = 18, DriverStatus = DriverStatus.Finished, DriverId = 4, RaceId = 1 },
                new RaceResult { Id = 3, FinishedPosition = 3, ScoredPoints = 15, DriverStatus = DriverStatus.Finished, DriverId = 3, RaceId = 1 },
                new RaceResult { Id = 4, FinishedPosition = 4, ScoredPoints = 12, DriverStatus = DriverStatus.Finished, DriverId = 5, RaceId = 1 },
                new RaceResult { Id = 5, FinishedPosition = 0, ScoredPoints = 0, DriverStatus = DriverStatus.DNF, DriverId = 2, RaceId = 1 },
                new RaceResult { Id = 6, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 3, RaceId = 2 },
                new RaceResult { Id = 7, FinishedPosition = 2, ScoredPoints = 18, DriverStatus = DriverStatus.Finished, DriverId = 1, RaceId = 2 },
                new RaceResult { Id = 8, FinishedPosition = 3, ScoredPoints = 15, DriverStatus = DriverStatus.Finished, DriverId = 5, RaceId = 2 },
                new RaceResult { Id = 9, FinishedPosition = 0, ScoredPoints = 0, DriverStatus = DriverStatus.DSQ, DriverId = 6, RaceId = 2 },
                new RaceResult { Id = 10, FinishedPosition = 1, ScoredPoints = 25, DriverStatus = DriverStatus.Finished, DriverId = 3, RaceId = 3 },
                new RaceResult { Id = 11, FinishedPosition = 2, ScoredPoints = 18, DriverStatus = DriverStatus.Finished, DriverId = 5, RaceId = 3 },
                new RaceResult { Id = 12, FinishedPosition = 3, ScoredPoints = 15, DriverStatus = DriverStatus.Finished, DriverId = 6, RaceId = 3 },
                new RaceResult { Id = 13, FinishedPosition = 0, ScoredPoints = 0, DriverStatus = DriverStatus.DNF, DriverId = 1, RaceId = 3 }
            );

            // FantasyTeams
            modelBuilder.Entity<FantasyTeam>().HasData(
                new FantasyTeam { Id = 1, Name = "Speed Demons", Budget = 88.5m, UserId = 1, ConstructorId = 2, FantasyLeagueId = 1 },
                new FantasyTeam { Id = 2, Name = "Tifosi Forza", Budget = 71.0m, UserId = 2, ConstructorId = 3, FantasyLeagueId = 1 },
                new FantasyTeam { Id = 3, Name = "Verstappen Fan Club", Budget = 80.0m, UserId = 3, ConstructorId = 1, FantasyLeagueId = 2 },
                new FantasyTeam { Id = 4, Name = "One Man Wolf Pack", Budget = 88.5m, UserId = 1, ConstructorId = 2, FantasyLeagueId = 3 },
                new FantasyTeam { Id = 5, Name = "Forza England", Budget = 71.0m, UserId = 2, ConstructorId = 3, FantasyLeagueId = 3 },
                new FantasyTeam { Id = 6, Name = "LH44", Budget = 80.0m, UserId = 3, ConstructorId = 1, FantasyLeagueId = 3 }
            );



            modelBuilder.Entity<FantasyTeam>()
            .HasMany(ft => ft.Drivers)
            .WithMany(d => d.FantasyTeams)
            .UsingEntity(j =>
            {
                j.ToTable("FantasyTeamDrivers");
                j.HasOne(typeof(Driver))
                .WithMany()
                .HasForeignKey("DriversId")
                .OnDelete(DeleteBehavior.NoAction);
                j.HasOne(typeof(FantasyTeam))
                .WithMany()
                .HasForeignKey("FantasyTeamsId")
                .OnDelete(DeleteBehavior.NoAction);
            
                j.HasData(
                new { FantasyTeamsId = 1, DriversId = 1 },
                new { FantasyTeamsId = 1, DriversId = 3 },
                new { FantasyTeamsId = 1, DriversId = 5 },
                new { FantasyTeamsId = 2, DriversId = 3 },
                new { FantasyTeamsId = 2, DriversId = 4 },
                new { FantasyTeamsId = 2, DriversId = 6 },
                new { FantasyTeamsId = 3, DriversId = 1 },
                new { FantasyTeamsId = 3, DriversId = 2 },
                new { FantasyTeamsId = 3, DriversId = 5 },
                new { FantasyTeamsId = 4, DriversId = 4 },
                new { FantasyTeamsId = 4, DriversId = 3 },
                new { FantasyTeamsId = 4, DriversId = 5 },
                new { FantasyTeamsId = 5, DriversId = 3 },
                new { FantasyTeamsId = 5, DriversId = 1 },
                new { FantasyTeamsId = 5, DriversId = 5 },
                new { FantasyTeamsId = 6, DriversId = 4 },
                new { FantasyTeamsId = 6, DriversId = 6 },
                new { FantasyTeamsId = 6, DriversId = 5 }
                );
            });
        }
    }
}