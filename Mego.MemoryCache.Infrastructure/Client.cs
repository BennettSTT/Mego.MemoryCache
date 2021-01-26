using Mego.MemoryCache.Infrastructure.Models;
using Mego.MemoryCache.Infrastructure.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Mego.MemoryCache.Infrastructure
{
    public class Client
    {
        private readonly string _name;
        private static Timer _timer;

        // Интервал и размер батча можно настраивать
        // Например, можно брать значения из конфига
        private const int CacheRefreshCheckInterval = 1000;
        private const int BatchSize = 100;

        private static readonly ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();

        public Client(string name)
        {
            _name = name;
        }

        public async Task RunAsync()
        {
            Init();

            await RefreshCacheAndPrintInfoAsync();

            while (true)
            {
                var command = Console.ReadLine();

                if (command == Constants.Commands.Refresh)
                {
                    await RefreshCacheAndPrintInfoAsync();

                    continue;
                }

                Console.WriteLine("Command not supported.");
            }
        }

        private void Init()
        {
            _timer = new Timer(CacheRefreshCheckInterval);
            _timer.Elapsed += async (_, __) => await TryRefreshMemoryCache();
            _timer.Start();
        }

        /// <summary>
        /// Метод был добавлен для демонстрации чтения данных бизнес логикой, явно нигде не вызывается
        /// </summary>
        public async Task<string> GetValueByKey(string key)
        {
            if (Cache.TryGetValue(key, out var value))
            {
                return value;
            }

            using var dataService = new DataService();
            var data = await dataService.GetByKeyAsync(key);

            return data != null ? Cache.GetOrAdd(key, data.Value) : null;
        }

        private async Task TryRefreshMemoryCache()
        {
            using var clientCommandService = new ClientCommandService();

            var clientCommand = await clientCommandService.GetClientCommandAsync(_name);
            if (clientCommand == null)
            {
                return;
            }

            if (clientCommand.Command == Constants.Commands.Refresh)
            {
                await RefreshCacheAndPrintInfoAsync();
            }

            await clientCommandService.CompleteClientCommandAsync(clientCommand);
        }

        private static async Task RefreshCacheAsync()
        {
            Cache.Clear();

            var offset = 0;
            IEnumerable<Data> currentDataElements;

            using var dataService = new DataService();

            while ((currentDataElements = await dataService.GetByRangeAsync(offset, BatchSize)).Any())
            {
                foreach (var data in currentDataElements)
                {
                    Cache.TryAdd(data.Key, data.Value);
                }

                offset += BatchSize;
            }
        }

        private static async Task RefreshCacheAndPrintInfoAsync()
        {
            await RefreshCacheAsync();

            Console.WriteLine($"The count of items in the cache: {Cache.Count}");
        }
    }
}