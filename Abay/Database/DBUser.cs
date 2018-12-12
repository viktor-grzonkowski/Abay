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
        public User Login(string userName, string password)
        {
            User user = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                            "FROM [User] " +
                                            "WHERE username = @username AND password = @password";
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", password);

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
                                    Password = (string)reader["password"],
                                    Admin = (bool)reader["admin"]
                                };
                            }
                        }
                        return user;
                    }
                }
                catch (SqlException e)
                {
                    Debug.Write("\n #### ERROR IN Login START #### \n");
                    Debug.Write("\n" + e + "\n");
                    Debug.Write("\n #### ERROR FOR Login END #### \n");

                    return user;
                }
            }
        }
        #endregion

        #region GetSalt()
        public string GetSalt(string userName)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT salt " +
                                            "FROM [User] " +
                                            "WHERE username = @username ";
                        cmd.Parameters.AddWithValue("@username", userName);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                return (string)reader["salt"];
                            }
                        }
                        return "";
                    }
                }
                catch (SqlException e)
                {
                    Debug.Write("\n #### ERROR IN Login START #### \n");
                    Debug.Write("\n" + e + "\n");
                    Debug.Write("\n #### ERROR FOR Login END #### \n");

                    return "";
                }
            }
        }
        #endregion

        #region InsertUser()
        public bool InsertUser(User user)
        {
            string hashedPassword = Security.CreateHashedPassword(user.Password, out string salt);

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO [User] (username, firstName, lastName, password, email, admin, salt) " +
                                            "VALUES " +
                                            "(@username, @firstName, @lastName, @password, @email, @admin, @salt)";
                        cmd.Parameters.AddWithValue("@username", user.UserName);
                        cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", user.LastName);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@email", user.Email);
                        cmd.Parameters.AddWithValue("@admin", user.Admin);
                        cmd.Parameters.AddWithValue("@salt", salt);
                        cmd.ExecuteScalar();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Write("\n #### ERROR IN InsertUser START #### \n");
                Debug.Write("\n" + e + "\n");
                Debug.Write("\n #### ERROR FOR InsertUser END #### \n");
                return false;
            }
        }
        #endregion

        #region CheckUserName()
        public bool CheckUserName(string userName)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT username " +
                                            "FROM [User] " +
                                            "WHERE username = @username";
                        cmd.Parameters.AddWithValue("@username", userName);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.Write("\n #### ERROR IN CheckUserName START #### \n");
                Debug.Write("\n" + e + "\n");
                Debug.Write("\n #### ERROR FOR CheckUserName END #### \n");
                return false;
            }
        }
        #endregion

        #region GetUserInformation()
        public User GetUserInformation(string userName)
        {
            User user = null;

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                            "FROM [User] " +
                                            "WHERE username = @username";
                        cmd.Parameters.AddWithValue("@username", userName);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user = new User {
                                    UserName = (string)reader["username"],
                                    FirstName = (string)reader["firstName"],
                                    LastName = (string)reader["lastName"],
                                    Email = (string)reader["email"]
                                };
                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception e)
            {
                Debug.Write("\n #### ERROR IN GetUserInformation START #### \n");
                Debug.Write("\n" + e + "\n");
                Debug.Write("\n #### ERROR FOR GetUserInformation END #### \n");
                return user;
            }
        }
        #endregion
    }
}
