
using System;
using Microsoft.EntityFrameworkCore;
using TelecomShop.DBModels;


namespace TelecomShop.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _contextDbSet;
        private readonly TelcoShopDbContext _context;

        public Repository(TelcoShopDbContext context)
        {
            _contextDbSet = context.Set<TEntity>();
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _contextDbSet.ToList();
        }

        public TEntity Add(TEntity entity)
        {
            return _contextDbSet.Add(entity).Entity;
        }

        public TEntity Get(int id)
        {
            return _contextDbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            _contextDbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            TEntity tEntity = _contextDbSet.Find(id);
            if (tEntity != null)
            {
                _contextDbSet.Remove(tEntity);
            }
        }
    }

}
