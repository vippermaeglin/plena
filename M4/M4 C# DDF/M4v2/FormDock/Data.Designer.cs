namespace M4.M4v2.FormDock
{
    partial class Data
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Data));
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.nTabControl = new Nevron.UI.WinForm.Controls.NTabControl();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement2 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.txtNameTab = new Telerik.WinControls.UI.CommandBarTextBox();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.btnNewWallet = new Telerik.WinControls.UI.CommandBarButton();
            this.btnDeleteWallet = new Telerik.WinControls.UI.CommandBarButton();
            this.btnGraphics = new Telerik.WinControls.UI.CommandBarButton();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarStripElement1.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.FloatingForm = null;
            this.commandBarStripElement1.Text = "";
            // 
            // nTabControl
            // 
            this.nTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nTabControl.Location = new System.Drawing.Point(0, 32);
            this.nTabControl.Name = "nTabControl";
            this.nTabControl.Padding = new System.Windows.Forms.Padding(4, 4, 4, 24);
            this.nTabControl.Selectable = true;
            this.nTabControl.SelectedIndex = -1;
            this.nTabControl.Size = new System.Drawing.Size(617, 231);
            this.nTabControl.TabAlign = Nevron.UI.WinForm.Controls.TabAlign.Bottom;
            this.nTabControl.TabIndex = 2;
            this.nTabControl.SelectedTabChanged += new System.EventHandler(this.NTabControlSelectedTabChanged);
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.commandBarRowElement1.AutoSize = true;
            this.commandBarRowElement1.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarRowElement1.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarRowElement1.DisplayName = null;
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Padding = new System.Windows.Forms.Padding(0);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement2});
            this.commandBarRowElement1.Text = "";
            this.commandBarRowElement1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // commandBarStripElement2
            // 
            this.commandBarStripElement2.AutoSize = true;
            this.commandBarStripElement2.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarStripElement2.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarStripElement2.DisplayName = "commandBarStripElement2";
            this.commandBarStripElement2.FloatingForm = null;
            this.commandBarStripElement2.GradientAngle = 90F;
            this.commandBarStripElement2.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.txtNameTab,
            this.commandBarSeparator1,
            this.btnNewWallet,
            this.btnDeleteWallet,
            this.btnGraphics});
            this.commandBarStripElement2.Name = "commandBarStripElement2";
            this.commandBarStripElement2.ShowHorizontalLine = false;
            this.commandBarStripElement2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            this.commandBarStripElement2.StretchHorizontally = false;
            this.commandBarStripElement2.StretchVertically = true;
            this.commandBarStripElement2.Text = "";
            // 
            // txtNameTab
            // 
            this.txtNameTab.AutoSize = false;
            this.txtNameTab.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.txtNameTab.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.txtNameTab.Bounds = new System.Drawing.Rectangle(0, 0, 100, 28);
            this.txtNameTab.DisplayName = "commandBarTextBox1";
            this.txtNameTab.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtNameTab.MaxSize = new System.Drawing.Size(0, 0);
            this.txtNameTab.Name = "txtNameTab";
            this.txtNameTab.StretchHorizontally = false;
            this.txtNameTab.StretchVertically = true;
            this.txtNameTab.Text = "";
            this.txtNameTab.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.txtNameTab.VisibleInOverflowMenu = true;
            this.txtNameTab.TextChanging += new Telerik.WinControls.TextChangingEventHandler(this.TxtNameTabTextChanging);
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.AccessibleDescription = "commandBarSeparator1";
            this.commandBarSeparator1.AccessibleName = "commandBarSeparator1";
            this.commandBarSeparator1.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.commandBarSeparator1.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarSeparator1.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.HorizontalLineWidth = 1;
            this.commandBarSeparator1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.commandBarSeparator1.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // btnNewWallet
            // 
            this.btnNewWallet.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNewWallet.AutoSize = true;
            this.btnNewWallet.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.btnNewWallet.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.btnNewWallet.DisplayName = "commandBarButton1";
            this.btnNewWallet.Image = global::M4.Properties.Resources.tab;
            this.btnNewWallet.Name = "btnNewWallet";
            this.btnNewWallet.Padding = new System.Windows.Forms.Padding(4);
            this.btnNewWallet.StretchHorizontally = false;
            this.btnNewWallet.Text = "";
            this.btnNewWallet.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.btnNewWallet.VisibleInOverflowMenu = true;
            this.btnNewWallet.Click += new System.EventHandler(this.BtnNewWalletClick);
            // 
            // btnDeleteWallet
            // 
            this.btnDeleteWallet.AutoSize = true;
            this.btnDeleteWallet.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.btnDeleteWallet.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.btnDeleteWallet.DisplayName = "";
            this.btnDeleteWallet.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteWallet.Image")));
            this.btnDeleteWallet.Name = "btnDeleteWallet";
            this.btnDeleteWallet.Padding = new System.Windows.Forms.Padding(0);
            this.btnDeleteWallet.Text = "";
            this.btnDeleteWallet.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.btnDeleteWallet.VisibleInOverflowMenu = true;
            this.btnDeleteWallet.Click += new System.EventHandler(this.BtnDeleteWalletClick);
            // 
            // btnGraphics
            // 
            this.btnGraphics.DisplayName = "";
            this.btnGraphics.Image = ((System.Drawing.Image)(resources.GetObject("btnGraphics.Image")));
            this.btnGraphics.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnGraphics.Name = "btnGraphics";
            this.btnGraphics.Text = "";
            this.btnGraphics.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.btnGraphics.VisibleInOverflowMenu = true;
            this.btnGraphics.Click += new System.EventHandler(this.BtnGraphicsClick);
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.AutoSize = true;
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(617, 32);
            this.radCommandBar1.TabIndex = 1;
            this.radCommandBar1.Text = "radCommandBar1";
            // 
            // Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nTabControl);
            this.Controls.Add(this.radCommandBar1);
            this.Name = "Data";
            this.Size = new System.Drawing.Size(617, 263);
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Nevron.UI.WinForm.Controls.NTabControl nTabControl;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement2;
        private Telerik.WinControls.UI.CommandBarTextBox txtNameTab;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton btnNewWallet;
        private Telerik.WinControls.UI.CommandBarButton btnDeleteWallet;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarButton btnGraphics;

    }
}
