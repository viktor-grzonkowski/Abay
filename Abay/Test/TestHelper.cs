using Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    static class TestHelper
    {
        public static void DeleteTest(string tableName, string columnName)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE " +
                                      $"FROM {tableName} " +
                                      $"WHERE {columnName} LIKE 'test%';";

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
