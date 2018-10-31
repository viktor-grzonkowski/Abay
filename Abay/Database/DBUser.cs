using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Database
{
    public class DBUser
    {
        #region Login()
        public bool Login(string userName, string password)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT *" +
                                            "FROM [User]" +
                                            "WHERE username = @username AND password = @password";
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.ExecuteScalar();
                    }
                    return true;
                }
                catch (SqlException e) {
                    return false;
                }
            }
        }
        #endregion

        #region GetUserByToken()
        public User GetUserByToken(string token)
        {
            User user = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT username, firstName, lastName, email " +
                                        "FROM [User] " +
                                        "WHERE username = @token";
                    cmd.Parameters.AddWithValue("@token", token);

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            user = new User
                            {
                                UserName = reader.GetString(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3)
                            };
                        }
                    }
                    return user;
                }
            }
        }
        #endregion
    }
}
