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
                                      "FROM Item i " +
                                      "LEFT JOIN Bid b ON i.bid_id = b.id ";
                    if (CatId != -1)
                        cmd.CommandText += "WHERE i.category_id = @catId AND i.state = 0";

                    cmd.Parameters.AddWithValue("@catId", CatId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User seller = new User
                            {
                                UserName = reader["seller_username"].ToString()
                            };

                            item = new Item
                            {
                                Id = int.Parse(CheckValue(reader["id"]).ToString()),
                                Name = CheckValue(reader["name"]).ToString(),
                                Description = CheckValue(reader["description"]).ToString(),
                                InitialPrice = (double)reader["initialPrice"],
                                StartDate = (DateTime)reader["startDate"],
                                EndDate = (DateTime)reader["endDate"],
                                State = (int)reader["state"],
                                SellerUser = seller,
                                Category = DBCategory.GetItemCategory((int)reader["category_id"])
                                //Bid = (int)reader["bid_id"] >= 0 ? DBBid.GetBid((int)reader["bid_id"]) : null

                            };

                            item.Bid = reader["bid_id"] != DBNull.Value
                                ? new Bid
                                {
                                    Id = (int)reader["bid_id"],
                                    UserName = (string)reader["username"],
                                    Amount = double.Parse(reader["amount"].ToString()),
                                    Timestamp = (DateTime)reader["timestamp"]
                                }
                                : null;

                            items.Add(item);
                        }
                    }
                    return items;
                }
            }
        }
        #endregion

        #region GetItemById(int id)WORKS
        public Item GetItemById(int id)
        {
            Item item = null;

            try { 
                using (SqlConnection connection = DBConnection.GetConnection())
                {

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                          "FROM [Item] i " +
                                          "LEFT JOIN [Bid] b " +
                                          "ON i.bid_id = b.id " +
                                          "WHERE i.id = @id";
                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    User seller = new User
                                    {
                                        UserName = reader["seller_username"].ToString()
                                    };

                                    item = new Item
                                    {
                                        Id = (int)reader["id"],
                                        Name = reader["name"].ToString(),
                                        Description = CheckValue(reader["description"]).ToString(),
                                        InitialPrice = (double)reader["initialPrice"],
                                        StartDate = (DateTime)reader["startDate"],
                                        EndDate = (DateTime)reader["endDate"],
                                        State = (int)reader["state"],
                                        SellerUser = seller,
                                        Category = DBCategory.GetItemCategory((int)reader["category_id"])
                                    };

                                    item.Bid = reader["bid_id"] != DBNull.Value
                                        ? new Bid
                                        {
                                            Id = (int)reader["bid_id"],
                                            UserName = (string)reader["username"],
                                            Amount = double.Parse(reader["amount"].ToString()),
                                            Timestamp = (DateTime)reader["timestamp"]
                                        }
                                        : null;
                                }
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
            User seller = null ;
            Item item = null;

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                          "FROM Item i " +
                                          "LEFT JOIN Bid b ON i.bid_id = b.id ";
                        if(categoryId != -1)
                            cmd.CommandText += "WHERE(i.name LIKE @value OR i.description LIKE @value OR i.seller_username LIKE @value) AND i.category_id = @catId";
                        else
                            cmd.CommandText += "WHERE(i.name LIKE @value OR i.description LIKE @value OR i.seller_username LIKE @value)";
                        cmd.Parameters.AddWithValue("@catId", categoryId);
                        cmd.Parameters.AddWithValue("@value", "%"+value+"%");

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                seller = new User
                                {
                                    UserName = (string)reader["seller_username"]
                                };

                                item = new Item
                                {
                                    Id = int.Parse(CheckValue(reader["id"]).ToString()),
                                    Name = CheckValue(reader["name"]).ToString(),
                                    Description = CheckValue(reader["description"]).ToString(),
                                    InitialPrice = (double)reader["initialPrice"],
                                    StartDate = (DateTime)reader["startDate"],
                                    EndDate = (DateTime)reader["endDate"],
                                    State = (int)reader["state"],
                                    SellerUser = seller,
                                    Category = DBCategory.GetItemCategory((int)reader["category_id"])
                                };

                                

                                item.Bid = reader["bid_id"] != DBNull.Value
                                    ? new Bid
                                    {
                                        Id = (int)reader["bid_id"],
                                        UserName = (string)reader["username"],
                                        Amount = double.Parse(reader["amount"].ToString()),
                                        Timestamp = (DateTime)reader["timestamp"]
                                    }
                                    : null;

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
                                          "SET name = @name, description = @description, state = @state ";
                        if (item.Bid != null)
                            cmd.CommandText += ", bid_id = @bidId ";
                        cmd.CommandText += "WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", item.Id);
                        cmd.Parameters.AddWithValue("@name", item.Name);
                        cmd.Parameters.AddWithValue("@description", item.Description);
                        cmd.Parameters.AddWithValue("@state", item.State);
                        cmd.Parameters.AddWithValue("@bidId", item.Bid.Id);
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