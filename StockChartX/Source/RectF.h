
#include "PointF.h"
#pragma once
class CRectF
{
public:
	double left,top,right,bottom;
	CRectF(void);
	CRectF(CRect rect);
	CRectF(double _left,double _top, double _right, double _bottom);
	CRectF(CPointF p1, CPointF p2);
	~CRectF(void);
	double Height(void);
	double Width(void);
};

