using System;
using System.Collections.Generic;
using System.Text;
using ASPNetCore31WithSQLiteAndCustomIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCore31WithSQLiteAndCustomIdentity.Data
{
    // very important: change the class declaration. This was the mysterious bit for me
    //public class ApplicationDbContext : IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // override default table names
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users"); // Your custom IdentityUser class
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins"); // use int not string
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens"); // use int not string
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims"); // use int not string
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles"); // use int not string
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims"); // use int not string
            builder.Entity<IdentityRole<int>>().ToTable("Roles"); // use int not string
        }

    }
}
