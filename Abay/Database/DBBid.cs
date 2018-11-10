using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using Entities;

namespace Database
{
    public class DBBid
    {
        #region Bid(Item item)
        public bool Bid(Item item)
        {
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
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
                        // Execute one command.
                        cmd.CommandText =   "UPDATE [Bid] " +
                                            "SET username = @username, amount = @amount " +
                                            "WHERE item_id = @id";
                        cmd.Parameters.AddWithValue("@id", item.Id);
                        cmd.Parameters.AddWithValue("@username", item.BuyerUser.UserName);
                        cmd.Parameters.AddWithValue("@amount", item.FinalPrice);
                        cmd.ExecuteNonQuery();

                        // Commit the transaction.
                        sqlTran.Commit();
                        scope.Complete();
                        return true;
                    }
                    catch (Exception e)
                    {
                        // Handle the exception if the transaction fails to commit.
                        Debug.Write("#### ERROR IN Bid START #### \n");
                        Debug.Write(e + "\n");
                        Debug.Write("#### ERROR FOR Bid END ####");

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
                            Debug.Write("#### ROLLBACK ERROR IN Bid START #### \n");
                            Debug.Write(exRollback.Message + "\n");
                            Debug.Write("#### ROLLBACK ERROR FOR Bid END ####");
                        }

                        return false;
                    }
                    finally
                    {
                        cmd.Dispose();
                        sqlTran.Dispose();
                        scope.Dispose();
                    }
                }
            }
        }
        #endregion

        #region GetBid(int itemId)
        public Bid GetBid(int itemId)
        {
            Bid bid = null;
            try {
                using (SqlConnection connection = DBConnection.GetConnection())
                {

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                            "FROM [Bid] " +
                                            "WHERE item_id = @id";
                        cmd.Parameters.AddWithValue("@id", itemId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                bid = new Bid
                                {
                                    UserName = reader["username"].ToString(),
                                    ItemId = int.Parse(reader["item_id"].ToString()),
                                    Amount = double.Parse(reader["amount"].ToString()),
                                    Timestamp = DateTime.Parse(reader["timestamp"].ToString())
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write("#### ERROR MESSAGE IN GetBid START #### \n");
                Debug.Write(e + "\n");
                Debug.Write("#### ERROR MESSAGE FOR GetBid END ####");
            }

            return bid;
        }
        #endregion

        #region InsertBid(Bid bid)
        public bool InsertBid(Bid bid)
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
                    // Execute one command.
                    cmd.CommandText =   "INSERT INTO [Bid] " +
                                        "(username ,item_id ,amount ,timestamp) " +
                                        "VALUES " +
                                        "(@username ,@item_id ,@amount ,@timestamp)";
                    cmd.Parameters.AddWithValue("@username", bid.UserName);
                    cmd.Parameters.AddWithValue("@item_id", bid.ItemId);
                    cmd.Parameters.AddWithValue("@amount", bid.Amount);
                    cmd.Parameters.AddWithValue("@timestamp", bid.Timestamp);
                    cmd.ExecuteNonQuery();

                    // Commit the transaction.
                    sqlTran.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    // Handle the exception if the transaction fails to commit.
                    Debug.Write("#### ERROR IN GetBid START #### \n");
                    Debug.Write(e + "\n");
                    Debug.Write("#### ERROR FOR GetBid END ####");

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
                        Debug.Write("#### ROLLBACK ERROR IN GetBid START #### \n");
                        Debug.Write(exRollback.Message + "\n");
                        Debug.Write("#### ROLLBACK ERROR FOR GetBid END ####");
                    }
                    return false;
                }
            }
        }
        #endregion
    }
}
