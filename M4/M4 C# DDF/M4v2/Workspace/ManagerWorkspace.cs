using System;
using System.IO;
using System.Linq;
using System.Xml;
using M4.M4v2.Chart;
using M4.M4v2.Themes;
using M4Data.List;
using STOCKCHARTXLib;
using Telerik.WinControls.UI.Docking;
using M4Data;

namespace M4.M4v2.Workspace
{
    public class ManagerWorkspace
    {
        private static ManagerWorkspace _managerWorkspace;
        private string _archiveName;

        public static ManagerWorkspace Instance()
        {
            return _managerWorkspace ?? new ManagerWorkspace();
        }


        public void LoadTheme()
        {
            XmlDocument xmlDocument = ListWorkspace.Instance().GetXmlWorkspaceLoad();

            if (xmlDocument == null)
                return;
            if(frmMain2.GInstance != null)
                frmMain2.GInstance.m_Style = xmlDocument.GetElementsByTagName("WORKSPACE")[0]["THEME"].InnerText;

            ChangeTheme.ChangeThemeName(xmlDocument.GetElementsByTagName("WORKSPACE")[0]["THEME"].InnerText);
        }

        public bool Load(frmMain2 frmMain)
        {
            XmlDocument xmlDocument = ListWorkspace.Instance().GetXmlWorkspaceLoad();

            if (xmlDocument == null)
                return false;

            LoadTheme();

            return true;
        }

        
        public void SaveTemplate(string theme, frmMain2 frmMain)
        {
            try
            {
                M4Core.Entities.Workspace workspace = new M4Core.Entities.Workspace
                {
                    Parent = "Workspaces",
                    Text = "LoadWorkspace",
                    Theme = theme,
                    Default = true
                };

                ListWorkspace.Instance().SaveWorkspaceLoad(workspace);

               // SaveConfigMain("WorkspaceTelerik.xml", frmMain);
            }
            catch (Exception ex)
            {
                
            }
        }

        public void SaveConfigMain(string archiveName, frmMain2 frmMain)
        {
            _archiveName = archiveName;

            if (File.Exists(ListWorkspace._path + _archiveName.Split('.')[0] +"\\" + _archiveName))
                File.Delete(ListWorkspace._path + _archiveName.Split('.')[0] +"\\" + _archiveName);
            if (!Directory.Exists(ListWorkspace._path + _archiveName.Split('.')[0] + "\\"))
            {
                Directory.CreateDirectory(ListWorkspace._path + _archiveName.Split('.')[0] + "\\");
            }
                       
            frmMain.radDock2.SaveToXml(ListWorkspace._path + _archiveName.Split('.')[0] +"\\" + _archiveName);
            VersionChecker.VersionChecker.InsertVersion(ListWorkspace._path + _archiveName.Split('.')[0] + "\\" + _archiveName, VersionChecker.VersionChecker.Version);
            StateStateSaved(frmMain.radDock2);
        }

        public void StateStateSaved(RadDock dock)
        {
            DockWindowCollection documentManager = dock.DockWindows;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.CreateElement("WORKSPACE");
            XmlNode nodeAtivos = xmlDocument.CreateNode(XmlNodeType.Element, "ATIVOS", null);

            if (!Directory.Exists(ListWorkspace._path + _archiveName.Split('.')[0]))
            {
                Directory.CreateDirectory(ListWorkspace._path + _archiveName.Split('.')[0]);
            }

            foreach (DockWindow document in documentManager.Where(document =>
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"]))))
            {
                if ((document.Controls.Count == 0) || (!document.AccessibleName.Equals("CtlPainelChart")))
                    continue;

                if (File.Exists(ListWorkspace._path + _archiveName.Split('.')[0] +"\\" + "ATIVOS_" + _archiveName))
                    File.Delete(ListWorkspace._path + _archiveName.Split('.')[0] + "\\" + "ATIVOS_" + _archiveName);

                XmlNode nodeAtivo = xmlDocument.CreateNode(XmlNodeType.Element, "ATIVO", null);

                XmlNode nodePropriedades = xmlDocument.CreateNode(XmlNodeType.Element, "PROPRIEDADES", null);

                XmlElement nodeNome = xmlDocument.CreateElement("NOME");
                nodeNome.InnerText = document.Text;

                XmlElement nodeWindowNome = xmlDocument.CreateElement("NOME_TOOLWINDOW");
                nodeWindowNome.InnerText = document.Name;

                XmlElement nodeScale = xmlDocument.CreateElement("VIEW_SCALE_SEMI_LOG");
                nodeScale.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.ScaleType == ScaleType.stLinearScale ? "0" : "1";

                XmlElement nodeXGrid = xmlDocument.CreateElement("SHOW_XGRID");
                nodeXGrid.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.XGrid ? "1" : "0";

                XmlElement nodeYGrid = xmlDocument.CreateElement("SHOW_YGRID");
                nodeYGrid.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.YGrid ? "1" : "0";

                XmlElement nodeSeparators = xmlDocument.CreateElement("SHOW_SEPARATORS");
                nodeSeparators.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.HorizontalSeparators ? "1" : "0";

                XmlElement nodeDarvasBoxes = xmlDocument.CreateElement("SHOW_DARVAS_BOXES");
                nodeDarvasBoxes.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.DarvasBoxes ? "1" : "0";

                XmlElement nodeFirstVisibleRecord = xmlDocument.CreateElement("FIRST_VISIBLE_RECORD");
                nodeFirstVisibleRecord.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.FirstVisibleRecord.ToString();

                XmlElement nodeLastVisibleRecord = xmlDocument.CreateElement("LAST_VISIBLE_RECORD");
                nodeLastVisibleRecord.InnerText = ((CtlPainelChart)document.Controls[0]).StockChartX1.LastVisibleRecord.ToString(); 
                
                XmlElement nodeColor = xmlDocument.CreateElement("COLOR");
                nodeColor.InnerText = ((CtlPainelChart)document.Controls[0]).m_SchemeColor;

                XmlElement nodeVisible = xmlDocument.CreateElement("VISIBLE");
                nodeVisible.InnerText = document.Controls[0].Visible ? "1" : "0";

                XmlElement nodeSimbolo = xmlDocument.CreateElement("SIMBOLO");
                nodeSimbolo.InnerText = ((Chart.CtlPainelChart)document.Controls[0]).MSymbol;

                XmlElement nodePeriodicidade = xmlDocument.CreateElement("PERIODICIDADE");
                nodePeriodicidade.InnerText = ((Chart.CtlPainelChart)document.Controls[0]).m_Periodicity.ToString();

				XmlElement nodeIntervalo = xmlDocument.CreateElement("INTERVALO");
                nodeIntervalo.InnerText = ((Chart.CtlPainelChart)document.Controls[0]).m_BarSize.ToString();

                nodePropriedades.AppendChild(nodeScale);
                nodePropriedades.AppendChild(nodeXGrid);
                nodePropriedades.AppendChild(nodeYGrid);
                nodePropriedades.AppendChild(nodeSeparators);
                nodePropriedades.AppendChild(nodeDarvasBoxes);
                nodePropriedades.AppendChild(nodeFirstVisibleRecord);
                nodePropriedades.AppendChild(nodeLastVisibleRecord);
                nodePropriedades.AppendChild(nodeColor);

                nodeAtivo.AppendChild(nodeNome);
                nodeAtivo.AppendChild(nodeWindowNome);
                nodeAtivo.AppendChild(nodeSimbolo);
                nodeAtivo.AppendChild(nodePeriodicidade);
                nodeAtivo.AppendChild(nodeIntervalo);
                nodeAtivo.AppendChild(nodeVisible);
                nodeAtivo.AppendChild(nodePropriedades);

                nodeAtivos.AppendChild(nodeAtivo);
                ((CtlPainelChart)document.Controls[0]).StockChartX1.SaveGeneralTemplate(ListWorkspace._path + _archiveName.Split('.')[0] + "\\" + document.Name + ".sct");
            }

            if (nodeAtivos.ChildNodes.Count <= 0)
                return;

            xmlDocument.AppendChild(nodeAtivos);
            xmlDocument.Save(ListWorkspace._path + _archiveName.Split('.')[0] + "\\" + "ATIVOS_" + _archiveName);
            VersionChecker.VersionChecker.InsertVersion(ListWorkspace._path + _archiveName.Split('.')[0] + "\\" + "ATIVOS_" + _archiveName, VersionChecker.VersionChecker.Version);
        }
    }
}
