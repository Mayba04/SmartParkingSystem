using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Infrastructure.Context
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext() : base() { }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Sensor> Sensors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ParkingSpot - ParkingLot
            modelBuilder.Entity<ParkingSpot>()
                .HasOne(ps => ps.ParkingLot)
                .WithMany(pl => pl.ParkingSpots)
                .HasForeignKey(ps => ps.ParkingLotId)
                .OnDelete(DeleteBehavior.Cascade);

            // ParkingSpot - Sensor
            modelBuilder.Entity<ParkingSpot>()
                .HasOne(ps => ps.Sensor)
                .WithMany(s => s.ParkingSpots)
                .HasForeignKey(ps => ps.SensorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation - AppUser
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reservation - ParkingSpot
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ParkingSpot)
                .WithMany()
                .HasForeignKey(r => r.ParkingSpotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
