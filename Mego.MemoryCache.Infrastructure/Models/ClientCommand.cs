namespace Mego.MemoryCache.Infrastructure.Models
{
    public class ClientCommand
    {
        public long Id { get; set; }

        public string ClientName { get; set; }

        public string Command { get; set; }

        public bool Completed { get; set; }
    }
}