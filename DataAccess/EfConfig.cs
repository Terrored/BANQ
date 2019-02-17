using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Model.Identity;

namespace DataAccess
{
    public class EfConfig
    {
        public static void ConfigureEf(DbModelBuilder modelBuilder)
        {
            
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
