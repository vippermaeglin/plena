using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M4.Properties;
using M4Core.Entities;
using M4Data.List;
using M4Utils.Language;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Xml;
using System.Data;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public partial class DataManagerExtends : UserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataManagerExtends));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor2 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor3 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor4 = new Telerik.WinControls.Data.SortDescriptor();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarToggleButton1 = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarToggleButton2 = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarRowElement2 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement2 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarToggleButton3 = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarToggleButton4 = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.radGridView1 = new M4.M4v2.GridviewRowDetailsExtended.GridViewRowDetailsExtended();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarRowElement1.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarRowElement1.DisplayName = null;
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            this.commandBarRowElement1.Text = "";
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarStripElement1.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.FloatingForm = null;
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarToggleButton1,
            this.commandBarToggleButton2});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            this.commandBarStripElement1.Text = "";
            // 
            // commandBarToggleButton1
            // 
            this.commandBarToggleButton1.AccessibleDescription = "commandBarToggleButton1";
            this.commandBarToggleButton1.AccessibleName = "commandBarToggleButton1";
            this.commandBarToggleButton1.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarToggleButton1.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarToggleButton1.DisplayName = "commandBarToggleButton1";
            this.commandBarToggleButton1.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButton1.Image")));
            this.commandBarToggleButton1.Name = "commandBarToggleButton1";
            this.commandBarToggleButton1.Text = "commandBarToggleButton1";
            this.commandBarToggleButton1.ToolTipText = "Show details only for Selected Row";
            this.commandBarToggleButton1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarToggleButton2
            // 
            this.commandBarToggleButton2.AccessibleDescription = "commandBarToggleButton2";
            this.commandBarToggleButton2.AccessibleName = "commandBarToggleButton2";
            this.commandBarToggleButton2.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarToggleButton2.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarToggleButton2.DisplayName = "commandBarToggleButton2";
            this.commandBarToggleButton2.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButton2.Image")));
            this.commandBarToggleButton2.Name = "commandBarToggleButton2";
            this.commandBarToggleButton2.Text = "commandBarToggleButton2";
            this.commandBarToggleButton2.ToolTipText = "Show details for All Rows";
            this.commandBarToggleButton2.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarRowElement2
            // 
            this.commandBarRowElement2.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarRowElement2.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarRowElement2.DisplayName = null;
            this.commandBarRowElement2.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement2.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement2});
            this.commandBarRowElement2.Text = "";
            // 
            // commandBarStripElement2
            // 
            this.commandBarStripElement2.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarStripElement2.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarStripElement2.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement2.FloatingForm = null;
            this.commandBarStripElement2.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarToggleButton3,
            this.commandBarToggleButton4});
            this.commandBarStripElement2.Name = "commandBarStripElement1";
            this.commandBarStripElement2.Text = "";
            // 
            // commandBarToggleButton3
            // 
            this.commandBarToggleButton3.AccessibleDescription = "commandBarToggleButton1";
            this.commandBarToggleButton3.AccessibleName = "commandBarToggleButton1";
            this.commandBarToggleButton3.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarToggleButton3.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarToggleButton3.DisplayName = "commandBarToggleButton1";
            this.commandBarToggleButton3.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButton3.Image")));
            this.commandBarToggleButton3.Name = "commandBarToggleButton3";
            this.commandBarToggleButton3.Text = "commandBarToggleButton1";
            this.commandBarToggleButton3.ToolTipText = "Show details only for Selected Row";
            this.commandBarToggleButton3.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarToggleButton4
            // 
            this.commandBarToggleButton4.AccessibleDescription = "commandBarToggleButton2";
            this.commandBarToggleButton4.AccessibleName = "commandBarToggleButton2";
            this.commandBarToggleButton4.BorderDrawMode = Telerik.WinControls.BorderDrawModes.HorizontalOverVertical;
            this.commandBarToggleButton4.BorderLeftShadowColor = System.Drawing.Color.Empty;
            this.commandBarToggleButton4.DisplayName = "commandBarToggleButton2";
            this.commandBarToggleButton4.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButton4.Image")));
            this.commandBarToggleButton4.Name = "commandBarToggleButton4";
            this.commandBarToggleButton4.Text = "commandBarToggleButton2";
            this.commandBarToggleButton4.ToolTipText = "Show details for All Rows";
            this.commandBarToggleButton4.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radGridView1
            // 
            this.radGridView1.AutoScroll = true;
            this.radGridView1.DetailsColumn = null;
            this.radGridView1.DetailsRowHeight = 130;
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.EnableCustomSorting = true;
            this.radGridView1.EnableFastScrolling = true;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            // 
            // radGridView1
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView1.MasterTemplate.AllowDragToGroup = false;
            this.radGridView1.MasterTemplate.AllowRowReorder = true;
            this.radGridView1.MasterTemplate.EnableCustomSorting = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.MasterTemplate.EnableSorting = false;
            sortDescriptor1.PropertyName = "Sort_Symbol";
            sortDescriptor2.PropertyName = "Sort_Last";
            sortDescriptor3.PropertyName = "Sort_Variation";
            sortDescriptor4.PropertyName = "Sort_Volume";
            this.radGridView1.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1,
            sortDescriptor2,
            sortDescriptor3,
            sortDescriptor4});
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(791, 562);
            this.radGridView1.TabIndex = 1;
            this.radGridView1.Text = "GridViewRowDetailsExtended1";
            this.radGridView1.RowFormatting += new Telerik.WinControls.UI.RowFormattingEventHandler(this.RadGridView1RowFormatting);
            this.radGridView1.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.RadGridView1CellFormatting);
            this.radGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridView1CellDoubleClick);
            // 
            // DataManagerExtends
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGridView1);
            this.Name = "DataManagerExtends";
            this.Size = new System.Drawing.Size(791, 562);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            this.ResumeLayout(false);

        }
        //private GridViewRowDetailsExtended GridViewRowDetailsExtended1;

        #endregion

        private CommandBarRowElement commandBarRowElement1;
        private CommandBarStripElement commandBarStripElement1;
        private CommandBarToggleButton commandBarToggleButton1;
        private CommandBarToggleButton commandBarToggleButton2;
        private CommandBarRowElement commandBarRowElement2;
        private CommandBarStripElement commandBarStripElement2;
        private CommandBarToggleButton commandBarToggleButton3;
        private CommandBarToggleButton commandBarToggleButton4;
        private GridViewRowDetailsExtended radGridView1;

    }
}

