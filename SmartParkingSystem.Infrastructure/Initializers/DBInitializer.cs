using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Infrastructure.Initializers
{
    public static class DBInitializer
    {
        public static string AdminRoleId = Guid.NewGuid().ToString();
        public static string UserRoleId = Guid.NewGuid().ToString();

        public static string AdminId = Guid.NewGuid().ToString();
        public static string UserId = Guid.NewGuid().ToString();

        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            foreach (var item in new List<(string id, string name)>()
            {
                (AdminRoleId, "Administrator"),
                (UserRoleId, "User"),
            })
            {
                modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
                {
                    Id = item.id,
                    Name = item.name,
                    NormalizedName = item.name.ToUpper()
                });
            }
        }
        public static void SeedAdministrator(this ModelBuilder modelBuilder)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var adminUser = new AppUser
            {
                Id = AdminId,
                FullName = "John Connor Johnovych",
                UserName = "admin@email.com",
                NormalizedUserName = "ADMIN@EMAIL.COM",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "",
                PhoneNumberConfirmed = false,
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Qwerty-1");
            modelBuilder.Entity<AppUser>().HasData(adminUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = AdminRoleId,
                UserId = AdminId
            });
        }

        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var sellerUser = new AppUser
            {
                Id = UserId,
                FullName = "John Doe Johnovych",
                UserName = "user@email.com",
                NormalizedUserName = "USER@EMAIL.COM",
                Email = "user@email.com",
                NormalizedEmail = "USER@EMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "",
                PhoneNumberConfirmed = false,
            };
            sellerUser.PasswordHash = passwordHasher.HashPassword(sellerUser, "Qwerty-1");
            modelBuilder.Entity<AppUser>().HasData(sellerUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = UserRoleId,
                UserId = UserId
            });
        }

        public static void SeedParkingLots(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingLot>().HasData(
                new ParkingLot { Id = 1, Name = "Main Street Parking", Location = "123 Main St", TotalCapacity = 100 },
                new ParkingLot { Id = 2, Name = "Downtown Garage", Location = "456 Center Ave", TotalCapacity = 200 },
                new ParkingLot { Id = 3, Name = "City Square Lot", Location = "789 Park Blvd", TotalCapacity = 150 }
            );
        }

        public static void SeedParkingSpots(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingSpot>().HasData(
                new ParkingSpot { Id = 1, Location = "Level 1 - Spot 1", IsOccupied = false, SensorId = 1, LastUpdated = DateTime.UtcNow, ParkingLotId = 1 },
                new ParkingSpot { Id = 2, Location = "Level 1 - Spot 2", IsOccupied = true, SensorId = 2, LastUpdated = DateTime.UtcNow, ParkingLotId = 1 },
                new ParkingSpot { Id = 3, Location = "Level 2 - Spot 1", IsOccupied = false, SensorId = 3, LastUpdated = DateTime.UtcNow, ParkingLotId = 2 }
            );
        }

        public static void SeedReservations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { Id = 1, UserId = UserId, ParkingSpotId = 1, StartTime = DateTime.UtcNow.AddHours(-1), EndTime = DateTime.UtcNow.AddHours(1) },
                new Reservation { Id = 2, UserId = UserId, ParkingSpotId = 3, StartTime = DateTime.UtcNow.AddHours(-2), EndTime = DateTime.UtcNow.AddHours(2) }
            );
        }

        public static void SeedSensors(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>().HasData(
                new Sensor { Id = 1, Type = "Ultrasonic", Status = "Active" },
                new Sensor { Id = 2, Type = "Camera", Status = "Inactive" },
                new Sensor { Id = 3, Type = "Pressure", Status = "Active" }
            );
        }
    }
}
