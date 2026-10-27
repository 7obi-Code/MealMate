using Npgsql;

namespace DataAccess.DAO
{
    public abstract class BaseDao
    {
        public string ConnectionString { get; set; }

        public BaseDao(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}
