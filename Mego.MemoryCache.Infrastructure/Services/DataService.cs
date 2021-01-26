using Dapper;
using Mego.MemoryCache.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Infrastructure.Services
{
    public class DataService : ServiceBase
    {
        public Task<Data> GetByKeyAsync(string key)
        {
            const string querySql = "SELECT * FROM [dbo].[Data] WHERE [Key] = @Key";
            return Connection.QueryFirstOrDefaultAsync<Data>(querySql, new { Key = key });
        }

        public Task<IEnumerable<Data>> GetByRangeAsync(long offset, long batchSize)
        {
            const string querySql = "SELECT * FROM [dbo].[Data] ORDER BY [Key] OFFSET (@offSet) ROWS FETCH NEXT (@batchSize) ROWS ONLY";
            return Connection.QueryAsync<Data>(querySql, new { offset, batchSize });
        }
    }
}