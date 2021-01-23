namespace Mego.MemoryCache.Infrastructure.Configs
{
    public class CacheConfig
    {
        public ClientConfig[] ClientConfigs { get; set; }
        
        public long BatchSize { get; set; }
    }
}