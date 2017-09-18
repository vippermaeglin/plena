using System;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Xml;
using M4Data.List;
using Telerik.WinControls.UI;

namespace M4.M4v2.Workspace
{
    public partial class FrmWorkspace : RadForm
    {
        private readonly frmMain2 _frmMain;
        private string _workspaceText;
        private string _workspaceDefault;
        private string _archiveName;
        public string WorkspaceLoaded { get; set; }

        #region Initialize

        public FrmWorkspace(frmMain2 frmMain)
        {
            InitializeComponent();

            _frmMain = frmMain;

            LoadDataWorkspace();

            LoadDictionary();
        }

        private void LoadDictionary()
        {
            Text = Program.LanguageDefault.DictionaryWorkspace["frmWorkspace"];
            btnDismiss.Text = Program.LanguageDefault.DictionaryWorkspace["btnDismiss"];

            mnuDefaultWorkspace.Text = Program.LanguageDefault.DictionaryWorkspace["mnuDefaultWorkspace"];
            mnuDefaultWorkspace.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            mnuLoadWorkspace.Text = Program.LanguageDefault.DictionaryWorkspace["mnuLoadWorkspace"];
            mnuRenameWorkspace.Text = Program.LanguageDefault.DictionaryWorkspace["mnuRenameWorkspace"];
        }

        private void LoadDataWorkspace()
        {
            trvWorkspace.Nodes.Clear();

            XmlNodeList nodeList = ListWorkspace.Instance().Load()[0].SelectNodes("SUPORTE");

            int i = 0;
            foreach (XmlNode nivelSuperior in nodeList)
            {
                AddNodeWorkspace(nivelSuperior, trvWorkspace.Nodes.Count > 0 ? trvWorkspace.Nodes[i] : null);
                i++;
            }

            trvWorkspace.Nodes[0].Selected = true;
            trvWorkspace.ExpandAll();
        }

        private void AddNodeWorkspace(XmlNode inXmlNode, RadTreeNode inTreeNode)
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

                    if (_workspaceText == null)
                        _workspaceText = (xNode.Name.Equals("TEXT")) ? xNode.InnerText : null;

                    if (_workspaceDefault == null)
                        _workspaceDefault = ((xNode.NextSibling != null) && (xNode.NextSibling.Name.Equals("DEFAULT"))) ? xNode.NextSibling.InnerText : null;

                    if (String.IsNullOrEmpty(_workspaceText))
                    {
                        if ((xNode.Name.Equals("WORKSPACE")) || (xNode.Name.Equals("SUPORTE")))
                        {
                            AddNodeWorkspace(xNode, inTreeNode != null ? inTreeNode.Nodes[inTreeNode.Nodes.Count - 1] : trvWorkspace.Nodes[0]);
                        }

                        continue;
                    }

                    if ((_workspaceText.ToUpper().Equals("WORKSPACE")) || (_workspaceText.ToUpper().Equals("WORKSPACES")))
                        _workspaceText = Program.LanguageDefault.DictionaryWorkspace["workspace"];

                    RadTreeNode radTreeNode = new RadTreeNode
                    {
                        Name = _workspaceText,
                        Text = _workspaceText,
                        Value = _workspaceDefault
                    };

                    if (false/*(_workspaceDefault != null) && (_workspaceDefault.Equals("1"))*/)
                        radTreeNode.ImageIndex = 0;
                    else
                        radTreeNode.ImageIndex = -1;

                    if (inTreeNode == null)
                        trvWorkspace.Nodes.Add(radTreeNode);
                    else
                        inTreeNode.Nodes.Add(radTreeNode);

                    _workspaceText = null;
                    _workspaceDefault = null;
                    AddNodeWorkspace(xNode, inTreeNode != null ? inTreeNode.Nodes[inTreeNode.Nodes.Count - 1] : trvWorkspace.Nodes[0]);
                }
            }
            else
            {
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
        }

        #endregion

        private void BtnNewClick(object sender, EventArgs e)
        {
            InsertTemplate(null);
            LoadDataWorkspace();
        }

        public void InsertTemplate(string textWorkspace)
        {
            FrmDescription frmDescription = new FrmDescription (_frmMain){ Insert = true };

            if (!String.IsNullOrEmpty(textWorkspace))
            {
                frmDescription.TextWorkspace = textWorkspace;
                frmDescription.SetPropertiesWorkspace();
            }

            DialogResult result = frmDescription.ShowDialog();

            if (!result.Equals(DialogResult.OK))
                return;

            try
            {
                M4Core.Entities.Workspace workspace = new M4Core.Entities.Workspace
                {
                    Parent = "Workspaces",
                    Text = frmDescription.TextWorkspace,
                    Default = frmDescription.DefaultWorkspace
                };

                if (ListWorkspace.Instance(Program.LanguageDefault).Insert(workspace))
                {
                    _archiveName = frmDescription.TextWorkspace + ".xml";

                    SaveConfigMain();
                }
                else
                {
                    InsertTemplate(frmDescription.TextWorkspace);
                }
            }
            finally
            {
                WorkspaceLoaded = frmDescription.TextWorkspace;
            }
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            if (trvWorkspace.SelectedNode == null) return;
            FrmDescription frmDescription = new FrmDescription(_frmMain)
            {
                ParentWorkspace = (trvWorkspace.SelectedNode.Parent != null) ? trvWorkspace.SelectedNode.Parent.Text : null,
                TextWorkspace = trvWorkspace.SelectedNode.Text,
                Insert = false
            };

            if (trvWorkspace.SelectedNode.Value == null)
                frmDescription.DefaultWorkspace = null;
            else
                frmDescription.DefaultWorkspace = (trvWorkspace.SelectedNode.Value.Equals("1")) ? true : false;

            frmDescription.SetPropertiesWorkspace();
            //frmDescription.VisibleOptionDefault(trvWorkspace.SelectedNode.Value != null);

            if (frmDescription.ShowDialog() != DialogResult.OK)
                return;

            DeleteWorkspace(ListWorkspace._path + trvWorkspace.SelectedNode.Text +"\\");

            _archiveName = frmDescription.TextWorkspace.Trim().ToUpper() + ".xml";
            _archiveName = trvWorkspace.SelectedNode.Text + ".xml";

            SaveConfigMain();

            LoadDataWorkspace();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            try
            {
                //Só remove o item se não existir sub-itens.
                if (trvWorkspace.SelectedNode.Nodes.Count > 0)
                {
                    Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgNotRootRemove"], " ");
                    return;
                }

                if (Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgRemoveWorkspace"], "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ListWorkspace.Instance(Program.LanguageDefault).Delete(trvWorkspace.SelectedNode.Text);

                    if (Directory.Exists(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\"))
                    {
                        ChangeFileAccess(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\");
                        Directory.Delete(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\",true);
                    }
                    //if (File.Exists(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\" + trvWorkspace.SelectedNode.Text.Trim() + ".xml"))
                    //    File.Delete(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\" + trvWorkspace.SelectedNode.Text.Trim() + ".xml");
                    //if (File.Exists(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\" + "ATIVOS_" + trvWorkspace.SelectedNode.Text.Trim() + ".xml"))
                    //    File.Delete(ListWorkspace._path + trvWorkspace.SelectedNode.Text.Trim() + "\\" + "ATIVOS_" + trvWorkspace.SelectedNode.Text.Trim() + ".xml");
                    LoadDataWorkspace();
                    btnApply.Enabled = false;
                    btnRemove.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void DeleteWorkspace(string workspaceName)
        {
            try
            {
                ChangeFileAccess(workspaceName);
                if (Directory.Exists(workspaceName))
                    Directory.Delete(workspaceName, true);
            }
            catch(UnauthorizedAccessException accessException)
            {
                //string userName = Environment.UserName;
                //foreach (var file in Directory.EnumerateFiles(workspaceName))
                //{
                //    FileInfo fileInfo = new FileInfo(workspaceName + file);
                //    FileSecurity fileSecurity = fileInfo.GetAccessControl();
                //    fileSecurity.AddAccessRule(new FileSystemAccessRule(userName, FileSystemRights.Delete, AccessControlType.Allow));
                //    File.SetAccessControl(workspaceName + file, fileSecurity);
                //}
                //foreach (var directory in Directory.EnumerateDirectories(workspaceName))
                //{
                //    DirectoryInfo DirectoryInfo = new DirectoryInfo(directory);
                //    DirectorySecurity DirectorySecurity = DirectoryInfo.GetAccessControl();
                //    DirectorySecurity.AddAccessRule(new FileSystemAccessRule(userName, FileSystemRights.Delete, AccessControlType.Allow));
                //    Directory.SetAccessControl(directory, DirectorySecurity);
                //}

                //foreach (var file in Directory.GetFileSystemEntries(workspaceName))
                //{
                //    File.SetAttributes(file,FileAttributes.Normal);
                //}
                ChangeFileAccess(workspaceName);
                if (Directory.Exists(workspaceName))
                    Directory.Delete(workspaceName, true);
            }
        }
        private static void ChangeFileAccess(string path)
        {
            string userName = Environment.UserName;
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                DirectoryInfo DirectoryInfo = new DirectoryInfo(directory);
                DirectorySecurity DirectorySecurity = DirectoryInfo.GetAccessControl();
                DirectorySecurity.AddAccessRule(new FileSystemAccessRule(userName, FileSystemRights.Delete, AccessControlType.Allow));
                Directory.SetAccessControl(directory, DirectorySecurity);
                ChangeFileAccess(directory);
            }
            foreach (var file in Directory.EnumerateFiles(path))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }
        }

        private void BtnDismissClick(object sender, EventArgs e)
        {
            Close();
        }

        private void TrvWorkspaceDragOverNode(object sender, RadTreeViewDragCancelEventArgs e)
        {
            if ((e.DropPosition == DropPosition.AsChildNode) || (e.TargetNode.Parent == null))
                e.Cancel = true;

            try
            {
                ListWorkspace.Instance().SaveXmlDragDrop(e.Node.Text, e.TargetNode.Parent.Text, false);
            }
            catch
            {
                e.Cancel = true;
            }
        }

        private void TrvWorkspaceDragEnded(object sender, RadTreeViewDragEventArgs e)
        {
            try
            {
                ListWorkspace.Instance().SaveXmlDragDrop(e.Node.Text, e.TargetNode.Parent.Text, true);
                LoadDataWorkspace();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
            }
        }

        private void CmnuWorkspaceDropDownClosed(object sender, EventArgs e)
        {
            if (((RadContextMenu)sender).DropDown.ClickedItem == null)
                return;

            if (((RadContextMenu)sender).DropDown.ClickedItem.Text.ToUpper() == Program.LanguageDefault.DictionaryWorkspace["mnuDefaultWorkspace"].ToUpper())
            {
                M4Core.Entities.Workspace workspace = new M4Core.Entities.Workspace
                                                          {
                                                              Parent = trvWorkspace.SelectedNode.Parent.Text,
                                                              Text = trvWorkspace.SelectedNode.Text,
                                                              Default = true
                                                          };

                ListWorkspace.Instance(Program.LanguageDefault).Update(trvWorkspace.SelectedNode.Text, workspace, false);

                LoadDataWorkspace();
            }
            else if (((RadContextMenu)sender).DropDown.ClickedItem.Text.ToUpper() == Program.LanguageDefault.DictionaryWorkspace["mnuLoadWorkspace"].ToUpper())
            {
                _frmMain.RestoreWorkspace(trvWorkspace.SelectedNode.Text.Trim());
                WorkspaceLoaded = trvWorkspace.SelectedNode.Text.Trim();

                ////create a custom palette
                //NPalette palette = new NUIPalette
                //                       {
                //                           ControlDark = Color.FromArgb(170, 170, 170),
                //                           ControlLight = Color.FromArgb(170, 170, 170),
                //                           ControlBorder = Color.FromArgb(170, 170, 170),
                //                           Control = Color.FromArgb(170, 170, 170),
                //                       };

                ////apply the palette
                //NUIManager.ApplyPalette(_frmMain, palette);
            }
            else if (((RadContextMenu)sender).DropDown.ClickedItem.Text.ToUpper() == Program.LanguageDefault.DictionaryWorkspace["mnuRenameWorkspace"].ToUpper())
            {
                BtnApplyClick(null, null);
            }
        }

        private void CmnuWorkspaceDropDownOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((trvWorkspace.SelectedNode == null) || (trvWorkspace.SelectedNode.Parent == null))
            {
                e.Cancel = true;
                return;
            }

            cmnuWorkspace.Items[2].Enabled = (!trvWorkspace.SelectedNode.Text.ToUpper().Equals("PLENA")) &&
                                             (!trvWorkspace.SelectedNode.Text.ToUpper().Equals("WORKSPACES"));

            cmnuWorkspace.Items[0].Enabled = trvWorkspace.SelectedNode.ImageIndex != 0;
        }

        private void SaveConfigMain()
        {
            ManagerWorkspace.Instance().SaveConfigMain(_archiveName, _frmMain);
        }

        private void TrvWorkspaceSelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node == null)
                return;

            btnApply.Enabled = (!e.Node.Text.ToUpper().Equals("PLENA")) && (!e.Node.Text.ToUpper().Equals("WORKSPACES"));
            btnRemove.Enabled = (!e.Node.Text.ToUpper().Equals("PLENA")) && (!e.Node.Text.ToUpper().Equals("WORKSPACES"));
        }

        private void TrvWorkspaceDoubleClick(object sender, EventArgs e)
        {
            if (trvWorkspace.SelectedNode.Text.ToUpper().Equals(Program.LanguageDefault.DictionaryWorkspace["workspace"].ToUpper()))
                return;

            _frmMain.RestoreWorkspace(trvWorkspace.SelectedNode.Text.Trim());
            WorkspaceLoaded = trvWorkspace.SelectedNode.Text.Trim();

            //create a custom palette
            //NPalette palette = new NUIPalette
            //{
            //    ControlDark = Color.FromArgb(170, 170, 170),
            //    ControlLight = Color.FromArgb(170, 170, 170),
            //    ControlBorder = Color.FromArgb(170, 170, 170),
            //    Control = Color.FromArgb(170, 170, 170),
            //};

            ////apply the palette
            //NUIManager.ApplyPalette(_frmMain, palette);
        }

        private void FrmWorkspaceFormClosing(object sender, FormClosingEventArgs e)
        {
            _frmMain.LoadWorkspaces();
            _frmMain.WorkspaceLoaded(WorkspaceLoaded);
        }

        private void trvWorkspace_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            if ((e.Node == null) || (!e.Node.Text.ToUpper().Equals(Program.LanguageDefault.DictionaryWorkspace["workspace"].ToUpper())))
                return;

            e.Cancel = true;
            return;
        }

        private void FrmWorkspace_Load(object sender, EventArgs e)
        {
            trvWorkspace.TreeViewElement.BackColor = Utils.GetDefaultBackColor();
        }
    }
}
