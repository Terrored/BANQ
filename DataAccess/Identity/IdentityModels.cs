using Microsoft.AspNet.Identity.EntityFramework;
using Model.Models;
using System.Collections.Generic;

namespace DataAccess.Identity
{
    // You can add profile data for the user by adding more properties to your ApplicationIdentityUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationIdentityUser :
        IdentityUser<int, ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>
    {
        public string Sex { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public ICollection<MoneyTransfer> MoneyTransfers { get; set; }
    }


    public class ApplicationIdentityRole : IdentityRole<int, ApplicationIdentityUserRole>
    {
        public ApplicationIdentityRole()
        {
        }

        public ApplicationIdentityRole(string name)
        {
            Name = name;
        }
    }

    public class ApplicationIdentityUserRole : IdentityUserRole<int>
    {
    }

    public class ApplicationIdentityUserClaim : IdentityUserClaim<int>
    {
    }

    public class ApplicationIdentityUserLogin : IdentityUserLogin<int>
    {
    }

    public class BankAccount : BaseBankAccount
    {
        public BankAccountType BankAccountType { get; set; }
        public int BankAccountTypeId { get; set; }
        public ApplicationIdentityUser ApplicationIdentityUser { get; set; }
        public int ApplicationIdentityUserId { get; set; }
    }

    public class MoneyTransfer : BaseMoneyTransfer
    {
        public ApplicationIdentityUser From { get; set; }
        public int FromId { get; set; }
        public ApplicationIdentityUser To { get; set; }
        public int ToId { get; set; }
    }

    public class BankAccountType : BaseBankAccountType
    {
    }
}
