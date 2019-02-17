using DataAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IService : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IService<TEntity> : IService where TEntity : BaseEntity
    {
        List<TEntity> GetAll();



        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
