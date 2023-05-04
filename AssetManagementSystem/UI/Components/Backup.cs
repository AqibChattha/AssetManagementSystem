using AssetManagementSystem.Services;
using AssetManagementSystem.Services.ModelServices;
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
            comboBox1.SelectedIndex = 0;
		}

		private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(textBox1.Text) && comboBox1.SelectedItem != null)
                {
                    BackupService.SetBackup(textBox1.Text, comboBox1.SelectedItem.ToString());
                    MessageBox.Show("Backup is set successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please choose the valid backup location or interval.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while setting the backup.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
