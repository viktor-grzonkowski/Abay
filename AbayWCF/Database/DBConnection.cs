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
            //string strCon = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            
            /* Raw connection string for testing */
            string strCon = "data source= kraka.ucn.dk; database= dmaj0917_1067689; user id= dmaj0917_1067689; password= Password1!; MultipleActiveResultSets=True;";

            SqlConnectionStringBuilder strBuilder = new SqlConnectionStringBuilder(strCon);

            SqlConnection connection = new SqlConnection(strBuilder.ToString());
            connection.Open();
            return connection;
        }
    }
}