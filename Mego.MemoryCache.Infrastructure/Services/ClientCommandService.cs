using Dapper;
using Mego.MemoryCache.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Infrastructure.Services
{
    public class ClientCommandService : ServiceBase
    {
        public Task<ClientCommand> GetClientCommandAsync(string clientName)
        {
            const string querySql = "SELECT TOP(1) * FROM [dbo].[ClientCommand] WHERE [ClientName] = @clientName AND [Completed] = 0 ORDER BY [CreationDate]";

            return Connection.QueryFirstOrDefaultAsync<ClientCommand>(querySql, new { clientName });
        }

        public Task CompleteClientCommandAsync(ClientCommand clientCommand)
        {
            const string updateSql = "UPDATE [dbo].[ClientCommand] SET [Completed] = @completed WHERE [Id] = @Id";

            return Connection.ExecuteAsync(updateSql, new { clientCommand.Id, completed = 1 });
        }

        public async Task InsertCommandAsync(string name, string command)
        {
            const string insertCommandSql = "INSERT INTO [dbo].[ClientCommand]([ClientName], [Command], [Completed], [CreationDate]) VALUES (@ClientName, @Command, @Completed, @CreationDate)";

            var parameters = new
            {
                ClientName = name,
                Command = command,
                Completed = 0,
                CreationDate = DateTime.UtcNow
            };

            await Connection.ExecuteAsync(insertCommandSql, parameters);
        }
    }
}