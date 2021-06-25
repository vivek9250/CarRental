using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class DBContext
    {
        protected string ConnectionString { get; set; }
        protected SqlConnection connection = null;
        public DBContext()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false)
            .Build();

            this.ConnectionString = configuration.GetSection("ConnectionStrings").GetSection("devConnection").Value;
        }
        protected SqlConnection GetConnection()
        {
            connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }
        protected void CloseConnection()
        {
            this.connection.Close();
        }
    }
}
