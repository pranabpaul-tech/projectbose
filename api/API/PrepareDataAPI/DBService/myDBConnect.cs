using MySqlConnector;
namespace PrepareDataAPI.DBService
{
    public class myDBConnect : IDisposable
    {
        public MySqlConnection Connection { get; }

        public myDBConnect(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
