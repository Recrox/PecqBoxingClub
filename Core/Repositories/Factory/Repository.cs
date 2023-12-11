using AutoMapper;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Factory;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext dbContext;
    protected readonly IMapper mapper;

    protected Repository(
        DbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var datas = await dbContext.Set<T>().ToListAsync();

        return datas;
    }

    public async Task<T> GetAsync(int id) => await dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id) ?? throw new NullReferenceException();

    public async Task AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
