using System.Linq;
using System.Xml;
using System.Collections.Generic;
using M4Core.Entities;
using M4Utils;

namespace M4Data.List
{
    public class ListStocks
    {
        #region Properties

        private const string NameArchiveXml = "Base\\SUMMARY\\Summary.xml";

        private static ListStocks _listStocks;

        #endregion

        #region Instance

        public static ListStocks Instance()
        {
            return _listStocks ?? new ListStocks();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Carrega os ativos do XML e cria uma lista de Ativos
        /// </summary>
        /// <returns>Lista de Ativos</returns>
        public List<Stock> LoadStocks(string caminho)
        {
            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(caminho);

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("ATIVO");

            return (from XmlNode node in nodeList
                    select new Stock
                    {
                        Name = node["NOME"].InnerText,
                        Code = node["COD"].InnerText.ToUpper(),
                        Sector = node["SETOR"].InnerText,
                        Source = node["FONTE"].InnerText,
                        CodeName = node["COD"].InnerText.ToUpper() + " - " + node["NOME"].InnerText
                    }).ToList();
        }

        #endregion
    }
}