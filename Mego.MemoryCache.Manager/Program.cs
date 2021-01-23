using Mego.MemoryCache.Infrastructure;
using Mego.MemoryCache.Infrastructure.Configs;
using Mego.MemoryCache.Infrastructure.Services;
using System;
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
                    using var clientCommandService = new ClientCommandService();
                    foreach (var clientConfig in cacheConfig.ClientConfigs)
                    {
                        await clientCommandService.InsertCommandAsync(clientConfig.Name, Constants.Commands.Refresh);
                    }

                    continue;
                }

                Console.WriteLine("Command not supported.");
            }
        }
    }
}