/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System.Threading;
using System.Windows.Forms;
using M4.AsyncOperations;

namespace M4
{
    public static class SplashScreenHelper
    {
        private static readonly object _showLock = new object();
        private static bool _visible;
        private static MethodInvoker _closer;
        private static SplashScreen _form;

        public static IBootTimeInformer Show()
        {
            if (!_visible)
            {
                lock (_showLock)
                {
                    if (!_visible)
                    {
                        var readyEvt = new ManualResetEvent(false);
                        var splashUIThread = new Thread(
                          () =>
                          {
                              _form = new SplashScreen();
                              var asyncOp = AsyncHelper.CreateOperation();
                              _closer = () => asyncOp.Post(_form.Close);
                              readyEvt.Set();
                              Application.Run(_form);
                          })
                                               {
                                                   IsBackground = true,
                                                   CurrentCulture = Thread.CurrentThread.CurrentCulture,
                                                   CurrentUICulture = Thread.CurrentThread.CurrentUICulture
                                               };
                        splashUIThread.SetApartmentState(ApartmentState.STA);
                        splashUIThread.Start();
                        readyEvt.WaitOne(); // Wait for form creation
                        _visible = true;
                    }
                }
            }
            return _form;
        }

        public static void Hide()
        {
            if (_visible)
                lock (_showLock)
                    if (_visible)
                    {
                        _visible = false;
                        _closer();
                        _form = null;
                    }
        }

        public static IBootTimeInformer BootTimeInformer
        {
            get { return _form; }
        }
    }
}
