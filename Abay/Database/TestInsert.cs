using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class TestInsert
    {
        public void Insert()
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "insert into [User] (username, firstName, lastName, password, email, admin) values (@username, @firstName, @lastName, @password, @email, @admin)";
                        cmd.Parameters.AddWithValue("@username", "KingXXL");
                        cmd.Parameters.AddWithValue("@firstName", "Viktor");
                        cmd.Parameters.AddWithValue("@lastName", "Grzonkowski");
                        cmd.Parameters.AddWithValue("@password", "test123");
                        cmd.Parameters.AddWithValue("@email", "V.Grzonk@gmail.com");
                        cmd.Parameters.AddWithValue("@admin", 1);
                        cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }
            
        }
    }
}
