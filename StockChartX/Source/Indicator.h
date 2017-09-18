// Indicator.h: interface for the CIndicator class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATOR_H__343E05B5_395F_42C2_9BE1_B74DC1F6AA19__INCLUDED_)
#define AFX_INDICATOR_H__343E05B5_395F_42C2_9BE1_B74DC1F6AA19__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Series.h"
#include "Field.h"

#include "IndPropDlg.h"

class CIndPropDlg;
class CChartPanel;
class CSeries;

class CIndicator : public CSeries
{

private:

	void SetMAComboSel(CComboBox* combo, LPCSTR paramDef);
	LPCTSTR MATypeToStr(UINT type);
	UINT GetMAType(CComboBox* combo);
	void EnumMATypes(CComboBox* combo);
	void SetComboDefault(CComboBox* combo, LPCTSTR item);
	void EnumPanels(CComboBox* combo);	
	void EnumSymbols(CComboBox* combo);
	void EnumSeries(CComboBox* combo);
	CComboBox* GetComboBox(int index);	
	CEdit* GetEdit(int index);
	CStatic* GetStatic(int index);
	CRect dlgRect;
	bool inputError;
	bool calculating;	

protected:
	void UpdateSeries(CSeries* series);
	CIndPropDlg* m_pIndDlg;
	CString	m_Name;
	double nSpace;
	int paramCount;
	CStockChartXCtrl *pCtrl;	


public:
	void OnCancleDialog();
	bool EnsureField(CField* field, LPCTSTR name);

	bool HasCircularReference(CSeries* series);

	CSeries* EnsureSeries(LPCTSTR name);	

	virtual void SetParamInfo();

	LPCTSTR GetParamName(UINT type);

	LPCTSTR GetParamDescription(int index);
	void ChangeFocus(int index);
	void SetParam(int index, int type, LPCTSTR defaultValue);
	void SetUserInput();

	bool dialogShown;
	bool GetUserInput();
	void OnDestroy();
	void Initialize();
	void ProcessError(LPCTSTR Description);

	void SetSeriesColor(LPCTSTR Name, OLE_COLOR Color);
	void SetSeriesSignal(LPCTSTR Name, OLE_COLOR Color, int Style, int Weight);

	CField* SeriesToField(LPCTSTR Name, LPCTSTR Series, int Length);

	void OnDoubleClick(CPoint point);
	void OnPaintXOR(CDC* pDC);	
	void OnRButtonUp(CPoint point);
	void OnLButtonDown(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnMouseMove(CPoint point);
	void OnPaint(CDC* pDC);

	void SetName( LPCTSTR pName );
	LPCTSTR GetName();
	virtual BOOL Calculate();
	void SetOwnerChartPanel( CChartPanel *pOwner );

	CIndicator::CIndicator();
	CIndicator(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicator();

};

#endif // !defined(AFX_INDICATOR_H__343E05B5_395F_42C2_9BE1_B74DC1F6AA19__INCLUDED_)
