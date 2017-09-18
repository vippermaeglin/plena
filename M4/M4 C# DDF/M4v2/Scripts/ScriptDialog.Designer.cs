namespace M4.M4v2.Scripts
{
    partial class ScriptDialog
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
            this.txtScript = new System.Windows.Forms.RichTextBox();
            this.btOk = new Telerik.WinControls.UI.RadButton();
            this.btTradescript = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btTradescript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // txtScript
            // 
            this.txtScript.Location = new System.Drawing.Point(12, 12);
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(290, 67);
            this.txtScript.TabIndex = 0;
            this.txtScript.Text = "";
            this.txtScript.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScript_KeyUp);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(75, 85);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "Ok";
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btTradescript
            // 
            this.btTradescript.Location = new System.Drawing.Point(156, 85);
            this.btTradescript.Name = "btTradescript";
            this.btTradescript.Size = new System.Drawing.Size(75, 23);
            this.btTradescript.TabIndex = 2;
            this.btTradescript.Text = "Tradescript";
            this.btTradescript.Click += new System.EventHandler(this.btTradescript_Click);
            // 
            // ScriptDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 113);
            this.Controls.Add(this.btTradescript);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.txtScript);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScriptDialog";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "";
            ((System.ComponentModel.ISupportInitialize)(this.btOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btTradescript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtScript;
        private Telerik.WinControls.UI.RadButton btOk;
        private Telerik.WinControls.UI.RadButton btTradescript;
    }
}