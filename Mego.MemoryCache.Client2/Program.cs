using Mego.MemoryCache.Infrastructure;

namespace Mego.MemoryCache.Client2
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("Client2");
            client.Run();        
        }
    }
}