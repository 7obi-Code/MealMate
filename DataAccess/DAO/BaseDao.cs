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
            var builder = new NpgsqlConnectionStringBuilder(ConnectionString)
            {
                Timeout = 1,
                CommandTimeout = 1
            };

            return new NpgsqlConnection(builder.ConnectionString);
        }
    }
}