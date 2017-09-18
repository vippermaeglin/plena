/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using M4.AsyncOperations;

namespace M4
{
    public partial class SplashScreen : Form, IBootTimeInformer
    {
        private readonly AsyncOperation _asynOp;
        public bool Finished;

        public SplashScreen()
        {
            InitializeComponent();

            _asynOp = AsyncHelper.CreateOperation();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            Version.Text = "Version Q1 2011"; //TODO: Change your version information here
        }

        private void tmrUnload_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            tmrUnload.Enabled = false;
            Finished = true;
        }

        #region Implementation of IBootTimeInformer

        public void SetStatus(string text)
        {
            _asynOp.Post(() => lblStatus.Text = text);
        }

        public void SetVisible(bool visible)
        {
            _asynOp.Post(() => Visible = visible);
        }

        #endregion
    }
}
