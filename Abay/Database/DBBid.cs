using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Transactions;
using Entities;

namespace Database
{
    public class DBBid
    {
        #region UpdateBid(Bid bid)
        public bool UpdateBid(Bid bid)
        {
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    // Enlist a command in the current transaction.
                    SqlCommand cmd = connection.CreateCommand();

                    try
                    {
                        // Execute one command.
                        cmd.CommandText =   "UPDATE [Bid] " +
                                            "SET username = @username, amount = @amount, timestamp = @timestamp " +
                                            "WHERE id = @id";
                        cmd.Parameters.AddWithValue("@username", bid.UserName);
                        cmd.Parameters.AddWithValue("@amount", bid.Amount);
                        cmd.Parameters.AddWithValue("@timestamp", bid.Timestamp);
                        cmd.ExecuteNonQuery();

                        scope.Complete();
                        return true;
                    }
                    catch (Exception e)
                    {
                        // Handle the exception if the transaction fails to commit.
                        Debug.Write("#### ERROR IN Bid START #### \n");
                        Debug.Write(e + "\n");
                        Debug.Write("#### ERROR FOR Bid END ####");

                        return false;
                    }
                    finally
                    {
                        cmd.Dispose();
                        scope.Dispose();
                    }
                }
            }
        }
        #endregion

        #region GetBid(int bidId)
        public Bid GetBid(int bidId)
        {
            Bid bid = null;
            try {
                using (SqlConnection connection = DBConnection.GetConnection())
                {

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                          "FROM [Bid] " +
                                          "WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", bidId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                bid = new Bid
                                {
                                    Id = bidId,
                                    UserName = reader["username"].ToString(),
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
        public int InsertBid(Bid bid)
        {
            int rowId = -1;
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
                                        "(username ,amount ,timestamp) " +
                                        "OUTPUT INSERTED.ID " +
                                        "VALUES (@username ,@amount ,@timestamp)";
                    cmd.Parameters.AddWithValue("@username", bid.UserName);
                    cmd.Parameters.AddWithValue("@amount", bid.Amount);
                    cmd.Parameters.AddWithValue("@timestamp", bid.Timestamp);
                    rowId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Commit the transaction.
                    sqlTran.Commit();
                    

                    return rowId;
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
                    return -1;
                }
            }
        }
        #endregion
    }
}
