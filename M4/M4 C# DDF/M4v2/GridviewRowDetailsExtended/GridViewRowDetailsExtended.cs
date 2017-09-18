
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.ComponentModel;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class GridViewRowDetailsExtended : RadGridView
    {
        private GridViewDataColumn renamedDetailsColumn;
        public int renamedDetailsRowHeight = 100; //Detail RowHeight
        public bool showRowDetails = true;
        public bool enableAllExpand = true;

        public GridViewRowDetailsExtended()
        {
            this.GridBehavior = new RowDetailsBehavior();
            this.ViewDefinition = new RowDetailsViewDefinition();
        }

        public override string ThemeClassName
        {
            get { return typeof(RadGridView).FullName; }
            set { }
        }

        [Browsable(false)]
        [Category("Row Details")]
        public GridViewDataColumn DetailsColumn
        {
            get { return this.renamedDetailsColumn; }

            set
            {

                if (!object.ReferenceEquals(renamedDetailsColumn, value))
                {
                    renamedDetailsColumn = value;
                    if (renamedDetailsColumn != null)
                    {
                        renamedDetailsColumn.MinWidth = 0;
                        renamedDetailsColumn.HeaderText = "";
                        renamedDetailsColumn.Width = 0;
                        renamedDetailsColumn.MaxWidth = 1;
                        renamedDetailsColumn.ReadOnly = true;
                        renamedDetailsColumn.VisibleInColumnChooser = true;
                    }
                    this.TableElement.UpdateView();
                }
            }
        }

        [Browsable(true)]
        [Category("Row Details")]
        [DefaultValue(100)]
        public int DetailsRowHeight
        {
            get { return this.renamedDetailsRowHeight; }
            set
            {
                if (renamedDetailsRowHeight != value)
                {
                    renamedDetailsRowHeight = value;
                    TableElement.Update(GridUINotifyAction.Reset);
                    TableElement.UpdateView();
                }
            }
        }

        [Browsable(true)]
        [Category("Row Details")]
        [DefaultValue(true)]
        public bool ShowRowDetails
        {
            get { return this.showRowDetails; }
            set
            {
                showRowDetails = value;
                this.DetailsColumn.IsVisible = value;
                TableElement.Update(GridUINotifyAction.Reset);
                TableElement.UpdateView();
            }
        }

        [Browsable(true)]
        [Category("Row Details")]
        [Description("Allows all rows to be expanded (true) or only selected row expanded (false)")]
        [DefaultValue(true)]
        public bool EnableAllExpand
        {
            get { return this.enableAllExpand; }
            set
            {
                this.enableAllExpand = value;
                TableElement.Update(GridUINotifyAction.Reset);
                TableElement.UpdateView();
            }
        }

        [Browsable(false)]
        [Category("Row Details")]
        [DefaultValue(true)]
        public bool RowDetailsShowing
        {
            get { return this.showRowDetails; }
        }

        protected override void OnCreateRow(object sender, GridViewCreateRowEventArgs e)
        {
            if (object.ReferenceEquals(e.RowType, typeof(GridDataRowElement)))
            {
                e.RowType = typeof(RowDetailsRowElement);
            }
            base.OnCreateRow(sender, e);
        }
    }
}
