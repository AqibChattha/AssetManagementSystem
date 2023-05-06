using AssetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem.UI
{
	public partial class MainForm : Form
	{
		private Button currentButton;
		private User user;

        public User LoggedInUser { get { return user; } private set { user = value; } }

		// Singleton pattern to ensure we get the same Main form.
		private static MainForm _instance;
		public static MainForm Instance
		{
			get
			{
				return _instance;
			}
		}

		public MainForm(User user)
		{
			InitializeComponent();
			_instance = this;
            this.user = user;
            currentButton = btnItemRecords;
            currentButton.BackColor = Color.FromArgb(21, 50, 132);
            ShowUserControl(Components.ItemRecord.ViewAll.Instance);
            if (user.Role == 3)
            {
                btnArchive.Visible = false;
                btnArchive.Enabled = false;
            }
        }

        private void ChangeCurrentButton(ref Button btn)
        {
            currentButton.BackColor = Color.FromArgb(14, 34, 88);
            btn.BackColor = Color.FromArgb(21, 50, 132);
			currentButton = btn;
        }

		public void ShowUserControl(UserControl userControl)
		{
			try
            {
                if (!pnlContant.Controls.Contains(userControl))
                {
                    pnlContant.Controls.Add(userControl);
                    userControl.Dock = DockStyle.Fill;
                    userControl.Refresh();
                    userControl.BringToFront();
                }
                else
                {
                    userControl.Refresh();
                    userControl.BringToFront();
                }
            }
			catch (Exception)
			{

			}
		}

		private void btnLogout_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeCurrentButton(ref btn);
            this.DialogResult = DialogResult.Retry;
			this.Close();
		}

		private void btnItemRecords_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeCurrentButton(ref btn);
            ShowUserControl(Components.ItemRecord.ViewAll.Instance);
		}

		private void btn_AddItem_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeCurrentButton(ref btn);
			ShowUserControl(Components.ItemRecord.Add.Instance);
		}

		private void btnBackup_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeCurrentButton(ref btn);
            ShowUserControl(Components.Backup.Instance);
		}

		private void btnArchive_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeCurrentButton(ref btn);
            ShowUserControl(Components.Archive.Instance);
		}

		private void btnAdmin_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ChangeCurrentButton(ref btn);
            Components.Admin.Instance.Refresh(user);
            ShowUserControl(Components.Admin.Instance);
        }
    }
}
