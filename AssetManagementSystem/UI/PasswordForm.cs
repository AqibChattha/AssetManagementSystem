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
    public partial class PasswordForm : Form
    {
        User user;

        public PasswordForm(User forUser)
        {
            InitializeComponent();
            user = forUser;
            label1.Text = "Enter password for " + forUser.Username;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {            
            if (textBox1.Text.Equals(user.Password))
            {
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                MessageBox.Show("Incorrect Password, try again.", "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            this.Close();
        }
    }
}
