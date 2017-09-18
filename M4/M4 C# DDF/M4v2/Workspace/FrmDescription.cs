using System;
using System.Windows.Forms;
using M4Data.List;

namespace M4.M4v2.Workspace
{
    public partial class FrmDescription : Telerik.WinControls.UI.RadForm
    {
        private frmMain2 frmMain;
        public string TextWorkspace { get; set; }

        public string ParentWorkspace { get; set; }

        public bool? DefaultWorkspace { get; set; }

        public bool Insert { get; set; }

        public FrmDescription(frmMain2 _frmMain)
        {
            frmMain = _frmMain;
            InitializeComponent();

            txtTextWorkspace.Focus();

            LoadDictionary();
        }

        private void LoadDictionary()
        {
            Text = Program.LanguageDefault.DictionaryWorkspace["mnuRenameWorkspace"];

            lblWorkspace.Text = Program.LanguageDefault.DictionaryWorkspace["workspace"];
            cbxDefaultWorkspace.Text = Program.LanguageDefault.DictionaryWorkspace["mnuDefaultWorkspace"];

            btnOk.Text = Program.LanguageDefault.DictionarySelectChart["btnOk"];
            btnCancel.Text = Program.LanguageDefault.DictionarySelectChart["btnCancel"];
        }

        public void SetPropertiesWorkspace()
        {
            txtTextWorkspace.Text = TextWorkspace;

            if (DefaultWorkspace != null)
                cbxDefaultWorkspace.Checked = DefaultWorkspace.Value;

            txtTextWorkspace.Focus();
        }

        public void VisibleOptionDefault(bool visible)
        {
            cbxDefaultWorkspace.Visible = visible;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtTextWorkspace.Text))
                    Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgWorkspaceEmpty"]);

                if ((!txtTextWorkspace.Text.Equals(TextWorkspace)) || (DefaultWorkspace != cbxDefaultWorkspace.Checked))
                {
                    if (!Insert)
                    {
                        M4Core.Entities.Workspace workspace = new M4Core.Entities.Workspace
                        {
                            Parent = ParentWorkspace,
                            Text = txtTextWorkspace.Text
                        };

                        if (DefaultWorkspace == null)
                            workspace.Default = null;
                        else
                            workspace.Default = cbxDefaultWorkspace.Checked;

                        ListWorkspace.Instance(Program.LanguageDefault).Update(TextWorkspace, workspace, (!txtTextWorkspace.Text.Equals(TextWorkspace)));
                    }

                    TextWorkspace = txtTextWorkspace.Text;

                    if ((DefaultWorkspace != null) || (Insert))
                        DefaultWorkspace = cbxDefaultWorkspace.Checked;
                }
                
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }

        private void FrmDescription_Shown(object sender, EventArgs e)
        {
            txtTextWorkspace.Focus();
            VisibleOptionDefault(false);
        }
    }
}
