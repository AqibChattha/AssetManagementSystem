using AssetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Services.ModelServices
{
    public static class BackupService
    {
        public static void CheckBackup()
        {
            using (var connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                // Check if backup table has one and only one entry
                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM backup", connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 1)
                    {
                        // Get backup interval and last backup time from table
                        using (var selectCommand = new SQLiteCommand("SELECT * FROM backup;", connection))
                        {
                            using (var reader = selectCommand.ExecuteReader())
                            {
                                reader.Read();
                                Backup backup = new Backup
                                {
                                    FileLocation = reader.GetString(0),
                                    Interval = reader.GetString(1),
                                    LastBackup = DateTime.Parse(reader.GetString(2))
                                };

                                // Calculate next backup time and take backup if necessary
                                DateTime nextBackup = Convert.ToDateTime(backup.LastBackup);
                                switch (backup.Interval)
                                {
                                    case "Daily":
                                        nextBackup = nextBackup.AddDays(1);
                                        break;
                                    case "Weekly":
                                        nextBackup = nextBackup.AddDays(7);
                                        break;
                                    case "Monthly":
                                        nextBackup = nextBackup.AddMonths(1);
                                        break;
                                    default:
                                        throw new Exception("Invalid backup interval specified in backup table.");
                                }

                                if (nextBackup <= DateTime.Now)
                                {
                                    // Take backup
                                    string backupLocation = backup.FileLocation + "\\backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".db";
                                    File.Copy("..\\..\\Data\\Database\\AssetManagementSystem.db", backupLocation, true);

                                    // Update last backup time in table
                                    using (var updateCommand = new SQLiteCommand("UPDATE backup SET last_backup = @lastBackup", connection))
                                    {
                                        updateCommand.Parameters.AddWithValue("@lastBackup", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        updateCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    else if (count > 1)
                    {
                        throw new Exception("Multiple entries found in backup table. There should be only one.");
                    }
                }
            }
        }

        public static void SetBackup(string backupLocation, string backupInterval)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM backup", conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            // Insert a new row
                            using (SQLiteCommand insertCmd = new SQLiteCommand("INSERT INTO backup (backup_location, backup_interval, last_backup) VALUES (@location, @interval, @lastBackup)", conn))
                            {
                                insertCmd.Parameters.AddWithValue("@location", backupLocation);
                                insertCmd.Parameters.AddWithValue("@interval", backupInterval);
                                insertCmd.Parameters.AddWithValue("@lastBackup", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else if (count == 1)
                        {
                            // Update existing row
                            using (SQLiteCommand updateCmd = new SQLiteCommand("UPDATE backup SET backup_location = @location, backup_interval = @interval, last_backup = @lastBackup", conn))
                            {
                                updateCmd.Parameters.AddWithValue("@location", backupLocation);
                                updateCmd.Parameters.AddWithValue("@interval", backupInterval);
                                updateCmd.Parameters.AddWithValue("@lastBackup", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            throw new Exception("Multiple backup rows found in database");
                        }
                    }

                    transaction.Commit();
                }
            }
        }


    }
}
