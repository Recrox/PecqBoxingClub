using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Repositories
{
    public interface IRepository<T, TId>
        : IRepository<T, T, TId>
        where TId : IEquatable<TId>
        where T : class
    { }

    public interface IRepository<T, TOut, TId> 
        where TId : IEquatable<TId> 
        where T : class
        where TOut : class
    {
        Task<TOut> GetByIdAsync(TId id, Expression<Func<T, TOut>> selector = null);
        Task<TOut> GetSingleAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, TOut>> selector = null);
        Task<TOut> GetFirstAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, TOut>> selector = null);
        Task<ICollection<TOut>> GetManyAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TOut>> selector = null);
        Task<ICollection<TOut>> GetManyAsync<TKey>(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, TKey>> orderBy = null, bool ascending = true,
            SieveModel parameters = null, 
            Expression<Func<T, TOut>> selector = null);
        Task<TOut> CreateAsync(TOut obj, bool saveChanges = true, bool reloadResult = false);
        Task<TOut> ReplaceAsync(TOut obj, bool saveChanges = true, bool reloadResult = false);
        Task<TOut> UpsertAsync(TOut obj, bool saveChanges = true, bool reloadResult = false);
        Task<bool> DeleteAsync(TId id, bool softDelete = false, bool saveChanges = true);
        Task SaveChangesAsync();
    }
}
