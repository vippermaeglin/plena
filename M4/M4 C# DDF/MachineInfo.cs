/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Management;

namespace M4
{
  public class MachineInfo
  {
    //##########################################################################
    //WARNING! Changing any code in this class is a violation of your license 
    //agreement and may result in fines and/or license revocation of M4!
    //##########################################################################

    public string GetMACAddress()
    {
      ManagementObjectCollection moc = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
      string MACAddress = string.Empty;
      foreach (ManagementObject mo in moc)
      {
        if ((MACAddress == string.Empty) && Convert.ToBoolean(mo["IPEnabled"])) //only return MAC Address from first card                
        {
          MACAddress = Convert.ToString(mo["MacAddress"]);
        }
        mo.Dispose();
      }
      return MACAddress.Replace(":", "");
    }

    public string GetMachineID()
    {
      //Returns drive volume serial and MAC address
      return (GetVolumeSerial("C") + GetMACAddress());
    }

    private static string GetVolumeSerial(string strDriveLetter)
    {
      if ((strDriveLetter == "") | (strDriveLetter == null))
      {
        strDriveLetter = "C";
      }
      ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter + ":\"");
      disk.Get();
      return Convert.ToString(disk["VolumeSerialNumber"]);
    }
  }
}
