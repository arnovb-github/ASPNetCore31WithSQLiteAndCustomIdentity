using Microsoft.AspNetCore.Identity;

namespace ASPNetCore31WithSQLiteAndCustomIdentity.Models
{
    // extend IdentityUser and use int as primary key
    public class ApplicationUser : IdentityUser<int>
    {
        public string ExampleColumn { get; set; }
    }
}
