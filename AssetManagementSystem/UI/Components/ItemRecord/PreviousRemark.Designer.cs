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
            this.gbTimeStamp.Location = new System.Drawing.Point(8, 6);
            this.gbTimeStamp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbTimeStamp.Name = "gbTimeStamp";
            this.gbTimeStamp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbTimeStamp.Size = new System.Drawing.Size(621, 74);
            this.gbTimeStamp.TabIndex = 0;
            this.gbTimeStamp.TabStop = false;
            this.gbTimeStamp.Text = "Time";
            // 
            // lbComment
            // 
            this.lbComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbComment.Location = new System.Drawing.Point(19, 22);
            this.lbComment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(584, 37);
            this.lbComment.TabIndex = 0;
            this.lbComment.Text = "comment";
            // 
            // PreviousRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTimeStamp);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PreviousRemark";
            this.Size = new System.Drawing.Size(639, 90);
            this.gbTimeStamp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTimeStamp;
        private System.Windows.Forms.Label lbComment;
    }
}
