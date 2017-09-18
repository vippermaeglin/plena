#if !defined(AFX_INDPROPDLG_H__88F3049D_4859_49E6_982F_6C548F42651B__INCLUDED_)
#define AFX_INDPROPDLG_H__88F3049D_4859_49E6_982F_6C548F42651B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// IndPropDlg.h : header file
//

#include "StockChartX.h"
#include "Indicator.h"

class CIndicator;
class CStockChartXCtrl;

/////////////////////////////////////////////////////////////////////////////
// CIndPropDlg dialog

class CIndPropDlg : public CDialog
{
// Construction
public:
	bool selected;
	OLE_COLOR color;
	int y;
	int x;
	void AddString(CComboBox* combo, CString string);
	bool canUnload;
	CIndPropDlg(CWnd* pParent = NULL);   // standard constructor
	CIndicator* pIndicator;
	CStockChartXCtrl* pCtrl;
	
public:
// Dialog Data
	//{{AFX_DATA(CIndPropDlg)
	enum { IDD = IDD_PROP_DIALOG };
	CButton	m_cmdColor;
	CStatic	m_Label7;
	CStatic	m_Label8;
	CStatic	m_Label6;
	CStatic	m_Label5;
	CStatic	m_Label4;
	CStatic	m_Label3;
	CStatic	m_Label2;
	CStatic	m_Label1;
	CEdit	m_Edit8;
	CEdit	m_Edit7;
	CEdit	m_Edit6;
	CEdit	m_Edit5;
	CEdit	m_Edit4;
	CEdit	m_Edit3;
	CEdit	m_Edit2;
	CEdit	m_Edit1;
	CComboBox	m_Combo8;
	CComboBox	m_Combo7;
	CComboBox	m_Combo6;
	CComboBox	m_Combo5;
	CComboBox	m_Combo4;
	CComboBox	m_Combo3;
	CComboBox	m_Combo2;
	CComboBox	m_Combo1;
	CButton	m_fraMain;
	CButton	m_cmdHelp;
	CButton	m_cmdCancel;
	CButton	m_cmdApply;
	CButton	m_txtInfo;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CIndPropDlg)				
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:	
	// Generated message map functions
	//{{AFX_MSG(CIndPropDlg)
	afx_msg void OnShowWindow(BOOL bShow, UINT nStatus);
	afx_msg void OnClose();
	afx_msg void OnCmdApply();
	afx_msg void OnCmdCancel();
	afx_msg void OnCmdHelp();	
	afx_msg void OnSetfocusCombo1();
	afx_msg void OnSetfocusCombo2();
	afx_msg void OnSetfocusCombo3();
	afx_msg void OnSetfocusCombo4();
	afx_msg void OnSetfocusCombo5();
	afx_msg void OnSetfocusCombo6();
	afx_msg void OnSetfocusCombo7();
	afx_msg void OnSetfocusCombo8();
	afx_msg void OnSetfocusEdit1();
	afx_msg void OnSetfocusEdit2();
	afx_msg void OnSetfocusEdit3();
	afx_msg void OnSetfocusEdit4();
	afx_msg void OnSetfocusEdit5();
	afx_msg void OnSetfocusEdit6();
	afx_msg void OnSetfocusEdit7();
	afx_msg void OnSetfocusEdit8();	
	virtual BOOL OnInitDialog();
	afx_msg void OnColor();	
	afx_msg void OnPaint();	
	afx_msg void OnSize(UINT nType, int cx, int cy);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_INDPROPDLG_H__88F3049D_4859_49E6_982F_6C548F42651B__INCLUDED_)
