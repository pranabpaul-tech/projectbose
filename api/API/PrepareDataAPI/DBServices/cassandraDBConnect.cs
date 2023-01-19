using Cassandra;
using Cassandra.DataStax.Auth;
using System.Diagnostics.Metrics;

namespace PrepareDataAPI.DBService
{
    public class cassandraDBConnect : IDisposable
    {
        public Cluster Connection { get; }

        public cassandraDBConnect(string clusterInfo, string user, string password)
        {
            var builder = Cluster.Builder()
                .AddContactPoint(clusterInfo)
                .WithAuthProvider(new DsePlainTextAuthProvider(user, password))
                .Build();
            Connection = builder;
        }

        public void Dispose() => Connection.Dispose();
    }
}
