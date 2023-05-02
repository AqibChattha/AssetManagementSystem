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
            if (MessageBox.Show("Are you sure you want to delete this assest?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    AssetServices.DeleteAsset(_asset.Id);
                    ViewAll.Instance.RemoveAssetCard(this);
                }
                catch (Exception)
                {

                }
            }
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
