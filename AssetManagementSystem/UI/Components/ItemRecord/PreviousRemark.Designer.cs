namespace AssetManagementSystem.UI.Components.ItemRecord
{
    partial class PreviousRemark
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbTimeStamp = new System.Windows.Forms.GroupBox();
            this.lbComment = new System.Windows.Forms.Label();
            this.gbTimeStamp.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTimeStamp
            // 
            this.gbTimeStamp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTimeStamp.Controls.Add(this.lbComment);
            this.gbTimeStamp.Location = new System.Drawing.Point(6, 5);
            this.gbTimeStamp.Name = "gbTimeStamp";
            this.gbTimeStamp.Size = new System.Drawing.Size(466, 60);
            this.gbTimeStamp.TabIndex = 0;
            this.gbTimeStamp.TabStop = false;
            this.gbTimeStamp.Text = "Time";
            // 
            // lbComment
            // 
            this.lbComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbComment.Location = new System.Drawing.Point(14, 18);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(438, 30);
            this.lbComment.TabIndex = 0;
            this.lbComment.Text = "comment";
            // 
            // PreviousRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTimeStamp);
            this.Name = "PreviousRemark";
            this.Size = new System.Drawing.Size(479, 73);
            this.gbTimeStamp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTimeStamp;
        private System.Windows.Forms.Label lbComment;
    }
}
