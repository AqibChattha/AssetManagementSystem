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

namespace AssetManagementSystem.UI
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            cmbUserName.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter the password and try again", "Enter Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<User> users = UserServices.GetAllUsers();
            var user = users.Where(u => u.Username == cmbUserName.SelectedItem.ToString() && u.Password == txtPassword.Text).FirstOrDefault();
            if (user != null)
            {
                MainForm main = new MainForm(user);
                this.Hide();
                if (main.ShowDialog() == DialogResult.Retry)
                {
                    ClearFields();
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid password try again", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            cmbUserName.SelectedIndex = 0;
            txtPassword.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !cbShowPassword.Checked;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }
    }
}
