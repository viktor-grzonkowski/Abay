using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Transactions;
using Entities;

namespace Database
{
    public class DBBid
    {
        #region UpdateBid(Bid prevBid, Bid newBid)
        /// <summary>
        ///     <para>
        ///         prevBid = UPDATEs existing bid
        ///     </para>
        ///     <para>
        ///         newBid = INSERTs new bid
        ///     </para>
        /// </summary>
        /// <param name="prevBid"></param>
        /// <param name="newBid"></param>
        /// <returns></returns>
        public bool UpdateBid(Bid prevBid, Bid newBid)
        {
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    try
                    {
                        if (prevBid != null) {

                            bool isEnough = false;

                            using (SqlCommand check = new SqlCommand("", connection))
                            {
                                check.CommandText = "SELECT " +
                                                      "IIF( " +
                                                        "amount >= @amount, " +
                                                        "'false', " +
                                                      "'true') AS 'isTrue' " +
                                                    "FROM [Bid] " +
                                                    "WHERE itemId = @itemId AND isWinning = 'True' ";

                                check.Parameters.AddWithValue("@itemId", prevBid.ItemId);
                                check.Parameters.AddWithValue("@amount", newBid.Amount);

                                SqlDataReader reader = check.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        isEnough = bool.Parse(reader["isTrue"].ToString());
                                    }
                                }
                            }

                            if (isEnough)
                            {
                                using (SqlCommand cmdUno = new SqlCommand("", connection))
                                {
                                    cmdUno.CommandText = "UPDATE [Bid] " +
                                                         "SET isWinning = @winning " +
                                                         "WHERE itemId = @itemId";
                                    cmdUno.Parameters.AddWithValue("@itemId", prevBid.ItemId);
                                    cmdUno.Parameters.AddWithValue("@winning", prevBid.Winning);
                                    cmdUno.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }

                        using (SqlCommand cmdDos = new SqlCommand("", connection))
                        {
                            cmdDos.CommandText = "INSERT INTO [Bid] " +
                                                    "(buyerName, itemId, amount, timestamp, isWinning) " +
                                                    "VALUES " +
                                                    "(@buyerName, @itemId, @amount, @timestamp, @winning)";

                            cmdDos.Parameters.AddWithValue("@buyerName", newBid.BuyerName);
                            cmdDos.Parameters.AddWithValue("@itemId", newBid.ItemId);
                            cmdDos.Parameters.AddWithValue("@amount", newBid.Amount);
                            cmdDos.Parameters.AddWithValue("@timestamp", newBid.Timestamp);
                            cmdDos.Parameters.AddWithValue("@winning", newBid.Winning);
                            cmdDos.ExecuteNonQuery();
                        }

                        scope.Complete();
                        return true;
                    }
                    catch (Exception e)
                    {
                        // Handle the exception if the transaction fails to commit.
                        Debug.Write("#### ERROR IN UpdateBid START #### \n");
                        Debug.Write(e + "\n");
                        Debug.Write("#### ERROR FOR UpdateBid END ####");

                        return false;
                    }
                }
            }
        }
        #endregion

        #region GetBid(int bidId, bool winning)
        public List<Bid> GetBids(int itemId, bool winning)
        {
            // return null if there is no bid
            Bid bid = null;
            List<Bid> bids = new List<Bid>();
            int winningBid = winning == true ? 1 : 0;

            try
            {

                using (SqlConnection connection = DBConnection.GetConnection())
                {

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                          "FROM [Bid] " +
                                          "WHERE itemId = @itemId AND isWinning = @winning " +
                                          "ORDER BY amount DESC";
                        cmd.Parameters.AddWithValue("@itemId", itemId);
                        cmd.Parameters.AddWithValue("@winning", winningBid);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                bid = new Bid
                                {
                                    BuyerName = reader["buyerName"].ToString(),
                                    ItemId = (int)reader["itemId"],
                                    Amount = double.Parse(reader["amount"].ToString()),
                                    Timestamp = DateTime.Parse(reader["timestamp"].ToString()),
                                    Winning = (bool)reader["isWinning"]
                                };

                                bids.Add(bid);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write("#### ERROR IN GetBid START #### \n");
                Debug.Write(e + "\n");
                Debug.Write("#### ERROR FOR GetBid END ####");
            }

            return bids;
        }
        #endregion

        #region GetAllBidsByItem(int itemId)
        public List<Bid> GetAllBidsByItem(int itemId)
        {
            Bid bid = null;
            List<Bid> bids = new List<Bid>();

            try
            {

                using (SqlConnection connection = DBConnection.GetConnection())
                {

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * " +
                                          "FROM [Bid] " +
                                          "WHERE itemId = @itemId " +
                                          "ORDER BY amount DESC";
                        cmd.Parameters.AddWithValue("@itemId", itemId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                bid = new Bid
                                {
                                    BuyerName = reader["buyerName"].ToString(),
                                    ItemId = (int)reader["itemId"],
                                    Amount = double.Parse(reader["amount"].ToString()),
                                    Timestamp = DateTime.Parse(reader["timestamp"].ToString()),
                                    Winning = (bool)reader["isWinning"]
                                };

                                bids.Add(bid);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write("#### ERROR IN GetAllBidsByItem START #### \n");
                Debug.Write(e + "\n");
                Debug.Write("#### ERROR FOR GetAllBidsByItem END ####");
            }

            return bids;
        }
        #endregion
    }
}
