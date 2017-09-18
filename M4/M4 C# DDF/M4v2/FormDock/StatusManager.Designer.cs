namespace M4.M4v2.FormDock
{
    partial class StatusManager
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
            this.outputWindowV21 = new M4.M4v2.FormDock.OutputWindowV2();
            this.SuspendLayout();
            // 
            // outputWindowV21
            // 
            this.outputWindowV21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputWindowV21.Location = new System.Drawing.Point(0, 0);
            this.outputWindowV21.Name = "outputWindowV21";
            this.outputWindowV21.Size = new System.Drawing.Size(800, 300);
            this.outputWindowV21.TabIndex = 0;
            // 
            // StatusManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.outputWindowV21);
            this.Name = "StatusManager";
            this.Size = new System.Drawing.Size(800, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private OutputWindowV2 outputWindowV21;


    }
}
