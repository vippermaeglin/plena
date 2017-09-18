// SeriesStandard.h: interface for the CSeriesStandard class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SERIESSTANDARD_H__62BE3D8E_8ECB_468E_9FE7_D5235FF25FB8__INCLUDED_)
#define AFX_SERIESSTANDARD_H__62BE3D8E_8ECB_468E_9FE7_D5235FF25FB8__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Series.h"

class CPriceStylePointAndFigure;

class CSeriesStandard : public CSeries  
{
public:
	void OnDoubleClick(CPoint point);
	void OnPaintXOR(CDC* pDC);	
	void OnRButtonUp(CPoint point);
	void OnLButtonDown(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnMouseMove(CPoint point);
	void OnPaint(CDC* pDC);
	CSeriesStandard(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CSeriesStandard();
private:	
	double nSpace;
	CStockChartXCtrl* pCtrl;
};

#endif // !defined(AFX_SERIESSTANDARD_H__62BE3D8E_8ECB_468E_9FE7_D5235FF25FB8__INCLUDED_)
