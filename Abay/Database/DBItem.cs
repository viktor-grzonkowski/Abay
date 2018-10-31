using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Database
{
    public class DBItem
    {
        #region GetAllItems(int page, int quantity)
        public List<Item> GetAllItems(int page, int quantity)
        {
            List<Item> items = new List<Item>();
            Item item = null;
            int from;

            if (page == 0)
                from = 0;
            else
                from = page * quantity;
            int to = from + quantity;

            using (SqlConnection connection = DBConnection.GetConnection())
            {

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY id ASC) AS RowNumber, seller_username, id, name, description, initialPrice, startDate, state, category_id " +
                                        "FROM [Item] "+
                                        "WHERE id < @to And id > @from";
                    cmd.Parameters.AddWithValue("@to", to);
                    cmd.Parameters.AddWithValue("@from", from);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User seller = new User
                        {
                            UserName = reader.GetString(1)
                        };

                        item = new Item
                        {
                            Id = reader.GetInt32(2),
                            Name = reader.GetString(3),
                            InitialPrice = reader.GetDouble(5),
                            State = reader.GetInt32(7),
                            SellerUsername = seller,
                            CategoryId = DBCategory.GetItemCategory(reader.GetInt32(8))
                        };

                        items.Add(item);
                    }
                    return items;
                }
            }
        }
        #endregion

        #region GetItemById(int id)
        public Item GetItemById(int id)
        {
            Item item = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT seller_username, id, name, description, initialPrice, startDate, state, category_id " +
                                        "FROM [Item] " +
                                        "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            User seller = new User
                            {
                                UserName = result.GetString(0)
                            };

                            item = new Item
                            {
                                Id = result.GetInt32(1),
                                Name = result.GetString(2),
                                Description = result.GetString(3),
                                InitialPrice = result.GetDouble(4),
                                StartDate = result.GetDateTime(5),
                                State = result.GetInt32(6),
                                SellerUsername = seller,
                                CategoryId = DBCategory.GetItemCategory(result.GetInt32(7))
                            };
                        }
                        return item;
                    }
                }
            }
        }
        #endregion

        #region SearchItems(string value, int categoryId)
        public List<Item> SearchItems(string value, int categoryId)
        {
            List<Item> items = new List<Item>();
            Item item = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name, description, initialPrice, finalPrice, startDate, endDate,state, seller_username, buyer_username,category_id " +
                                        "FROM [Item] ";
                    if(categoryId != -1)
                        cmd.CommandText += "WHERE(id = @id OR name like @value OR description LIKE @value OR seller_username LIKE @value) AND category_id = @id";
                    else
                        cmd.CommandText += "WHERE(id = @id OR name like @value OR description LIKE @value OR seller_username LIKE @value)";
                    cmd.Parameters.AddWithValue("@id", categoryId);
                    cmd.Parameters.AddWithValue("@value", value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User seller = new User
                            {
                                UserName = reader.GetString(8)
                            };

                            /*
                            User buyer = new User
                            {
                                UserName = reader.GetString(9)
                            };
                            */

                            item = new Item
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                InitialPrice = reader.GetDouble(3),
                                FinalPrice = reader.GetDouble(4),
                                //StartDate = reader.GetDateTime(5),
                                //EndDate = reader.GetDateTime(6),
                                State = reader.GetInt32(7),
                                SellerUsername = seller,
                                //BuyerUsername = buyer,
                                CategoryId = DBCategory.GetItemCategory(reader.GetInt32(10))
                            };

                            items.Add(item);
                        }
                    }
                    return items;
                }
            }
        }
        #endregion



        //SELECT*
        //FROM[dbo].[Item]
        //WHERE(id = '1' OR name like '1' OR description LIKE '1' OR seller_username LIKE '1') AND category_id = '1'





        #region SelectItem(int id)
        #endregion

        #region InsertItem(Item item)
        public string InsertItem(Item item)
        {
            string message = "";

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = sqlTran;

                try
                {
                    // Execute one commands.
                    cmd.CommandText = "INSERT INTO [Item] " +
                                       "(name ,initialPrice ,state ,seller_username ,category_name) " +
                                       "VALUES " +
                                       "(@name ,@initialPrice ,@state ,@seller_username ,@category_name)";
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@initialPrice", item.InitialPrice);
                    cmd.Parameters.AddWithValue("@state", item.State);
                    cmd.Parameters.AddWithValue("@seller_username", item.SellerUsername);
                    cmd.Parameters.AddWithValue("@category_id", item.CategoryId);
                    cmd.ExecuteNonQuery();

                    // Commit the transaction.
                    sqlTran.Commit();
                    message = "Item inserted.";
                    return message;
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    message = ex.Message;

                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                        return message;
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection 
                        // is closed or the transaction has already been rolled 
                        // back on the server.
                        message += "/n" + exRollback.Message;
                        return message;
                    }
                }
            }
        }
        #endregion

        #region UpdateItem(Item item)
        public void UpdateItem(Item item)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =   "UPDATE [Item] " +
                                        "SET name = @name, description = @description " +
                                        "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", item.Id);
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region DeleteItem(int id)
        public string DeleteItem(int id)
        {
            string message = "";

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = sqlTran;

                try
                {
                    // Execute one commands.
                    cmd.CommandText = "DELETE FROM [Item] " +
                                       "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    // Commit the transaction.
                    sqlTran.Commit();
                    message = "Item deleted.";
                    return message;
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    message = ex.Message;

                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                        return message;
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection 
                        // is closed or the transaction has already been rolled 
                        // back on the server.
                        message += "/n" + exRollback.Message;
                        return message;
                    }
                }
            }
        }
        #endregion
    }
}