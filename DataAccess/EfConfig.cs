using DataAccess.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DataAccess
{
    public class EfConfig
    {
        public static void ConfigureEf(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationIdentityUser>()
                .HasOptional(e => e.BankAccount)
                .WithRequired(b => b.ApplicationIdentityUser);
            modelBuilder.Entity<ApplicationIdentityUser>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationIdentityRole>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationIdentityUserClaim>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


        }
    }
}
