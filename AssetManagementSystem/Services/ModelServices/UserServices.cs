using AssetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Services.ModelServices
{
    public static class UserServices
    {
        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.Id = Convert.ToInt32(reader["id"]);
                            user.Username = reader["username"].ToString();
                            user.Password = reader["password"].ToString();
                            user.Role = Convert.ToInt32(reader["role"]);
                            users.Add(user);
                        }
                    }
                }
                connection.Close();
            }
            return users;
        }

        public static User GetUserById(int id)
        {
            User user = null;
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE id=@id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User();
                            user.Id = (int)reader["id"];
                            user.Username = (string)reader["username"];
                            user.Password = (string)reader["password"];
                            user.Role = (int)reader["role"];
                        }
                    }
                }
                connection.Close();
            }
            return user;
        }

        public static User GetUserByRole(int role)
        {
            User user = null;
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE role=@role";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@role", role);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User();
                            user.Id = Convert.ToInt32(reader["id"]);
                            user.Username = reader["username"].ToString();
                            user.Password = reader["password"].ToString();
                            user.Role = Convert.ToInt32(reader["role"]);
                        }
                    }
                }
                connection.Close();
            }
            return user;
        }

        public static void AddUser(User user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO users(username, password, role) VALUES (@username, @password, @role)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void UpdateUser(User user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET username=@username, password=@password, role=@role WHERE id=@id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void DeleteUser(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM users WHERE id=@id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
