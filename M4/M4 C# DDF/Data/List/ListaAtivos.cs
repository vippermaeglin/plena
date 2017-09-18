using System.Xml;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using M4.Core.Entities;

namespace M4.Data.List
{
    public class ListaAtivos
    {
        #region Propriedades

        private const string CaminhoXml = "~/RegAtivos.xml";
        private static ListaAtivos _listaAtivos;

        #endregion

        #region Instancia

        public static ListaAtivos Instancia()
        {
            return _listaAtivos ?? new ListaAtivos();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os ativos do XML e cria uma lista de Ativos
        /// </summary>
        /// <returns>Lista de Ativos</returns>
        public List<Ativos> CarregarListaAtivos()
        {
            XmlDocument xmlDocument = CarregarXmlComXmlDocument();

            List<Ativos> lista = (from XmlNode node in xmlDocument.DocumentElement.SelectSingleNode("REG_ATIVOS")
                                 where node != null
                                 select new Ativos
                                            {
                                                Abertura = node["ABERTURA"].InnerText,
                                                Ativo = node["ATIVO"].InnerText,
                                                Fechamento = node["FECHAMENTO"].InnerText,
                                                Hora = node["HORA"].InnerText,
                                                Maximo = node["MAXIMO"].InnerText,
                                                Minimo = node["MINIMO"].InnerText,
                                                Negocios = node["NEGOCIOS"].InnerText,
                                                Quantidade = node["QUANTIDADE"].InnerText,
                                                Ultimo = node["ULTIMO"].InnerText,
                                                Variacao = node["VARIACAO"].InnerText,
                                                Volume = node["VOLUME"].InnerText
                                            }).ToList();

            return lista;
        }

        #region LoadXML

        /// <summary>
        /// Carrega o xml com os valores dos ativos
        /// </summary>
        /// <returns>XmlDocumento com os valores carregados</returns>
        public XmlDocument CarregarXmlComXmlDocument()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(CaminhoXml);
            return xmlDocument;
        }

        /// <summary>
        /// Carrega um objeto DataSet com os valores dos ativos
        /// </summary>
        /// <returns>DataSet com os valores carregados</returns>
        public DataSet CarregarXmlComDataSet()
        {
            DataSet ds = new DataSet("RegAtivos");
            ds.ReadXml(CaminhoXml);
            return ds;
        }

        #endregion

        #endregion
    }
}