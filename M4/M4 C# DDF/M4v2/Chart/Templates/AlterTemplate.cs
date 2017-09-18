using System;
using System.Windows.Forms;
using M4Core.Entities;
using M4Data.List;

namespace M4.M4v2.Chart.Templates
{
    public partial class AlterTemplate : Telerik.WinControls.UI.RadForm
    {
        public string TextTemplate { get; set; }

        public string ParentTemplate { get; set; }

        public bool? DefaultWorkspace { get; set; }

        public bool Insert { get; set; }

        public AlterTemplate()
        {
            InitializeComponent();

            LoadDictionary();

            txtTextTemplate.Focus();
        }

        private void LoadDictionary()
        {
            Text = Program.LanguageDefault.DictionaryTemplate["frmTemplate"];
            cbxDefault.Text = Program.LanguageDefault.DictionaryTemplate["cbxDefault"];
            lblDescription.Text = Program.LanguageDefault.DictionaryTemplate["lblDescription"];
        }

        public void SetTextTemplate()
        {
            txtTextTemplate.Text = TextTemplate;

            if (DefaultWorkspace != null)
                cbxDefault.Checked = DefaultWorkspace.Value;

            txtTextTemplate.Focus();
        }

        public void VisibleOptionDefault(bool visible)
        {
            cbxDefault.Visible = visible;
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
                if (String.IsNullOrEmpty(txtTextTemplate.Text))
                    Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgTemplateEmpty"]);

                if ((!txtTextTemplate.Text.Equals(TextTemplate)) || (DefaultWorkspace != cbxDefault.Checked))
                {
                    if (!Insert)
                    {
                        Template template = new Template
                                                {
                                                    Parent = ParentTemplate,
                                                    Text = txtTextTemplate.Text
                                                };

                        if (DefaultWorkspace == null)
                            template.Default = null;
                        else
                            template.Default = cbxDefault.Checked;

                        ListTemplates.Instance().Update(TextTemplate, template, (!txtTextTemplate.Text.Equals(TextTemplate)));
                    }

                    TextTemplate = txtTextTemplate.Text;

                    if ((DefaultWorkspace != null) || (Insert))
                        DefaultWorkspace = cbxDefault.Checked;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }

        private void AlterTemplateActivated(object sender, EventArgs e)
        {
            txtTextTemplate.Focus();
        }
    }
}
