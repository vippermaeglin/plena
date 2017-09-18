/***************************************************************************
 *  Project: WebControl
 *  File:    SingleInstance.cs
 *  Version: 1.0.0.0
 *
 *  Copyright ©2012 Perikles C. Stephanidis; All rights reserved.
 *  This code is provided "AS IS" without warranty of any kind.
 *__________________________________________________________________________
 *
 *  Notes:
 *
 *  Utility class that ensures a single instance of a an application per 
 *  Windows user session.
 *   
 ***************************************************************************/

#region Using

using System;
using System.Threading;

#endregion

namespace M4.WebControlSample
{
    #region SingleInstanceMode
    internal enum SingleInstanceMode : int
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        NotInited = 0,

        /// <summary>
        /// Every user can have own single instance.
        /// </summary>
        ForEveryUser
    }
    #endregion

    internal sealed class SingleInstance
    {
        #region Fields
        private static Action<object> SecondInstanceCallback;
        #endregion


        #region Ctors
        private SingleInstance()
        {
        }
        #endregion


        #region Methods
        /// <summary>
        /// Processing single instance with <see cref="SingleInstanceMode.ForEveryUser"/> mode.
        /// </summary>
        internal static void Make( Action<object> callback )
        {
            Make( SingleInstanceMode.ForEveryUser, callback );
        }

        /// <summary>
        /// Processing single instance.
        /// </summary>
        internal static void Make( SingleInstanceMode mode, Action<object> callback )
        {
            SecondInstanceCallback = callback;

#if DEBUG
            var appName = string.Format( "{0}DEBUG", typeof( SingleInstance ).Assembly.ManifestModule.ScopeName );
#else
		    var appName = typeof(SingleInstance).Assembly.ManifestModule.ScopeName;
#endif

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var keyUserName = ( ( windowsIdentity != null ) ? windowsIdentity.User.ToString() : string.Empty );

            // Be careful! Max 260 chars!
            var eventWaitHandleName = string.Format( "{0}{1}", appName, ( ( mode == SingleInstanceMode.ForEveryUser ) ? keyUserName : string.Empty ) );

            try
            {
                using ( var waitHandle = EventWaitHandle.OpenExisting( eventWaitHandleName ) )
                {
                    // It informs first instance about other startup attempting.
                    waitHandle.Set();
                }

                // Let's terminate this posterior startup.
                // For that exit no interception.
                Environment.Exit( 0 );
            }
            catch
            {
                // It's first instance.
                // Register EventWaitHandle.
                using ( var eventWaitHandle = new EventWaitHandle( false, EventResetMode.AutoReset, eventWaitHandleName ) )
                {
                    ThreadPool.RegisterWaitForSingleObject( eventWaitHandle, OtherInstanceAttemptedToStart, null, Timeout.Infinite, false );
                }
            }
        }
        #endregion

        #region Event Handlers
        private static void OtherInstanceAttemptedToStart( object state, bool timedOut )
        {
            if ( SecondInstanceCallback != null )
                SecondInstanceCallback.Invoke( state );
        }
        #endregion
    }
}
