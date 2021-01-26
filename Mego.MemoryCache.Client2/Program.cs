using Mego.MemoryCache.Infrastructure;
using System.Threading.Tasks;

namespace Mego.MemoryCache.Client2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client("Client2");
            await client.RunAsync();
        }
    }
}