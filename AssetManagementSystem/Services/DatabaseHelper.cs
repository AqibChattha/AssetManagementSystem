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
        public static string ConnectionString = @"Data Source=AssetManagementSystem.db;Version=3;";

        public static void InitializeDatabase()
        {
            if (!System.IO.File.Exists("AssetManagementSystem.db"))
            {
                SQLiteConnection.CreateFile("AssetManagementSystem.db");
            }

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    string createUserTableSql = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT NOT NULL, password TEXT NOT NULL, role INTEGER NOT NULL); ";
                    using (SQLiteCommand command = new SQLiteCommand(createUserTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    string createUsersSql = "INSERT INTO [users] ([id],[username],[password],[role]) VALUES (1,'Level 1 User','Level1',1); " +
                        "INSERT INTO [users] ([id],[username],[password],[role]) VALUES (2,'Level 2 User','Level2',2); " +
                        "INSERT INTO [users] ([id],[username],[password],[role]) VALUES (3,'Level 3 User','Level3',3);";
                    using (SQLiteCommand command = new SQLiteCommand(createUsersSql, conn))
                    {
                        try { command.ExecuteNonQuery(); } catch (Exception) { }
                    }

                    string createAssetTableSql = "CREATE TABLE IF NOT EXISTS assets (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, brand TEXT, specifications TEXT, procurement_date TEXT NOT NULL, colour TEXT, image BLOB, price REAL, condition_category TEXT NOT NULL, quantity INTEGER NOT NULL, minute_sheet_number TEXT NOT NULL, minute_sheet_document BLOB, m_s_document_name TEXT, comments TEXT);";
                    using (SQLiteCommand command = new SQLiteCommand(createAssetTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createInspectionTableSql = "CREATE TABLE IF NOT EXISTS comments (id INTEGER PRIMARY KEY AUTOINCREMENT, asset_id INTEGER NOT NULL, comment_timestamp TEXT NOT NULL, comments TEXT, FOREIGN KEY (asset_id) REFERENCES assets(id))";
                    using (SQLiteCommand command = new SQLiteCommand(createInspectionTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createDistributionTableSql = "CREATE TABLE IF NOT EXISTS distributions (id INTEGER PRIMARY KEY AUTOINCREMENT, asset_id INTEGER NOT NULL, responsibility TEXT NOT NULL, place TEXT NOT NULL, issue_date TEXT NOT NULL, FOREIGN KEY (asset_id) REFERENCES assets(id))";
                    using (SQLiteCommand command = new SQLiteCommand(createDistributionTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createBackupTableSql = "CREATE TABLE IF NOT EXISTS backup (backup_location TEXT, backup_interval TEXT, last_backup TEXT);";
                    using (SQLiteCommand command = new SQLiteCommand(createBackupTableSql, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
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
