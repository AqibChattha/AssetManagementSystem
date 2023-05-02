using AssetManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem.UI.Components
{
	public partial class Backup : UserControl
    {
        private static Backup _instance;
		public static Backup Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Backup();
				}
				return _instance;
			}
		}

		private Backup()
		{
			InitializeComponent();
		}

		private void btnBackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please select a backup location.");
                return;
            }

            if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("The given location does not exist please select the location again.");
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                $"Do you want to create a backup of the database at: \n{textBox1.Text}",
                "Confirm Backup",
                MessageBoxButtons.YesNo);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                DatabaseHelper.CopyDatabase("..\\..\\Data\\Database\\AssetManagementSystem.db", textBox1.Text +  $"\\AMS_Backup_{DateTime.Now.ToString("dd-MM-yyyy HH-mm")}.db");
                MessageBox.Show($"Database backup created at: \n{textBox1.Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating database backup: \n{ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    textBox1.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
