using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiApp.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RequestClient> RequestClients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string DispatcherRoleName = "Диспетчер";
            string DriverRoleName = "Водитель";

            Role DispatcherRole = new Role { Id = 1, Name = DispatcherRoleName };
            Role DriverRole = new Role { Id = 2, Name = DriverRoleName };
            User Default = new User() {
            Id = 1, Login = "Не назначен", RoleId = 2
            };

            modelBuilder.Entity<User>().HasData(Default);
            modelBuilder.Entity<Role>().HasData(new Role[] { DispatcherRole, DriverRole });
            modelBuilder.Entity<RequestClient>().Property(r => r.UserId).HasDefaultValue(1);
        }
    }
}
