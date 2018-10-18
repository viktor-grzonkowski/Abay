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

            int from = page * quantity;
            int to = from + quantity;

            using (SqlConnection connection = DBConnection.GetConnection())
            {

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY id ASC) AS RowNumber, *" +
                                        "FROM [Category]"+
                                        "WHERE id < @to And id > @from";
                    cmd.Parameters.AddWithValue("@to", to);
                    cmd.Parameters.AddWithValue("@from", from);

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            User seller = new User
                            {
                                UserName = result["seller_username"].ToString()
                            };

                            item = new Item
                            {
                                Id = Int32.Parse(result["id"].ToString()),
                                Name = result["name"].ToString(),
                                InitialPrice = Double.Parse(result["initialPrice"].ToString()),
                                State = Int32.Parse(result["state"].ToString()),
                                SellerUsername = seller,
                                CategoryId = DBCategory.GetItemCategory(Int32.Parse(result["category_id"].ToString()))
                            };

                            items.Add(item);
                        }
                        return items;
                    }
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
                    cmd.CommandText = "SELECT *" +
                                        "FROM [Item]" +
                                        "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            User seller = new User
                            {
                                UserName = result["seller_username"].ToString()
                            };

                            item = new Item
                            {
                                Id = Int32.Parse(result["id"].ToString()),
                                Name = result["name"].ToString(),
                                Description = result["description"].ToString(),
                                InitialPrice = Double.Parse(result["initialPrice"].ToString()),
                                State = Int32.Parse(result["state"].ToString()),
                                SellerUsername = seller,
                                CategoryId = DBCategory.GetItemCategory(Int32.Parse(result["category_id"].ToString()))
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
                    cmd.CommandText =   "SELECT *" +
                                        "FROM [Item]";
                    if(categoryId != -1)
                        cmd.CommandText += "WHERE(id = @value OR name like @value OR description LIKE @value OR seller_username LIKE @value) AND category_id = @id";
                    else
                        cmd.CommandText += "WHERE(id = @value OR name like @value OR description LIKE @value OR seller_username LIKE @value)";
                    cmd.Parameters.AddWithValue("@id", categoryId);
                    cmd.Parameters.AddWithValue("@value", value);

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            User seller = new User
                            {
                                UserName = result["seller_username"].ToString()
                            };

                            User buyer = new User
                            {
                                UserName = result["buyer_username"].ToString()
                            };

                            item = new Item
                            {
                                Id = Int32.Parse(result["id"].ToString()),
                                Name = result["name"].ToString(),
                                Description = result["description"].ToString(),
                                InitialPrice = Double.Parse(result["initialPrice"].ToString()),
                                FinalPrice = Double.Parse(result["finalPrice"].ToString()),
                                StartDate = DateTime.Parse(result["startDate"].ToString()),
                                EndDate = DateTime.Parse(result["endDate"].ToString()),
                                State = Int32.Parse(result["state"].ToString()),
                                SellerUsername = seller,
                                BuyerUsername = buyer,
                                CategoryId = DBCategory.GetItemCategory(Int32.Parse(result["category_id"].ToString()))
                            };

                            items.Add(item);
                        }
                        return items;
                    }
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
                    cmd.CommandText = "INSERT INTO [Item]" +
                                       "(name ,initialPrice ,state ,seller_username ,category_name)" +
                                       "VALUES" +
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
                    cmd.CommandText =   "UPDATE [Item]" +
                                        "SET name = @name, description = @description" +
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
                    cmd.CommandText = "DELETE FROM [Item]" +
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