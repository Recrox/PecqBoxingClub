using Dapper;
using Microsoft.EntityFrameworkCore;
using RamDam.BackEnd.Core.Models.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Repositories.Implementations
{
    public class SPBaseRepository
    {
        private readonly RamDamContext _context;

        public SPBaseRepository(RamDamContext context)
        {
            _context = context;
        }

        public async Task<dynamic> ExecuteQueryStoredProcedure<TResult>(SPBase objectToCast)
        {
            var result = (await _context.Database.GetDbConnection().QueryAsync<TResult>(objectToCast.getSPName(),
                    objectToCast,
                    commandType: System.Data.CommandType.StoredProcedure,
                    commandTimeout: 0)).AsList();
            return result;
        }

        public async Task<dynamic> ExecuteQueryFirstStoredProcedure<TResult>(SPBase objectToCast)
        {
            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<TResult>(objectToCast.getSPName(),
                     objectToCast,
                     commandType: System.Data.CommandType.StoredProcedure,
                     commandTimeout: 0);
            return result;
        }

        public async Task<bool> ExecuteStoredProcedure(SPBase objectToCast)
        {
            var result = await _context.Database.GetDbConnection().ExecuteScalarAsync<int>(objectToCast.getSPName(),
                    objectToCast,
                    commandType: System.Data.CommandType.StoredProcedure,
                    commandTimeout: 0);
            return result == 1;
        }

        public async Task<dynamic> ExecuteQueryMultipleStoredProcedure<TResult>(SPBase objectToCast)
        {
            var res = new List<List<TResult>>();
            using (var multi = await _context.Database.GetDbConnection().QueryMultipleAsync(objectToCast.getSPName(),
                   objectToCast,
                   commandType: System.Data.CommandType.StoredProcedure,
                   commandTimeout: 0))
            {
                while (!multi.IsConsumed)
                {
                    res.Add((await multi.ReadAsync<TResult>()).AsList<TResult>());
                }
            }

            return res;
        }

        public async Task<Tuple<List<TResult1>, List<List<TResult2>>>> ExecuteQueryMultipleTypesStoredProcedure<TResult1, TResult2>(SPBase objectToCast)
        {
            var res2 = new List<List<TResult2>>();
            List<TResult1> res1;
            using (var multi = await _context.Database.GetDbConnection().QueryMultipleAsync(objectToCast.getSPName(),
                   objectToCast,
                   commandType: System.Data.CommandType.StoredProcedure,
                   commandTimeout: 0))
            {
                res1 = (await multi.ReadAsync<TResult1>()).AsList<TResult1>();
                while (!multi.IsConsumed)
                {
                    res2.Add((await multi.ReadAsync<TResult2>()).AsList<TResult2>());
                }
            }

            return (res1, res2).ToTuple();
        }
    }
}
