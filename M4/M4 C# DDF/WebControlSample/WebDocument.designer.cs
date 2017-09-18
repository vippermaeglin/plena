namespace M4.WebControlSample
{
    partial class WebDocument
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebDocument));
            this.pageToolStrip = new System.Windows.Forms.ToolStrip();
            this.pageImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.addressBox = new Awesomium.Windows.Forms.ToolStripAddressBox();
            this.webControl = new Awesomium.Windows.Forms.WebControl();
            this.pageToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageToolStrip
            // 
            this.pageToolStrip.CanOverflow = false;
            this.pageToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.pageToolStrip.ImageList = this.pageImageList;
            this.pageToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.addressBox});
            this.pageToolStrip.Location = new System.Drawing.Point(0, 0);
            this.pageToolStrip.Name = "pageToolStrip";
            this.pageToolStrip.Size = new System.Drawing.Size(474, 25);
            this.pageToolStrip.Stretch = true;
            this.pageToolStrip.TabIndex = 1;
            // 
            // pageImageList
            // 
            this.pageImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("pageImageList.ImageStream")));
            this.pageImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.pageImageList.Images.SetKeyName(0, "development 23 grey.png");
            this.pageImageList.Images.SetKeyName(1, "development 23.png");
            this.pageImageList.Images.SetKeyName(2, "development 21 grey.png");
            this.pageImageList.Images.SetKeyName(3, "development 21.png");
            this.pageImageList.Images.SetKeyName(4, "development 39 grey.png");
            this.pageImageList.Images.SetKeyName(5, "development 39.png");
            this.pageImageList.Images.SetKeyName(6, "development 20 grey.png");
            this.pageImageList.Images.SetKeyName(7, "development 20.png");
            this.pageImageList.Images.SetKeyName(8, "development 53 grey.png");
            this.pageImageList.Images.SetKeyName(9, "development 53.png");
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.ImageIndex = 0;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "&Back";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            this.toolStripButton1.MouseEnter += new System.EventHandler(this.toolStripButton1_MouseEnter);
            this.toolStripButton1.MouseLeave += new System.EventHandler(this.toolStripButton1_MouseLeave);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.ImageIndex = 2;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "&Forward";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            this.toolStripButton2.MouseEnter += new System.EventHandler(this.toolStripButton1_MouseEnter);
            this.toolStripButton2.MouseLeave += new System.EventHandler(this.toolStripButton1_MouseLeave);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.ImageIndex = 4;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "&Refresh";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            this.toolStripButton3.MouseEnter += new System.EventHandler(this.toolStripButton1_MouseEnter);
            this.toolStripButton3.MouseLeave += new System.EventHandler(this.toolStripButton1_MouseLeave);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.ImageIndex = 6;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "&Home";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            this.toolStripButton4.MouseEnter += new System.EventHandler(this.toolStripButton1_MouseEnter);
            this.toolStripButton4.MouseLeave += new System.EventHandler(this.toolStripButton1_MouseLeave);
            // 
            // addressBox
            // 
            this.addressBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.addressBox.Name = "addressBox";
            this.addressBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.addressBox.Size = new System.Drawing.Size(348, 25);
            this.addressBox.URL = null;
            this.addressBox.WebControl = this.webControl;
            // 
            // webControl
            // 
            this.webControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webControl.Enabled = false;
            this.webControl.Location = new System.Drawing.Point(0, 25);
            this.webControl.Name = "webControl";
            this.webControl.SelfUpdate = true;
            this.webControl.Size = new System.Drawing.Size(474, 377);
            this.webControl.TabIndex = 2;
            this.webControl.BeginLoading += new Awesomium.Core.BeginLoadingEventHandler(this.webControl_BeginLoading);
            this.webControl.TargetUrlChanged += new Awesomium.Core.UrlEventHandler(this.webControl_TargetUrlChanged);
            this.webControl.DomReady += new System.EventHandler(this.webControl_DomReady);
            this.webControl.OpenExternalLink += new Awesomium.Core.OpenExternalLinkEventHandler(this.webControl_OpenExternalLink);
            this.webControl.Download += new Awesomium.Core.UrlEventHandler(this.webControl_Download);
            this.webControl.SelectLocalFiles += new Awesomium.Core.SelectLocalFilesEventHandler(this.webControl_SelectLocalFiles);
            this.webControl.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.webControl_PropertyChanged);
            // 
            // WebDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 402);
            this.Controls.Add(this.webControl);
            this.Controls.Add(this.pageToolStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebDocument";
            this.Text = "Loading...";
            this.pageToolStrip.ResumeLayout(false);
            this.pageToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip pageToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private Awesomium.Windows.Forms.ToolStripAddressBox addressBox;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private Awesomium.Windows.Forms.WebControl webControl;
        private System.Windows.Forms.ImageList pageImageList;

    }
}