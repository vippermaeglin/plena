using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using M4Core.Entities;
using Telerik.WinControls.Themes;
using M4.Properties;
using M4Data.List;

namespace M4.M4v2.Authentication.Login
{
    public partial class FrmRecovery : Telerik.WinControls.UI.RadForm
    {
        public FrmRecovery()
        {
            InitializeComponent();
            TranslateForm();
            radPanorama1.PanelImage = Resources.bg_pattern_grey_little;
            radPanorama1.PanelImageSize = new Size(250, 150);
            this.ThemeName = (new VisualStudio2012LightTheme()).ThemeName;
            mtbCPF.Select(); 
            mtbCPF.SelectionStart = 0;
            mtbCPF.SelectionLength = 0;
        }

        private void TranslateForm()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            Text = "RECUPERAR";
            button1.Text = "Enviar";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    if (Server.Instance(Program.LanguageDefault).Recovery(mtbCPF.Text, tbEmail.Text))
                    {
                        Cursor = Cursors.Arrow;
                        RadMessageBox.Show("Uma nova senha foi enviada para o email informado.", " ");
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Arrow;
                    RadMessageBox.Show(ex.Message, "");
                }
            }
        }

        public void LoadUserData(UserRegister user)
        {
            mtbCPF.Text = user.CPF;
            tbEmail.Text = user.Email;
        }

        private bool ValidateFields()
        {
            //CPF = 
            if (mtbCPF.Text == "" || !CheckCPF())
            {
                RadMessageBox.Show("CPF inválido.", " ");
                return false;
            }
            //Email = 
            if (tbEmail.Text == "" || !CheckEmail())
            {
                RadMessageBox.Show("Email inválido.", " ");
                return false;
            }
            return true;
        }

        public bool CheckCPF()
        {
            mtbCPF.Text = mtbCPF.Text.Trim(new char[] { ' ' });
            if (mtbCPF.Text == "" || mtbCPF.Text.Length != 11) return false;
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            string cpf = mtbCPF.Text;
            cpf = mtbCPF.Text.Replace(".", "").Replace("-", "").Replace(" ", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public bool CheckEmail()
        {
            tbEmail.Text = tbEmail.Text.Replace(" ", "");
            string[] arroba = tbEmail.Text.Split(new char[] { '@' });
            if (arroba.Length != 2) return false;
            else
            {
                string[] dot = arroba[1].Split(new char[] { '.' });
                if (dot.Length < 2 || dot.Length > 3) return false;
                else if (dot[dot.Length - 1] == "") return false;
            }
            return true;
        }

        private void mtbCPF_KeyDown(object sender, KeyEventArgs e)
        {
            mtbCPF.SelectionStart = mtbCPF.Text.Trim().Length;
            mtbCPF.SelectionLength = 0;
        }
    }
}
