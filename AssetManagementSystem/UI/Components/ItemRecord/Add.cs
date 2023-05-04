using AssetManagementSystem.Models;
using AssetManagementSystem.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class Add : UserControl
    {
        private Asset assetData;
        private byte[] document;
        private string documentName;

        // Singleton pattern to ensure only one instance of this class is instantiated.
        private static Add _instance;
        public static Add Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Add();
                return _instance;
            }
        }

        private Add()
        {
            InitializeComponent();

        }
        private void ClearAllFields()
        {
            tbItemName.Text = "";
            tbBrand.Text = "";
            rtbSpecification.Text = "";
            dtp_DOP.Value = DateTime.Now;
            cmb_Color.SelectedIndex = 0;
            cmb_ConditionCategory.SelectedIndex = 0;

            pbImage.Image = null;
            tbPrice.Text = "";
            cmb_ConditionCategory.SelectedIndex = 0;
            tbQuantity.Text = "";
            tb_MSN.Text = "";

            rtb_Remarks.Text = "";
            btn_MSN_Docunent.Text = "Upload Document";
            btn_Image.Text = "Upload Image";
            documentName = "";
            document = null;

            tbResponsibility.Text = "";
            tbPlace.Text = "";
            dtp_DOI.Value = DateTime.Now;
        }

        override
        public void Refresh()
        {
            ClearAllFields();
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

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {


                if (AddAsset(
                        tbItemName.Text,
                        tbBrand.Text,
                        rtbSpecification.Text,
                        dtp_DOP.Value,
                        cmb_Color.Text.ToString(),
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
                    MessageBox.Show("Asset added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ClearAllFields();
                    MainForm.Instance.ShowUserControl(ViewAll.Instance);
                }
                else
                {
                    MessageBox.Show("Please enter input in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool AddAsset(string name, string brand, string specifications, DateTime procurementDate,
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
            

            var distribution = new Distribution()
            {
                Responsibility = responsibleOfficial,
                Place = place,
                IssueDate = doi
            };
            var asset = new Asset()
            {
                Name = name,
                Brand = brand,
                Specifications = specifications,
                ProcurementDate = procurementDate,
                Colour = colour,
                Image = image,
                Price = dprice,
                ConditionCategory = conditionCategory,
                Quantity = iquantity,
                MinuteSheetNumber = minuteSheetNumber,
                MinuteSheetDocument = minuteSheetDocument,
                MS_DocumentName = documentName,
                Comments = comments,
                Distribution = distribution
            };
            return AssetServices.AddAsset(asset);
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

        private void dtp_DOP_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmb_ConditionCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_ConditionCategory.SelectedItem != null && cmb_ConditionCategory.SelectedItem.ToString().Equals("Unserviceable (US)"))
            {
                MessageBox.Show("The asset will be added to the archive by selecting the Unserviceable (US) option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
