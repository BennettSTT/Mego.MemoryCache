using Mego.MemoryCache.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Mego.MemoryCache.Infrastructure
{
    public class Client
    {
        private readonly string _name;
        private readonly int _currentClientCount;
        private readonly int _allClientCount;
        private static Timer _timer;

        private const int Interval = 1000; 
        private static Dictionary<string, string> _cache;

        public Client(string name, int currentClientCount, int allClientCount)
        {
            _name = name;
            _currentClientCount = currentClientCount;
            _allClientCount = allClientCount;
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
            _timer = new Timer(Interval);
            _timer.Elapsed += async (_, __) => await TryRefreshMemoryCache();
            _timer.Start();
        }

        private async Task TryRefreshMemoryCache()
        {
            using var clientCommandService = new ClientCommandService();
            var clientCommand = await clientCommandService.GetClientCommandAsync(_name);

            if (clientCommand?.Command == Constants.Commands.Refresh)
            {
                await RefreshCacheAndPrintInfoAsync();

                await clientCommandService.CompleteClientCommandAsync(clientCommand);
            }
        }

        private async Task RefreshCacheAsync()
        {
            using var dataService = new DataService();
            var count = await dataService.GetAllCountAsync();

            var (batchSize, offset) = GetRange(count);
            var cacheElements = await dataService.GetByRangeAsync(offset, batchSize);

            _cache = cacheElements.ToDictionary(element => element.Key, element => element.Value);
        }

        private async Task RefreshCacheAndPrintInfoAsync()
        {
            await RefreshCacheAsync();
            var count = _cache.Count;

            Console.WriteLine($"The count of items in the cache: {count}");
        }

        private (long batchSize, long offset) GetRange(long countElements)
        {
            var batchSize = (long) Math.Ceiling(countElements / (double) _allClientCount);
            var offset = _currentClientCount == 1 ? 0 : (_currentClientCount - 1) * batchSize;

            return (batchSize, offset);
        }
    }
}