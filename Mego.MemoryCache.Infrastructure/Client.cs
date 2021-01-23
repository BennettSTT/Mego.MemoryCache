using Dapper;
using Mego.MemoryCache.Infrastructure.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Timers;

namespace Mego.MemoryCache.Infrastructure
{
    public class Client
    {
        private readonly string _name;
        private static Timer _timer;
        
        private static BigMemoryCache _bigMemoryCache;

        public Client(string name)
        {
            _name = name;
        }

        public void Run()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += async (_, __) => await HandleTimerAsync();
            _timer.Start();

            _bigMemoryCache = new BigMemoryCache();
            _bigMemoryCache.Refresh();

            while (true)
            {
                var command = Console.ReadLine();

                if (command == Constants.Commands.Refresh)
                {
                    _bigMemoryCache.Refresh();
                    _bigMemoryCache.Count();

                    continue;
                }

                Console.WriteLine("Command not supported.");
            }
        }

        private async Task HandleTimerAsync()
        {
            await using var connection = new SqlConnection("Server=localhost;Database=Mego;Trusted_Connection=True;");
            await connection.OpenAsync();

            const string querySql = "SELECT TOP(1) * FROM [dbo].[ClientCommand] WHERE [ClientName] = @ClientName AND [Completed] = 0";
            var clientCommand = await connection.QueryFirstOrDefaultAsync<ClientCommand>(querySql, new { ClientName = _name });

            if (clientCommand?.Command == Constants.Commands.Refresh)
            {
                _bigMemoryCache.Refresh();

                const string commandSql = "UPDATE [dbo].[ClientCommand] SET [Completed] = @Completed WHERE [Id] = @Id";

                await connection.ExecuteAsync(commandSql, new { clientCommand.Id, Completed = 1 });
            }
        }
    }
}