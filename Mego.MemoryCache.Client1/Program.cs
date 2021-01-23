using Mego.MemoryCache.Infrastructure;

namespace Mego.MemoryCache.Client1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("Client1");
            client.Run();
        }
    }
}