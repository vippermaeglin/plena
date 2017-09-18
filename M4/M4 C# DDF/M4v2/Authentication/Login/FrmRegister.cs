using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using M4.Properties;
using M4Data.List;
using M4Core.Entities;
using System.Globalization;
using M4Utils;
using Telerik.WinControls.Themes;
using System.Xml;

namespace M4.M4v2.Authentication.Login
{
    public partial class FrmRegister : Telerik.WinControls.UI.RadForm
    {
        public FrmLogin frmLogin;
        public FrmRegister(FrmLogin parent)
        {
            InitializeComponent();
            radPanorama1.PanelImage = Resources.bg_pattern_grey;

            this.ThemeName = (new VisualStudio2012LightTheme()).ThemeName;
            lblSearchCEP.ForeColor = Color.Blue;

            //Comment just for tests (register as User Master):
            cbMaster.Visible = false;
            cbMaster.Checked = false;

            //ThemeResolutionService.ApplicationThemeName = VisualStudio2012LightTheme.ThemeName;
            CreateOptions();
            TranslateForm();
            frmLogin = parent;

            tbName.Select();
        }
        public void LoadUserData(UserRegister user)
        {
            tbName.Text = user.FirstName;
            tbSurName.Text = user.LastName;
            mtbCPF.Text = user.CPF;
            tbEmail.Text = user.Email;
            tbUser.Text = user.UserName;
            ddlBday1.Text = user.Birthday.Value.Day.ToString();
            ddlBday2.SelectedIndex = user.Birthday.Value.Month - 1;
            ddlBday3.Text = user.Birthday.Value.Year.ToString();
            mtbCEP.Text = user.CEP;
            tbNumber.Text = user.Number;
            tbComplement.Text = user.Complement;
        }
        public void TranslateForm()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            this.Text = " REGISTRAR";
            lblName.Text = "Nome *";
            lblSurName.Text = "Sobrenome *";
            lblUser.Text = "Nome de Usuário *";
            lblPassword.Text = "Senha *";
            lblPassword2.Text = "Confirme Senha *";
            lblBday.Text = "Data de Nascimento ";
            lblState.Text = "Estado ";
            lblCity.Text = "Cidade ";
            lblStreet.Text = "Lagradouro ";
            lblNumber.Text = "Número ";
            lblComplement.Text = "Complemento";
            lblSearchCEP.Text = "Buscar CEP";
            lblBairro.Text = "Bairro ";
            lblCEP.Text = "CEP";
            ddlBday1.Text = "";
            ddlBday2.Text = "";
            ddlBday3.Text = "";
            btSend.Text = "Enviar";
            btnCancel.Text = "Cancelar";

        }
        public void CreateOptions()
        {
            for (int i = 1; i < 32; i++) ddlBday1.Items.Add(i.ToString());
            if (Program.LanguageStockChartX == 1)
            {
                ddlBday2.Items.AddRange(new string[] { 
                                                         "Jan",
                                                         "Fev",
                                                         "Mar",
                                                         "Abr",
                                                         "Mai",
                                                         "Jun",
                                                         "Jul",
                                                         "Ago",
                                                         "Set",
                                                         "Out",
                                                         "Nov",
                                                         "Dez",
                                                        });

            }
            else
            {
                ddlBday2.Items.AddRange(new string[] { 
                                                         "Jan",
                                                         "Feb",
                                                         "Mar",
                                                         "Apr",
                                                         "May",
                                                         "Jun",
                                                         "Jul",
                                                         "Aug",
                                                         "Sep",
                                                         "Oct",
                                                         "Nov",
                                                         "Dec"
                                                        });
            }
            ddlState.Items.AddRange(new string[]{
                                                    "Acre (AC)",
                                                    "Alagoas (AL)",
                                                    "Amapá (AP)",
                                                    "Amazonas (AM)",
                                                    "Bahia (BA)",
                                                    "Ceará (CE)",
                                                    "Distrito Federal (DF)",
                                                    "Espírito Santo (ES)",
                                                    "Goiás (GO)",
                                                    "Maranhão (MA)",
                                                    "Mato Grosso (MT)",
                                                    "Mato Grosso do Sul (MS)",
                                                    "Minas Gerais (MG)",
                                                    "Pará (PA) ",
                                                    "Paraíba (PB)",
                                                    "Paraná (PR)",
                                                    "Pernambuco (PE)",
                                                    "Piauí (PI)",
                                                    "Rio de Janeiro (RJ)",
                                                    "Rio Grande do Norte (RN)",
                                                    "Rio Grande do Sul (RS)",
                                                    "Rondônia (RO)",
                                                    "Roraima (RR)",
                                                    "Santa Catarina (SC)",
                                                    "São Paulo (SP)",
                                                    "Sergipe (SE)",
                                                    "Tocantins (TO)"
                                                    });
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 120; i--)
            {
                ddlBday3.Items.Add(i.ToString());
            }
            ddlBday1.SelectedIndex = 0;
            ddlBday2.SelectedIndex = 0;
            ddlBday3.SelectedIndex = 0;
            lblSearchCEP.Cursor = Cursors.Hand;
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            if (!VerifyFields()) return;
            DateTime? Birthday = null;
            try
            {
                Birthday = DateTime.Parse(ddlBday1.Text + "/" + (ddlBday2.SelectedIndex + 1).ToString() + "/" + ddlBday3.Text + " 00:00:00", new CultureInfo("pt-BR"));
            }
            catch (Exception)
            {
                Birthday = new DateTime(1900, 1, 1);
            }
            UserRegister userRegister = new UserRegister()
            {
                UserName = tbUser.Text,
                Password = Utility.Cript(tbPassword.Text),
                Birthday = Birthday,
                CEP = mtbCEP.Text,
                City = tbCity.Text,
                Complement = tbComplement.Text,
                ConfirmPassword = tbPassword2.Text,
                CPF = mtbCPF.Text,
                District = tbBairro.Text,
                Email = tbEmail.Text,
                FirstName = tbName.Text,
                LastName = tbSurName.Text,
                Number = tbNumber.Text,
                State = ddlState.Text,
                Street = tbStreet.Text,
                Tipo = cbMaster.Checked? EPerfil.Master:EPerfil.User_EOD
            };
            try
            {
                Cursor = Cursors.WaitCursor;
                if (Server.Instance(Program.LanguageDefault).Register(userRegister))
                {
                    frmLogin.SetTxtLogin(userRegister.UserName);
                    MessageBox.Show("Usuário registrado com sucesso.", " ");
                    Cursor = Cursors.Arrow;
                    Close();
                }
                else
                {
                    //Server.Instance(Program.LanguageDefault).ReloadLogin(userRegister.UserName,userRegister.Password,"");
                    Cursor = Cursors.Arrow;
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
                RadMessageBox.Show(ex.Message, " ");
                //Server.Instance(Program.LanguageDefault).ReloadLogin(userRegister.UserName, userRegister.Password, "");
            }
        }

        private bool VerifyFields()
        {
            mtbCEP.Text = mtbCEP.Text.Replace(" ", "");
            mtbCPF.Text = mtbCPF.Text.Replace(" ", "");
            tbEmail.Text = tbEmail.Text.Replace(" ", "");
            //UserName = 
            if (tbUser.Text == "")
            {
                RadMessageBox.Show("Nome de usuário inválido.", " ");
                return false;
            }
            //Password = 
            if (tbPassword.Text == "" || tbPassword2.Text == "" || tbPassword.Text != tbPassword2.Text || tbPassword.Text == tbUser.Text)
            {
                RadMessageBox.Show("Senha inválida.", " ");
                return false;
            }
            //Birthday = 
            DateTime birthday = new DateTime();
            try
            {
                birthday = DateTime.Parse(ddlBday1.Text + "/" + (ddlBday2.SelectedIndex + 1).ToString() + "/" + ddlBday3.Text + " 00:00:00", new CultureInfo("pt-BR"));
            }
            catch { }
            if (birthday == DateTime.MinValue)
            {
                //RadMessageBox.Show("Data de nascimento inválida.");
                //return false;
                ddlBday1.Text = "";
                ddlBday2.Text = "";
                ddlBday3.Text = "";
            }
            //CEP =
            /*if (mtbCEP.Text == "")
            {
                RadMessageBox.Show("CEP inválido.");
                return false;
            }
            //District = 
            if(tbBairro.Text=="")
            {
                RadMessageBox.Show("Bairro inválido.");
                return false;
            }
            //Number = 
            if (tbNumber.Text == "")
            {
                RadMessageBox.Show("Número da residência inválido.");
                return false;
            }
            //State = 
            if (ddlState.Text == "")
            {
                RadMessageBox.Show("Estado inválido.");
                return false;
            }
            //Street = 
            if (tbStreet.Text == "")
            {
                RadMessageBox.Show("Lagradouro inválido.");
                return false;
            }      
            //City = 
            if(tbCity.Text=="")
            {
                RadMessageBox.Show("Cidade inválida.");
                return false;
            }*/
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
            //FirstName = 
            if (tbName.Text == "")
            {
                RadMessageBox.Show("Nome inválido.", " ");
                return false;
            }
            //LastName = 
            if (tbSurName.Text == "")
            {
                RadMessageBox.Show("Sobrenome inválido.", " ");
                return false;
            }
            return true;
        }

        private void lblSearchCEP_Click(object sender, EventArgs e)
        {
            mtbCEP.Text = mtbCEP.Text.Replace(" ", "");
            mtbCEP.Text = mtbCEP.Text.Trim(new char[] { ' ' });
            if (mtbCEP.Text == "" || mtbCEP.Text.Length < 8)
            {
                RadMessageBox.Show("CEP inválido.", " ");
                return;
            }
            string _uf, _cidade, _bairro, _tipo_lagradouro, _lagradouro, _resultato_txt;
            string _resultado;
            try
            {
                _uf = "";
                _cidade = "";
                _bairro = "";
                _tipo_lagradouro = "";
                _lagradouro = "";
                _resultato_txt = "CEP não  encontrado";
                DataSet ds = new DataSet();
                Cursor = Cursors.WaitCursor;
                ds.ReadXml("http://cep.republicavirtual.com.br/web_cep.php?cep=" + mtbCEP.Text.Trim(new char[] { ' ' }) + "&formato=xml");
                Cursor = Cursors.Arrow;
                Application.UseWaitCursor = false;
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _resultado = ds.Tables[0].Rows[0]["resultado"].ToString();
                        switch (_resultado)
                        {
                            case "1":
                                _uf = ds.Tables[0].Rows[0]["uf"].ToString().Trim();
                                _cidade = ds.Tables[0].Rows[0]["cidade"].ToString().Trim();
                                _bairro = ds.Tables[0].Rows[0]["bairro"].ToString().Trim();
                                _tipo_lagradouro = ds.Tables[0].Rows[0]["tipo_logradouro"].ToString().Trim();
                                _lagradouro = ds.Tables[0].Rows[0]["logradouro"].ToString().Trim();
                                _resultato_txt = "CEP completo";
                                break;
                            case "2":
                                _uf = ds.Tables[0].Rows[0]["uf"].ToString().Trim();
                                _cidade = ds.Tables[0].Rows[0]["cidade"].ToString().Trim();
                                _bairro = "";
                                _tipo_lagradouro = "";
                                _lagradouro = "";
                                _resultato_txt = "CEP  único";
                                break;
                            default:
                                _uf = "";
                                _cidade = "";
                                _bairro = "";
                                _tipo_lagradouro = "";
                                _lagradouro = "";
                                _resultato_txt = "CEP não  encontrado";
                                break;
                        }
                    }
                }
                if (_resultato_txt == "CEP completo")
                {
                    foreach (Telerik.WinControls.UI.RadListDataItem item in ddlState.Items)
                    {
                        if (item.Text.Contains("(" + _uf + ")")) ddlState.SelectedItem = item;
                    }
                    tbCity.Text = _cidade;
                    tbBairro.Text = _bairro;
                    tbStreet.Text = _lagradouro;

                }
                else RadMessageBox.Show("CEP não encontrado.", "Erro");
            }
            catch (Exception)
            {
                Cursor = Cursors.Arrow;
                RadMessageBox.Show("Não foi possível buscar o CEP.", "Erro");

            }
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

        private void mtbCEP_KeyDown(object sender, KeyEventArgs e)
        {
            mtbCEP.SelectionStart = mtbCEP.Text.Trim().Length;
            mtbCEP.SelectionLength = 0;

        }

        private void FrmRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmLogin.ThemeName = "Windows7";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }




    }
}
