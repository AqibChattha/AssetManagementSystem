using AssetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManagementSystem.UI.Components.ItemRecord
{
    public partial class PreviousRemark : UserControl
    {
        private PreviousComments _comment;

        public PreviousRemark(PreviousComments comment)
        {
            InitializeComponent();
            gbTimeStamp.Text = comment.commentTimeStamp.ToString("dd/MM/yyyy h:m:tt");
            lbComment.Text = comment.comments;
        }
    }
}
