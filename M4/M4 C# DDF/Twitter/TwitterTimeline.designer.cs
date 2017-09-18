namespace M4
{
    partial class TwitterTimeline
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
          this.TextLabel = new System.Windows.Forms.Label();
          this.DateLabel = new System.Windows.Forms.Label();
          this.SuspendLayout();
          // 
          // TextLabel
          // 
          this.TextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.TextLabel.BackColor = System.Drawing.Color.White;
          this.TextLabel.Location = new System.Drawing.Point(8, 24);
          this.TextLabel.Name = "TextLabel";
          this.TextLabel.Size = new System.Drawing.Size(282, 38);
          this.TextLabel.TabIndex = 2;
          this.TextLabel.Text = "label1";
          // 
          // DateLabel
          // 
          this.DateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.DateLabel.BackColor = System.Drawing.Color.White;
          this.DateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
          this.DateLabel.Location = new System.Drawing.Point(6, 4);
          this.DateLabel.Name = "DateLabel";
          this.DateLabel.Size = new System.Drawing.Size(100, 15);
          this.DateLabel.TabIndex = 3;
          this.DateLabel.Text = "Date";
          // 
          // TwitterTimeline
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.White;
          this.Controls.Add(this.DateLabel);
          this.Controls.Add(this.TextLabel);
          this.Name = "TwitterTimeline";
          this.Size = new System.Drawing.Size(301, 62);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label TextLabel;
        private System.Windows.Forms.Label DateLabel;
    }
}
