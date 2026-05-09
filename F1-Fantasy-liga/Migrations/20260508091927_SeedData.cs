using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace F1_Fantasy_liga.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Circuits",
                columns: new[] { "Id", "City", "Country", "Length", "Name", "NumberOfLaps" },
                values: new object[,]
                {
                    { 1, "Sakhir", "Bahrain", 5.4119999999999999, "Bahrain International Circuit", 57 },
                    { 2, "Monte Carlo", "Monaco", 3.3370000000000002, "Circuit de Monaco", 78 },
                    { 3, "Monza", "Italy", 5.7930000000000001, "Autodromo Nazionale Monza", 53 }
                });

            migrationBuilder.InsertData(
                table: "Constructors",
                columns: new[] { "Id", "FoundedDate", "Name", "Nationality" },
                values: new object[,]
                {
                    { 1, new DateTime(2005, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Bull Racing", "Austrian" },
                    { 2, new DateTime(1950, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scuderia Ferrari", "Italian" },
                    { 3, new DateTime(1954, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mercedes-AMG Petronas", "German" },
                    { 4, new DateTime(1963, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "McLaren", "British" },
                    { 5, new DateTime(2021, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aston Martin", "British" },
                    { 6, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alpine", "French" },
                    { 7, new DateTime(1977, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Williams", "British" },
                    { 8, new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visa Cash App RB", "Italian" },
                    { 9, new DateTime(1993, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kick Sauber", "Swiss" },
                    { 10, new DateTime(2016, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Haas F1 Team", "American" }
                });

            migrationBuilder.InsertData(
                table: "FantasyLeagues",
                columns: new[] { "Id", "Description", "EndDate", "LeagueType", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, "Privatna fantasy liga", new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Prijatelji Liga 2024", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Otvorena liga za sve", new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Javna Liga 2024", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Liga za napredne igrače", new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Elitna Liga 2024", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role", "Surname" },
                values: new object[,]
                {
                    { 1, "marko@email.com", "Marko", "hash1", 0, "Horvat" },
                    { 2, "ivana@email.com", "Ivana", "hash2", 1, "Zec" },
                    { 3, "pero@email.com", "Pero", "hash3", 1, "Kovač" }
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "Id", "ConstructorId", "Name", "Number", "Price", "Surname" },
                values: new object[,]
                {
                    { 1, 1, "Max", 1, 33.5m, "Verstappen" },
                    { 2, 1, "Sergio", 11, 18.0m, "Perez" },
                    { 3, 2, "Charles", 16, 26.5m, "Leclerc" },
                    { 4, 2, "Carlos", 55, 23.0m, "Sainz" },
                    { 5, 3, "Lewis", 44, 28.0m, "Hamilton" },
                    { 6, 3, "George", 63, 21.0m, "Russell" },
                    { 7, 4, "Lando", 4, 25.0m, "Norris" },
                    { 8, 4, "Oscar", 81, 21.5m, "Piastri" },
                    { 9, 5, "Fernando", 14, 24.0m, "Alonso" },
                    { 10, 5, "Lance", 18, 16.0m, "Stroll" },
                    { 11, 6, "Esteban", 31, 17.5m, "Ocon" },
                    { 12, 6, "Pierre", 10, 18.0m, "Gasly" },
                    { 13, 7, "Alexander", 23, 15.5m, "Albon" },
                    { 14, 7, "Logan", 2, 12.0m, "Sargeant" },
                    { 15, 8, "Yuki", 22, 17.0m, "Tsunoda" },
                    { 16, 8, "Daniel", 3, 18.5m, "Ricciardo" },
                    { 17, 9, "Valtteri", 77, 16.5m, "Bottas" },
                    { 18, 9, "Guanyu", 24, 15.0m, "Zhou" },
                    { 19, 10, "Kevin", 20, 15.5m, "Magnussen" },
                    { 20, 10, "Nico", 27, 16.0m, "Hulkenberg" }
                });

            migrationBuilder.InsertData(
                table: "FantasyTeams",
                columns: new[] { "Id", "Budget", "ConstructorId", "FantasyLeagueId", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, 88.5m, 2, 1, "Speed Demons", 1 },
                    { 2, 71.0m, 3, 1, "Tifosi Forza", 2 },
                    { 3, 80.0m, 1, 2, "Verstappen Fan Club", 3 },
                    { 4, 88.5m, 2, 3, "One Man Wolf Pack", 1 },
                    { 5, 71.0m, 3, 3, "Forza England", 2 },
                    { 6, 80.0m, 1, 3, "LH44", 3 }
                });

            migrationBuilder.InsertData(
                table: "Races",
                columns: new[] { "Id", "CircuitId", "Name", "RaceDate" },
                values: new object[,]
                {
                    { 1, 1, "Bahrain Grand Prix", new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, "Monaco Grand Prix", new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, "Italian Grand Prix", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "FantasyTeamDrivers",
                columns: new[] { "DriversId", "FantasyTeamsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 1, 5 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 4 },
                    { 3, 5 },
                    { 4, 2 },
                    { 4, 4 },
                    { 4, 6 },
                    { 5, 1 },
                    { 5, 3 },
                    { 5, 4 },
                    { 5, 5 },
                    { 5, 6 },
                    { 6, 2 },
                    { 6, 6 }
                });

            migrationBuilder.InsertData(
                table: "RaceResults",
                columns: new[] { "Id", "DriverId", "DriverStatus", "FinishedPosition", "RaceId", "ScoredPoints" },
                values: new object[,]
                {
                    { 1, 1, 0, 1, 1, 25 },
                    { 2, 4, 0, 2, 1, 18 },
                    { 3, 3, 0, 3, 1, 15 },
                    { 4, 5, 0, 4, 1, 12 },
                    { 5, 2, 2, 0, 1, 0 },
                    { 6, 3, 0, 1, 2, 25 },
                    { 7, 1, 0, 2, 2, 18 },
                    { 8, 5, 0, 3, 2, 15 },
                    { 9, 6, 3, 0, 2, 0 },
                    { 10, 3, 0, 1, 3, 25 },
                    { 11, 5, 0, 2, 3, 18 },
                    { 12, 6, 0, 3, 3, 15 },
                    { 13, 1, 2, 0, 3, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "FantasyTeamDrivers",
                keyColumns: new[] { "DriversId", "FantasyTeamsId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RaceResults",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FantasyTeams",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FantasyTeams",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FantasyTeams",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FantasyTeams",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FantasyTeams",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FantasyTeams",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Circuits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Circuits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Circuits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Constructors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FantasyLeagues",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FantasyLeagues",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FantasyLeagues",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
