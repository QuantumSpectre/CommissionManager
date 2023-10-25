using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommissionManager.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        void Create(T entity);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entity);
    }
}