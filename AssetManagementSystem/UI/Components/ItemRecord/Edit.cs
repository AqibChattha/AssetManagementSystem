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
        private string documentName;

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

            document = asset.MinuteSheetDocument;
            documentName = asset.MS_DocumentName;

            btn_Image.Text = "Update Image";
            btn_MSN_Docunent.Text = "Update Document";

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
            else
            {
                pbImage.Image = null;
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
            if (comments.Count < 1)
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


        private void btn_MSN_Docunent_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files|*.pdf|Word Documents|*.docx|Excel Worksheets|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Store the document bytes in the local property
                document = File.ReadAllBytes(dialog.FileName);
                documentName = dialog.SafeFileName;

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
            if (image == null)
            {
                return null;
            }
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
                        tbPrice.Text,
                        cmb_ConditionCategory.SelectedItem?.ToString(),
                        tbQuantity.Text,
                        tb_MSN.Text,
                        document,
                        rtb_Remarks.Text,
                        tbResponsibility.Text,
                        tbPlace.Text,
                        dtp_DOI.Value
                    ))
                {
                    MessageBox.Show("Asset updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MainForm.Instance.ShowUserControl(ViewAll.Instance);
                }
                else
                {
                    MessageBox.Show("Sorry there was an error updating the asset info, Please make sure you entered the correct information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool UpdateAsset(string name, string brand, string specifications, DateTime procurementDate,
            string colour, byte[] image, string price, string conditionCategory, string quantity, string minuteSheetNumber, byte[] minuteSheetDocument, string comments,
            string responsibleOfficial, string place, DateTime doi)
        {
            decimal dprice;
            int iquantity;
            // Perform validation checks
            if (string.IsNullOrWhiteSpace(name))
            {
                tbItemName.Focus();
                throw new ArgumentException("Please enter the item name.");
            }
            if (string.IsNullOrWhiteSpace(price))
            {
                tbPrice.Focus();
                throw new ArgumentException("Please enter the asset price.");
            }
            try
            {
                dprice = Convert.ToDecimal(price);
            }
            catch (Exception)
            {
                tbPrice.Focus();
                throw new ArgumentException("Price must be a number.");
            }
            if (dprice < 0)
            {
                tbPrice.Focus();
                throw new ArgumentException("Price must be a positive value.");
            }
            if (string.IsNullOrWhiteSpace(quantity))
            {
                tbQuantity.Focus();
                throw new ArgumentException("Please enter the asset quantity.");
            }
            try
            {
                iquantity = Convert.ToInt32(quantity);
            }
            catch (Exception)
            {
                tbQuantity.Focus();
                throw new ArgumentException("The Price field only accepts numbers (0-9) and a decimal (.) sign.");
            }
            if (iquantity < 0)
            {
                tbQuantity.Focus();
                throw new ArgumentException("Quantity must be a positive integer.");
            }
            if (string.IsNullOrWhiteSpace(brand))
            {
                tbBrand.Focus();
                throw new ArgumentException("Please enter the asset brand.");
            }
            if (string.IsNullOrWhiteSpace(conditionCategory))
            {
                cmb_ConditionCategory.Focus();
                throw new ArgumentException("Please enter the asset condition category.");
            }
            if (string.IsNullOrWhiteSpace(colour))
            {
                cmb_Color.Focus();
                throw new ArgumentException("Please select an asset color.");
            }
            if (string.IsNullOrWhiteSpace(specifications))
            {
                rtbSpecification.Focus();
                throw new ArgumentException("Please enter the asset specifications.");
            }
            if (string.IsNullOrWhiteSpace(minuteSheetNumber))
            {
                tb_MSN.Focus();
                throw new ArgumentException("Please enter the minute sheet number.");
            }
            if (string.IsNullOrWhiteSpace(responsibleOfficial))
            {
                tbResponsibility.Focus();
                throw new ArgumentException("Please enter the responsible official name ");
            }
            if (string.IsNullOrWhiteSpace(place))
            {
                tbPlace.Focus();
                throw new ArgumentException("Please enter the distribution place.");
            }

            assetData.Name = name;
            assetData.Brand = brand;
            assetData.Specifications = specifications;
            assetData.ProcurementDate = procurementDate;
            assetData.Colour = colour;
            assetData.Image = image;
            assetData.Price = dprice;
            assetData.ConditionCategory = conditionCategory;
            assetData.Quantity = iquantity;
            assetData.MinuteSheetNumber = minuteSheetNumber;
            assetData.MinuteSheetDocument = minuteSheetDocument;
            assetData.MS_DocumentName = documentName;
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

        private void cmb_ConditionCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_ConditionCategory.SelectedItem != null && cmb_ConditionCategory.SelectedItem.ToString().Equals("Unserviceable (US)"))
            {
                if (MessageBox.Show("Are you sure you want to mark this item as Unserviceable (US)? Doing so will delete this from the available items list and it will be moved to archived items.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    cmb_ConditionCategory.SelectedIndex = 0;
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlComments_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void rtbSpecification_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

        private void rtb_Remarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void cmb_Color_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void tbPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void tbBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void tbQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tbItemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void pbImage_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gnProcurement_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void tb_MSN_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void dtp_DOP_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void dtp_DOI_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void tbPlace_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void tbResponsibility_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox18_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbItemName_Click(object sender, EventArgs e)
        {

        }

        private void lbConditionCategory_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbItemNumber_Click(object sender, EventArgs e)
        {

        }
    }
}
