using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    abstract public class BaseDao
    {
        public string ConnectionString { get; set; }


        public BaseDao(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
