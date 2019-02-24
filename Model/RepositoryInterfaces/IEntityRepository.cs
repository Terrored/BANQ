using System.Linq;

namespace Model.RepositoryInterfaces
{
    public interface IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetSingle(int id);
        IQueryable<TEntity> GetAll();

        void Delete(int id);
        void Update(TEntity entity);
        void Create(TEntity entity);
    }
}
