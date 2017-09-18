#include "stdafx.h"
#include "RectF.h"


CRectF::CRectF(void)
{
}
CRectF::~CRectF(void)
{
}


CRectF::CRectF(CRect rect){
	left=rect.left;
	top=rect.top;
	right=rect.right;
	bottom=rect.bottom;
}

CRectF::CRectF(double _left,double _top, double _right, double _bottom){
	left=_left;
	top=_top;
	right=_right;
	bottom=_bottom;
}

CRectF::CRectF(CPointF p1, CPointF p2){
	left=p1.x;
	top=p1.y;
	right=p2.x;
	bottom=p2.y;
}

double CRectF::Height(void){
	return abs(top-bottom);
}

double CRectF::Width(void){
	return abs(right-left);
}

