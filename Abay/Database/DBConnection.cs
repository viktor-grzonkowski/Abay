using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Database
{
    public static class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            string strCon = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            SqlConnectionStringBuilder strBuilder = new SqlConnectionStringBuilder(strCon);

            SqlConnection connection = new SqlConnection(strBuilder.ToString());
            connection.Open();
            return connection;
        }
    }
}