using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using M4Core.Entities;
using M4Data.List;
using Telerik.WinControls.UI.Docking;
using M4.AsyncOperations;
using System.IO;
using M4.M4v2.Settings;

namespace M4.M4v2.Chart
{
    public class CtlPainelChartList
    {
        private List<CtlPainelChart> Members = new List<CtlPainelChart>();

        public int Count()
        {
            return Members.Count;
        }


        public void FillList()
        {
            for (int i = 0; i < 10; i++)
            {
                CtlPainelChart chart;
                chart = ObjectPool.New<CtlPainelChart>();
                chart.LoadCtlPainelChart(frmMain2.GInstance, frmMain2.GInstance._mCtlData,
                    "BRKM5", Periodicity.Daily, 1, 20000, "Plena", true);
                chart.StateDummy = true;
                chart.StateUsed = false;
                chart.SuspendEvents();
                Members.Add(chart);
            }
        }

        public CtlPainelChart TakeFromList()
        {
            try
            {
                CtlPainelChart chart;
                foreach (CtlPainelChart Chart in Members)
                {
                    if (!Chart.StateUsed)
                    {
                        chart = Chart;
                        return chart;
                    }
                }
                chart = ObjectPool.New<CtlPainelChart>();
                chart.LoadCtlPainelChart(frmMain2.GInstance, frmMain2.GInstance._mCtlData,
                    "BRKM5", Periodicity.Daily, 1, 20000, "Plena", true);
                chart.StateDummy = true;
                chart.StateUsed = false;
                Members.Add(chart);
                chart.SuspendEvents();
                return chart;
                //   CtlPainelChart chart = Members.First();
                //   Members.Remove(chart);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("Erro ao acessar lista de Charts: \n" + ex.Message);
                return null;
            }
        }

        public bool IsAllUsed()
        {
            foreach (CtlPainelChart Chart in Members)
            {
                if (!Chart.StateUsed)
                {
                    return false;
                }
            }
            return true;
        }


        public void AddChart(CtlPainelChart chart, DockWindow Window, XmlNode Propriedades, XmlNode node, string simbolo, Periodicity periodicity, int barSize, int mBars, string windowName)
        {

            chart.StockChartX1.Visible = false;

            chart.LoadCtlPainelChart(frmMain2.GInstance, frmMain2.GInstance._mCtlData, simbolo, periodicity, barSize,
                                     mBars, "Plena", true);


            chart.StateDummy = true;
            chart.BlockUpdateStock = true;
            chart.m_StopLoadScroll = true;
            Window.Controls.Add(chart);
            chart.BlockUpdateStock = false;
            frmMain2.XmlChartPropriedades(chart, Propriedades);
            if (Window.DockState == DockState.Floating)
            {

                Window.FloatingParent.ShowInTaskbar = true;
                Window.FloatingParent.MinimizeBox = true;
                Window.FloatingParent.MaximizeBox = true;
                Window.FloatingParent.FormBorderStyle = FormBorderStyle.Sizable;
                Window.FloatingParent.Activated += new EventHandler(frmMain2.GInstance.Window_Activated);
                Window.FloatingParent.Closing += new System.ComponentModel.CancelEventHandler(frmMain2.GInstance.Window_Closing);
                frmMain2.Window_Initialize(Window, chart);
            }

            if ((node["VISIBLE"] != null) && (node["VISIBLE"].InnerText.Equals("0")))
                return;
            chart.StateDummy = false;
            chart.InitRTChartAsync(b => chart._asyncOp.Post(() =>
            {
                if (b)
                {
                    chart.StockChartX1.FirstVisibleRecord = int.Parse(Propriedades["FIRST_VISIBLE_RECORD"].InnerText);
                    chart.StockChartX1.LastVisibleRecord = int.Parse(Propriedades["LAST_VISIBLE_RECORD"].InnerText);
                    chart.BindContextMenuEvents();
                    chart.m_SchemeColor = Propriedades["COLOR"].InnerText;
                    Scheme.Instance().UpdateChartColors(chart.StockChartX1, chart.m_SchemeColor);
                    //chart.StockChartX1.Update();
                    //chart.StockChartX1.ForcePaint();
                    frmMain2.GInstance.MActiveChart = chart;
                    chart.LoadDataTemplate("-987654321.123456789");
                    chart.UpdateMenus();
                    chart.StockChartX1.Visible = true;
                    chart.StockChartX1.Width = chart.Width - 4;
                    //frmMain2.GInstance.LoadColorScheme(chart);
                    if (File.Exists(windowName))
                        chart.StockChartX1.LoadGeneralTemplate(windowName);
                    chart.LoadScroll();
                    return;
                }
                return;
            }));


        }

        public void InsertFromDockControls(RadDock dock)
        {
            foreach (DockWindow window in dock.DockWindows.Where(doc => doc.AccessibleName == "CtlPainelChart"))
            {

                CtlPainelChart chart = (CtlPainelChart)window.Controls[0];
                if (Members.Contains(chart))
                {
                    chart.StateUsed = false;
                }
                else
                {
                    Members.Add(chart);
                }
                window.Controls.Clear();
                chart.SuspendEvents();
            }
        }

        public void InsertCtlFromWindow(DockWindow window)
        {
            if (window.Controls.Count > 0 && window.Controls.ContainsKey("CtlPainelChart"))
            {
                CtlPainelChart chart = (CtlPainelChart)window.Controls[0];
                if (Members.Contains(chart))
                {
                    chart.StateUsed = false;
                }
                else
                {
                    Members.Add(chart);
                }
                window.Controls.Clear();
                chart.SuspendEvents();
            }
        }

        public void InsertToList(CtlPainelChart chart)
        {
            Members.Add(chart);
        }

        public bool IsEmpty()
        {
            return Members.Count == 0 ? true : false;
        }
    }
}
