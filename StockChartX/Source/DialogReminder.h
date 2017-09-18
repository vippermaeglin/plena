#if !defined(AFX_DIALOGREMINDER_H__F1678D0A_026A_46CF_A34A_17E4567DBFF5__INCLUDED_)
#define AFX_DIALOGREMINDER_H__F1678D0A_026A_46CF_A34A_17E4567DBFF5__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DialogReminder.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDialogReminder dialog

class CDialogReminder : public CDialog
{
// Construction
public:
	CDialogReminder(CWnd* pParent = NULL);   // standard constructor
	CString GetVersionInfo();
	bool m_minimized;
	bool m_display;
	bool m_ok;
	bool m_expired;
	CString m_daysLeft;
	CString m_usesLeft;

// Dialog Data
	//{{AFX_DATA(CDialogReminder)
	enum { IDD = IDD_REMINDER };
	CButton	m_cmdOK;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDialogReminder)
	public:
	virtual int DoModal();	
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

private:	
	int CoderDecoder();
	CString encode(char*);
	CString decode(char*);

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDialogReminder)
	afx_msg void OnBuyNow();
	afx_msg void OnClose();
	afx_msg void OnPaint();
	virtual BOOL OnInitDialog();
	virtual void OnOK();
	afx_msg void OnLiveHelp();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIALOGREMINDER_H__F1678D0A_026A_46CF_A34A_17E4567DBFF5__INCLUDED_)
