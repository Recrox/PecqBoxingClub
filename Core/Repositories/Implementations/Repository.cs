using RamDam.BackEnd.Core.Models.Table;
using System;
using System.Threading.Tasks;
using RamDam.BackEnd.Core.Models.Api;
using AutoMapper;
using Sieve.Services;
using RamDam.BackEnd.Core.Repositories;
using RamDam.BackEnd.Core.Models.Table;
using RamDam.BackEnd.Core.Models.Api;

namespace RamDam.BackEnd.Core.Repositories
{
    public abstract class Repository<T, TApi, TId> : BaseRepository<T, TApi, TId>, IRepository<T, TApi, TId>
        where TId : IEquatable<TId>
        where T : class, ITableObject<TId>, new()
        where TApi : class, IApiObject<T, TId>
    {
        private readonly IMapper _mapper;

        public Repository(RamDamContext context, CurrentContext currentContext, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, currentContext, sieveProcessor)
        {
            _mapper = mapper;
        }

        public virtual async Task<TApi> CreateAsync(TApi obj, bool saveChanges = true, bool reloadResult = false)
        {
            var tableObj = obj.ToTableObject(_mapper.Map(obj, new T()), _currentContext);
            await base.CreateAsync(tableObj, saveChanges);
            obj.SetId(tableObj);

            return reloadResult ? await GetByIdAsync(obj.Id) : obj;
        }

        public virtual async Task<TApi> ReplaceAsync(TApi obj, bool saveChanges = true, bool reloadResult = false)
        {
            var tableObj = await GetTableObjectAsync(obj.Id);
            if (tableObj == null)
                return null;

            obj.ToTableObject(_mapper.Map(obj, tableObj), _currentContext);
            await base.ReplaceAsync(tableObj, saveChanges);
            obj.SetId(tableObj);

            return reloadResult ? await GetByIdAsync(obj.Id) : obj;
        }

        public virtual async Task<TApi> UpsertAsync(TApi obj, bool saveChanges = true, bool reloadResult = false)
        {
            if (obj.Id.Equals(default))
            {
                return await CreateAsync(obj, saveChanges, reloadResult);
            }
            else
            {
                return await ReplaceAsync(obj, saveChanges, reloadResult);
            }
        }

        protected virtual async Task<T> GetTableObjectAsync(TId id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }

    public abstract class IdentityRepository<T, TId> : BaseRepository<T, TId>, IRepository<T, TId>
        where TId : IEquatable<TId>
        where T : class, ITableObject<TId>
    {
        public IdentityRepository(RamDamContext context, CurrentContext currentContext)
            : base(context, currentContext)
        {
        }
    }
}
