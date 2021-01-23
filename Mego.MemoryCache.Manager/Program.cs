using Dapper;
using Mego.MemoryCache.Infrastructure;
using Mego.MemoryCache.Infrastructure.Configs;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Manager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cacheConfig = new CacheConfig
            {
                ClientConfigs = new[]
                {
                    new ClientConfig { Name = "Client1" }, 
                    new ClientConfig { Name = "Client2" }
                }
            };

            while (true)
            {
                var command = Console.ReadLine();

                if (command == Constants.Commands.Refresh)
                {
                    await using var connection = new SqlConnection("Server=localhost;Database=Mego;Trusted_Connection=True;");
                    await connection.OpenAsync();

                    foreach (var clientConfig in cacheConfig.ClientConfigs)
                    {
                        const string commandSql = @"INSERT INTO [dbo].[ClientCommand]([ClientName],[Command],[Completed]) 
                                                    VALUES (@ClientName, @Command, @Completed)";

                        var parameters = new
                        {
                            ClientName = clientConfig.Name,
                            Command = Constants.Commands.Refresh,
                            Completed = 0
                        };
                        await connection.ExecuteAsync(commandSql, parameters);
                    }

                    continue;
                }

                Console.WriteLine("Command not supported.");
            }
        }
    }
}