using Dapper;
using Mego.MemoryCache.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Infrastructure.Services
{
    public class DataService : ServiceBase
    {
        public Task<long> GetAllCountAsync()
        {
            return Connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM [dbo].[Data]");
        }

        public Task<IEnumerable<Data>> GetByRangeAsync(long offset, long batchSize)
        {
            const string querySql = "SELECT * FROM [dbo].[Data] ORDER BY [Key] OFFSET (@offSet) ROWS FETCH NEXT (@batchSize) ROWS ONLY";
            return Connection.QueryAsync<Data>(querySql, new { offset, batchSize });
        }
    }
}