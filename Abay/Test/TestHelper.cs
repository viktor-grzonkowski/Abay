using Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class TestHelper
    {
        public static void DeleteRows(string tableName, string columnName)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("DELETE " +
                                      "FROM [{0}] " +
                                      "WHERE {1} LIKE 'test%';", tableName, columnName);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
