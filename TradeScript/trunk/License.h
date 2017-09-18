// License.h: interface for the CLicense class.
//
//////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// Online Backprop Neural Net with Simulated Annealing
// Copyright 2003 Modulus Financial Engineering
// http://www.modulusfe.com
// Use of this source code is subject to the licensing
// agreement found at http://www.modulusfe.com/download/license.asp
//
// License.h MUST BE INCLUDED AND IN WORKING CONDITION or you will
// be in violation of the licensing agreement!
/////////////////////////////////////////////////////////////////////////////



#if !defined(AFX_LICENSE_H__93664924_FBF1_4006_83C8_C7E5A25321CC__INCLUDED_)
#define AFX_LICENSE_H__93664924_FBF1_4006_83C8_C7E5A25321CC__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "stdafx.h"
#include <string>
#include <fstream>



class CLicense
{
protected:
   static BOOL VerifyLicenseKey(BSTR bstr)
   {
      USES_CONVERSION;
	  // Runtime license (called if client was compiled with correct license)	  
      return !lstrcmp(OLE2T(bstr), _T("Modu-8d7v9-3jFDo-871kT"));
   }

   static BOOL VerifyUserLicense()
   {	   
	  return TRUE;
   }

   static BOOL GetLicenseKey(DWORD dwReserved, BSTR* pBstr) 
   {
      USES_CONVERSION;	  	  
      *pBstr = SysAllocString( T2OLE(_T("Modu-8d7v9-3jFDo-871kT"))); 
      return TRUE;
   }

   static BOOL IsLicenseValid()
   {
	   // Runtime license
	   return GetDeveloperKey();
   }

   static BOOL IsServer()
   {
   
		HKEY hKey;
		char *psz;
		TCHAR szBuff1[400]={0};
		TCHAR szBuff2[400]={0};
		DWORD dwType = 0, dwLen=sizeof(szBuff1);
  
		psz = "SYSTEM\\ControlSet001\\Control\\ProductOptions";
		HRESULT hr = RegOpenKeyEx(HKEY_LOCAL_MACHINE, psz, 0, KEY_READ, &hKey);

		psz = "ProductType";
		hr = RegQueryValueEx(hKey, psz, 0, &dwType, (LPBYTE)szBuff1, &dwLen);

		string osType = szBuff1;	
		int found = osType.find("Server");
		if(found == -1) found = osType.find("server");
		if(found == -1) found = osType.find("svr");
		if(found == -1) found = osType.find("Svr");
		if(found > -1)
		{
			return TRUE;
		}		

		return FALSE;
   }

   static BOOL GetDeveloperKey()
   {

	   return true;

		// First, check to see if we're in the development environment
		bool dev = GetModuleHandle("VB6.exe") != NULL ||
			GetModuleHandle("VB4.exe") != NULL ||
			GetModuleHandle("VB3.exe") != NULL ||
			GetModuleHandle("VB.exe") != NULL ||
			GetModuleHandle("VB32.exe") != NULL ||
			GetModuleHandle("MSDEV.exe") != NULL ||
			GetModuleHandle("DEVENV.exe") != NULL ||
			GetModuleHandle("IEXPLORE.exe") != NULL ||
			GetModuleHandle("FPXPRESS.exe") != NULL ||
			GetModuleHandle("D7.exe") != NULL ||
			GetModuleHandle("D5.exe") != NULL ||
			GetModuleHandle("D6.exe") != NULL ||
			GetModuleHandle("D7.exe") != NULL ||
			GetModuleHandle("BCB.exe") != NULL ||
			GetModuleHandle("DCC32.exe") != NULL ||			
			GetModuleHandle("FP.exe") != NULL ||
			GetModuleHandle("FOXPRO.exe") != NULL ||
			GetModuleHandle("FP6.exe") != NULL ||
			GetModuleHandle("FP7.exe") != NULL ||
			GetModuleHandle("FP5.exe") != NULL ||
			GetModuleHandle("PB6.exe") != NULL ||
			GetModuleHandle("PB7.exe") != NULL ||
			GetModuleHandle("MSSQL.exe") != NULL ||
			GetModuleHandle("DELPHI.exe") != NULL;



		// Do not allow running on a server platform
		if(IsServer())
		{
			MessageBox(NULL, 
				"TradeScript is not licensed to run on a server platform.", "License", MB_ICONERROR);
			return FALSE;
		} 



#ifdef DEMO_MODE
	string remaining;
	string daysLeft;
	char left[255];
	if(GetEnvironmentVariable("DAYSLEFT", left, 255)) daysLeft = left;
	if(daysLeft == "") daysLeft = "0";
	remaining = "Your TradeScript Trial has " + daysLeft + " days remaining!";
	MessageBox(NULL, remaining.c_str(), "Reminder", MB_OK + MB_ICONINFORMATION);
	if(atol(daysLeft.c_str()) < 1)
		return FALSE;
	else
		return TRUE;
#endif



		// the personal license must always check for the license 
		// file and is limited to only three installations:
#ifdef PERSONAL_LICENSE
	// Personal license must check the license file
	// because it can only run on this computer
#else
	// Commercial license can run on another computer
	// so long as its not in the development environment.
	if(!dev) return TRUE;
#endif


		// Get the hardware id		
		std::string id;
		HKEY hKey;
		char *psz;
		TCHAR szBuff1[400]={0};
		TCHAR szBuff2[400]={0};
		DWORD dwType = 0, dwLen=sizeof(szBuff1);
  
		// Bios date
		psz = "HARDWARE\\DESCRIPTION\\System";
		HRESULT hr = RegOpenKeyEx(HKEY_LOCAL_MACHINE, psz, 0, KEY_READ, &hKey);
		
		psz = "SystemBiosDate";
		hr = RegQueryValueEx(hKey, psz, 0, &dwType, (LPBYTE)szBuff1, &dwLen);
	
		// Central processor 0 Identifier
		dwLen=sizeof(szBuff2);
		psz = "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";
		hr = RegOpenKeyEx(HKEY_LOCAL_MACHINE, psz, 0, KEY_READ, &hKey);
	
		psz = "Identifier";
		hr = RegQueryValueEx(hKey, psz, 0, &dwType, (LPBYTE)szBuff2, &dwLen);
	
		// Append
		id.append(szBuff1);
		id.append(szBuff2);
		if(id == "") id = "modulus-bt8-keycode"; //Failsafe

		// Encode or decode and extract letters
		std::string key = "bt8";
		std::string temp, chr;
		for(int i=0; i < (int)id.length(); ++i)
		{			
			char c = (char)key[ i % key.length() ] ^ id[i];
			if((c >= 48 && c <= 57) ||
				(c >= 65 && c <= 90) ||
				(c >= 97 && c <= 122)) temp += c;		
		}
		id = temp;

		// Now create the encoded hardware id
		char hid[1024];
		strncpy_s(hid,id.c_str(),1024); 
		std::string encId;
		temp = chr = "";
		int i = 0;
		for(i=0; i < (int)id.length(); ++i)
		{
			char c = hid[i];
			if((c >= 48 && c < 57) ||
				(c >= 65 && c < 90) ||
				(c >= 97 && c < 121))
			{
				temp += (c + 1);
			}
			else
			{
				temp += c;
			}
		}
		encId = temp;


		//Read the license from file
		char path[MAX_PATH] = "";
		::GetModuleFileName(GetModuleHandle("TradeScript.dll"), path, MAX_PATH);
		string fileName = path;
		for(i = fileName.length(); i != 0; --i)
		{
			if(fileName.substr(i,1) == "\\")			
				break;
		}
		if(i > 0) fileName = fileName.substr(0,i);
		fileName.append("\\TradeScript.lic");
		
		//Read the license from file
		char licenseText[1024] = "";
		ifstream in(fileName.c_str(), ios::binary);
		in.read( (char*) &licenseText, sizeof(licenseText) );
		std::string license = licenseText;
		if(license != encId)
		{
			MessageBox(NULL, 
				"Development license missing or invalid.", "License", MB_ICONERROR);
			return FALSE;
		}
		else{
			return TRUE;
		}

   }  



};

#endif // !defined(AFX_LICENSE_H__93664924_FBF1_4006_83C8_C7E5A25321CC__INCLUDED_)
