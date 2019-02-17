using System;
using System.Data.Entity;

namespace DataAccess
{
    //public class Repository<T> : IRepository<T> where T : class
    //{
    //    private readonly System.Data.Entity.DbContext _context;
    //    private DbSet<T> _entities;
    //    private string _errorMessage = string.Empty;

    //    public Repository(System.Data.Entity.DbContext context)
    //    {
    //        _context = context;
    //        _entities = context.Set<T>();
    //    }


    //    public void Insert(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            throw new ArgumentNullException();
    //        }

    //        _entities.Add(entity);
    //        _context.SaveChanges();

    //    }
    //}
}
