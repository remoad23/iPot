using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace iPotAPI.Model
{
    public class PotContext : DbContext
    {
        public DbSet<PlantState> PlantState { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public PotContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Settings>().HasData(new Settings
            {
                SettingsId = 1,
                MoistureMinimum = 2,
                LightMinimum = 2,
                AutomaticLight = true,
                AutomaticWatering = true,
                NotificationLight = true,
                NotificationMoisture = true,
                NotificationWater = true,
                UptimeStart = "00:00:00",
                UptimeEnd = "00:20:00"
            });
            
            modelBuilder.Entity<PlantState>().HasData(new PlantState
            {
                PlantStateId = 1,
                SettingsId = 1,
                LedIntensity = 50,
                AmbientLightIntensity = 30,
                WaterStorage =  50,
                MoistureValue =  5,
            });
        }
    }
}