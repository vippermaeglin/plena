// COL.h: interface for the CCOL class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_Coordinates_H__1C5B8C95_F2A0_424C_9CAB_CCF5E76B642E__INCLUDED_)
#define AFX_Coordinates_H__1C5B8C95_F2A0_424C_9CAB_CCF5E76B642E__INCLUDED_

//#include "ValueView.h"	// Added by ClassView

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "ValueView.h"	// Added by ClassView
#include "COL.h"
#include <math.h>

class CStockChartXCtrl;

class Coordinates
{
public:	
	double PI;
	Coordinates();
	virtual ~Coordinates();
	CPointF MovePolar(double x, double y, double radius, double theta);
	CPointF MovePolarDouble(double x, double y, double radius, double theta);
	double DegreeToRad(double degree);
private:
	//CPoint actualPoint;

};

#endif // !defined(AFX_COL_H__1C5B8C95_F2A0_424C_9CAB_CCF5E76B642E__INCLUDED_)
