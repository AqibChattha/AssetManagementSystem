using AssetManagementSystem.Models;
using AssetManagementSystem.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class Details : UserControl
    {
        private Asset _asset;

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
            _asset = asset;

            lbItemName.Text = asset.Name;
            lbConditionCategory.Text = asset.ConditionCategory;
            lbItemNumber.Text = "Item # " + asset.Id;

            tbBrand.Text = asset.Brand;
            rtbSpecification.Text = asset.Specifications;
            dtp_DOP.Value = asset.ProcurementDate;
            if (asset.Image != null)
            {
                pbImage.Image = System.Drawing.Image.FromStream(new MemoryStream(asset.Image));
            }
            tbPrice.Text = asset.Price.ToString();
            tbQuantity.Text = asset.Quantity.ToString();
            tb_MSN.Text = asset.MinuteSheetNumber;
            rtb_Remarks.Text = asset.Comments;
            cmb_Color.Text = asset.Colour;

            tbResponsibility.Text = asset.Distribution.Responsibility;
            tbPlace.Text = asset.Distribution.Place;
            dtp_DOI.Value = asset.Distribution.IssueDate;

            LoadComments(CommentsServices.GetAssetComments(asset.Id));
        }

        private void LoadComments(List<PreviousComments> comments)
        {
            pnlComments.Controls.Clear();
            if (comments.Count <= 0)
            {
                panel6.Visible = true;
            }
            else
            {
                panel6.Visible = false;
                foreach (var item in comments)
                {
                    PreviousRemark previousRemark = new PreviousRemark(item);
                    pnlComments.Controls.Add(previousRemark);
                    previousRemark.Dock = DockStyle.Top;
                }
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

        private void btn_MSN_Docunent_Click(object sender, EventArgs e)
        {
            if (_asset.MinuteSheetDocument != null)
            {
                //using (var dialog = new FolderBrowserDialog())
                //{
                //    DialogResult result = dialog.ShowDialog();
                //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                //    {
                        try
                        {
                            //string path = Path.Combine(dialog.SelectedPath, _asset.MS_DocumentName);

                            string extension = Path.GetExtension(_asset.MS_DocumentName);

                            string newFile = "ViewDocument" + extension;
                            
                            File.WriteAllBytes(newFile, _asset.MinuteSheetDocument);

                            Process.Start(newFile);
                        }
                        catch (Exception)
                        {

                        }
                //        }
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("No document to view.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
