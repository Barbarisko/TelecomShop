
using TelecomShop.DBModels;

namespace TelecomShop.Repository
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        TEntity Get(int id);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
