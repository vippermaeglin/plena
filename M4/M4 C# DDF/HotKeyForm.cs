/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

//This class manages system-wide hot keys

namespace M4
{
  public partial class HotKeyForm : NForm
  {
    private readonly HotKeyCollection m_hotKeys;

    public event HotKeyPressedEventHandler HotKeyPressed = delegate { };
    public event PrintWindowPressedEventHandler PrintWindowPressed = delegate { };
    public event PrintDesktopPressedEventHandler PrintDesktopPressed = delegate { };

    public HotKeyCollection HotKeys
    {
      get { return m_hotKeys; }
    }

    public void RestoreAndActivate()
    {
      if (!UnmanagedMethods.IsWindowVisible(Handle))
      {
        UnmanagedMethods.ShowWindow(Handle, UnmanagedMethods.SW_SHOW);
      }
      else
      {
        if (UnmanagedMethods.IsIconic(Handle))
        {
          UnmanagedMethods.SendMessage(Handle, UnmanagedMethods.WM_SYSCOMMAND, UnmanagedMethods.SC_RESTORE, IntPtr.Zero);
        }
      }
      UnmanagedMethods.SetForegroundWindow(Handle);
    }

    protected override void WndProc(ref Message m)
    {
      base.WndProc(ref m);

      if (m.Msg != UnmanagedMethods.WM_HOTKEY) return;

      int hotKeyId = m.WParam.ToInt32();
      switch (hotKeyId)
      {
        case UnmanagedMethods.IDHOT_SNAPDESKTOP:
          PrintDesktopPressed(this, EventArgs.Empty);
          break;
        case UnmanagedMethods.IDHOT_SNAPWINDOW:
          PrintWindowPressed(this, EventArgs.Empty);
          break;
        default:
          foreach (HotKey htk in m_hotKeys)
          {
            if (htk.AtomId.Equals(m.WParam))
            {
              HotKeyPressed(this, new HotKeyPressedEventArgs(htk));
            }
          }
          break;
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      HotKeys.Clear();
      base.OnClosed(e);
    }

    public HotKeyForm()
    {
      InitializeComponent();

      m_hotKeys = new HotKeyCollection(this);
    }
  }

  public delegate void HotKeyPressedEventHandler(object sender, HotKeyPressedEventArgs e);
  public delegate void PrintWindowPressedEventHandler(object sender, EventArgs e);
  public delegate void PrintDesktopPressedEventHandler(object sender, EventArgs e);

  public class HotKeyPressedEventArgs : EventArgs
  {
    private readonly HotKey _hotKey;
    public HotKey HotKey { get { return _hotKey; } }
    public HotKeyPressedEventArgs(HotKey hotKey)
    {
      _hotKey = hotKey;
    }
  }

  public class HotKeyCollection : CollectionBase
  {
    private readonly Form ownerForm;

    protected override void OnClear()
    {
      foreach (HotKey key in InnerList)
      {
        RemoveHotKey(key);
      }
      base.OnClear();
    }

    protected override void OnInsert(int index, object value)
    {
      //validate item is a hot key:
      HotKey htk = new HotKey();
      if (!value.GetType().IsInstanceOfType(htk))
      {
        throw new InvalidCastException("Invalid object.");
      }
      //check if the name, keycode and modifiers have been set up:
      htk = (HotKey)value;
      //throws ArgumentException if there is a problem:
      htk.Validate();
      //throws Unable to add HotKeyException:
      AddHotKey(htk);
      //ok
      base.OnInsert(index, value);
    }

    protected override void OnRemove(int index, object value)
    {
      //get the item to be removed:
      HotKey htk = (HotKey)value;
      RemoveHotKey(htk);
      base.OnRemove(index, value);
    }

    protected override void OnSet(int index, object oldValue, object newValue)
    {
      //remove old hot key:
      HotKey htk = (HotKey)oldValue;
      RemoveHotKey(htk);

      //add new hotkey:
      htk = (HotKey)newValue;
      AddHotKey(htk);

      base.OnSet(index, oldValue, newValue);
    }

    protected override void OnValidate(object value)
    {
      ((HotKey)value).Validate();
    }

    public void Add(HotKey hotKey)
    {
      //throws argument exception:
      hotKey.Validate();
      //throws unable to add hot key exception:
      AddHotKey(hotKey);
      //assuming all is well:
      InnerList.Add(hotKey);
    }

    public HotKey this[int index]
    {
      get
      {
        return (HotKey)InnerList[index];
      }
    }

    private void RemoveHotKey(HotKey hotKey)
    {
      // remove the hot key:
      UnmanagedMethods.UnregisterHotKey(ownerForm.Handle, hotKey.AtomId.ToInt32());
      // unregister the atom:
      UnmanagedMethods.GlobalDeleteAtom(hotKey.AtomId);
    }

    private void AddHotKey(HotKey hotKey)
    {
      //generate the id:
      string atomName = hotKey.Name + "_" + UnmanagedMethods.GetTickCount();
      if (atomName.Length > 255)
        atomName = atomName.Substring(0, 255);

      //Create a new atom:
      IntPtr id = UnmanagedMethods.GlobalAddAtom(atomName);
      if (id.Equals(IntPtr.Zero))
        throw new HotKeyAddException("Failed to add GlobalAtom for HotKey");

      bool ret = UnmanagedMethods.RegisterHotKey(ownerForm.Handle, id.ToInt32(), (int)hotKey.Modifiers, (int)hotKey.KeyCode);
      if (!ret)
      {
        //Remove the atom:
        UnmanagedMethods.GlobalDeleteAtom(id);
        //failed
        throw new HotKeyAddException("Failed to register HotKey");
      }
      hotKey.AtomName = atomName;
      hotKey.AtomId = id;
    }

    public HotKeyCollection(Form ownerForm)
    {
      this.ownerForm = ownerForm;
    }
  }

  public class HotKeyAddException : Exception
  {
    public HotKeyAddException() { }
    public HotKeyAddException(string message) : base(message) { }
    public HotKeyAddException(string message, Exception innerException) : base(message, innerException) { }
  }

  public class HotKey
  {
    [Flags]
    public enum HotKeyModifiers
    {
      MOD_ALT = 0x1,
      MOD_CONTROL = 0x2,
      MOD_SHIFT = 0x4,
      MOD_WIN = 0x8
    }

    private IntPtr _atomId;
    public IntPtr AtomId
    {
      get { return _atomId; }
      set { _atomId = value; }
    }

    private string _atomName;
    public string AtomName
    {
      get { return _atomName; }
      set { _atomName = value; }
    }

    private string _name;
    public string Name
    {
      get { return _name;}
      set { _name = value; }
    }

    private Keys _keyCode;
    public Keys KeyCode
    {
      get { return _keyCode; }
      set { _keyCode = value; }
    }

    private HotKeyModifiers _modifiers;
    public HotKeyModifiers Modifiers
    {
      get { return _modifiers; }
      set { _modifiers = value; }
    }

    public void Validate()
    {
      string msg = "";

      if (Name.Trim().Length == 0)
        msg = "Name parameter cannot be zero length";

      if (KeyCode == Keys.Alt ||
          KeyCode == Keys.Control ||
          KeyCode == Keys.Shift ||
          KeyCode == Keys.ShiftKey ||
          KeyCode == Keys.ControlKey)
        msg = "KeyCode cannot be set to a modifier key";

      if (msg.Length > 0)
        throw new ArgumentException(msg);
    }

    public HotKey() { }

    public HotKey(string name, Keys keyCode, HotKeyModifiers modifiers)
    {
      Name = name;
      KeyCode = keyCode;
      Modifiers = modifiers;
    }
  }
}
