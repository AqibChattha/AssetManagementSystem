using AssetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Services.ModelServices
{
    public static class DistributionServices
    {
        public static List<Distribution> GetAllDistributions()
        {
            List<Distribution> distributions = new List<Distribution>();

            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM distributions";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Distribution distribution = new Distribution();
                            distribution.Id = Convert.ToInt32(reader["id"]);
                            distribution.AssetId = Convert.ToInt32(reader["asset_id"]);
                            distribution.Responsibility = Convert.ToString(reader["responsibility"]);
                            distribution.Place = Convert.ToString(reader["place"]);
                            distribution.IssueDate = Convert.ToDateTime(reader["issue_date"]);
                            distributions.Add(distribution);
                        }
                    }
                }
            }

            return distributions;
        }

        public static Distribution GetDistributionById(int id)
        {
            Distribution distribution = null;

            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM distributions WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            distribution = new Distribution();
                            distribution.Id = Convert.ToInt32(reader["id"]);
                            distribution.AssetId = Convert.ToInt32(reader["asset_id"]);
                            distribution.Responsibility = Convert.ToString(reader["responsibility"]);
                            distribution.Place = Convert.ToString(reader["place"]);
                            distribution.IssueDate = Convert.ToDateTime(reader["issue_date"]);
                        }
                    }
                }
            }

            return distribution;
        }

        public static Distribution GetDistributionByAssetId(int id)
        {
            Distribution distribution = null;

            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM distributions WHERE asset_id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            distribution = new Distribution();
                            distribution.Id = Convert.ToInt32(reader["id"]);
                            distribution.AssetId = Convert.ToInt32(reader["asset_id"]);
                            distribution.Responsibility = Convert.ToString(reader["responsibility"]);
                            distribution.Place = Convert.ToString(reader["place"]);
                            distribution.IssueDate = Convert.ToDateTime(reader["issue_date"]);
                        }
                    }
                }
            }

            return distribution;
        }

        public static void AddDistribution(Distribution distribution)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                string query = "INSERT INTO distributions(asset_id, responsibility, place, issue_date) VALUES(@asset_id, @responsibility, @place, @issue_date)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@asset_id", distribution.AssetId);
                    command.Parameters.AddWithValue("@responsibility", distribution.Responsibility);
                    command.Parameters.AddWithValue("@place", distribution.Place);
                    command.Parameters.AddWithValue("@issue_date", distribution.IssueDate);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateDistribution(Distribution distribution)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                string query = "UPDATE distributions SET asset_id = @asset_id, responsibility = @responsibility, place = @place, issue_date = @issue_date WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", distribution.Id);
                    command.Parameters.AddWithValue("@asset_id", distribution.AssetId);
                    command.Parameters.AddWithValue("@responsibility", distribution.Responsibility);
                    command.Parameters.AddWithValue("@place", distribution.Place);
                    command.Parameters.AddWithValue("@issue_date", distribution.IssueDate);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool DeleteDistribution(int distributionId)
        {
            try
            {
                using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "DELETE FROM distributions WHERE id = @id";
                        command.Parameters.AddWithValue("@id", distributionId);
                        var result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
