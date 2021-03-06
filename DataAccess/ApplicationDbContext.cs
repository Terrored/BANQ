﻿using DataAccess.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, int, ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>
    {
        private static readonly object Lock = new object();
        //public DbSet<MoneyTransfer> MoneyTransfers { get; set; }

        public ApplicationDbContext()
            : base("name=BANQConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            EfConfig.ConfigureEf(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}