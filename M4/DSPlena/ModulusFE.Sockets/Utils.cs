using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ModulusFE.Sockets
{
  public static class Utils
  {
    public static IEnumerable<IPAddress> LocalAddresses
    {
      get
      {
        string machineName = Dns.GetHostName();
        IPHostEntry host = Dns.GetHostEntry(machineName);

        foreach (IPAddress address in host.AddressList)
        {
          if (address.AddressFamily == AddressFamily.InterNetworkV6) continue;

          yield return address;
        }
      }
    }

    public static void BI(this Control self, Action a)
    {
      if (self.InvokeRequired)
        self.EndInvoke(self.BeginInvoke(a));
      else
        a();
    }

    public static void BI<T>(this Control self, Action<T> a, T p)
    {
      if (self.InvokeRequired)
        self.EndInvoke(self.BeginInvoke(a, p));
      else
        a(p);
    }

    public static void BI<T1, T2>(this Control self, Action<T1, T2> a, T1 p1, T2 p2)
    {
      if (self.InvokeRequired)
        self.EndInvoke(self.BeginInvoke(a, p1, p2));
      else
        a(p1, p2);
    }

    public static void I(this Control self, Action a)
    {
      if (self.InvokeRequired)
        self.Invoke(a);
      else
        a();
    }

    public static void I<T>(this Control self, Action<T> a, T p)
    {
      if (self.InvokeRequired)
        self.Invoke(a, p);
      else
        a(p);
    }

    public static void I<T1, T2>(this Control self, Action<T1, T2> a, T1 p1, T2 p2)
    {
      if (self.InvokeRequired)
        self.Invoke(a, p1, p2);
      else
        a(p1, p2);
    }
  }
}
