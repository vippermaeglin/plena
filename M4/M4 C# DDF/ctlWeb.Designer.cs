using M4.Controls;

namespace M4
{
  partial class ctlWeb
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
            this.WebBrowser1 = new System.Windows.Forms.WebBrowser();
            this.pageToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbRedo = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbPageHome = new System.Windows.Forms.ToolStripButton();
            this.addressBox = new M4.Controls.ToolStripSpringTextBox();
            this.tsbNavigate = new System.Windows.Forms.ToolStripButton();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.pageToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            // 
            // WebBrowser1
            // 
            this.WebBrowser1.Location = new System.Drawing.Point(3, 28);
            this.WebBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser1.Name = "WebBrowser1";
            this.WebBrowser1.ScriptErrorsSuppressed = true;
            this.WebBrowser1.Size = new System.Drawing.Size(529, 265);
            this.WebBrowser1.TabIndex = 138;
            this.WebBrowser1.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.WebBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser1DocumentCompleted);
            this.WebBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WebBrowser1Navigating);
            // 
            // pageToolStrip
            // 
            this.pageToolStrip.CanOverflow = false;
            this.pageToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.pageToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbUndo,
            this.tsbRedo,
            this.tsbRefresh,
            this.tsbPageHome,
            this.addressBox,
            this.tsbNavigate});
            this.pageToolStrip.Location = new System.Drawing.Point(0, 0);
            this.pageToolStrip.Name = "pageToolStrip";
            this.pageToolStrip.Size = new System.Drawing.Size(547, 25);
            this.pageToolStrip.Stretch = true;
            this.pageToolStrip.TabIndex = 139;
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUndo.Image = global::M4.Properties.Resources.PageUndo;
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Size = new System.Drawing.Size(23, 22);
            this.tsbUndo.Text = "&Back";
            this.tsbUndo.Click += new System.EventHandler(this.ToolStripButton1Click);
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRedo.Image = global::M4.Properties.Resources.PageRedo;
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.Size = new System.Drawing.Size(23, 22);
            this.tsbRedo.Text = "&Forward";
            this.tsbRedo.Click += new System.EventHandler(this.ToolStripButton2Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = global::M4.Properties.Resources.PageRefresh;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "&Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.ToolStripButton3Click);
            // 
            // tsbPageHome
            // 
            this.tsbPageHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPageHome.Image = global::M4.Properties.Resources.PageHome;
            this.tsbPageHome.Name = "tsbPageHome";
            this.tsbPageHome.Size = new System.Drawing.Size(23, 22);
            this.tsbPageHome.Text = "&Home";
            this.tsbPageHome.Click += new System.EventHandler(this.ToolStripButton4Click);
            // 
            // addressBox
            // 
            this.addressBox.Name = "addressBox";
            this.addressBox.Size = new System.Drawing.Size(398, 25);
            this.addressBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressBoxKeyDown);
            // 
            // tsbNavigate
            // 
            this.tsbNavigate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNavigate.Image = global::M4.Properties.Resources.PageNavigate;
            this.tsbNavigate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNavigate.Name = "tsbNavigate";
            this.tsbNavigate.Size = new System.Drawing.Size(23, 22);
            this.tsbNavigate.Text = "toolStripButton5";
            this.tsbNavigate.Click += new System.EventHandler(this.ToolStripButton5Click);
            // 
            // radPanel1
            // 
            this.radPanel1.AllowDrop = true;
            this.radPanel1.CausesValidation = false;
            this.radPanel1.Controls.Add(this.pageToolStrip);
            this.radPanel1.Controls.Add(this.WebBrowser1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = this.Size;
            this.radPanel1.TabIndex = 2;
            // 
            // ctlWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanel1);
            this.Name = "ctlWeb";
            this.Size = new System.Drawing.Size(547, 365);
            this.pageToolStrip.ResumeLayout(false);
            this.pageToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.WebBrowser WebBrowser1;
    private System.Windows.Forms.ToolStrip pageToolStrip;
    private System.Windows.Forms.ToolStripButton tsbUndo;
    private System.Windows.Forms.ToolStripButton tsbRedo;
    private System.Windows.Forms.ToolStripButton tsbRefresh;
    private System.Windows.Forms.ToolStripButton tsbPageHome;
    private System.Windows.Forms.ToolStripButton tsbNavigate;
    private Telerik.WinControls.UI.RadPanel radPanel1;
    private ToolStripSpringTextBox addressBox;
  }
}
