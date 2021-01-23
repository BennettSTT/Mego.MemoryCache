using Dapper;
using Mego.MemoryCache.Infrastructure.Models;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Infrastructure.Services
{
    public class ClientCommandService : ServiceBase
    {
        public Task<ClientCommand> GetClientCommandAsync(string clientName)
        {
            const string querySql = "SELECT TOP(1) * FROM [dbo].[ClientCommand] WHERE [ClientName] = @clientName AND [Completed] = 0";

            return Connection.QueryFirstOrDefaultAsync<ClientCommand>(querySql, new { clientName });
        }

        public Task CompleteClientCommandAsync(ClientCommand clientCommand)
        {
            const string commandSql = "UPDATE [dbo].[ClientCommand] SET [Completed] = @completed WHERE [Id] = @Id";

            return Connection.ExecuteAsync(commandSql, new { clientCommand.Id, completed = 1 });
        }

        public async Task InsertCommandAsync(string name, string command)
        {
            const string insertCommandSql = "INSERT INTO [dbo].[ClientCommand]([ClientName],[Command],[Completed]) VALUES (@ClientName, @Command, @Completed)";

            var parameters = new
            {
                ClientName = name,
                Command = command,
                Completed = 0
            };

            await Connection.ExecuteAsync(insertCommandSql, parameters);
        }
    }
}