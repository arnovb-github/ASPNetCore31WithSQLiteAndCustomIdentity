# Purpose #
This project shows a number of common customizations to using ASP.NET `Authentication` with `Individual User Acounts`. That is: you control the authentication database. I created it because I simply cannot remember the steps and they keep changing with every iteration of .Net Core. This is for .Net Core 3.1, it may be relevant to 5.0, it may not be.

- Use SQLite instead of MS SQL Server (this is entirely optional).
- Customize `IdentityUser`.
- Change the Primary Key data type.
- Rename the columns that `Microsoft.AspNetCore.Identity` uses by default.

# Step-by-step guide #
- In Visual Studio, create a new ASP.Net Core 3.x project (I used ASP.NET Core Web Application).
- make sure you select the option to use Authentication and set it to Individual Accounts
- Delete all files under `Migrations`

You can do also this with the `dotnet` CLI and Visual Studio Code. You can even bring in Authentication in existing projects, this repository is not about that.

# Using SQLite #
*Skip this step if you are fine with MS SQL Server*
- Install the `Microsoft.EntityFrameworkCore.Sqlite` package. Do not just bring in the `Microsoft.EntityFrameworkCore.Sqlite.Core` package.
- (optionally)Create a folder that will hold your database
- (optionally)Remove the now unneeded reference to `Microsoft.EntityFrameworkCore.SqlServer`
- Edit `appsettings.json` to point to SQLite (see source)
- Edit the `ConfigureServices` method in `Startup.cs` to use SQLite (see source)

This will make you application use SQLite instead of MS SQL Server

# Customize IdentityUser #
- Add a class `ApplicationUser` (I put it in a folder `Models`), make it inherit from `IdentityUser<int>`. This allows you to extend the properties of `IdentityUser` and it also sets the primary key to use an `int` instead of a string (default).
- Replace all other instances of `IdentityUser` in the entire project with `ApplicationUser`
- Do not forget to bring in the namespace of `ApplicationUser` in the files where it is used (like `_LoginPartial.cshtml`)

# Customize table names #
In `ApplicationDbContext`, change the class declaration to:

```c#
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
```

Changing the type parameters of `IdentityDbContext` took me days to figure out. You'd think you'd be fine with passing in just your customized `IdentityUser` type, e.g. `ApplicationUser`. Except no. You also need the role and type. Go figure. I believe you can from this point extend the declaration of `IdentityDbContext` as you further modify basic Identity classes, but I haven't tried it and I want the example to be simple.

Once you get the context to wpork at all, overriding the creation of the tablenames is simple, also notice the overriding of the default primary key types (to `int`):

```c#
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
```

Add and apply a migration in the Package Manager Console:
- Add-Migration CreateIdentitySchema
- Update-Database

Notice that if you changed you database to SQLite, EF is smart enough to generate SQLite syntax.

Inspect your database. It should now have the tablenames your defined in `ApplicationDbContext`. Your `Users` table should have a column `ExampleColumn`.

Run you app. (It may complain about SSL. Accept the suggested certificate for now.) It should just work.


