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
            pbImage.Image = null;
            tbPrice.Text = "";
            cmb_ConditionCategory.SelectedIndex = 0;
            tbQuantity.Text = "";
            tb_MSN.Text = "";
            rtb_Remarks.Text = "";
            btn_MSN_Docunent.Text = "Upload Document";
            btn_Image.Text = "Upload Image";
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
                if (AddUpdateAsset(
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
                    ClearAllFields();
                    MainForm.Instance.ShowUserControl(ViewAll.Instance);
                }
                else
                {
                    MessageBox.Show("Sorry there was an error performing the task, Please make sure you entered the correct information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public bool AddUpdateAsset(string name, string brand, string specifications, DateTime procurementDate,
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
                Price = price,
                ConditionCategory = conditionCategory,
                Quantity = quantity,
                MinuteSheetNumber = minuteSheetNumber,
                MinuteSheetDocument = minuteSheetDocument,
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
    }
}
