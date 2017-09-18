// IndPropDlg.cpp : implementation file
//

#include "stdafx.h"
#include "StockChartX.h"
#include "IndPropDlg.h"
#include "dpi.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CIndPropDlg dialog

HHOOK hHook = NULL;
 

CIndPropDlg::CIndPropDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CIndPropDlg::IDD, pParent)
{	
	//{{AFX_DATA_INIT(CIndPropDlg)	
	//}}AFX_DATA_INIT	
}


void CIndPropDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CIndPropDlg)
	DDX_Control(pDX, IDC_COLOR, m_cmdColor);
	DDX_Control(pDX, IDC_LABEL7, m_Label7);
	DDX_Control(pDX, IDC_LABEL8, m_Label8);
	DDX_Control(pDX, IDC_LABEL6, m_Label6);
	DDX_Control(pDX, IDC_LABEL5, m_Label5);
	DDX_Control(pDX, IDC_LABEL4, m_Label4);
	DDX_Control(pDX, IDC_LABEL3, m_Label3);
	DDX_Control(pDX, IDC_LABEL2, m_Label2);
	DDX_Control(pDX, IDC_LABEL1, m_Label1);
	DDX_Control(pDX, IDC_EDIT8, m_Edit8);
	DDX_Control(pDX, IDC_EDIT7, m_Edit7);
	DDX_Control(pDX, IDC_EDIT6, m_Edit6);
	DDX_Control(pDX, IDC_EDIT5, m_Edit5);
	DDX_Control(pDX, IDC_EDIT4, m_Edit4);
	DDX_Control(pDX, IDC_EDIT3, m_Edit3);
	DDX_Control(pDX, IDC_EDIT2, m_Edit2);
	DDX_Control(pDX, IDC_EDIT1, m_Edit1);
	DDX_Control(pDX, IDC_COMBO8, m_Combo8);
	DDX_Control(pDX, IDC_COMBO7, m_Combo7);
	DDX_Control(pDX, IDC_COMBO6, m_Combo6);
	DDX_Control(pDX, IDC_COMBO5, m_Combo5);
	DDX_Control(pDX, IDC_COMBO4, m_Combo4);
	DDX_Control(pDX, IDC_COMBO3, m_Combo3);
	DDX_Control(pDX, IDC_COMBO2, m_Combo2);
	DDX_Control(pDX, IDC_COMBO1, m_Combo1);
	DDX_Control(pDX, IDC_FRAME, m_fraMain);
	DDX_Control(pDX, IDC_CMD_HELP, m_cmdHelp);
	DDX_Control(pDX, IDC_CMD_CANCEL, m_cmdCancel);
	DDX_Control(pDX, IDC_CMD_APPLY, m_cmdApply);
	DDX_Control(pDX, IDC_INFO, m_txtInfo);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CIndPropDlg, CDialog)
	//{{AFX_MSG_MAP(CIndPropDlg)
	ON_WM_SHOWWINDOW()	
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_CMD_APPLY, OnCmdApply)
	ON_BN_CLICKED(IDC_CMD_CANCEL, OnCmdCancel)
	ON_BN_CLICKED(IDC_CMD_HELP, OnCmdHelp)			
	ON_CBN_SETFOCUS(IDC_COMBO1, OnSetfocusCombo1)
	ON_CBN_SETFOCUS(IDC_COMBO2, OnSetfocusCombo2)
	ON_CBN_SETFOCUS(IDC_COMBO3, OnSetfocusCombo3)
	ON_CBN_SETFOCUS(IDC_COMBO4, OnSetfocusCombo4)
	ON_CBN_SETFOCUS(IDC_COMBO5, OnSetfocusCombo5)
	ON_CBN_SETFOCUS(IDC_COMBO6, OnSetfocusCombo6)
	ON_CBN_SETFOCUS(IDC_COMBO7, OnSetfocusCombo7)
	ON_CBN_SETFOCUS(IDC_COMBO8, OnSetfocusCombo8)
	ON_EN_SETFOCUS(IDC_EDIT1, OnSetfocusEdit1)
	ON_EN_SETFOCUS(IDC_EDIT2, OnSetfocusEdit2)
	ON_EN_SETFOCUS(IDC_EDIT3, OnSetfocusEdit3)
	ON_EN_SETFOCUS(IDC_EDIT4, OnSetfocusEdit4)
	ON_EN_SETFOCUS(IDC_EDIT5, OnSetfocusEdit5)
	ON_EN_SETFOCUS(IDC_EDIT6, OnSetfocusEdit6)
	ON_EN_SETFOCUS(IDC_EDIT7, OnSetfocusEdit7)
	ON_EN_SETFOCUS(IDC_EDIT8, OnSetfocusEdit8)
	ON_BN_CLICKED(IDC_COLOR, OnColor)
	ON_WM_PAINT()
	ON_WM_MOVE()		
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CIndPropDlg message handlers

void CIndPropDlg::OnShowWindow(BOOL bShow, UINT nStatus) 
{
	CDialog::OnShowWindow(bShow, nStatus);	
	pIndicator->dialogShown = true;	
	if(bShow) pCtrl->FireOnDialogShown();
}

void CIndPropDlg::OnClose() 
{

	if(!canUnload) return;


	pCtrl->locked = false;
	pIndicator->dialogShown = false;
	pCtrl->FireOnDialogHiden();
	
	CDialog::OnClose();
}

void CIndPropDlg::OnCmdApply() 
{
	
	try{
		// Win9x workaround
		::UnhookWindowsHookEx (hHook);
	}
	catch(...){}

	canUnload = true;
	pIndicator->dialogShown = false;
	pCtrl->FireOnDialogHiden();
	pIndicator->SetUserInput();
}

void CIndPropDlg::OnCmdCancel() 
{		
	
	try{
		// Win9x workaround
		::UnhookWindowsHookEx (hHook);
	}
	catch(...){}

	pCtrl->FireOnDialogCancel();
	canUnload = true;
	pCtrl->updatingIndicator = false;
	pCtrl->locked = false;
	pIndicator->OnCancleDialog();
	pIndicator->dialogShown = false;
	pCtrl->DelayMessage("", TIMER_IND_DLG_CANCEL, 500);
	CDialog::OnCancel();
}

void CIndPropDlg::OnCmdHelp() 
{
	pCtrl->ShowHtmlHelp("TA.chm", pIndicator->indicatorType );
}

void CIndPropDlg::OnSetfocusCombo1() 
{
	pIndicator->ChangeFocus(0);
}

void CIndPropDlg::OnSetfocusCombo2() 
{
	pIndicator->ChangeFocus(1);
}

void CIndPropDlg::OnSetfocusCombo3() 
{
	pIndicator->ChangeFocus(2);
}

void CIndPropDlg::OnSetfocusCombo4() 
{
	pIndicator->ChangeFocus(3);
}

void CIndPropDlg::OnSetfocusCombo5() 
{
	pIndicator->ChangeFocus(4);
}

void CIndPropDlg::OnSetfocusCombo6() 
{
	pIndicator->ChangeFocus(5);
}

void CIndPropDlg::OnSetfocusCombo7() 
{
	pIndicator->ChangeFocus(6);
}

void CIndPropDlg::OnSetfocusCombo8() 
{
	pIndicator->ChangeFocus(7);
}

void CIndPropDlg::OnSetfocusEdit1() 
{
	pIndicator->ChangeFocus(0);
	CString text; m_Edit1.GetWindowText(text);
	m_Edit1.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit2() 
{
	pIndicator->ChangeFocus(1);
	CString text; m_Edit2.GetWindowText(text);
	m_Edit2.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit3() 
{
	pIndicator->ChangeFocus(2);
	CString text; m_Edit3.GetWindowText(text);
	m_Edit3.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit4() 
{
	pIndicator->ChangeFocus(3);
	CString text; m_Edit4.GetWindowText(text);
	m_Edit4.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit5() 
{
	pIndicator->ChangeFocus(4);
	CString text; m_Edit5.GetWindowText(text);
	m_Edit5.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit6() 
{	
	pIndicator->ChangeFocus(5);
	CString text; m_Edit6.GetWindowText(text);
	m_Edit6.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit7() 
{
	pIndicator->ChangeFocus(6);
	CString text; m_Edit7.GetWindowText(text);
	m_Edit7.SetSel(0, text.GetLength());
}

void CIndPropDlg::OnSetfocusEdit8() 
{
	pIndicator->ChangeFocus(7);
	CString text; m_Edit8.GetWindowText(text);
	m_Edit8.SetSel(0, text.GetLength());
}








// Hook procedure for WH_GETMESSAGE hook type.
LRESULT CALLBACK GetMessageProc(int nCode, WPARAM wParam, LPARAM lParam){

// Switch the module state for the correct handle to be used.
      AFX_MANAGE_STATE(AfxGetStaticModuleState( ));



      // If this is a keystrokes message, translate it in controls'
      // PreTranslateMessage().
      LPMSG lpMsg = (LPMSG) lParam;
      if( (nCode >= 0) &&
         PM_REMOVE == wParam &&
         (lpMsg->message >= WM_KEYFIRST && lpMsg->message <= WM_KEYLAST) &&
         AfxGetApp()->PreTranslateMessage((LPMSG)lParam) )
         {
         // The value returned from this hookproc is ignored, and it cannot
         // be used to tell Windows the message has been handled. To avoid
         // further processing, convert the message to WM_NULL before
         // returning.
         lpMsg->message = WM_NULL;
         lpMsg->lParam = 0L;
         lpMsg->wParam = 0;
         }



      // Passes the hook information to the next hook procedure in
      // the current hook chain.
      return ::CallNextHookEx(hHook, nCode, wParam, lParam);

 }

BOOL CIndPropDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();


	selected = false;
	color = 255;
	y = 0;
	x = 0;
	canUnload = false;
	

      // Install the WH_GETMESSAGE hook function.
      hHook = ::SetWindowsHookEx(
         WH_GETMESSAGE,
         GetMessageProc,
         AfxGetInstanceHandle(),
         GetCurrentThreadId());
      ASSERT (hHook);


	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}


void CIndPropDlg::AddString(CComboBox *combo, CString string)
{

	combo->AddString(string);

	// Find the longest string in the combo box.
	CSize   sz;
	int     dx=0;
	CDC*    pDC = combo->GetDC();
	for (int i=0;i < combo->GetCount();i++)
	{
	   combo->GetLBText( i, string );
	   sz = pDC->GetTextExtent(string);

	   if (sz.cx > dx)
		  dx = sz.cx;
	}
	combo->ReleaseDC(pDC);

	// Set the horizontal extent so every character of all strings can 
	// be scrolled to.
	combo->SetHorizontalExtent(dx);

}

void CIndPropDlg::OnColor() 
{
	CColorDialog dlg;
	if(dlg.DoModal() == IDOK){
		color = dlg.GetColor();
		Invalidate(FALSE);
	}
}

void CIndPropDlg::OnPaint() 
{
	CPaintDC dc(this); // device context for painting
	
	CBrush* br;
	CRect rect;
	
	// Color
	rect.left = x;
	rect.right = x + SCALEX(93);
	rect.top = y;
	rect.bottom = rect.top + SCALEY(9);
	br = new CBrush(color);
	
	dc.FillRect(&rect, br);	
	dc.Draw3dRect(&rect,RGB(230,230,230),RGB(150,150,150));
	
	br->DeleteObject();
	if(NULL != br) delete br;

	if(!selected) m_cmdApply.SetFocus();
	selected = true;

}



void CIndPropDlg::OnSize(UINT nType, int cx, int cy) 
{
	CDialog::OnSize(nType, cx, cy);
}


