using System;
using System.Collections.Generic;

namespace Model.Identity
{
    public class AppUser
    {
        public AppUser()
        {
            Claims = new List<ApplicationUserClaim>();
            Roles = new List<ApplicationUserRole>();
            Logins = new List<ApplicationUserLogin>();
        }
        public int AccessFailedCount { get; set; }
        public ICollection<ApplicationUserClaim> Claims { get; private set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Id { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public ICollection<ApplicationUserLogin> Logins { get; private set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public ICollection<ApplicationUserRole> Roles { get; private set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; }
    }
}
