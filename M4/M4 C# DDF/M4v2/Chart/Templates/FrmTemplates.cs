using System;
using System.Windows.Forms;
using System.Xml;
using M4Core.Entities;
using M4Data.List;
using Telerik.WinControls.UI;

namespace M4.M4v2.Chart.Templates
{
    public partial class FrmTemplates : RadForm
    {
        private string _templateText;
        private string _templateParent;

        public FrmTemplates()
        {
            InitializeComponent();

            LoadData();
            ListTemplates.Instance().SetLanguage(Program.LanguageDefault);
        }

        private void AddNode(XmlNode inXmlNode, RadTreeNode inTreeNode)
        {
            XmlNode xNode;
            XmlNodeList nodeList;

            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                int i;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];

                    if (_templateText == null)
                        _templateText = (xNode.Name.Equals("TEXT")) ? xNode.InnerText : null;

                    if (_templateParent == null)
                        _templateParent = (xNode.Name.Equals("PARENT")) ? xNode.InnerText : null;

                    if (String.IsNullOrEmpty(_templateText))
                    {
                        if ((xNode.Name.Equals("TEMPLATE")) || (xNode.Name.Equals("SUPORTE")))
                            AddNode(xNode, inTreeNode != null ? inTreeNode.Nodes[inTreeNode.Nodes.Count - 1] : trvTemplates.Nodes[0]);

                        continue;
                    }

                    if (_templateText.ToUpper().Equals("TEMPLATES"))
                        _templateText = Program.LanguageDefault.DictionaryTemplate["frmTemplate"];

                    if (inTreeNode == null)
                        trvTemplates.Nodes.Add(new RadTreeNode {Name = _templateText, Text = _templateText, Value = _templateParent});
                    else
                        inTreeNode.Nodes.Add(new RadTreeNode { Name = _templateText, Text = _templateText, Value = _templateParent });

                    _templateText = null;
                    _templateParent = null;
                    AddNode(xNode, inTreeNode != null ? inTreeNode.Nodes[inTreeNode.Nodes.Count - 1] : trvTemplates.Nodes[0]);
                }
            }
            else
            {
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
        }

        private void LoadData()
        {
            trvTemplates.Nodes.Clear();

            XmlNodeList nodeList = ListTemplates.Instance().LoadTemplates()[0].SelectNodes("SUPORTE");

            int i = 0;
            foreach (XmlNode nivelSuperior in nodeList)
            {
                AddNode(nivelSuperior, trvTemplates.Nodes.Count > 0 ? trvTemplates.Nodes[i] : null);
                i++;
            }

            //trvTemplates.Nodes[0].Selected = true;
            trvTemplates.ExpandAll();
        }

        private void LoadDictionary()
        {
            Text = Program.LanguageDefault.DictionarySelectIndicator["FrmSelectIndicatorTitle"];
            btnDismiss.Text = Program.LanguageDefault.DictionarySelectChart["btnDismiss"];
        }

        private void BtnNewClick(object sender, EventArgs e)
        {
            InsertTemplate(null);
            LoadData();
        }

        public void InsertTemplate(string textTemplate)
        {
            AlterTemplate alterTemplate = new AlterTemplate { Insert = true };
            DialogResult result = alterTemplate.ShowDialog();

            if (!result.Equals(System.Windows.Forms.DialogResult.OK))
                return;

            try
            {
                Template template = new Template
                {
                    Parent = trvTemplates.SelectedNode.Text,
                    Text = alterTemplate.TextTemplate
                };
                ListTemplates.Instance().Insert(template);
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgTemplateAdded"]);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                InsertTemplate(alterTemplate.TextTemplate);
            }
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            AlterTemplate alterTemplate = new AlterTemplate
                                              {
                                                  ParentTemplate = (trvTemplates.SelectedNode.Parent != null) ? trvTemplates.SelectedNode.Parent.Text : null,
                                                  TextTemplate = trvTemplates.SelectedNode.Text,
                                                  Insert = false
                                              };
            alterTemplate.SetTextTemplate();
            alterTemplate.ShowDialog();
            trvTemplates.SelectedNode.Text = alterTemplate.TextTemplate;
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            //Só remove o item se não existir sub-itens.
            if (trvTemplates.SelectedNode.Nodes.Count > 0)
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgNotRootRemove"]);
                return;
            }

            ListTemplates.Instance().Delete(trvTemplates.SelectedNode.Text);
            trvTemplates.SelectedNode.Remove();

            Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgTemplateDeleted"]);
        }

        private void BtnDismissClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
