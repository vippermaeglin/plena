namespace M4.M4v2.TradeDiary
{
    public partial class TradeDiaryView 
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeDiaryView));
            this.imageList32Size = new System.Windows.Forms.ImageList(this.components);
            this.office2010BlueTheme1 = new Telerik.WinControls.Themes.Office2010BlueTheme();
            this.radSpellChecker1 = new Telerik.WinControls.UI.RadSpellChecker();
            this.radButtonElement1 = new Telerik.WinControls.UI.RadButtonElement();
            this.BnAddPost = new Telerik.WinControls.UI.RadButton();
            this.radRichTextBox1 = new Telerik.WinControls.RichTextBox.RadRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BnAddPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextBox1)).BeginInit();
            this.radRichTextBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList32Size
            // 
            this.imageList32Size.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32Size.ImageStream")));
            this.imageList32Size.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList32Size.Images.SetKeyName(0, "Hyperlink");
            this.imageList32Size.Images.SetKeyName(1, "PageBreak");
            this.imageList32Size.Images.SetKeyName(2, "Picture");
            this.imageList32Size.Images.SetKeyName(3, "PageMargin");
            this.imageList32Size.Images.SetKeyName(4, "PageOrientation");
            this.imageList32Size.Images.SetKeyName(5, "PageSize");
            this.imageList32Size.Images.SetKeyName(6, "WebLayout");
            this.imageList32Size.Images.SetKeyName(7, "SpellCheck");
            this.imageList32Size.Images.SetKeyName(8, "PageOrientationLandscape.png");
            this.imageList32Size.Images.SetKeyName(9, "PageOrientationPortrait.png");
            this.imageList32Size.Images.SetKeyName(10, "PageOrientationRotate180.png");
            this.imageList32Size.Images.SetKeyName(11, "PageOrientationRotate270.png");
            this.imageList32Size.Images.SetKeyName(12, "new.png");
            this.imageList32Size.Images.SetKeyName(13, "save.png");
            this.imageList32Size.Images.SetKeyName(14, "saveas.png");
            this.imageList32Size.Images.SetKeyName(15, "Html.png");
            this.imageList32Size.Images.SetKeyName(16, "Pdf.png");
            this.imageList32Size.Images.SetKeyName(17, "PlainText.png");
            this.imageList32Size.Images.SetKeyName(18, "Rtf.png");
            this.imageList32Size.Images.SetKeyName(19, "worddoc.png");
            this.imageList32Size.Images.SetKeyName(20, "Xaml.png");
            this.imageList32Size.Images.SetKeyName(21, "open.png");
            this.imageList32Size.Images.SetKeyName(22, "exit.png");
            this.imageList32Size.Images.SetKeyName(23, "Table.png");
            // 
            // radButtonElement1
            // 
            this.radButtonElement1.AccessibleDescription = "Picture";
            this.radButtonElement1.AccessibleName = "Picture";
            this.radButtonElement1.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.radButtonElement1.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radButtonElement1.Class = "RibbonBarButtonElement";
            this.radButtonElement1.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentContent;
            this.radButtonElement1.Image = ((System.Drawing.Image)(resources.GetObject("radButtonElement1.Image")));
            this.radButtonElement1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement1.ImageKey = "Picture";
            this.radButtonElement1.Name = "radButtonElement1";
            this.radButtonElement1.Text = "Picture";
            this.radButtonElement1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radButtonElement1.TextWrap = true;
            this.radButtonElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // BnAddPost
            // 
            this.BnAddPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BnAddPost.Image = global::M4.Properties.Resources.new_small_icon;
            this.BnAddPost.Location = new System.Drawing.Point(685, 516);
            this.BnAddPost.Name = "BnAddPost";
            // 
            // 
            // 
            this.BnAddPost.RootElement.Padding = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.BnAddPost.Size = new System.Drawing.Size(89, 44);
            this.BnAddPost.TabIndex = 0;
            this.BnAddPost.Text = "New";
            this.BnAddPost.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BnAddPost.Click += new System.EventHandler(this.BnAddPost_Click);
            // 
            // radRichTextBox1
            // 
            this.radRichTextBox1.Controls.Add(this.BnAddPost);
            this.radRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radRichTextBox1.HyperlinkNavigationMode = Telerik.WinControls.RichTextBox.HyperlinkNavigationMode.Click;
            this.radRichTextBox1.IsReadOnly = true;
            this.radRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.radRichTextBox1.Name = "radRichTextBox1";
            this.radRichTextBox1.Size = new System.Drawing.Size(793, 563);
            this.radRichTextBox1.TabIndex = 0;
            this.radRichTextBox1.Text = "radRichTextBox1";
            this.radRichTextBox1.Click += new System.EventHandler(this.radRichTextBox1_Click);
            this.radRichTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.radRichTextBox1_KeyDown);
            // 
            // TradeDiaryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radRichTextBox1);
            this.Name = "TradeDiaryView";
            this.Size = new System.Drawing.Size(793, 563);
            ((System.ComponentModel.ISupportInitialize)(this.BnAddPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextBox1)).EndInit();
            this.radRichTextBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ImageList imageList32Size;
        private Telerik.WinControls.UI.RadSpellChecker radSpellChecker1;
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
        private Telerik.WinControls.UI.RadButtonElement radButtonElement1;
        private Telerik.WinControls.UI.RadButton BnAddPost;
        private Telerik.WinControls.RichTextBox.RadRichTextBox radRichTextBox1;
	}
}

