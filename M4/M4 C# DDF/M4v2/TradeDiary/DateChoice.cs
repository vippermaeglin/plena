using System;
using System.Windows.Forms;
using M4.M4v2.Portfolio;

namespace M4.M4v2.TradeDiary
{
    public partial class DateChoice : Telerik.WinControls.UI.RadForm
    {
        public PortfolioView1 ParentControl;
        public string Date;
        public bool IsClick { get; set; }

        public DateChoice(PortfolioView1 parent, string date, DateTime dateInit)
        {
            IsClick = false;
            InitializeComponent();
            Date = date;
            ParentControl = parent;
            radCalendar1.SelectedDate = dateInit;
            radCalendar1.FocusedDate = dateInit;
        }

        private void RadCalendar1SelectionChanged(object sender, EventArgs e)
        {
            if (IsClick == false)
                return;

            DateTime dateChoosed = radCalendar1.FocusedDate;
            dateChoosed.AddHours(0);
            dateChoosed.AddMinutes(0);
            dateChoosed.AddSeconds(0);

            switch (Date)
            {
                case "start":
                    if (ParentControl.DiaryEnd < dateChoosed)
                        MessageBox.Show("Start Date must be less recent than End Date!");
                    else
                        ParentControl.DiaryStart = radCalendar1.FocusedDate;
                    break;
                case "end":
                    if (ParentControl.DiaryStart > dateChoosed)
                        MessageBox.Show("End Date must be most recent than Start Date!");
                    else
                        ParentControl.DiaryEnd = radCalendar1.FocusedDate;
                    break;
            }

            Close();
        }
    }
}
