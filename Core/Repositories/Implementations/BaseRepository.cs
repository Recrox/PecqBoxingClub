using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using RamDam.BackEnd.Core.Models.Table;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Sieve.Models;
using Sieve.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RamDam.BackEnd.Core;

namespace RamDam.BackEnd.Core.Repositories
{
    public abstract class BaseRepository<T, TId>
        : BaseRepository<T, T, TId>
        where TId : IEquatable<TId>
        where T : class, ITableObject<TId>
    {
        public BaseRepository(RamDamContext context, CurrentContext currentContext) 
            : base(context, currentContext, null)
        {
        }
    }

    public abstract class BaseRepository<T, TApi, TId>
        where TId : IEquatable<TId>
        where T : class, ITableObject<TId>
        where TApi : class
    {
        protected readonly RamDamContext _context;
        protected readonly CurrentContext _currentContext;
        protected readonly ISieveProcessor _sieveProcessor;

        public BaseRepository(RamDamContext context, CurrentContext currentContext, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _currentContext = currentContext;
            _sieveProcessor = sieveProcessor;
        }

        public virtual async Task<TApi> GetByIdAsync(TId id, Expression<Func<T, TApi>> selector = null)
        {
            return await (from e in _context.Set<T>()
                          where e.Id.Equals(id)
                          select e)
                          .Select(selector ?? Select)
                          .SingleOrDefaultAsync();
        }

       
        public virtual async Task<TApi> GetSingleAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, TApi>> selector = null)
        {
            var query = GetBaseQuery();
            return await (from e in query
                          select e)
                         .Where(predicate)
                         .Select(selector ?? Select)
                         .SingleOrDefaultAsync();
        }


        public virtual async Task<TApi> GetFirstAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, TApi>> selector = null)
        {
            var query = GetBaseQuery();
            return await (from e in query
                          select e)
                         .Where(predicate)
                         .Select(selector ?? Select)
                         .FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<TApi>> GetManyAsync(
            Expression<Func<T, bool>> predicate = null, 
            Expression<Func<T, TApi>> selector = null)
        {
            var query = GetManyQuery(predicate);
          
            return await query
                .Select(selector ?? Select)
                .ToListAsync();
        }

        public virtual async Task<ICollection<TApi>> GetManyAsync<TKey>(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, TKey>> orderBy = null, bool ascending = true, 
            SieveModel parameters = null,
            Expression<Func<T, TApi>> selector = null)
        {
            var query = GetManyQuery(predicate);

            query = ApplyParameters(query, parameters);

            if (orderBy != null && (parameters == null || parameters.Sorts == null))
                query = ascending ?
                        query.OrderBy(orderBy) :
                        query.OrderByDescending(orderBy);

            return await query
                .Select(selector ?? Select)
                .ToListAsync();
        }

        public virtual async Task<T> CreateAsync(T obj, bool saveChanges = true, bool reloadResult = false)
        {
            //obj.CreatedOn = _currentContext.DateTime;
            //obj.CreatedBy = _currentContext.UserId;
            if (_context.Entry(obj).State == EntityState.Detached)
            {
                await _context.Set<T>().AddAsync(obj);
            }
            if (_context.Entry(obj).State == EntityState.Added && saveChanges)
            {
                await _context.SaveChangesAsync();
            }
            return obj;
        }

        public virtual async Task<T> ReplaceAsync(T obj, bool saveChanges = true, bool reloadResult = false)
        {
            //obj.ModifiedOn = _currentContext.DateTime;
            //obj.ModifiedBy = _currentContext.UserId;
            if (_context.Entry(obj).State == EntityState.Detached)
            {
                _context.Entry(obj).State = EntityState.Modified;
            }

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var property in properties)
            {
                var value = property.GetValue(obj);
                if (value is ITableObject<Guid> baseTableObject && _context.Entry(value).State == EntityState.Detached)
                {
                    if (baseTableObject.Id.Equals(default))
                        _context.Entry(value).State = EntityState.Added;
                    else
                        _context.Entry(value).State = EntityState.Modified;
                }
            }

            if (saveChanges)
                await _context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task<T> UpsertAsync(T obj, bool saveChanges = true, bool reloadResult = false)
        {
            if (obj.Id.Equals(default))
            {
                return await CreateAsync(obj, saveChanges);
            }
            else
            {
                return await ReplaceAsync(obj, saveChanges);
            }
        }

        public virtual async Task<bool> DeleteAsync(TId id, bool softDelete = false, bool saveChanges = true)
        {
            var obj = await _context.Set<T>().FindAsync(id);
            if (obj == null)
                return false;

            if (softDelete)
            {
                if (obj is IDeletable deletableObj)
                    deletableObj.IsDeleted = true;
                else
                    throw new Exception("This object is not IDeletable");
            }
            else
                _context.Set<T>().Remove(obj);
            
            if (saveChanges)
                await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<T> GetManyQuery(Expression<Func<T, bool>> predicate = null)
        {
            var query = GetBaseQuery();

            if (query is IQueryable<IDeletable>)
                query = query.Where(e => !((IDeletable)e).IsDeleted);
            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        private IQueryable<T> ApplyParameters(IQueryable<T> query, SieveModel parameters)
        {
            if (parameters != null)
            {
                query = _sieveProcessor.Apply(parameters, query, applyPagination: false);
                if (parameters.Page.HasValue && parameters.PageSize.HasValue)
                {
                    var pagination = new Models.Pagination.Pagination
                    {
                        PageNumber = parameters.Page.Value,
                        PageSize = parameters.PageSize.Value,
                        TotalRecords = query.Count()
                    };
                   
                    _currentContext.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                    _currentContext.HttpContext.Response.Headers.Add("X-Pagination",
                        JsonConvert.SerializeObject(
                            pagination,
                            new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            }));

                    query = _sieveProcessor.Apply(parameters, query, applyFiltering: false, applySorting: false);
                }
            }
            return query;
        }

        protected virtual IQueryable<T> GetBaseQuery()
        {
            return from e in _context.Set<T>()
                   select e;
        }

        protected virtual Expression<Func<T, bool>> PaginationFilter { get; }
        protected abstract Expression<Func<T, TApi>> Select { get; }
    }
}
