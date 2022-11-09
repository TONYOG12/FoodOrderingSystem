using Domedo.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.Domain.Context
{
    public class DomedoDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DomedoDbContext(DbContextOptions<DomedoDbContext> options) : base(options)
        {

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Role>().ToTable("roles");


            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("userroles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("userclaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("userlogins");

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("roleclaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("usertokens");


        }
    }
}
