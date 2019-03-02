using System;
using System.Linq;
using System.Linq.Expressions;

namespace Model.RepositoryInterfaces
{
    public interface IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetSingle(int id);
        TEntity GetSingle(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        void Delete(int id);
        void Update(TEntity entity);
        void Create(TEntity entity);
        int CreateAndReturnId(TEntity entity);
    }
}
