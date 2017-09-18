namespace M4
{
    partial class ctlData
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
            components = new System.ComponentModel.Container();
            tmrAlerts = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // tmrAlerts
            // 
            tmrAlerts.Enabled = true;
            tmrAlerts.Interval = 5000;
            tmrAlerts.Tick += new System.EventHandler(tmrAlerts_Tick);
            // 
            // ctlData
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Name = "ctlData";
            ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Timer tmrAlerts;
    }
}
