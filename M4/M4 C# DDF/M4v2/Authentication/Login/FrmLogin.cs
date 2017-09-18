using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using M4.Properties;
using M4Data.List;
using M4Utils;
using M4Core.Entities;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using M4.M4v2.Workspace;
using Telerik.WinControls.Themes;

namespace M4.M4v2.Authentication.Login
{
    public partial class FrmLogin : RadForm
    {
        public const int WmSize = 5;
        private RadTitleBarElement _titleBar;
        private bool _isFormMoving;

        #region Initialization

        public FrmLogin()
        {
            try
            {
                Program.Log.Info("\n[FrmLogin] Initializing Component... ");
                InitializeComponent();
                Program.Log.Info("\n[FrmLogin] Set members... ");
                this.ThemeName = (new VisualStudio2012LightTheme()).ThemeName;
                lblForgotPassword.ForeColor = Color.Blue;
                lblRegister.ForeColor = Color.Blue;
                panorama.ScrollingBackground = true;
                panorama.PanelImage = Resources.bg_pattern;
                panorama.PanoramaElement.BackgroundImagePrimitive.ImageLayout = ImageLayout.Tile;
                panorama.SizeChanged += radTilePanel1_SizeChanged;
                panorama.ScrollBarAlignment = HorizontalScrollAlignment.Bottom;
                panorama.ScrollBarThickness = 5;
                panorama.PanoramaElement.GradientStyle = GradientStyles.Solid;
                panorama.PanoramaElement.DrawFill = true;
                FormElement.TitleBar.MaxSize = new Size(0, 1);
                Text = "Login";

                Program.Log.Info("\n[FrmLogin] PrepareTitleBar()... ");
                PrepareTitleBar();

                Program.Log.Info("\n[FrmLogin] TranslateForm()... ");
                TranslateForm();

                Program.Log.Info("\n[FrmLogin] VerifyLoginSaved()... ");
                VerifyLoginSaved();

                txtLogin.Focus();
            }
            catch(Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("FrmLogin(): " + ex.Message, " ");
                Program.Log.Info("\n[FrmLogin] FrmLogin() "+ex.Message);
            }
        }

        private void VerifyLoginSaved()
        {
            try
            {
                Program.LoginAuthentication = ListLoginAuthentication.Instance(Program.LanguageDefault).LoginSaved();

                if ((Program.LoginAuthentication == null) || (!Program.LoginAuthentication.Remember))
                    return;

                txtLogin.Text = Program.LoginAuthentication.Login.ToLower();
                txtPassword.Text = Utility.Decript(Program.LoginAuthentication.Password);
                cbxLembrar.Checked = Program.LoginAuthentication.Remember;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("VerifyLoginSaved(): " + ex.Message, " ");
            }
        }

        private void TranslateForm()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            Text = Program.LanguageDefault.DictionaryLogin["frmLogin"];

            lblPassword.Text = Program.LanguageDefault.DictionaryLogin["lblPassword"];
            lblLogin.Text = Program.LanguageDefault.DictionaryLogin["lblLogin"];
            btnGuest.Text = Program.LanguageDefault.DictionaryLogin["btnGuest"];
            lblForgotPassword.Text = Program.LanguageDefault.DictionaryLogin["lblForgotPassword"];
            lblRegister.Text = Program.LanguageDefault.DictionaryLogin["lblRegister"];
            btnOk.Text = Program.LanguageDefault.DictionaryLogin["btnOk"];
            cbxLembrar.Text = Program.LanguageDefault.DictionaryLogin["cbxLembrar"];
            cbxOffline.Text = Program.LanguageDefault.DictionaryLogin["cbxOffline"];

            lblVersion.Text = Program.LanguageDefault.DictionaryLogin["lblVersion"];
        }

        private void PrepareTitleBar()
        {
            _titleBar = new RadTitleBarElement
                            {
                                AllowResize = false,
                                MaxSize = new Size(0, 30),
                                PositionOffset = new SizeF(20, 15)
                            };

            _titleBar.FillPrimitive.Visibility = ElementVisibility.Hidden;
            _titleBar.Children[1].Visibility = ElementVisibility.Hidden;

            _titleBar.CloseButton.Parent.PositionOffset = new SizeF(-20, -10);
            //_titleBar.CloseButton.MinSize = new Size(50, 50);
            _titleBar.CloseButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            _titleBar.MinimizeButton.Visibility = ElementVisibility.Hidden;
            _titleBar.MaximizeButton.Visibility = ElementVisibility.Hidden;

            _titleBar.CloseButton.SetValue(RadFormElement.IsFormActiveProperty, true);

            _titleBar.Close += TitleBarClose;
            panorama.PanoramaElement.PanGesture += radTilePanel1_PanGesture;
            panorama.PanoramaElement.Children.Add(_titleBar);
        }

        private void TitleBarClose(object sender, EventArgs args)
        {
            Application.Exit();
        }

        #endregion

        #region Event Handlers

        void radTilePanel1_PanGesture(object sender, PanGestureEventArgs e)
        {
            if (e.IsBegin && _titleBar.ControlBoundingRectangle.Contains(e.Location))
            {
                _isFormMoving = true;
            }

            if (_isFormMoving)
            {
                Location = new Point(Location.X + e.Offset.Width, Location.Y + e.Offset.Height);
            }
            else
            {
                e.Handled = false;
            }

            if (e.IsEnd)
            {
                _isFormMoving = false;
            }
        }

        void radTilePanel1_SizeChanged(object sender, EventArgs e)
        {
            Width = 398;
            Height = 290;
            int width = panorama.Width + Math.Max((panorama.PanoramaElement.ScrollBar.Maximum - panorama.Width) / 4, 1);
            panorama.PanelImageSize = new Size(width, 321);
            panorama.PanoramaElement.UpdateViewOnScroll();
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmSize)
            {
                _titleBar.CloseButton.SetValue(RadFormElement.FormWindowStateProperty, WindowState);
                _titleBar.MinimizeButton.SetValue(RadFormElement.FormWindowStateProperty, WindowState);
                _titleBar.MaximizeButton.SetValue(RadFormElement.FormWindowStateProperty, WindowState);
            }

            base.WndProc(ref m);
        }

        protected override FormControlBehavior InitializeFormBehavior()
        {
            return new MyFormBehavior(this, true);
        }

        private void VerifyFields()
        {
            if (String.IsNullOrEmpty(txtLogin.Text))
                throw new Exception(Program.LanguageDefault.DictionaryLogin["txtLoginEmpty"]);

            if (String.IsNullOrEmpty(txtPassword.Text))
                throw new Exception(Program.LanguageDefault.DictionaryLogin["txtPasswordEmpty"]);
        }

        private void EnableFields(bool enable)
        {
            txtLogin.Enabled = enable;
            txtLogin.Refresh();
            txtPassword.Enabled = enable;
            txtPassword.Refresh();
            lblLogin.Enabled = enable;
            lblLogin.Refresh();
            lblPassword.Enabled = enable;
            lblPassword.Refresh();
            cbxLembrar.Enabled = enable;
            cbxLembrar.Refresh();
            cbxOffline.Enabled = enable;
            cbxOffline.Refresh();
            lblForgotPassword.Enabled = enable;
            lblForgotPassword.Refresh();
            lblRegister.Enabled = enable;
            lblRegister.Refresh();
            btnOk.Enabled = enable;
            btnOk.Refresh();
            btnGuest.Enabled = enable;
            btnGuest.Refresh();
            _titleBar.CloseButton.Enabled = enable;
            //_titleBar.CloseButton.Invalidate();
            panorama.Refresh();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                VerifyFields();

                EnableFields(false);

                LoadStatus( Program.LanguageDefault.DictionaryLogin["ServerValidate"]);

                //if (cbxLembrar.Checked)
                //    Thread.Sleep(1000);

                if (cbxOffline.Checked)
                {
                    Program.LoginAuthentication = ListLoginAuthentication.Instance(Program.LanguageDefault).LoginOffline(txtLogin.Text.ToLower(),
                        Utility.Cript(txtPassword.Text.ToLower()), cbxLembrar.Checked);
                }
                else
                {
                    Program.LoginAuthentication = ListLoginAuthentication.Instance(Program.LanguageDefault).LoadLogin(txtLogin.Text.ToLower(),
                        Utility.Cript(txtPassword.Text.ToLower()), cbxLembrar.Checked);
                }

                if (Program.LoginAuthentication != null)
                {
                    Program.LoginAuthentication.Offline = cbxOffline.Checked;

                    LoadStatus(Program.LanguageDefault.DictionaryLogin["ValidationSuccessful"]);
                    ManagerWorkspace.Instance().LoadTheme();
                    //Thread.Sleep(1000);
                    Program._applicationContext.MainForm = null;
                    return;
                }

                RadMessageBox.Show(Program.LanguageDefault.DictionaryLogin["invalidLogin"], " ");
                EnableFields(true);
                LoadStatus(Program.LanguageDefault.DictionaryLogin["InsertLogin"]);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                RadMessageBox.Show(ex.Message, " ");
                EnableFields(true);
                LoadStatus(Program.LanguageDefault.DictionaryLogin["InsertLogin"]);
            }
        }

        private void BtnGuestClick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now > Properties.Settings.Default.DateInstallation.AddDays(3))
                {
                    RadMessageBox.Show(Program.LanguageDefault.DictionaryLogin["msgGuestInvalid"], "Mensagem", MessageBoxButtons.OK);
                    return;
                }
                RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider() { OkButton = "Continuar" };
                DialogResult res = RadMessageBox.Show(Program.LanguageDefault.DictionaryLogin["msgGuestInfo"], "Mensagem", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    EnableFields(false);

                    Program.LoginAuthentication = ListLoginAuthentication.Instance(Program.LanguageDefault).LoginOffline("guest",
                            Utility.Cript("123456"), false);
                    Program._applicationContext.MainForm = null;
                }
                RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
                return;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, " ");
                EnableFields(true);
                LoadStatus(Program.LanguageDefault.DictionaryLogin["InsertLogin"]);
            }
        }

        private void FrmLoginLoad(object sender, EventArgs e)
        {
            RoundRectShape shape = new RoundRectShape
                                       {
                                           BottomLeftRounded = true,
                                           BottomRightRounded = true,
                                           TopLeftRounded = true,
                                           TopRightRounded = true,
                                           Radius = 5
                                       };

            //txtLogin.RootElement.StretchVertically = true;
            //txtLogin.TextBoxElement.StretchVertically = true;
            //txtLogin.TextBoxElement.TextBoxItem.Alignment = ContentAlignment.MiddleLeft;

            //txtPassword.RootElement.StretchVertically = true;
            //txtPassword.TextBoxElement.StretchVertically = true;
            //txtPassword.TextBoxElement.TextBoxItem.Alignment = ContentAlignment.MiddleLeft;

            txtLogin.Height = 25;
            txtPassword.Height = 25;

            //txtLogin.TextBoxElement.Shape = shape;
            //txtPassword.TextBoxElement.Shape = shape;

            lblStatus.Location = new Point(6, (Height - lblStatus.Height) - 8);
            lblVersion.Location = new Point((Width - lblVersion.Width) - 10, (Height - lblVersion.Height) - 8);

            LoadStatus(Program.LanguageDefault.DictionaryLogin["InsertLogin"]);

            txtLogin.TabIndex = 0;
            txtPassword.TabIndex = 1;

            cbxOffline.Checked = true;

            txtLogin.Focus();
            txtLogin.Select();

            BtnOkClick(new object(), new EventArgs());
        }

        public void LoadStatus(string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    lblStatus.Text = status;
                    lblStatus.Width = Width;
                    lblStatus.Refresh();
                    panorama.Refresh();
                }));
            }
            else
            {
                lblStatus.Text = status;
                lblStatus.Width = Width;
                lblStatus.Refresh();
                panorama.Refresh();
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            FrmRegister frmRegister = new FrmRegister(this);
            /*frmRegister.LoadUserData(new UserRegister() { 
                Birthday = new DateTime(1988,8,27),
                CEP = "31530450",
                CPF = "09962411670",
                Email = "vinicius.arruda@seamus-fs.com.br",
                FirstName = "Vinicius",
                LastName = "Arruda",
                Number = "75",
                UserName = "vinicius.arruda",
                Complement = "Casa"
            });*/
            frmRegister.ShowDialog();
        }

        public void SetTxtLogin(string text)
        {
            txtLogin.Text = text;
            txtPassword.Text = "";
            cbxLembrar.Checked = true;
            cbxOffline.Checked = false;
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            FrmRecovery frmRecovery = new FrmRecovery();
            /*frmRecovery.LoadUserData(new UserRegister() { 
                Birthday = new DateTime(1988,8,27),
                CEP = "31530450",
                CPF = "09962411670",
                Email = "vinicius.arruda@seamus-fs.com.br",
                FirstName = "Vinicius",
                LastName = "Arruda",
                Number = "75",
                UserName = "teste",
                Complement = "Casa"
            });*/
            frmRecovery.ShowDialog();
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Return) BtnOkClick(null, null);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Return) BtnOkClick(null, null);
        }
    }

    public class MyFormBehavior : RadFormBehavior
    {
        public MyFormBehavior(IComponentTreeHandler treeHandler, bool shouldCreateChildren) :
            base(treeHandler, shouldCreateChildren)
        {
        }

        public override Padding BorderWidth
        {
            get
            {
                return new Padding(1);
            }
        }
    }
}
