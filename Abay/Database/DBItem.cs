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
        #region InsertItem(Item item)
        public int InsertItem(Item item)
        {
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                //row id where the item was inserted
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
                                       "(name, description, initialPrice, startDate, endDate, state, sellerUsername, categoryId) " +
                                       "OUTPUT INSERTED.ID " +
                                       "VALUES (@name, @description, @initialPrice, @startDate, @endDate, @state, @sellerUsername, @categoryId)";
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@initialPrice", item.InitialPrice);
                    cmd.Parameters.AddWithValue("@startDate", item.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", item.EndDate);
                    cmd.Parameters.AddWithValue("@state", item.State);
                    cmd.Parameters.AddWithValue("@sellerUsername", item.SellerUser.UserName);
                    cmd.Parameters.AddWithValue("@categoryId", item.Category.Id);

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

        #region GetAllActiveItemsByCategory(int CatId)
        public List<Item> GetAllActiveItemsByCategory(int catId)
        {
            List<Item> items = new List<Item>();
            Item item = null;
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * " +
                                      "FROM Item ";
                    if (catId != -1)
                        cmd.CommandText += "WHERE categoryId = @catId AND state = 0";
                    else
                        cmd.CommandText += "WHERE state = 0";

                    cmd.Parameters.AddWithValue("@catId", catId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User seller = new User
                            {
                                UserName = reader["sellerUsername"].ToString()
                            };

                            

                            item = new Item
                            {
                                Id = (int)reader["id"],
                                Name = CheckValue(reader["name"]).ToString(),
                                Description = CheckValue(reader["description"]).ToString(),
                                InitialPrice = (double)reader["initialPrice"],
                                StartDate = (DateTime)reader["startDate"],
                                EndDate = (DateTime)reader["endDate"],
                                State = (int)reader["state"],
                                SellerUser = seller,
                                Category = DBCategory.GetItemCategory((int)reader["categoryId"])
                            };

                            List<Bid> winningBid = new DBBid().GetBids((int)reader["id"], true);
                            item.WinningBid = winningBid.Count != 0 ? winningBid[0] : null;

                            items.Add(item);
                        }
                    }
                    return items;
                }
            }
        }
        #endregion

        #region GetAllItems()
        public List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();
            Item item = null;
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * " +
                                      "FROM Item ";

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User seller = new User
                            {
                                UserName = reader["sellerUsername"].ToString()
                            };

                            item = new Item
                            {
                                Id = (int)reader["id"],
                                Name = CheckValue(reader["name"]).ToString(),
                                Description = CheckValue(reader["description"]).ToString(),
                                InitialPrice = (double)reader["initialPrice"],
                                StartDate = (DateTime)reader["startDate"],
                                EndDate = (DateTime)reader["endDate"],
                                State = (int)reader["state"],
                                SellerUser = seller,
                                Category = DBCategory.GetItemCategory((int)reader["categoryId"])
                            };

                            List<Bid> winningBid = new DBBid().GetBids(item.Id, true);
                            List<Bid> prevBids = new DBBid().GetBids(item.Id, false);
                            item.WinningBid = winningBid.Count != 0 ? winningBid[0] : null;
                            item.OldBids = prevBids.Count != 0 ? prevBids : null;

                            items.Add(item);
                        }
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
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    User seller = new User
                                    {
                                        UserName = reader["sellerUsername"].ToString()
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
                                        Category = DBCategory.GetItemCategory((int)reader["categoryId"])
                                    };

                                    List<Bid> winningBid = new DBBid().GetBids(item.Id, true);
                                    List<Bid> prevBids = new DBBid().GetAllBidsByItem(item.Id);
                                    item.WinningBid = winningBid.Count != 0 ? winningBid[0] : null;
                                    item.OldBids = prevBids.Count != 0 ? prevBids : null;

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
                                          "FROM Item ";
                        if(categoryId != -1)
                            cmd.CommandText += "WHERE(name LIKE @value OR description LIKE @value OR sellerUsername LIKE @value) AND categoryId = @catId";
                        else
                            cmd.CommandText += "WHERE(name LIKE @value OR description LIKE @value OR sellerUsername LIKE @value)";
                        cmd.Parameters.AddWithValue("@catId", categoryId);
                        cmd.Parameters.AddWithValue("@value", "%"+value+"%");

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                seller = new User
                                {
                                    UserName = (string)reader["sellerUsername"]
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
                                    Category = DBCategory.GetItemCategory((int)reader["categoryId"])
                                };

                                List<Bid> winningBid = new DBBid().GetBids((int)reader["id"], true);
                                item.WinningBid = winningBid.Count != 0 ? winningBid[0] : null;

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
                                          "SET name = @name, description = @description, categoryId = @categoryId " +
                                          "WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", item.Id);
                        cmd.Parameters.AddWithValue("@name", item.Name);
                        cmd.Parameters.AddWithValue("@description", item.Description);
                        cmd.Parameters.AddWithValue("@categoryId", item.Category.Id);
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
        public bool DeleteItem(int id)
        {
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
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Debug.Write(ex);

                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection 
                        // is closed or the transaction has already been rolled 
                        // back on the server.
                        Debug.Write(exRollback);
                    }
                }
                return false;
            }
        }
        #endregion

        public object CheckValue(object value)
        {
            return value == DBNull.Value ? "" : value;
        }
    }
}