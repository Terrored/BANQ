using DataAccess.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, int, ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>
    {
        DbSet<BankAccount> BankAccounts { get; set; }

        //private ObjectContext _objectContext;
        //private DbTransaction _transaction;
        private static readonly object Lock = new object();
        //private static bool _databaseInitialized;

        public ApplicationDbContext()
            : base("name=BANQConnectionString")
        {
            //if (_databaseInitialized)
            //{
            //    return;
            //}
            //lock (Lock)
            //{
            //    if (!_databaseInitialized)
            //    {
            //        // Set the database intializer which is run once during application start
            //        // This seeds the database with admin user credentials and admin role
            //        Database.SetInitializer(new ApplicationDbInitializer());
            //        _databaseInitialized = true;
            //    }
            //}
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EfConfig.ConfigureEf(modelBuilder);
        }

        //private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : BaseEntity
        //{
        //    var dbEntityEntry = GetDbEntityEntrySafely(entity);
        //    dbEntityEntry.State = entityState;
        //}

        //private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : BaseEntity
        //{
        //    var dbEntityEntry = Entry<TEntity>(entity);
        //    if (dbEntityEntry.State == EntityState.Detached)
        //    {
        //        Set<TEntity>().Attach(entity);
        //    }
        //    return dbEntityEntry;
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
        //        {
        //            _objectContext.Connection.Close();
        //        }
        //        if (_objectContext != null)
        //        {
        //            _objectContext.Dispose();
        //        }
        //        if (_transaction != null)
        //        {
        //            _transaction.Dispose();
        //        }
        //    }
        //    base.Dispose(disposing);
        //}
    }
}