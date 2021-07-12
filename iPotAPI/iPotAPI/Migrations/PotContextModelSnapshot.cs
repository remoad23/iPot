﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iPotAPI.Model;

namespace iPotAPI.Migrations
{
    [DbContext(typeof(PotContext))]
    partial class PotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("iPotAPI.Model.PlantState", b =>
                {
                    b.Property<int>("PlantStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("AmbientLightIntensity")
                        .HasColumnType("REAL");

                    b.Property<float>("LedIntensity")
                        .HasColumnType("REAL");

                    b.Property<float>("MoistureValue")
                        .HasColumnType("REAL");

                    b.Property<int>("SettingsId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("WaterStorage")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlantStateId");

                    b.HasIndex("SettingsId")
                        .IsUnique();

                    b.ToTable("PlantState");

                    b.HasData(
                        new
                        {
                            PlantStateId = 1,
                            AmbientLightIntensity = 30f,
                            LedIntensity = 50f,
                            MoistureValue = 5f,
                            SettingsId = 1,
                            WaterStorage = (byte)50
                        });
                });

            modelBuilder.Entity("iPotAPI.Model.Settings", b =>
                {
                    b.Property<int>("SettingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AutomaticLight")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AutomaticWatering")
                        .HasColumnType("INTEGER");

                    b.Property<float>("LightMinimum")
                        .HasColumnType("REAL");

                    b.Property<float>("MoistureMinimum")
                        .HasColumnType("REAL");

                    b.Property<bool>("NotificationLight")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NotificationMoisture")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NotificationWater")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UptimeEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("UptimeStart")
                        .HasColumnType("TEXT");

                    b.HasKey("SettingsId");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            SettingsId = 1,
                            AutomaticLight = true,
                            AutomaticWatering = true,
                            LightMinimum = 2f,
                            MoistureMinimum = 2f,
                            NotificationLight = true,
                            NotificationMoisture = true,
                            NotificationWater = true,
                            UptimeEnd = "00:20:00",
                            UptimeStart = "00:00:00"
                        });
                });

            modelBuilder.Entity("iPotAPI.Model.PlantState", b =>
                {
                    b.HasOne("iPotAPI.Model.Settings", "Settings")
                        .WithOne("PlantState")
                        .HasForeignKey("iPotAPI.Model.PlantState", "SettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("iPotAPI.Model.Settings", b =>
                {
                    b.Navigation("PlantState");
                });
#pragma warning restore 612, 618
        }
    }
}