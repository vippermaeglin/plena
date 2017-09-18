using System;
using System.Collections.Generic;
using System.Windows.Forms;
using M4.M4v2.Chart;
using M4.M4v2.Base;
using M4.M4v2.TradeDiary;
using Telerik.WinControls.UI.Docking;
using Telerik.WinControls.UI;
using System.IO;
using M4Core.Entities;
using M4Data.List;
using System.Linq;
using M4.M4v2.Portfolio;


namespace M4.M4v2.Portfolio
{
    public partial class NewDiaryPost : Telerik.WinControls.UI.RadForm
    {
        private TradeDiaryView ParentControl;
        private string New_Symbol;
        private string[] split_time;
        private string[] split_date;
        private string year, month, day, hour, minute;
        private bool create_new;
        private string filename;
        private string URL;

        public NewDiaryPost(TradeDiaryView PARENTCONTROL) //Create new post
        {
            InitializeComponent();
            ParentControl = PARENTCONTROL;
            create_new = true;
            foreach (string s in ParentControl.ParentControl.AllAssetsComplete)
            {
                ddlFilter.Items.Add(s);
            }
            if (DateTime.Now.Hour.ToString().Length == 1) hour = "0" + DateTime.Now.Hour.ToString();
            else hour = DateTime.Now.Hour.ToString();
            if (DateTime.Now.Minute.ToString().Length == 1) minute = "0" + DateTime.Now.Minute.ToString();
            else minute = DateTime.Now.Minute.ToString();
            TxtTime.Text = hour + ":" + minute;
            year = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
                month = "0" + DateTime.Now.Month.ToString();
            else month = DateTime.Now.Month.ToString();
            if (DateTime.Now.Day.ToString().Length == 1)
                day = "0" + DateTime.Now.Day.ToString();
            else day = DateTime.Now.Day.ToString();
            TxtDate.Text = day + "/" + month + "/" + year;
        }

        public NewDiaryPost(TradeDiaryView PARENTCONTROL,string url) //Edit Date and Time on created post
        {
            URL = url;
            string[] symbol_split;
            filename = url.Replace(Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\", "");
            InitializeComponent();
            ParentControl = PARENTCONTROL;
            create_new = false;
            ddlFilter.Items.Add(filename);
            ddlFilter.Enabled = false;
            ddlFilter.SelectedIndex = 0;
            symbol_split = filename.Replace(".html", "").Split(new char[] { '_' });
            New_Symbol = symbol_split[0];
            hour = symbol_split[4];
            minute = symbol_split[5];
            TxtTime.Text = hour + ":" + minute;
            year = symbol_split[1];
            month = symbol_split[2];
            day = symbol_split[3];
            TxtDate.Text = day + "/" + month + "/" + year;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(create_new)
            {
                try
                {
                    if (ddlFilter.SelectedItem.Text == null)
                    {
                        MessageBox.Show("Set a valid symbol!");
                        return;
                    }
                    else
                    {

                        string[] split_symbol;
                        split_symbol = ddlFilter.SelectedItem.Text.ToString().Split(new char[] {'-'});
                        New_Symbol = split_symbol[0].Trim();
                    }
                    if (TxtTime.Text.Length!=5)
                    {
                        MessageBox.Show("Set a valid time!");
                        return;
                    }
                    else
                    {
                        split_time = TxtTime.Text.Split(new char[] {':'});
                    }
                    if (TxtDate.Text == null)
                    {
                        MessageBox.Show("Set a valid date!");
                        return;
                    }
                    else
                    {
                        split_date = TxtDate.Text.Split(new char[] {'/'});
                    }
                    string url = Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\" + New_Symbol + "_" +
                                 split_date[2] + "_" + split_date[1] + "_" +split_date[0]+"_"+ split_time[0] +
                                 "_" + split_time[1] + "_00.html";
                    Stream html_writer = new FileStream(url, FileMode.CreateNew);
                    html_writer.Close();
                    if (!ParentControl.ParentControl.SymbolsDiary.Contains(New_Symbol)) ParentControl.ParentControl.SymbolsDiary.Add(New_Symbol);

                    DiaryEditor _editor = new DiaryEditor(url, ParentControl);
                    _editor.Show();
                    this.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                split_time = TxtTime.Text.Split(new char[] { ':' });
                split_date = TxtDate.Text.Split(new char[] { '/' }); 
                string new_url = Directory.GetCurrentDirectory() + "\\Base\\TRADEDIARY\\" + New_Symbol + "_" +
                                  split_date[2] + "_" + split_date[1] + "_" + split_date[0] + "_" + split_time[0] +
                                  "_" + split_time[1] + "_00.html";
                //MessageBox.Show("New:" + new_url + "\nOld:" + URL);
                File.Move(URL,new_url);
                this.Close();
            }
        }

        
        private void radCalendar1_SelectionChanged(object sender, EventArgs e)
        {
            if (radCalendar1.SelectedDate.Year != 1900)
            {
                year = radCalendar1.SelectedDate.Year.ToString();
                if (radCalendar1.SelectedDate.Month.ToString().Length == 1)
                    month = "0" + radCalendar1.SelectedDate.Month.ToString();
                else month = radCalendar1.SelectedDate.Month.ToString();
                if (radCalendar1.SelectedDate.Day.ToString().Length == 1)
                    day = "0" + radCalendar1.SelectedDate.Day.ToString();
                else day = radCalendar1.SelectedDate.Day.ToString();
                TxtDate.Text = day + "/" + month + "/" + year;
            }
        }

        private void radCalendar1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radCalendar1.SelectedDate.Year != 1900)
            {
                radCalendar1.SelectedDate = radCalendar1.FocusedDate;
                year = radCalendar1.SelectedDate.Year.ToString();
                if (radCalendar1.SelectedDate.Month.ToString().Length == 1)
                    month = "0" + radCalendar1.SelectedDate.Month.ToString();
                else month = radCalendar1.SelectedDate.Month.ToString();
                if (radCalendar1.SelectedDate.Day.ToString().Length == 1)
                    day = "0" + radCalendar1.SelectedDate.Day.ToString();
                else day = radCalendar1.SelectedDate.Day.ToString();
                TxtDate.Text = day + "/" + month + "/" + year;
            }
        }


        
    }
}
