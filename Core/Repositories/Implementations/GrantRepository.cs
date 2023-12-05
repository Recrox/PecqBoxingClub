using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RamDam.BackEnd.Core.Models.Table;


namespace RamDam.BackEnd.Core.Repositories
{
    public class GrantRepository : IGrantRepository
    {
        private RamDamContext _context;

        public GrantRepository(RamDamContext dbContext)
        {
            _context = dbContext;
        }

        public async Task DeleteAsync(string key)
        {
            var grant = await (from dbGrant in _context.Grants
                               where dbGrant.Key == key
                               select dbGrant).SingleOrDefaultAsync();

            _context.Grants.Remove(grant);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string subjectId, string clientId)
        {
            var grant = await (from dbGrant in _context.Grants
                               where dbGrant.SubjectId == subjectId
                               //&& dbGrant.ClientId == clientId
                               select dbGrant).SingleOrDefaultAsync();

            _context.Grants.Remove(grant);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string subjectId, string clientId, string type)
        {
            var grant = await (from dbGrant in _context.Grants
                               where dbGrant.SubjectId == subjectId
                               //&& dbGrant.ClientId == clientId
                               && dbGrant.Type == type
                               select dbGrant).SingleOrDefaultAsync();

            _context.Grants.Remove(grant);

            await _context.SaveChangesAsync();
        }

        public async Task<Grant> GetByKeyAsync(string key)
        {
            return await (from dbGrant in _context.Grants
                          where dbGrant.Key == key
                          select dbGrant).SingleOrDefaultAsync();
        }

        public async Task<ICollection<Grant>> GetManyAsync(string subjectId)
        {
            return await (from dbGrant in _context.Grants
                          where dbGrant.SubjectId == subjectId
                          select dbGrant).ToListAsync();
        }

        public async Task SaveAsync(Grant grant)
        {
            if (_context.Entry(grant).State == EntityState.Detached)
            {
                var current = await (from dbGrant in _context.Grants
                                     where dbGrant.Key == grant.Key
                                     select dbGrant).SingleOrDefaultAsync();

                if (current == null)
                {
                    await _context.Grants.AddAsync(grant);
                }
                else
                {
                    current.Type = grant.Type;
                    current.SubjectId = grant.SubjectId;
                    //current.ClientId = grant.ClientId;
                    current.CreationDate = grant.CreationDate;
                    current.ExpirationDate = grant.ExpirationDate;
                    current.Data = grant.Data;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
