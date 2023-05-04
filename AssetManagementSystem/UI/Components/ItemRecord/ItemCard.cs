using AssetManagementSystem.Models;
using AssetManagementSystem.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class ItemCard : UserControl
    {
        private Asset _asset;

        public ItemCard(Asset asset)
        {
            InitializeComponent();
            _asset = asset;
            SetAssetDetails();
        }

        private void SetAssetDetails()
        {
            lbItemName.Text = _asset.Name;
            lbQuantity.Text = "#" + _asset.Quantity.ToString();
            lbConditionCategory.Text = _asset.ConditionCategory;
            if (_asset.ConditionCategory.Equals("Partially serviceable (PWS)"))
            {
                lbConditionCategory.BackColor = Color.FromArgb(255, 224, 192);
                lbConditionCategory.ForeColor = Color.FromArgb(255, 128, 0);
            } 
            else if (_asset.ConditionCategory.Equals("Unserviceable (US)"))
            {
                lbConditionCategory.BackColor = Color.FromArgb(255, 192, 192);
                lbConditionCategory.ForeColor = Color.Red;
            }
            else
            {
                lbConditionCategory.BackColor = Color.FromArgb(194, 240, 194);
                lbConditionCategory.ForeColor = Color.FromArgb(35, 144, 35);
            }
            lbComments.Text = _asset.Specifications;
            if (_asset.Image != null)
            {
                pbImage.Image = Image.FromStream(new MemoryStream(_asset.Image));
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Details.Instance.Refresh(_asset);
                MainForm.Instance.ShowUserControl(Details.Instance);
            }
            catch (Exception)
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MainForm.Instance.LoggedInUser.Role == 3)
            {
                PasswordForm level2Password = new PasswordForm(UserServices.GetUserByRole(2));
                if (level2Password.ShowDialog() == DialogResult.Yes)
                {
                    PasswordForm level1Password = new PasswordForm(UserServices.GetUserByRole(1));
                    if (level1Password.ShowDialog() == DialogResult.Yes)
                    {
                        MessageBox.Show("The asset has been deleted successfully.", "Delete Asset", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        try
                        {
                            AssetServices.DeleteAsset(_asset.Id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            //if (_asset.ConditionCategory == "Unserviceable (US)")
            //{
            //    DialogResult result = MessageBox.Show("Are you sure you want to permanently delete this assest?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (result == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            AssetServices.DeleteAsset(_asset.Id);
            //            Archive.Instance.RemoveAssetCard(this);
            //        }
            //        catch (Exception)
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    DialogResult result = MessageBox.Show("Do you want to save the asset to archive?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
            //    if (result == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            _asset.ConditionCategory = "Unserviceable (US)";
            //            AssetServices.UpdateAsset(_asset);
            //            ViewAll.Instance.RemoveAssetCard(this);
            //        }
            //        catch (Exception)
            //        {

            //        }
            //    }
            //    else if (result == DialogResult.No)
            //    {
            //        DialogResult result2 = MessageBox.Show("Are you sure you want to permanently delete this assest?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //        if (result2 == DialogResult.Yes)
            //        {
            //            try
            //            {
            //                AssetServices.DeleteAsset(_asset.Id);
            //                ViewAll.Instance.RemoveAssetCard(this);
            //            }
            //            catch (Exception)
            //            {

            //            }
            //        }
            //    }
            //}
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Edit.Instance.Refresh(_asset);
                MainForm.Instance.ShowUserControl(Edit.Instance);
            }
            catch (Exception)
            {

            }
        }
    }
}
