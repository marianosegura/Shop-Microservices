using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : EntityBase
    {  // interface of database CRUD operations for a entity class
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);  // query is a linq query expression to filter
        
        Task<IReadOnlyList<T>> GetAsync(  // query with order by option
            Expression<Func<T, bool>> predicate=null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy=null,
            string includeString=null,
            bool disableTracking=true
        ); 
        
        Task<IReadOnlyList<T>> GetAsync(  // query with order by and inclues option
            Expression<Func<T, bool>> predicate=null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy=null,
            List<Expression<Func<T, object>>> includes=null,
            bool disableTracking=true
        );  

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
