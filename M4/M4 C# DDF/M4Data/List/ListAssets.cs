using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using M4Core.Entities;

namespace M4Data.List
{
    public class ListAssets
    {
        #region Properties

        public string Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SUMMARY\\Summary.xml";

        private static ListAssets _listAssets;

        #endregion

        #region Instance

        public static ListAssets Instance()
        {
            return _listAssets ?? new ListAssets();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Carrega os ativos do XML e cria uma lista de Ativos
        /// </summary>
        /// <returns>Lista de Ativos</returns>
        public List<Assets> LoadListAssets(string caminho)
        {
            XmlDocument xmlDocument = LoadXmlWithXmlDocument(caminho);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("STATUS_ATIVOS");

            List<Assets> list = new List<Assets>();

            for (int i = 0; i < nodeList.Count; i++)
            {
                try
                {
                    list.Add(new Assets
                    {
                        Open = decimal.Parse(nodeList[i]["OPEN"].InnerText),
                        Symbol = nodeList[i]["SYMBOL"].InnerText,
                        Close = decimal.Parse(nodeList[i]["CLOSE"].InnerText),
                        Time = nodeList[i]["TIME"].InnerText,
                        High = decimal.Parse(nodeList[i]["HIGH"].InnerText),
                        Low = decimal.Parse(nodeList[i]["LOW"].InnerText),
                        Trades = decimal.Parse(nodeList[i]["TRADES"].InnerText),
                        Last = decimal.Parse(nodeList[i]["LAST"].InnerText),
                        Variation = decimal.Parse(nodeList[i]["VARIATION"].InnerText),
                        Volume = double.Parse(nodeList[i]["VOLUME"].InnerText)
                    });
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }

        #region LoadXML

        /// <summary>
        /// Carrega o xml com os valores dos ativos
        /// </summary>
        /// <returns>XmlDocumento com os valores carregados</returns>
        public XmlDocument LoadXmlWithXmlDocument(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            return xmlDocument;
        }

        #endregion

        #endregion
    }
}