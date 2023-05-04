using AssetManagementSystem.Models;
using AssetManagementSystem.Services.ModelServices;
using AssetManagementSystem.UI.Components.ItemRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AssetManagementSystem.UI.Components
{
	public partial class Archive : UserControl
    {

        private int currentPage;
        private readonly int PAGE_SIZE = 20;
        
        private static Archive _instance;
		public static Archive Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Archive();
				}
				return _instance;
			}
		}

		private Archive()
		{
			InitializeComponent();
            currentPage = 1;
            textBox1.Text = currentPage.ToString();
        }

        private void LoadAssetsData()
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                List<Asset> assets = AssetServices.GetAllAssets(currentPage, PAGE_SIZE, false);

                if (AssetServices.GetTotalPages(PAGE_SIZE, false) <= 1)
                {
                    panel2.Visible = false;
                }
                else
                {
                    panel2.Visible = true;
                }
                SetAssetCardInPanel(assets);
            }
            catch (Exception)
            {

            }
        }

        public void SetAssetCardInPanel(List<Asset> assets)
        {
            foreach (var item in assets)
            {
                ItemCard card = new ItemCard(item);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        override
        public void Refresh()
        {
            currentPage = 1;
            textBox1.Text = currentPage.ToString();
            LoadAssetsData();
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
        }

        public void RemoveAssetCard(ItemCard card)
        {
            flowLayoutPanel1.Controls.Remove(card);
        }
        
        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.Controls.Count > 0)
            {
                label3.Hide();
            }
            else
            {
                label3.Show();
            }
        }

        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control.Controls.Count > 0)
            {
                label3.Hide();
            }
            else
            {
                label3.Show();
            }
        }

        private void Archive_Load(object sender, EventArgs e)
        {
            LoadAssetsData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int totalPages = AssetServices.GetTotalPages(PAGE_SIZE, false);
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
                        if (cPage >= 1 && cPage <= AssetServices.GetTotalPages(PAGE_SIZE, false))
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
