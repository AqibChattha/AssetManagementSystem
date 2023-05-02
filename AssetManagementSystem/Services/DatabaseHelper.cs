using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Services
{
    public static class DatabaseHelper
    {
        public static string ConnectionString = @"Data Source=..\..\Data\Database\AssetManagementSystem.db;Version=3;";

        public static void InitializeDatabase()
        {
            if (!System.IO.File.Exists("..\\..\\Data\\Database\\AssetManagementSystem.db"))
            {
                SQLiteConnection.CreateFile("..\\..\\Data\\Database\\AssetManagementSystem.db");

                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();

                    string createUserTableSql = "CREATE TABLE users (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT NOT NULL, password TEXT NOT NULL, role INTEGER NOT NULL)";
                    using (SQLiteCommand command = new SQLiteCommand(createUserTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createAssetTableSql = "CREATE TABLE assets (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, brand TEXT, specifications TEXT, procurement_date TEXT NOT NULL, colour TEXT, image BLOB, price REAL, condition_category TEXT NOT NULL, quantity INTEGER NOT NULL, minute_sheet_number TEXT NOT NULL, minute_sheet_document BLOB, comments TEXT)";
                    using (SQLiteCommand command = new SQLiteCommand(createAssetTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createInspectionTableSql = "CREATE TABLE comments (id INTEGER PRIMARY KEY AUTOINCREMENT, asset_id INTEGER NOT NULL, comment_timestamp TEXT NOT NULL, comments TEXT FOREIGN KEY (asset_id) REFERENCES assets(id))";
                    using (SQLiteCommand command = new SQLiteCommand(createInspectionTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createDistributionTableSql = "CREATE TABLE distributions (id INTEGER PRIMARY KEY AUTOINCREMENT, asset_id INTEGER NOT NULL, responsibility TEXT NOT NULL, place TEXT NOT NULL, issue_date TEXT NOT NULL, FOREIGN KEY (asset_id) REFERENCES assets(id))";
                    using (SQLiteCommand command = new SQLiteCommand(createDistributionTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CopyDatabase(string src, string dest)
        {
            string srcFile = Path.Combine(Directory.GetCurrentDirectory(), src);

            if (File.Exists(src))
            {
                File.Copy(src, dest, true);
            }
        }
    }
}
