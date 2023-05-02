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
using static System.Net.Mime.MediaTypeNames;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class Details : UserControl
    {

        private static Details _instance;
        public static Details Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Details();
                return _instance;
            }
        }

        private Details()
        {
            InitializeComponent();
        }
        
        public void Refresh(Asset asset)
        {
            lbItemName.Text = asset.Name;
            lbConditionCategory.Text = asset.ConditionCategory;
            lbItemNumber.Text = "Item # " + asset.Id;

            tbBrand.Text = asset.Brand;
            rtbSpecification.Text = asset.Specifications;
            dtp_DOP.Value = asset.ProcurementDate;
            foreach (var item in cmb_Color.Items)
            {
                if (item.ToString().Equals(asset.Colour))
                {
                    cmb_Color.SelectedItem = item;
                }
            }
            if (asset.Image != null)
            {
                pbImage.Image = System.Drawing.Image.FromStream(new MemoryStream(asset.Image));
            }
            tbPrice.Text = asset.Price.ToString();
            tbQuantity.Text = asset.Quantity.ToString();
            tb_MSN.Text = asset.MinuteSheetNumber;
            rtb_Remarks.Text = asset.Comments;

            tbResponsibility.Text = asset.Distribution.Responsibility;
            tbPlace.Text = asset.Distribution.Place;
            dtp_DOI.Value = asset.Distribution.IssueDate;

            LoadComments(CommentsServices.GetAssetComments(asset.Id));
        }

        private void LoadComments(List<PreviousComments> comments)
        {
            pnlComments.Controls.Clear();
            foreach (var item in comments)
            {
                PreviousRemark previousRemark = new PreviousRemark(item);
                pnlComments.Controls.Add(previousRemark);
                previousRemark.Dock = DockStyle.Top;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.Instance.ShowUserControl(ViewAll.Instance);
            }
            catch (Exception)
            {

            }
        }
    }
}
