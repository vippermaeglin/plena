using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using M4Core.Entities;
using M4Utils;
using M4Utils.Language;

namespace M4Data.List
{
    public class ListWorkspace
    {
        #region Properties

        public static string _path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\WORKSPACE\\";
        private readonly string _pathXml = _path + "Workspace.xml";
        private readonly string _pathXmlLoad = _path + "WorkspaceLoad.xml";
        private XmlNode _xmlNodeText;
        private XmlNode _xmlNodeParent;
        private XmlNode _xmlNode;
        private static ListWorkspace _listWorkspace;
        public static LanguageDefault LanguageDefault;
        private bool updateDefault;

        #endregion

        #region Instance

        public static ListWorkspace Instance(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
            return _listWorkspace ?? new ListWorkspace();
        }

        public static ListWorkspace Instance()
        {
            return _listWorkspace ?? new ListWorkspace();
        }

        public void SetLanguage(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Carrega os ativos do XML e cria uma lista de Workspace
        /// </summary>
        /// <returns>Lista de Workspace</returns>
        public XmlNodeList Load()
        {
            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_pathXml);
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

        public bool Insert(Workspace workspace)
        {
            if (workspace.Default != null)
                if (workspace.Default.Value)
                    DisableDefault();

            XmlDocument xmlDocument = GetXmlWorkspace();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, workspace.Text);

            if (_xmlNodeText != null)
            {
                MessageBox.Show(LanguageDefault.DictionarySelectChart["msgWorkspaceExists"]);
                return false;
            }

            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, workspace.Parent);

            if (_xmlNodeText != null)
            {
                if ((_xmlNodeText.ParentNode.Name.Equals("SUPORTE")) || (_xmlNodeText.ParentNode.Name.Equals("CONJUNTO")))
                {
                    //Adiciona no node SUPORTE
                    XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, "WORKSPACE", null);

                    XmlElement parentWorkspace = xmlDocument.CreateElement("PARENT");
                    parentWorkspace.InnerText = _xmlNodeText.InnerText;
                    XmlElement textWorkspace = xmlDocument.CreateElement("TEXT");
                    textWorkspace.InnerText = workspace.Text;
                    XmlElement defaultWorkspace = xmlDocument.CreateElement("DEFAULT");
                    if (workspace.Default != null)
                        defaultWorkspace.InnerText = workspace.Default.Value ? "1" : "0";

                    node.AppendChild(parentWorkspace);
                    node.AppendChild(textWorkspace);
                    node.AppendChild(defaultWorkspace);

                    if (_xmlNodeText.ParentNode.Name.Equals("CONJUNTO"))
                        _xmlNodeText.AppendChild(node);
                    else
                        _xmlNodeText.ParentNode.AppendChild(node);
                }
                else
                {
                    //Cria um novo node SUPORTE
                    string xmlNodeText = _xmlNodeText.InnerText;
                    string xmlDefaultNode = _xmlNodeText.NextSibling.InnerText;

                    XmlNode nodeParent = _xmlNodeText.ParentNode.ParentNode;

                    foreach (XmlNode n in nodeList)
                        DeleteNodeFound(null, n, _xmlNodeText.InnerText);

                    XmlNode nodeSuporte = xmlDocument.CreateNode(XmlNodeType.Element, "SUPORTE", null);

                    string nivel = null;
                    int numNivel = 0;

                    _xmlNodeText = null;

                    while (numNivel != -1)
                    {
                        nivel = "Nível " + numNivel;

                        foreach (XmlNode n in nodeList.Cast<XmlNode>().Where(n => _xmlNodeText == null))
                            VerifyNodeText(n, nivel);

                        if (_xmlNodeText == null)
                        {
                            numNivel = -1;
                            continue;
                        }

                        numNivel++;
                        _xmlNodeText = null;
                    }

                    XmlElement parentWorkspace = xmlDocument.CreateElement("PARENT");
                    parentWorkspace.InnerText = "";
                    XmlElement textWorkspace = xmlDocument.CreateElement("TEXT");
                    textWorkspace.InnerText = nivel;

                    nodeSuporte.AppendChild(parentWorkspace);
                    nodeSuporte.AppendChild(textWorkspace);

                    XmlNode node = xmlDocument.CreateNode(XmlNodeType.Element, "WORKSPACE", null);

                    parentWorkspace = xmlDocument.CreateElement("PARENT");
                    parentWorkspace.InnerText = nivel;
                    textWorkspace = xmlDocument.CreateElement("TEXT");
                    textWorkspace.InnerText = xmlNodeText;
                    XmlElement defaultWorkspace = xmlDocument.CreateElement("DEFAULT");
                    defaultWorkspace.InnerText = xmlDefaultNode;

                    node.AppendChild(parentWorkspace);
                    node.AppendChild(textWorkspace);
                    node.AppendChild(defaultWorkspace);

                    nodeSuporte.AppendChild(node);

                    node = xmlDocument.CreateNode(XmlNodeType.Element, "WORKSPACE", null);

                    parentWorkspace = xmlDocument.CreateElement("PARENT");
                    parentWorkspace.InnerText = nivel;
                    textWorkspace = xmlDocument.CreateElement("TEXT");
                    textWorkspace.InnerText = workspace.Text;
                    defaultWorkspace = xmlDocument.CreateElement("DEFAULT");

                    if (workspace.Default != null)
                        defaultWorkspace.InnerText = workspace.Default.Value ? "1" : "0";

                    node.AppendChild(parentWorkspace);
                    node.AppendChild(textWorkspace);
                    node.AppendChild(defaultWorkspace);

                    nodeSuporte.AppendChild(node);
                    nodeParent.AppendChild(nodeSuporte);
                }
            }

            xmlDocument.Save(_pathXml);
            return true;
        }

        public void SaveWorkspaceLoad(Workspace workspace)
        {
            XmlDocument xmlDocument = GetXmlWorkspaceLoad();

            if (xmlDocument == null)
            {
                xmlDocument = new XmlDocument();

                XmlNode nodeWorkspace = xmlDocument.CreateNode(XmlNodeType.Element, "WORKSPACE", null);

                XmlElement textWorkspace = xmlDocument.CreateElement("TEXT");
                textWorkspace.InnerText = workspace.Text;
                XmlElement themeWorkspace = xmlDocument.CreateElement("THEME");
                themeWorkspace.InnerText = workspace.Theme;

                nodeWorkspace.AppendChild(textWorkspace);
                nodeWorkspace.AppendChild(themeWorkspace);

                xmlDocument.AppendChild(nodeWorkspace);
            }
            else
            {
                xmlDocument.GetElementsByTagName("WORKSPACE")[0]["THEME"].InnerText = workspace.Theme;
            }

            xmlDocument.Save(_pathXmlLoad);
        }

        public void Update(string text, Workspace workspace, bool add)
        {
            if (workspace.Default != null)
                if (workspace.Default.Value)
                    DisableDefault();

            //Pega os dados do xml
            XmlDocument xmlDocument = GetXmlWorkspace();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            if (add)
            {
                //Verifica se existe alguma descrição idêntica
                foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                    VerifyNodeText(node, workspace.Text);

                if (_xmlNodeText != null)
                    throw new Exception(LanguageDefault.DictionarySelectIndicator["msgWorkspaceExists"]);
            }

            //Busca o registro a ser alterado
            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, text);

            if (_xmlNodeText == null)
                return;

            if (_xmlNodeText.ParentNode.Name.Equals("CONJUNTO"))
            {
                _xmlNodeText.InnerXml = _xmlNodeText.InnerXml.Replace(_xmlNodeText.InnerText, workspace.Text);
                xmlDocument.Save(_pathXml);
            }
            else
            {
                string lastText = _xmlNodeText.InnerText;
                _xmlNodeText.InnerText = workspace.Text;

                if (workspace.Default != null)
                    _xmlNodeText.NextSibling.InnerText = workspace.Default.Value ? "1" : "0";

                xmlDocument.Save(_pathXml);

                if (!String.IsNullOrEmpty(_xmlNodeParent.InnerText))
                    return;

                nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

                foreach (XmlNode node in nodeList)
                    VerifyNodeParent(node, workspace.Text, lastText);

                xmlDocument.Save(_pathXml);
            }
        }

        public void DisableDefault()
        {
            //Pega os dados do xml
            XmlDocument xmlDocument = GetXmlWorkspace();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            //Verifica se existe alguma descrição idêntica
            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => _xmlNodeText == null))
                VerifyNodeText(node, "1");

            if (_xmlNodeText == null)
                return;

            _xmlNodeText.InnerXml = "0";
            xmlDocument.Save(_pathXml);

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
                XmlDocument xmlDocument = GetXmlWorkspace();
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

                //Procura o item a ser excluído
                foreach (XmlNode node in nodeList)
                    DeleteNodeFound(null, node, text);

                xmlDocument.Save(_pathXml);

                if (!updateDefault)
                    return;

                Workspace workspace = new Workspace
                {
                    Parent = "Workspaces",
                    Text = "Plena",
                    Default = true
                };

                Update("Plena", workspace, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteNodeFound(XmlNode nodeMain, XmlNode inXmlNode, string text)
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
                        throw new Exception(LanguageDefault.DictionarySelectChart["msgWorkspaceMainNotRemoved"]);

                    if (inXmlNode.ParentNode.Name.Equals("CONJUNTO"))
                        inXmlNode.InnerXml = inXmlNode.InnerXml.Remove(inXmlNode.InnerXml.IndexOf(xNode.OuterXml), xNode.OuterXml.Length);
                    else
                    {
                        if (inXmlNode.LastChild.InnerText.Equals("1"))
                            updateDefault = true;

                        nodeMain.RemoveChild(inXmlNode);
                    }

                    break;
                }

                DeleteNodeFound(inXmlNode, xNode, text);
            }
        }

        private XmlDocument GetXmlWorkspace()
        {
            FileStream fs = new FileStream(_pathXml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fs);
            fs.Close();
            return xmldoc;
        }

        public XmlDocument GetXmlWorkspaceLoad()
        {
            if (!File.Exists(_pathXmlLoad))
                return null;

            FileStream fs = new FileStream(_pathXmlLoad, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fs);
            fs.Close();
            return xmldoc;
        }

        public XmlDocument GetXmlWorkspace(string name)
        {
            FileStream fs = new FileStream(_path + name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fs);
            fs.Close();
            return xmldoc;
        }

        public void SaveXmlDragDrop(string text, string parent, bool alter)
        {
            //Pega os dados do xml
            XmlDocument xmlDocument = GetXmlWorkspace();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("CONJUNTO");

            _xmlNode = null;
            foreach (XmlNode node in nodeList)
                VerifyNode(node, text, false, parent);

            _xmlNodeParent = null;
            foreach (XmlNode node in nodeList)
                VerifyNode(node, parent, true);

            _xmlNodeParent.AppendChild(_xmlNode);

            if (alter)
                xmlDocument.Save(_pathXml);
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

                            if (_xmlNode.Name.Equals("WORKSPACE"))
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

        public void CreateWorkspacePath(string name)
        {
            if (!File.Exists(_path + name))
                File.Create(_path + name).Close();
        }

        private static void DeleteWorkspacePath(string name)
        {
            if (File.Exists(_path + name))
                File.Delete(_path + name);
        }

        #endregion
    }
}