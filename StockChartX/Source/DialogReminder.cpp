// DialogReminder.cpp : implementation file
//

#include "stdafx.h"
#include "StockChartX.h"
#include "DialogReminder.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDialogReminder dialog


CDialogReminder::CDialogReminder(CWnd* pParent /*=NULL*/)
	: CDialog(CDialogReminder::IDD, pParent)
{
	m_ok = false;
	//{{AFX_DATA_INIT(CDialogReminder)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CDialogReminder::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDialogReminder)
	DDX_Control(pDX, IDOK, m_cmdOK);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDialogReminder, CDialog)
	//{{AFX_MSG_MAP(CDialogReminder)
	ON_BN_CLICKED(IDBUYNOW, OnBuyNow)
	ON_WM_CLOSE()
	ON_WM_PAINT()
	ON_BN_CLICKED(IDC_LIVEHELP, OnLiveHelp)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDialogReminder message handlers

void CDialogReminder::OnBuyNow()
{

	::ShellExecute(this->m_hWnd, "open", 
		"https://secure.modulusfe.com/order/stockchartx.asp", 
		0, 0, SW_MAXIMIZE
		);

	m_minimized = true;

	ShowWindow(SW_SHOWMINIMIZED);
}

int CDialogReminder::DoModal()
{
	return CDialog::DoModal();
}

void CDialogReminder::OnClose() 
{
	if(!m_ok) return;
	CDialog::OnClose();
}

void CDialogReminder::OnPaint()
{
	CPaintDC dc(this); // device context for painting

	if(m_minimized) return;

	CStatic* pOKLabel = (CStatic*)GetDlgItem(IDC_ENABLE_OK);
	CStatic* pMsgLabel = (CStatic*)GetDlgItem(IDC_MESSAGE);
	CStatic* pTimeLeft = (CStatic*)GetDlgItem(IDC_DAYS_LEFT);	

	CString res;
	res.LoadString(IDS_REMINDER);
	pMsgLabel->SetWindowText(res);

	if(m_usesLeft == "0")
	{
		//AfxMessageBox("Either your trial has expired or you are not running this project as the Windows Administrator");
		//m_expired = true;
		//CDialog::OnOK();
		//return;
	}

	res = "Your StockChartX trial installation has " + 
			m_usesLeft + " uses left before expiration.";
	
	if(m_expired) res = "Your StockChartX trial has expired!";

	pTimeLeft->SetWindowText(res);

	MSG msg;
	while (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
	

	CString sec;
	if(!m_display){
		long startTime = GetTickCount();
		for(int second = 0; second != 0; --second){
		
			res.LoadString(IDS_ENABLE_OK);
			sec.Format("%d", second);
			res.Replace("%1", sec);
			pOKLabel->SetWindowText(res);			
			
			for(int n = 0; n != 100; ++n){
				Sleep(10);
				while (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
				{
					TranslateMessage(&msg);
					DispatchMessage(&msg);
				}
			}

		}
		m_display = true;
		pOKLabel->SetWindowText("");
		m_cmdOK.EnableWindow();		
	}


}

BOOL CDialogReminder::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	m_expired = false;
	m_minimized = false;
	m_display = false;
	m_ok = false;

	m_usesLeft = "0";


	// Create / check the flag value
	LONG resultFlag;
    HKEY hKeyFlag;
    char szValueFlag[100];
    DWORD dwBufLenFlag = sizeof(szValueFlag);
	bool newFlag = false;

    // test
    RegOpenKeyEx( HKEY_LOCAL_MACHINE,
        "SOFTWARE\\Microsoft\\Windows\\Win9\\",
        0, KEY_QUERY_VALUE | KEY_SET_VALUE, &hKeyFlag );
	resultFlag = RegQueryValueEx( hKeyFlag, "Flag", NULL, NULL,
        (LPBYTE) szValueFlag, &dwBufLenFlag);
	if(ERROR_SUCCESS != resultFlag)
	{
		// create
		DWORD dwDisp = 0;
		LPDWORD lpdwDisp = &dwDisp;
		CString l_strKey = "SOFTWARE\\Microsoft\\Windows\\Win9\\";
		resultFlag = RegCreateKeyEx( HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\Win9\\", 0L,NULL, 
			REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &hKeyFlag,lpdwDisp);
		if(resultFlag == ERROR_SUCCESS)
		{
			CString szFlag = "1";
			RegSetValueEx(hKeyFlag, "Flag", 0, REG_SZ, (LPBYTE) 
				szFlag.GetBuffer(szFlag.GetLength()), szFlag.GetLength() + 1);
		}
		// open
		resultFlag = RegOpenKeyEx( HKEY_LOCAL_MACHINE,
			"SOFTWARE\\Microsoft\\Windows\\Win9\\",
			0, KEY_QUERY_VALUE | KEY_SET_VALUE, &hKeyFlag );    
		resultFlag = RegQueryValueEx( hKeyFlag, "Flag", NULL, NULL,
			(LPBYTE) szValueFlag, &dwBufLenFlag);
		newFlag = true;
	}
    if(ERROR_SUCCESS != resultFlag) return FALSE;
     


 


	LONG result;
    HKEY hKey;
    char szValue[100];
    DWORD dwBufLen = sizeof(szValue);

    // test
    RegOpenKeyEx( HKEY_LOCAL_MACHINE,
        "SOFTWARE\\Keys\\STX\\5.0\\",
        0, KEY_QUERY_VALUE | KEY_SET_VALUE, &hKey );
	result = RegQueryValueEx( hKey, "Regsvr", NULL, NULL,
        (LPBYTE) szValue, &dwBufLen);
	if(ERROR_SUCCESS != result)
	{


		// registry tampered?
		if(!newFlag)
		{
			AfxMessageBox("The StockChartX trial is invalid and will now exit");
			m_expired = true;
			CDialog::OnOK();
			return FALSE;
		}


		// create
		DWORD dwDisp = 0;
		LPDWORD lpdwDisp = &dwDisp;
		CString l_strKey = "SOFTWARE\\Keys\\STX\\5.0\\";
		LONG iSuccess = RegCreateKeyEx( HKEY_LOCAL_MACHINE, "SOFTWARE\\Keys\\STX\\5.0\\", 0L,NULL, 
			REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &hKey,lpdwDisp);
		if(iSuccess == ERROR_SUCCESS)
		{
			CString szCount = "51";
			szCount = encode(szCount.GetBuffer(szCount.GetLength()));
			RegSetValueEx(hKey, "Regsvr", 0, REG_SZ, (LPBYTE) 
				szCount.GetBuffer(szCount.GetLength()), szCount.GetLength() + 1);
		}

		// open
		result = RegOpenKeyEx( HKEY_LOCAL_MACHINE,
			"SOFTWARE\\Keys\\STX\\5.0\\",
			0, KEY_QUERY_VALUE | KEY_SET_VALUE, &hKey );    
		result = RegQueryValueEx( hKey, "Regsvr", NULL, NULL,
			(LPBYTE) szValue, &dwBufLen);
	}
    if(ERROR_SUCCESS != result) return FALSE;
    
	
	CString szTemp = decode(szValue);

	// count usage	
	int uses = atoi(szTemp);
	uses--;
	m_usesLeft.Format("%d", uses);
	if(uses < 1 || uses > 51)
	{
		//m_expired = true;
		//AfxMessageBox("Your StockChartX trial has expired!");
		//CDialog::OnOK();
	}

    // write new value
	szTemp = encode(m_usesLeft.GetBuffer(m_usesLeft.GetLength()));
    result = RegSetValueEx(hKey, "Regsvr", 0, REG_SZ, (LPBYTE) 
				szTemp.GetBuffer(szTemp.GetLength()), szTemp.GetLength() + 1);
    if(ERROR_SUCCESS != result) return 1;
     
    // close the key
    RegCloseKey( hKey );
	



	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CDialogReminder::OnOK() 
{
	if(!m_expired) m_ok = true;
	CDialog::OnOK();
}

void CDialogReminder::OnLiveHelp() 
{

	CString url = "http://www.modulusfe.com/support/livehelp.asp?product=StockChartX&";
	url += GetVersionInfo();

	::ShellExecute(this->m_hWnd, "open", url, 0, 0, SW_MAXIMIZE);


	m_minimized = true;

	ShowWindow(SW_SHOWMINIMIZED);

}



CString CDialogReminder::GetVersionInfo(){


	OSVERSIONINFO OSversion;
	
OSversion.dwOSVersionInfoSize=sizeof(OSVERSIONINFO);
::GetVersionEx(&OSversion);

	CString szRet;

	CString csd = OSversion.szCSDVersion;
	csd.Replace(" ", "_");

switch(OSversion.dwPlatformId)
{
   case VER_PLATFORM_WIN32s: 
           szRet.Format("Windows_%d.%d",OSversion.dwMajorVersion,OSversion.dwMinorVersion);
	   break;
   case VER_PLATFORM_WIN32_WINDOWS:
  	  if(OSversion.dwMinorVersion==0)
	      szRet="Windows_95";  
	  else
          if(OSversion.dwMinorVersion==10)  
	      szRet="Windows_98";
       	  else
          if(OSversion.dwMinorVersion==90)  
	      szRet="Windows_Me";
          break;
   case VER_PLATFORM_WIN32_NT:
  	 if(OSversion.dwMajorVersion==5 && OSversion.dwMinorVersion==0)
             szRet.Format("Windows_2000_With_%s", csd);
         else	
         if(OSversion.dwMajorVersion==5 &&   OSversion.dwMinorVersion==1)
             szRet.Format("Windows_XP_%s",csd);
         else	
	 if(OSversion.dwMajorVersion<=4)
	    szRet.Format("Windows_NT_%d.%d_with_%s",
                           OSversion.dwMajorVersion,
                           OSversion.dwMinorVersion,
                           csd);
         else	
             //for unknown windows/newest windows version
	    szRet.Format("Windows_%d.%d_",
                           OSversion.dwMajorVersion,
                           OSversion.dwMinorVersion);
         break;
	}
		
	return szRet;
}


CString CDialogReminder::encode(char* input)
{
	CString temp = input;
	CString out = "";
	for(int n = 0; n < temp.GetLength(); ++n)
	{
		if(temp.Mid(n, 1) == "0") out += "X";
		if(temp.Mid(n, 1) == "1") out += "T";
		if(temp.Mid(n, 1) == "2") out += "E";
		if(temp.Mid(n, 1) == "3") out += "A";
		if(temp.Mid(n, 1) == "4") out += "N";
		if(temp.Mid(n, 1) == "5") out += "J";
		if(temp.Mid(n, 1) == "6") out += "O";
		if(temp.Mid(n, 1) == "7") out += "Q";
		if(temp.Mid(n, 1) == "8") out += "R";
		if(temp.Mid(n, 1) == "9") out += "Y";
	}	
	return out;
}

CString CDialogReminder::decode(char* input)
{
	CString temp = input;
	CString out = "";
	for(int n = 0; n < temp.GetLength(); ++n)
	{
		if(temp.Mid(n, 1) == "X") out += "0";
		if(temp.Mid(n, 1) == "T") out += "1";
		if(temp.Mid(n, 1) == "E") out += "2";
		if(temp.Mid(n, 1) == "A") out += "3";
		if(temp.Mid(n, 1) == "N") out += "4";
		if(temp.Mid(n, 1) == "J") out += "5";
		if(temp.Mid(n, 1) == "O") out += "6";
		if(temp.Mid(n, 1) == "Q") out += "7";
		if(temp.Mid(n, 1) == "R") out += "8";
		if(temp.Mid(n, 1) == "Y") out += "9";
	}	
	return out;
}
