using DataAccess.DTOs;
using System.Linq;

namespace DataAccess
{
    public interface IEntityRepository<TEntityDto> where TEntityDto : BaseDto
    {
        TEntityDto GetSingle(int id);
        IQueryable<TEntityDto> GetAll();

        void Create(TEntityDto entityDto);
        void Update(TEntityDto entityDto);
        void Delete(int id);
    }
}
