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
        #region GetAllItems(int CatId)
        public List<Item> GetAllItems(int CatId)
        {
            List<Item> items = new List<Item>();
            Item item = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * " +
                                      "FROM [Item] ";
                    if (CatId != -1)
                        cmd.CommandText += "WHERE category_id = @catId AND state = 0";

                    cmd.Parameters.AddWithValue("@catId", CatId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        User seller = new User
                        {
                            UserName = reader["seller_username"].ToString()
                        };

                        User buyer = new User
                        {
                            UserName = CheckValue(reader["buyer_username"]).ToString()
                        };

                        item = new Item
                        {
                            Id = int.Parse(CheckValue(reader["id"]).ToString()),
                            Name = CheckValue(reader["name"]).ToString(),
                            Description = CheckValue(reader["description"]).ToString(),
                            InitialPrice = (double)reader["initialPrice"],
                            FinalPrice = reader["finalPrice"] == DBNull.Value ? 0 : (double)reader["finalPrice"],
                            StartDate = (DateTime)reader["startDate"],
                            EndDate = (DateTime)reader["endDate"],
                            State = (int)reader["state"],
                            SellerUser = seller,
                            BuyerUser = buyer,
                            Category = DBCategory.GetItemCategory((int)reader["category_id"])
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
                        cmd.CommandText = "SELECT * " +
                                            "FROM [Item] " +
                                            "WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User seller = new User
                                {
                                    UserName = reader["seller_username"].ToString()
                                };

                                User buyer = new User
                                {
                                    UserName = CheckValue(reader["buyer_username"]).ToString()
                                };

                                item = new Item
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Description = CheckValue(reader["description"]).ToString(),
                                    InitialPrice = (double)reader["initialPrice"],
                                    FinalPrice = reader["finalPrice"] == DBNull.Value ? 0 : (double)reader["finalPrice"],
                                    StartDate = (DateTime)reader["startDate"],
                                    EndDate = (DateTime)reader["endDate"],
                                    State = (int)reader["state"],
                                    SellerUser = seller,
                                    BuyerUser = buyer,
                                    Category = DBCategory.GetItemCategory((int)reader["category_id"])
                                };
                            }
                            
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Write("\n #### ERROR IN GetItemById START #### \n");
                Debug.Write("\n" + e + "\n");
                Debug.Write("\n #### ERROR FOR GetItemById END #### \n");
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

                                buyer.UserName = CheckValue(reader["buyer_username"]).ToString();
                            
                                item.Id = int.Parse(CheckValue(reader["id"]).ToString());
                                item.Name = CheckValue(reader["name"]).ToString();
                                item.Description = CheckValue(reader["description"]).ToString();
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
                Debug.Write("\n #### ERROR IN SearchItems START #### \n");
                Debug.Write("\n" + e + "\n");
                Debug.Write("\n #### ERROR FOR SearchItems END #### \n");
            }

            return items;
        }
        #endregion

        #region InsertItem(Item item)
        public int InsertItem(Item item)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                int itemId = -1;
                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = sqlTran;

                try
                {
                    // Execute one command.
                    cmd.CommandText = "INSERT INTO [Item] " +
                                       "(name, description, initialPrice, startDate, endDate, state, seller_username, category_id) " +
                                       "OUTPUT INSERTED.ID " +
                                       "VALUES (@name, @description, @initialPrice, @startDate, @endDate, @state, @seller_username, @category_id)";
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@initialPrice", item.InitialPrice);
                    cmd.Parameters.AddWithValue("@startDate", item.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", item.EndDate);
                    cmd.Parameters.AddWithValue("@state", item.State);
                    cmd.Parameters.AddWithValue("@seller_username", item.SellerUser.UserName);
                    cmd.Parameters.AddWithValue("@category_id", item.Category.Id);

                    itemId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Commit the transaction.
                    sqlTran.Commit();
                    return itemId;
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Debug.Write(ex);

                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                        return itemId;
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection 
                        // is closed or the transaction has already been rolled 
                        // back on the server.
                        Debug.Write(exRollback);
                        return itemId;
                    }
                }
            }
        }
        #endregion

        #region UpdateItem(Item item)
        public bool UpdateItem(Item item)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE [Item] " +
                                            "SET name = @name, description = @description, finalPrice = @finalPrice, state = @state, buyer_username = @buyer " +
                                            "WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", item.Id);
                        cmd.Parameters.AddWithValue("@name", item.Name);
                        cmd.Parameters.AddWithValue("@description", item.Description);
                        cmd.Parameters.AddWithValue("@finalPrice", item.FinalPrice);
                        cmd.Parameters.AddWithValue("@state", item.State);
                        cmd.Parameters.AddWithValue("@buyer", item.BuyerUser.UserName);
                        cmd.ExecuteScalar();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Debug.Write("\n #### ERROR IN UpdateItem START #### \n");
                    Debug.Write("\n"+ e + "\n");
                    Debug.Write("\n #### ERROR FOR UpdateItem END #### \n");
                }
                return false;
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

        public object CheckValue(object value)
        {
            return value == DBNull.Value ? "" : value;
        }
    }
}