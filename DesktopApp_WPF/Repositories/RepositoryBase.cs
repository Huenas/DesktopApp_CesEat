using DotNetEnv;
using System.Data.SqlClient;

namespace DesktopApp_WPF.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {

            Env.TraversePath().Load();

            var dbServer = Env.GetString("DB_SERVER");

            Env.Load("DB_SERVER");
            string dbName = Env.GetString("DB_NAME");
            string dbUser = Env.GetString("DB_USER");
            string dbPassword = Env.GetString("DB_PASSWORD");

            _connectionString = $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};";

        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

