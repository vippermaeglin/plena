/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Runtime.InteropServices;

namespace M4
{
  internal static class UnmanagedMethods
  {
    public const int IDHOT_SNAPWINDOW = -1; /* SHIFT-PRINTSCRN  */
    public const int IDHOT_SNAPDESKTOP = -2;  /* PRINTSCRN        */
    public const int WM_HOTKEY = 0x312;

    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("kernel32", SetLastError = true)]
    public static extern IntPtr GlobalAddAtom(string lpString);

    [DllImport("kernel32", SetLastError = true)]
    public static extern IntPtr GlobalDeleteAtom(IntPtr nAtom);

    [DllImport("kernel32")]
    public static extern int GetTickCount();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);

    public const int WM_SYSCOMMAND = 0x112;
    public const int SC_RESTORE = 0xF120;
    public const int SW_SHOW = 5;

    [DllImport("user32.dll")]
    public static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ShowWindow(IntPtr hWnd, int iCmdShow);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("winmm.dll")]
    public static extern int PlaySound(string lpszSoundName, int hModule, int dwFlags);

    public const int SND_FILENAME = 0x20000;
    public const int SND_ASYNC = 0x1;
  }
}
