using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace M4.M4v2.Base
{
    class ListPortfolios
    {

        #region Properties

        public string path_portfolios = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\PORTFOLIO\\UserPortfolios.xml";
        private static ListPortfolios _listPortfolios;

        #endregion

        #region Instance

        public static ListPortfolios Instance()
        {
            return _listPortfolios ?? new ListPortfolios();
        }

        #endregion

        #region Methods

        public List<Portfolios> LoadListPortfolios()
        {
            List<Portfolios> list = new List<Portfolios>();
            try
            {
                XmlDocument xmlPortfolio = new XmlDocument();
                xmlPortfolio.Load(path_portfolios);
                XmlNodeList nodeList = xmlPortfolio.GetElementsByTagName("STATUS_PORTFOLIO");

                foreach (XmlNode node in nodeList)
                {
                    Portfolios portfolio = new Portfolios { Assets = new List<string>() };
                    XmlNodeList nodeAssets = node.ChildNodes;

                    foreach (XmlNode nodeAsset in nodeAssets)
                    {
                        if (nodeAsset.Name == "STATUS_NAME") portfolio.Name = nodeAsset.InnerText;
                        else portfolio.Assets.Add(nodeAsset.InnerText);
                    }

                    list.Add(portfolio);
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("LoadListPortifolios"+ex.Message);
            }

            return list;
        }

        public void UpdatePortfolios(List<Portfolios> NewPortfolio)
        {
            StreamWriter doc = new StreamWriter(path_portfolios);

            XmlTextWriter xml_out = new XmlTextWriter(doc);

            xml_out.WriteStartDocument();
            xml_out.WriteStartElement("REG_PORTFOLIOS");

            foreach (Portfolios p in NewPortfolio)
            {
                xml_out.WriteStartElement("STATUS_PORTFOLIO");
                xml_out.WriteStartElement("STATUS_NAME");
                xml_out.WriteString(p.Name);
                xml_out.WriteEndElement();

                foreach (string s in p.Assets)
                {
                    xml_out.WriteStartElement("ATIVO");
                    xml_out.WriteString(s);
                    xml_out.WriteEndElement();
                }

                xml_out.WriteEndElement();
            }

            xml_out.WriteEndElement();
            xml_out.WriteEndDocument();
            xml_out.Close();
        }

        public void ReorderPosition(string statusName, string asset, int newPositionCurrent, bool after)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path_portfolios);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("STATUS_NAME");
            
            XmlNodeList nodeAssets = null;
            XmlNode parent = null;
            
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (!nodeList[i].InnerText.Equals(statusName)) 
                    continue;

                parent = nodeList[i].ParentNode;
                nodeAssets = nodeList[i].ParentNode.SelectNodes("ATIVO");
                break;
            }

            XmlNode nodeOld = null;
            XmlNode nodeNewPosition = nodeAssets[newPositionCurrent];

            for (int i = 0; i < nodeAssets.Count; i++)
            {
                if (!nodeAssets[i].InnerText.Equals(asset)) 
                    continue;

                nodeOld = nodeAssets[i];
                break;
            }

            parent.RemoveChild(nodeOld);

            if (after)
                parent.InsertAfter(nodeOld, nodeNewPosition);
            else
                parent.InsertBefore(nodeOld, nodeNewPosition);

            xmlDocument.Save(path_portfolios);
        }

        #endregion
    }
}
