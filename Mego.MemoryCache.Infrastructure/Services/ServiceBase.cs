using System;
using System.Data.SqlClient;

namespace Mego.MemoryCache.Infrastructure.Services
{
    public abstract class ServiceBase : IDisposable
    {
        protected SqlConnection Connection;

        protected ServiceBase()
        {
            Connection = new SqlConnection("Server=localhost;Database=Mego;Trusted_Connection=True;");
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection == null) return;

            Connection.Dispose();
            Connection = null;
        }
    }
}