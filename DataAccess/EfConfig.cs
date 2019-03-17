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

            modelBuilder.Entity<ApplicationIdentityUser>().Property(u => u.Sex).IsRequired();
            modelBuilder.Entity<ApplicationIdentityUser>().Property(u => u.LastName).IsRequired();

            modelBuilder.Entity<BankAccountType>().Property(t => t.Name).IsRequired();

            modelBuilder.Entity<BankAccount>().HasKey(ba => ba.ApplicationIdentityUserId);

            modelBuilder.Entity<BankAccount>().HasRequired(ba => ba.BankAccountType);

            modelBuilder.Entity<ApplicationIdentityUser>()
                .Ignore(u => u.MoneyTransfers);

            modelBuilder.Entity<MoneyTransfer>()
                .HasRequired(mt => mt.From)
                .WithMany(u => u.MoneyTransfers)
                .HasForeignKey(u => u.FromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MoneyTransfer>()
                .HasRequired(mt => mt.To)
                .WithMany(u => u.MoneyTransfers)
                .HasForeignKey(u => u.ToId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Credit>().HasRequired(c => c.BankAccount).WithOptional(ba => ba.Credit);
            modelBuilder.Entity<CreditInstallment>().HasRequired(ci => ci.Credit).WithMany(c => c.Installments).WillCascadeOnDelete(false);
            modelBuilder.Entity<Credit>().Ignore(c => c.Installments);

            modelBuilder.Entity<Loan>().HasRequired(l => l.BankAccount).WithOptional(ba => ba.Loan);
            modelBuilder.Entity<LoanInstallment>().HasRequired(l => l.Loan).WithMany(l => l.Installments).WillCascadeOnDelete(false);
            modelBuilder.Entity<Loan>().Ignore(l => l.Installments);

            #region Identity
            modelBuilder.Entity<ApplicationIdentityUser>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationIdentityRole>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationIdentityUserClaim>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            #endregion

        }
    }
}
