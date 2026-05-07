using Microsoft.EntityFrameworkCore;
using F1_Fantasy_liga.Models;

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
            });
        }
    }
}