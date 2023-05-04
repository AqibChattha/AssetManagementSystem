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
    public static class CommentsServices
    {

        public static List<PreviousComments> GetAllComments()
        {
            List<PreviousComments> comments = new List<PreviousComments>();

            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM comments", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comments.Add(new PreviousComments
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                AssetId = Convert.ToInt32(reader["asset_id"]),
                                commentTimeStamp = Convert.ToDateTime(reader["comment_timestamp"]),
                                comments = Convert.ToString(reader["comments"]),
                            });
                        }
                    }
                }
            }

            return comments;
        }

        public static List<PreviousComments> GetAssetComments(int assetId)
        {
            List<PreviousComments> comments = new List<PreviousComments>();

            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM comments WHERE asset_id = @assetId ORDER BY comment_timestamp DESC LIMIT -1 OFFSET 1", connection))
                {
                    command.Parameters.AddWithValue("@assetId", assetId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(reader["comments"])))
                            {
                                comments.Add(new PreviousComments
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    AssetId = Convert.ToInt32(reader["asset_id"]),
                                    commentTimeStamp = Convert.ToDateTime(reader["comment_timestamp"]),
                                    comments = Convert.ToString(reader["comments"]),
                                });
                            }
                        }
                    }
                }
            }

            return comments;
        }

        public static PreviousComments GetCommentById(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM comments WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PreviousComments
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                AssetId = Convert.ToInt32(reader["asset_id"]),
                                commentTimeStamp = Convert.ToDateTime(reader["comment_timestamp"]),
                                comments = Convert.ToString(reader["comments"]),
                            };
                        }
                    }
                }
            }

            return null;
        }

        public static void AddComment(PreviousComments comment)
        {
            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("INSERT INTO comments (asset_id, comment_timestamp, comments) VALUES (@assetId, @commentTimeStamp, @comments)", connection))
                {
                    command.Parameters.AddWithValue("@assetId", comment.AssetId);
                    command.Parameters.AddWithValue("@commentTimeStamp", comment.commentTimeStamp);
                    command.Parameters.AddWithValue("@comments", comment.comments);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateComment(PreviousComments comment)
        {
            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("UPDATE comments SET asset_id=@assetId, comment_timestamp=@commentTimeStamp, comments=@comments WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@assetId", comment.AssetId);
                    command.Parameters.AddWithValue("@commentTimeStamp", comment.commentTimeStamp);
                    command.Parameters.AddWithValue("@comments", comment.comments);
                    command.Parameters.AddWithValue("@id", comment.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteComment(int id)
        {
            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = "DELETE FROM comments WHERE id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to delete comment.", ex);
                    }
                }
            }
        }

    }
}
