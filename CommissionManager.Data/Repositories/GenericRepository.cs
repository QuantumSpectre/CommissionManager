using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CommissionManager.Data.Repositories
{
    public class GenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _appDbContext = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        //Takes a lamda expression as predicate variable to return entites that match condition
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _appDbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _appDbContext.Entry(entity).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }

        public void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
                _appDbContext.SaveChanges();
            }

        }

        public void Delete(T entity)
        {
            if(_appDbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
                
            }

            _dbSet.Remove(entity);
            _appDbContext.SaveChanges();
        }
    }
}
