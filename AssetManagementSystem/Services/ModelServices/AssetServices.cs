using AssetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AssetManagementSystem.Services.ModelServices
{
    public static class AssetServices
    {
        public static List<Asset> GetAllAssets(int pageNumber, int pageSize, bool? isServiceable = null)
        {
            List<Asset> assets = new List<Asset>();

            try
            {
                using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
                {
                    connection.Open();
                    string queriy = "";
                    if (isServiceable == null)
                    {
                        queriy = $"SELECT * FROM assets ORDER BY id desc LIMIT {(pageNumber - 1) * pageSize}, {pageSize}";
                    }
                    else if (isServiceable == true)
                    {
                        queriy = $"SELECT * FROM assets WHERE condition_category <> 'Unserviceable (US)' ORDER BY id desc LIMIT {(pageNumber - 1) * pageSize}, {pageSize}";
                    }
                    else
                    {
                        queriy = $"SELECT * FROM assets WHERE condition_category = 'Unserviceable (US)' ORDER BY id desc LIMIT {(pageNumber - 1) * pageSize}, {pageSize}";
                    }

                    var command = new SQLiteCommand(queriy, connection);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Asset asset = new Asset
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Brand = reader["brand"].ToString(),
                                Specifications = reader["specifications"].ToString(),
                                ProcurementDate = Convert.ToDateTime(reader["procurement_date"].ToString()),
                                Colour = reader["colour"].ToString(),
                                Image = reader["image"] == DBNull.Value ? null : (byte[])reader["image"],
                                Price = Convert.ToDecimal(reader["price"]),
                                ConditionCategory = reader["condition_category"].ToString(),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                MinuteSheetNumber = reader["minute_sheet_number"].ToString(),
                                MinuteSheetDocument = reader["minute_sheet_document"] == DBNull.Value ? null : (byte[])reader["minute_sheet_document"],
                                MS_DocumentName = reader["m_s_document_name"].ToString(),
                                Comments = reader["comments"].ToString(),
                                Distribution = DistributionServices.GetDistributionByAssetId(Convert.ToInt32(reader["id"]))
                            };

                            assets.Add(asset);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return assets;
        }

        public static int GetTotalPages(int pageSize, bool? isServiceable = null)
        {
            int totalAssets = 0;
            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string queriy = "";
                if (isServiceable == null)
                {
                    queriy = $"SELECT COUNT(*) FROM assets";
                }
                else if (isServiceable == true)
                {
                    queriy = $"SELECT COUNT(*) FROM assets WHERE condition_category <> 'Unserviceable (US)'";
                }
                else
                {
                    queriy = $"SELECT COUNT(*) FROM assets WHERE condition_category = 'Unserviceable (US)'";
                }
                var command = new SQLiteCommand(queriy, connection);
                totalAssets = Convert.ToInt32(command.ExecuteScalar());
            }
            int totalPages = (int)Math.Ceiling((double)totalAssets / pageSize);
            return totalPages;
        }

        public static Asset GetAssetById(int id)
        {
            using (var conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM assets WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Asset
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Brand = reader.IsDBNull(reader.GetOrdinal("brand")) ? null : reader.GetString(reader.GetOrdinal("brand")),
                                Specifications = reader.IsDBNull(reader.GetOrdinal("specifications")) ? null : reader.GetString(reader.GetOrdinal("specifications")),
                                ProcurementDate = reader.GetDateTime(reader.GetOrdinal("procurement_date")),
                                Colour = reader.IsDBNull(reader.GetOrdinal("colour")) ? null : reader.GetString(reader.GetOrdinal("colour")),
                                Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("image")),
                                Price = (decimal)(reader.IsDBNull(reader.GetOrdinal("price")) ? null : (double?)reader.GetDouble(reader.GetOrdinal("price"))),
                                ConditionCategory = reader.GetString(reader.GetOrdinal("condition_category")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                MinuteSheetNumber = reader.GetString(reader.GetOrdinal("minute_sheet_number")),
                                MinuteSheetDocument = reader.IsDBNull(reader.GetOrdinal("minute_sheet_document")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("minute_sheet_document")),
                                MS_DocumentName = reader.GetString(reader.GetOrdinal("m_s_document_name")),
                                Comments = reader.IsDBNull(reader.GetOrdinal("comments")) ? null : reader.GetString(reader.GetOrdinal("comments")),
                                Distribution = DistributionServices.GetDistributionByAssetId(Convert.ToInt32(reader["id"]))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public static bool AddAsset(Asset asset)
        {
            try
            {
                using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        string query = @"INSERT INTO assets 
                                (name, brand, specifications, procurement_date, colour, image, price, condition_category, quantity, minute_sheet_number, minute_sheet_document, m_s_document_name, comments) 
                                VALUES 
                                (@name, @brand, @specifications, @procurement_date, @colour, @image, @price, @condition_category, @quantity, @minute_sheet_number, @minute_sheet_document, @m_s_document_name, @comments);
                                SELECT last_insert_rowid();";

                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", asset.Name);
                            command.Parameters.AddWithValue("@brand", asset.Brand ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@specifications", asset.Specifications ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@procurement_date", asset.ProcurementDate);
                            command.Parameters.AddWithValue("@colour", asset.Colour ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@image", asset.Image ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@price", asset.Price);
                            command.Parameters.AddWithValue("@condition_category", asset.ConditionCategory);
                            command.Parameters.AddWithValue("@quantity", asset.Quantity);
                            command.Parameters.AddWithValue("@minute_sheet_number", asset.MinuteSheetNumber);
                            command.Parameters.AddWithValue("@minute_sheet_document", asset.MinuteSheetDocument ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@m_s_document_name", asset.MS_DocumentName);
                            command.Parameters.AddWithValue("@comments", asset.Comments ?? (object)DBNull.Value);

                            int id = Convert.ToInt32(command.ExecuteScalar());

                            string query2 = "INSERT INTO distributions (asset_id, responsibility, place, issue_date) VALUES(@asset_id, @responsibility, @place, @issue_date)";
                            using (SQLiteCommand command2 = new SQLiteCommand(query2, connection))
                            {
                                command2.Parameters.AddWithValue("@asset_id", id);
                                command2.Parameters.AddWithValue("@responsibility", asset.Distribution.Responsibility);
                                command2.Parameters.AddWithValue("@place", asset.Distribution.Place);
                                command2.Parameters.AddWithValue("@issue_date", asset.Distribution.IssueDate);
                                command2.ExecuteNonQuery();
                            }

                            using (var command3 = new SQLiteCommand("INSERT INTO comments (asset_id, comment_timestamp, comments) VALUES (@assetId, @commentTimeStamp, @comments)", connection))
                            {
                                command3.Parameters.AddWithValue("@assetId", id);
                                command3.Parameters.AddWithValue("@commentTimeStamp", DateTime.Now);
                                command3.Parameters.AddWithValue("@comments", asset.Comments);
                                command3.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static bool UpdateAsset(Asset asset)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {

                        var command = new SQLiteCommand(connection);

                        command.CommandText = "UPDATE assets SET name = @name, brand = @brand, " +
                            "specifications = @specifications, procurement_date = @procurement_date, colour = @colour, " +
                            "image = @image, price = @price, condition_category = @condition_category, quantity = @quantity, " +
                            "minute_sheet_number = @minute_sheet_number, minute_sheet_document = @minute_sheet_document, m_s_document_name = @m_s_document_name, " +
                            "comments = @comments WHERE id = @id";

                        command.Parameters.AddWithValue("@id", asset.Id);
                        command.Parameters.AddWithValue("@name", asset.Name);
                        command.Parameters.AddWithValue("@brand", asset.Brand ?? string.Empty);
                        command.Parameters.AddWithValue("@specifications", asset.Specifications ?? string.Empty);
                        command.Parameters.AddWithValue("@procurement_date", asset.ProcurementDate);
                        command.Parameters.AddWithValue("@colour", asset.Colour ?? string.Empty);
                        command.Parameters.AddWithValue("@image", asset.Image ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@price", asset.Price);
                        command.Parameters.AddWithValue("@condition_category", asset.ConditionCategory);
                        command.Parameters.AddWithValue("@quantity", asset.Quantity);
                        command.Parameters.AddWithValue("@minute_sheet_number", asset.MinuteSheetNumber);
                        command.Parameters.AddWithValue("@minute_sheet_document", asset.MinuteSheetDocument ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@m_s_document_name", asset.MS_DocumentName);
                        command.Parameters.AddWithValue("@comments", asset.Comments ?? string.Empty);
                        int rowsUpdated = command.ExecuteNonQuery();

                        string query2 = "INSERT INTO distributions(asset_id, responsibility, place, issue_date) VALUES(@asset_id, @responsibility, @place, @issue_date)";
                        using (SQLiteCommand command2 = new SQLiteCommand(query2, connection))
                        {
                            command2.Parameters.AddWithValue("@asset_id", asset.Id);
                            command2.Parameters.AddWithValue("@responsibility", asset.Distribution.Responsibility);
                            command2.Parameters.AddWithValue("@place", asset.Distribution.Place);
                            command2.Parameters.AddWithValue("@issue_date", asset.Distribution.IssueDate);
                            command2.ExecuteNonQuery();
                        }

                        if (rowsUpdated > 0)
                        {
                            result = true;
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            return result;
        }

        public static void DeleteAsset(int id)
        {
            try
            {
                using (var conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = "DELETE FROM comments WHERE asset_id = @id; DELETE FROM distributions WHERE asset_id = @id; DELETE FROM assets WHERE id=@id";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
