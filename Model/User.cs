using Microsoft.AspNet.Identity.EntityFramework;

namespace Model
{
    // You can add profile data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public string Nickname { get; set; }

    }
}