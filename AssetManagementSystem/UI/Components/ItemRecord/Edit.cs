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
using System.Xml.Linq;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class Edit : UserControl
    {
        private Asset assetData;
        private byte[] document;

        private static Edit _instance;
        public static Edit Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Edit();
                return _instance;
            }
        }

        private Edit()
        {
            InitializeComponent();
        }
        
        public void Refresh(Asset asset)
        {
            label1.Visible = true;
            lbItemName.Text = asset.Name;
            lbConditionCategory.Text = asset.ConditionCategory;
            lbItemNumber.Text = "Item # " + asset.Id;

            tbItemNo.Text = asset.Id.ToString();
            assetData = asset;
            tbItemName.Text = asset.Name;
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
                pbImage.Image = Image.FromStream(new MemoryStream(asset.Image));
            }
            tbPrice.Text = asset.Price.ToString();
            foreach (var item in cmb_ConditionCategory.Items)
            {
                if (item.ToString().Equals(asset.ConditionCategory))
                {
                    cmb_ConditionCategory.SelectedItem = item;
                }
            }
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
            if (comments.Count < 10)
            {
                label1.Visible = false;
            }
            else
            {
                foreach (var item in comments)
                {
                    PreviousRemark previousRemark = new PreviousRemark(item);
                    pnlComments.Controls.Add(previousRemark);
                    previousRemark.Dock = DockStyle.Top;
                }
            }
        }


        private void btn_MSN_Docunent_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files|*.pdf|Word Documents|*.docx|Excel Worksheets|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Store the document bytes in the local property
                document = File.ReadAllBytes(dialog.FileName);

                // Change the button text to indicate that a file has been selected
                btn_MSN_Docunent.Text = Path.GetFileName(dialog.FileName);
            }
        }

        private void btn_Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image image = Image.FromFile(openFileDialog.FileName);
                    pbImage.Image = image;

                    // Change the button text to indicate that a file has been selected
                    btn_Image.Text = Path.GetFileName(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }

        public byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateAsset(
                        tbItemName.Text,
                        tbBrand.Text,
                        rtbSpecification.Text,
                        dtp_DOP.Value,
                        cmb_Color.SelectedItem?.ToString(),
                        ImageToByteArray(pbImage.Image),
                        Convert.ToDecimal(tbPrice.Text),
                        cmb_ConditionCategory.SelectedItem?.ToString(),
                        Convert.ToInt32(tbQuantity.Text),
                        tb_MSN.SelectedText,
                        document,
                        rtb_Remarks.Text,
                        tbResponsibility.Text,
                        tbPlace.Text,
                        dtp_DOI.Value
                    ))
                {
                    MainForm.Instance.ShowUserControl(ViewAll.Instance);
                }
                else
                {
                    MessageBox.Show("Sorry there was an error updating the asset info, Please make sure you entered the correct information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("There was an error performing the task, Please make sure you entered the correct information. If this still doesn't fix your issue try restarting the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool UpdateAsset(string name, string brand, string specifications, DateTime procurementDate,
            string colour, byte[] image, decimal price, string conditionCategory, int quantity, string minuteSheetNumber, byte[] minuteSheetDocument, string comments,
            string responsibleOfficial, string place, DateTime doi)
        {
            // Perform validation checks
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(conditionCategory))
                throw new ArgumentException("Condition category cannot be null or empty.");
            if (quantity < 0)
                throw new ArgumentException("Quantity must be a positive integer.");
            if (string.IsNullOrWhiteSpace(responsibleOfficial))
                throw new ArgumentException("Responsible official name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(place))
                throw new ArgumentException("Place cannot be null or empty.");

            assetData.Name = name;
            assetData.Brand = brand;
            assetData.Specifications = specifications;
            assetData.ProcurementDate = procurementDate;
            assetData.Colour = colour;
            assetData.Image = image;
            assetData.Price = price;
            assetData.ConditionCategory = conditionCategory;
            assetData.Quantity = quantity;
            assetData.MinuteSheetNumber = minuteSheetNumber;
            assetData.MinuteSheetDocument = minuteSheetDocument;
            if (!assetData.Comments.Equals(comments))
            {
                PreviousComments comment = new PreviousComments
                {
                    AssetId = assetData.Id,
                    commentTimeStamp = DateTime.Now,
                    comments = comments
                };
                CommentsServices.AddComment(comment);
                assetData.Comments = comments;
            }
            assetData.Distribution.Responsibility = responsibleOfficial;
            assetData.Distribution.Place = place;
            assetData.Distribution.IssueDate = doi;
            return AssetServices.UpdateAsset(assetData);
        }

        private void btnCancel_Click(object sender, EventArgs e)
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
