using AssetManagementSystem.Models;
using AssetManagementSystem.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem.UI.Components
{
	public partial class Admin : UserControl
	{
		private User user;
		private User updatingFor;

		private static Admin _instance;
		public static Admin Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Admin();
				}
				return _instance;
			}
		}

		private Admin()
		{
			InitializeComponent();
		}


        public void Refresh(User user)
		{
			ClearFields();
            this.user = user;
			label1.Text = "Logged in as: " + user.Username;
            if (user.Role <= 2)
            {
                label6.Visible = false;
                btnLevel2ChangePass.Visible = true;
                btnLevel3ChangePass.Visible = true;
                label3.Visible = true;
                label5.Visible = true;
            }
            if (user.Role <= 1)
            {
                btnLevel1ChangePass.Visible = true;
                label2.Visible = true;
            }
        }
		
		private void ClearFields()
		{
			label6.Visible = true;
            ClearChangePasswordFields();
            btnLevel1ChangePass.Visible = false;
			btnLevel2ChangePass.Visible = false;
			btnLevel3ChangePass.Visible = false;
			label2.Visible = false;
			label3.Visible = false;
			label5.Visible = false;
        }

        private void ClearChangePasswordFields()
        {
            groupBox1.Visible = false;
            updatingFor = null;
            txtNewPassword.Text = "";
            txtOldPassword.Text = "";
        }

		private void button3_Click(object sender, EventArgs e)
		{
			if (!txtOldPassword.Text.Equals(user.Password))
			{
				MessageBox.Show(user.Username + "'s password is incorrect.");
				return;
			}
			else
			{
				if (MessageBox.Show(user.Username+" are you sure you want to update the password for " + updatingFor.Username, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					updatingFor.Password = txtNewPassword.Text;
					UserServices.UpdateUser(updatingFor);
                    ClearChangePasswordFields();
                }
			}
		}

		private void ShowPasswordChangePanal()
		{
            groupBox1.Visible = true;
        }

        private void btnLevel1ChangePass_Click(object sender, EventArgs e)
        {
			if (user.Role == 1)
            {
                updatingFor = UserServices.GetUserByRole(1);
                lblPassword.Text = updatingFor.Username + " New Password";
                lbUserPassword.Text = user.Username + " Password";
                ShowPasswordChangePanal();
			}
			else
			{
				MessageBox.Show("Sorry you are current logged in a " + user.Username + ", but you need level 1 to be able to access this.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
        }

        private void btnLevel2ChangePass_Click(object sender, EventArgs e)
        {
            if (user.Role <= 2)
            {
                updatingFor = UserServices.GetUserByRole(2);
                lblPassword.Text = updatingFor.Username + " New Password";
                lbUserPassword.Text = user.Username + " Password";
                ShowPasswordChangePanal();
            }
            else
            {
                MessageBox.Show("Sorry you are current logged in a " + user.Username + ", but you need level 1 or 2 to be able to access this.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLevel3ChangePass_Click(object sender, EventArgs e)
        {
            if (user.Role <= 2)
            {
                updatingFor = UserServices.GetUserByRole(3);
                lblPassword.Text = updatingFor.Username + " New Password";
                lbUserPassword.Text = user.Username + " Password";
                ShowPasswordChangePanal();
            }
            else
            {
                MessageBox.Show("Sorry you are current logged in a " + user.Username + ", but you need level 1 or 2 to be able to access this.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
