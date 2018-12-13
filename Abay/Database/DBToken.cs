using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DBToken
    {
        #region GetToken()
        public Token GetToken(string secureToken)
        {
            Token token = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * " +
                                      "FROM [Token] " +
                                      "WHERE token = @token";
                    cmd.Parameters.AddWithValue("@token", secureToken);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            token = new Token
                            {
                                SecureToken = (string)reader["token"],
                                UserName = (string)reader["username"],
                                CreateDate = (DateTime)reader["createDate"]
                            };
                        }
                    }
                    return token;
                }
            }
        }
        #endregion

        #region InsertToken()
        public bool InsertToken(Token token)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO [Token] " +
                                          "(token ,username ,createDate) " +
                                          "VALUES " +
                                          "(@token ,@username ,@createDate)";
                        cmd.Parameters.AddWithValue("@token", token.SecureToken);
                        cmd.Parameters.AddWithValue("@username", token.UserName);
                        cmd.Parameters.AddWithValue("@createDate", token.CreateDate);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Write("\n #### ERROR IN InsertToken START #### \n");
                    Debug.Write("\n" + e + "\n");
                    Debug.Write("\n #### ERROR FOR InsertToken END #### \n");

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
                    cmd.CommandText = "SELECT * " +
                                      "FROM [User] " +
                                      "WHERE username IN " +
                                      "(SELECT username " +
                                      "FROM Token " +
                                      "WHERE token = @token)";
                    cmd.Parameters.AddWithValue("@token", token);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            user = new User
                            {
                                UserName = (string)reader["username"],
                                FirstName = (string)reader["firstName"],
                                LastName = (string)reader["lastName"],
                                Email = (string)reader["email"],
                                Admin = (bool)reader["admin"]
                            };
                        }
                    }
                    return user;
                }
            }
        }
        #endregion

        #region DeleteToken()
        public bool DeleteToken(string userName)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "DELETE token " +
                                          "FROM [Token] " +
                                          "WHERE username = @username ";
                        cmd.Parameters.AddWithValue("@username", userName);

                        cmd.ExecuteReader();
                        
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Debug.Write("\n #### ERROR IN DeleteToken START #### \n");
                    Debug.Write("\n" + e + "\n");
                    Debug.Write("\n #### ERROR FOR DeleteToken END #### \n");

                    return false;
                }
            }
        }
        #endregion

        #region UserExisting()
        public bool UserExisting(string userName)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                          "FROM [Token] " +
                                          "WHERE username = @username ";
                        cmd.Parameters.AddWithValue("@username", userName);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                            return true;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Write("\n #### ERROR IN UserExisting START #### \n");
                    Debug.Write("\n" + e + "\n");
                    Debug.Write("\n #### ERROR FOR UserExisting END #### \n");

                    return false;
                }
            }
        }
        #endregion
    }
}
