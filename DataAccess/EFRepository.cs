﻿using Model;
using Model.RepositoryInterfaces;
using System.Data.Entity;
using System.Linq;

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

        public TEntity GetSingle(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
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
