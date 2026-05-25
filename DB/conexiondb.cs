using MySql.Data.MySqlClient;

namespace appInventario.DB
{
    public class conexiondb
    {
        private string connectionString;

        public conexiondb(string server, string database, string username, string password)
        {
            connectionString = $"Server={server};Database={database};User ID={username};Password={password};";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {

        }

        public void CloseConnection()
        {

        }


    }
}
