using AssetManagementSystem.Models;
using AssetManagementSystem.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class ViewAll : UserControl
    {

        private int currentPage;
        private readonly int PAGE_SIZE = 20;

        // Singleton pattern to ensure only one instance of this class is instantiated.
        private static ViewAll _instance;
        public static ViewAll Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ViewAll();
                return _instance;
            }
        }

        private ViewAll()
        {
            InitializeComponent();
            currentPage = 1;
        }

        private void LoadAssetsData()
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                List<Asset> assets = AssetServices.GetAllAssets(currentPage, PAGE_SIZE, true);
                SetAssetCardInPanel(assets);
                if (AssetServices.GetTotalPages(PAGE_SIZE, true) <= 1)
                {
                    panel2.Visible = false;
                }
                else
                {
                    panel2.Visible = true;
                }
            }
            catch (Exception)
            {

            }
        }

        public void SetAssetCardInPanel(List<Asset> assets)
        {
            flowLayoutPanel1.Controls.Clear();
            if (assets.Count < 1)
            {
                flowLayoutPanel1.Visible = false;
            }
            else
            {
                flowLayoutPanel1.Visible = true;
                foreach (var item in assets)
                {
                    ItemCard card = new ItemCard(item);
                    flowLayoutPanel1.Controls.Add(card);
                }
            }
        }

        override
        public void Refresh()
        {
            currentPage = 1;
            LoadAssetsData();
            textBox1.Text = currentPage.ToString();
            textBox1.Text = currentPage.ToString();
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
        }

        public void RemoveAssetCard(ItemCard card)
        {
            flowLayoutPanel1.Controls.Remove(card);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.Instance.ShowUserControl(Add.Instance);
            }
            catch (Exception)
            {

            }
        }

        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.Controls.Count > 0)
            {
                label2.Hide();
            }
            else
            {
                label2.Show();
            }
        }

        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control.Controls.Count > 0)
            {
                label2.Hide();
            }
            else
            {
                label2.Show();
            }
        }

        private void ViewAll_Load(object sender, EventArgs e)
        {
            LoadAssetsData();
        }

        private void ViewAll_SizeChanged(object sender, EventArgs e)
        {
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int totalPages = AssetServices.GetTotalPages(PAGE_SIZE, true);
            if (currentPage < totalPages)
            {
                currentPage++;
                if (currentPage >= totalPages)
                {
                    currentPage = totalPages;
                    btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                }
                textBox1.Text = currentPage.ToString();
                LoadAssetsData();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    try
                    {
                        int cPage = Convert.ToInt32(textBox1.Text);
                        if (cPage >= 1 && cPage <= AssetServices.GetTotalPages(PAGE_SIZE, true))
                        {
                            currentPage = cPage;
                            LoadAssetsData();
                            btnNext.Enabled = true;
                            btnPrev.Enabled = true;
                        }
                        else
                        {
                            textBox1.Text = currentPage.ToString();
                            btnNext.Enabled = true;
                            btnPrev.Enabled = true;
                        }
                    }
                    catch (Exception)
                    {
                        textBox1.Text = currentPage.ToString();
                    }
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                textBox1.Text = currentPage.ToString();
                if (currentPage <= 1)
                {
                    currentPage = 1;
                    btnPrev.Enabled = false;
                }
                else
                {
                    btnNext.Enabled = true;
                }
                textBox1.Text = currentPage.ToString();
                LoadAssetsData();
            }
        }
    }
}
