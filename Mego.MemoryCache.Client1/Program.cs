using Mego.MemoryCache.Infrastructure;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Client1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client("Client1", 1, 2);
            await client.RunAsync();
        }
    }
}