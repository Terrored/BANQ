using Model;
using Model.RepositoryInterfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess
{
    public class EFRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _entities;

        public EFRepository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public int CreateAndReturnId(TEntity entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public void Delete(int id)
        {
            TEntity entityToDelete = _entities.FirstOrDefault(e => e.Id == id);

            if (entityToDelete != null)
            {
                _entities.Remove(entityToDelete);
            }

            _context.SaveChanges();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entities;
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> result = _entities;

            foreach (var property in includeProperties)
            {
                result = result.Include(property);
            }
            return result;
        }

        public TEntity GetSingle(int id)
        {
            return _entities.SingleOrDefault(e => e.Id == id);
        }

        public TEntity GetSingle(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> result = _entities;
            foreach (var property in includeProperties)
            {
                result = result.Include(property);
            }
            return result.SingleOrDefault(e => e.Id == id);
        }

        public void Update(TEntity entity)
        {
            var item = _entities.Find(entity.Id);
            if (item != null)
            {
                _context.Entry(item).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }
    }
}
