using System;
using System.IO;
using System.Linq;
using System.Xml;
using M4Core.Entities;
using M4Utils;
using M4Utils.Language;
using VersionChecker;


namespace M4Data.List
{
    public class ListTemplates
    {
        #region Properties

        public static string _path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\TEMPLATES\\";
        private readonly string _pathTemplateXml = _path + "Templates.xml";
        private XmlNode _xmlNodeText;
        private XmlNode _xmlNodeParent;
        private XmlNode _xmlNode;
        private static ListTemplates _listTemplates;
        public static LanguageDefault LanguageDefault;

        #endregion

        #region Instance

        public static ListTemplates Instance()
        {
            return _listTemplates ?? new ListTemplates();
        }

        public void SetLanguage(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Carrega os ativos do XML e cria uma lista de Templates
        /// </summary>
        /// <returns>Lista de Templates</returns>
        public XmlNodeList LoadTemplates()
        {
            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_pathTemplateXml);
            if (xmlDocument.ChildNodes.Count<1)
            {
                xmlDocument = new XmlDocument();
                XmlNode parentNode = xmlDocument.CreateNode(XmlNodeType.Element, "CONJUNTO", null);
                //Adiciona o node SUPORTE
                XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, "SUPORTE", null);

                XmlElement parent = xmlDocument.CreateElement("PARENT");
                XmlElement text = xmlDocument.CreateElement("TEXT");
                text.InnerText = "Templates";
                XmlElement suporte = xmlDocument.CreateElement("SUPORTE");
                suporte.InnerText = "None";

                node.AppendChild(parent);
                node.AppendChild(text);
                node.AppendChild(suporte);
                parentNode.AppendChild(node);
                xmlDocument.AppendChild(parentNode);


                xmlDocument.InsertBefore(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null), xmlDocument.DocumentElement);
                xmlDocument.Save(_pathTemplateXml);
                VersionChecker.VersionChecker.InsertVersion(_pathTemplateXml, VersionChecker.VersionChecker.Version);

                Insert(new Template() { Default = true, Parent = "None", Text = "Plena"});

                
            }
            return xmlDocument.GetElementsByTagName("CONJUNTO");
        }

        private void VerifyNodeText(XmlNode inXmlNode, string text)
        {
            if ((!inXmlNode.HasChildNodes) || (_xmlNodeText != null))
                return;

            int i;
            for (i = 0; i < inXmlNode.ChildNodes.Count; i++)
            {
                XmlNode xNode = inXmlNode.ChildNodes[i];

                if (xNode.Name.Equals("PARENT"))
                    _xmlNodeParent = xNode;

                if (xNode.InnerText.ToUpper().Equals(text.ToUpper()))
                {
                    _xmlNodeText = xNode;
                    break;
                }

                VerifyNodeText(xNode, text);
            }
        }

        private static void VerifyNodeParent(XmlNode inXmlNode, string text, string lastText)
        {
            int i;
            for (i = 0; i < inXmlNode.ChildNodes.Count; i++)
            {
                XmlNode xNode = inXmlNode.ChildNodes[i];

                if ((xNode.Name.Equals("PARENT")) && (xNode.InnerText.ToUpper().Equals(lastText.ToUpper())))
                {
                    xNode.InnerText = text;
                    break;
                }

                VerifyNodeParent(xNode, text, lastText);
            }
        }

        public void Insert(Template template)
        {
            if (template.Default != null)
                if (template.Default.Value)
                    DisableDefault();

            XmlDocument xmlDocument = GetXmlTemplate();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, template.Text);

            if (_xmlNodeText != null)
                throw new Exception(LanguageDefault.DictionarySelectChart["msgTemplateExists"]);

            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, template.Parent);

            if (_xmlNodeText != null)
            {
                //Adiciona no node SUPORTE
                XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, "TEMPLATE", null);

                XmlElement parentTemplate = xmlDocument.CreateElement("PARENT");
                parentTemplate.InnerText = _xmlNodeText.InnerText;
                XmlElement textTemplate = xmlDocument.CreateElement("TEXT");
                textTemplate.InnerText = template.Text;
                XmlElement defaultTemplate = xmlDocument.CreateElement("DEFAULT");
                if (template.Default != null)
                    defaultTemplate.InnerText = template.Default.Value ? "1" : "0";

                node.AppendChild(parentTemplate);
                node.AppendChild(textTemplate);
                node.AppendChild(defaultTemplate);

                if (_xmlNodeText.ParentNode.Name.Equals("CONJUNTO"))
                    _xmlNodeText.AppendChild(node);
                else
                    _xmlNodeText.ParentNode.AppendChild(node);
            }

            xmlDocument.Save(_pathTemplateXml);
        }

        public void Update(string text, Template template, bool add)
        {
            if (template.Default != null)
                if (template.Default.Value)
                    DisableDefault();

            //Pega os dados do xml
            XmlDocument xmlDocument = GetXmlTemplate();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            if (add)
            {
                //Verifica se existe alguma descrição idêntica
                foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                    VerifyNodeText(node, template.Text);

                if (_xmlNodeText != null)
                    throw new Exception(LanguageDefault.DictionarySelectIndicator["msgTemplateExists"]);
            }

            //Busca o registro a ser alterado
            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, text);

            if (_xmlNodeText == null)
                return;

            if (_xmlNodeText.ParentNode.Name.Equals("CONJUNTO"))
            {
                _xmlNodeText.InnerXml = _xmlNodeText.InnerXml.Replace(_xmlNodeText.InnerText, template.Text);
                xmlDocument.Save(_pathTemplateXml);
            }
            else
            {
                string lastText = _xmlNodeText.InnerText;
                _xmlNodeText.InnerText = template.Text;

                if (template.Default != null)
                    _xmlNodeText.NextSibling.InnerText = template.Default.Value ? "1" : "0";

                xmlDocument.Save(_pathTemplateXml);

                if (!String.IsNullOrEmpty(_xmlNodeParent.InnerText))
                    return;

                nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

                foreach (XmlNode node in nodeList)
                    VerifyNodeParent(node, template.Text, lastText);

                xmlDocument.Save(_pathTemplateXml);
            }
        }

        public void DisableDefault()
        {
            //Pega os dados do xml
            XmlDocument xmlDocument = GetXmlTemplate();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            //Verifica se existe alguma descrição idêntica
            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, "1");

            if (_xmlNodeText == null)
                return;

            _xmlNodeText.InnerXml = "0";
            xmlDocument.Save(_pathTemplateXml);

            _xmlNodeText = null;
        }

        /// <summary>
        /// Remove um item
        /// </summary>
        /// <param name="text"></param>
        public void Delete(string text)
        {
            try
            {
                //Pega os dados do xml
                XmlDocument xmlDocument = GetXmlTemplate();
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

                //Procura o item a ser excluído
                foreach (XmlNode node in nodeList)
                    DeleteNodeFound(null, node, text);

                xmlDocument.Save(_pathTemplateXml);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DeleteNodeFound(XmlNode nodeMain, XmlNode inXmlNode, string text)
        {
            if (!inXmlNode.HasChildNodes)
                return;

            int i;
            for (i = 0; i < inXmlNode.ChildNodes.Count; i++)
            {
                XmlNode xNode = inXmlNode.ChildNodes[i];

                if (xNode.InnerText.Equals(text))
                {
                    if (nodeMain == null)
                        throw new Exception(LanguageDefault.DictionarySelectChart["msgTemplateMainNotRemoved"]);

                    if (inXmlNode.ParentNode.Name.Equals("CONJUNTO"))
                        inXmlNode.InnerXml = inXmlNode.InnerXml.Remove(inXmlNode.InnerXml.IndexOf(xNode.OuterXml), xNode.OuterXml.Length);
                    else
                        nodeMain.RemoveChild(inXmlNode);

                    break;
                }

                DeleteNodeFound(inXmlNode, xNode, text);
            }
        }

        private XmlDocument GetXmlTemplate()
        {
            FileStream fs = new FileStream(_pathTemplateXml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fs);
            fs.Close();
            return xmldoc;
        }

        public void SaveXmlDragDrop(string text, string parent, bool alter)
        {
            //Pega os dados do xml
            XmlDocument xmlDocument = GetXmlTemplate();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            _xmlNode = null;
            foreach (XmlNode node in nodeList)
                VerifyNode(node, text, false, parent);

            _xmlNodeParent = null;
            foreach (XmlNode node in nodeList)
                VerifyNode(node, parent, true);

            _xmlNodeParent.AppendChild(_xmlNode);

            if (alter)
                xmlDocument.Save(_pathTemplateXml);
        }

        private void VerifyNode(XmlNode inXmlNode, string text, bool isParent, params string[] parent)
        {
            if (!inXmlNode.HasChildNodes)
                return;

            int i;
            for (i = 0; i < inXmlNode.ChildNodes.Count; i++)
            {
                XmlNode xNode = inXmlNode.ChildNodes[i];

                if (xNode.InnerText.ToUpper().Equals(text.ToUpper()))
                {
                    if (!isParent)
                    {
                        if ((xNode.Name.Equals("PARENT")) && (parent.Count() > 0))
                        {
                            _xmlNode.ChildNodes[0].InnerText = parent[0];
                            break;
                        }

                        if (_xmlNode == null)
                        {
                            _xmlNode = xNode.ParentNode;

                            if (_xmlNode.Name.Equals("TEMPLATE"))
                            {
                                _xmlNode.ChildNodes[0].InnerText = parent[0];
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (_xmlNodeParent == null)
                            _xmlNodeParent = xNode.ParentNode;

                        break;
                    }
                }

                VerifyNode(xNode, text, isParent, parent);
            }
        }

        #endregion
    }
}