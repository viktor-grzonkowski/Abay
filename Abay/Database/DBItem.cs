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
    public class DBItem
    {
        #region GetAllItems(int page, int quantity)
        public List<Item> GetAllItems(int page, int quantity)
        {
            List<Item> items = new List<Item>();
            Item item = null;
            int from;

            if (page == 1)
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
                            Id = int.Parse(checkValue(reader["id"]).ToString()),
                            Name = checkValue(reader["name"]).ToString(),
                            InitialPrice = (double)reader["initialPrice"],
                            State = (int)reader["state"],
                            SellerUser = seller,
                            Category = DBCategory.GetItemCategory(reader.GetInt32(8))
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

            try { 
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
                                    Description = checkValue(result["description"]).ToString(),
                                    InitialPrice = result.GetDouble(4),
                                    StartDate = (DateTime)result["startDate"],
                                    State = result.GetInt32(6),
                                    SellerUser = seller,
                                    Category = DBCategory.GetItemCategory((int)result["category_id"])
                                };
                            }
                            
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Write("#### ERROR MESSAGE IN GetItemById START #### \n");
                Debug.Write(e+"\n");
                Debug.Write("#### ERROR MESSAGE FOR GetItemById END ####");
            }

            return item;
        }
        #endregion

        #region SearchItems(string value, int categoryId)
        public List<Item> SearchItems(string value, int categoryId)
        {
            List<Item> items = new List<Item>();
            User seller = new User();
            User buyer = new User();
            Item item = new Item();

            try
            {
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

                            
                                seller.UserName = (string)reader["seller_username"];

                                buyer.UserName = checkValue(reader["buyer_username"]).ToString();
                            
                                item.Id = int.Parse(checkValue(reader["id"]).ToString());
                                item.Name = checkValue(reader["name"]).ToString();
                                item.Description = checkValue(reader["description"]).ToString();
                                item.InitialPrice = (double)reader["initialPrice"];
                                item.FinalPrice = reader["finalPrice"] == System.DBNull.Value ? 0 : (double)reader["finalPrice"];
                                item.StartDate = (DateTime)reader["startDate"];
                                item.EndDate = (DateTime)reader["endDate"];
                                item.State = (int)reader["state"];
                                item.SellerUser = seller;
                                item.BuyerUser = buyer;
                                item.Category = DBCategory.GetItemCategory((int)reader["category_id"]);
                            
                                items.Add(item);
                            }
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write("#### ERROR MESSAGE IN SearchItems START #### \n");
                Debug.Write(e);
                Debug.Write("#### ERROR MESSAGE FOR SearchItems END #### \n" + e);
            }

            return items;
        }
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
                                       "(name ,initialPrice ,state ,seller_username ,category_id) " +
                                       "VALUES " +
                                       "(@name ,@initialPrice ,@state ,@seller_username ,@category_id)";
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@initialPrice", item.InitialPrice);
                    cmd.Parameters.AddWithValue("@state", item.State);
                    cmd.Parameters.AddWithValue("@seller_username", item.SellerUser.UserName);
                    cmd.Parameters.AddWithValue("@category_id", item.Category.Id);
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
                    Debug.Write(ex);

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

        public object checkValue(object value)
        {
            return value == DBNull.Value ? "" : value;
        }
    }
}