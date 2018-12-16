using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DBCategory
    {

        public static ItemCategory GetItemCategory(int categoryId)
        {
            ItemCategory build = null;

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =   "SELECT id, name " +
                                        "FROM [Category] " +
                                        "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", categoryId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            build = new ItemCategory
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                    }

                    return build;
                }
            }
        }

        public static List<ItemCategory> GetAllCategories()
        {
            List<ItemCategory> categories = new List<ItemCategory>();
            ItemCategory categorie;

            using (SqlConnection connection = DBConnection.GetConnection())
            {

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =   "SELECT *" +
                                        "FROM [Category]";

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            categorie = new ItemCategory
                            {
                                Id = Int32.Parse(result["id"].ToString()),
                                Name = result["name"].ToString()
                            };
                            categories.Add(categorie);
                        }
                    }
                }
            }
            return categories;
        }
    }
}
