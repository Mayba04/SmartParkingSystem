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
            var adminUserId = Guid.NewGuid().ToString();
            var adminUser = new AppUser
            {
                Id = adminUserId,
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
                UserId = adminUserId
            });
        }

        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var UserId = Guid.NewGuid().ToString();
            var sellerUser = new AppUser
            {
                Id = UserId,
                FullName = "John Doe Johnovych",
                UserName = "seller@email.com",
                NormalizedUserName = "SELLER@EMAIL.COM",
                Email = "seller@email.com",
                NormalizedEmail = "SELLER@EMAIL.COM",
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
    }
}
