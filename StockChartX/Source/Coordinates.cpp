// COL.cpp: implementation of the CCOL class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "math.h"
#include "COL.h"
#include "Coordinates.h"

//#define _CONSOLE_DEBUG 1

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
Coordinates::Coordinates(){
	PI = 3.14159265;
}

Coordinates::~Coordinates(){
	
}

CPointF Coordinates::MovePolar(double x, double y, double radius, double theta){	
	CPointF pointReturn;
	pointReturn.x = (double) x + radius*cos(theta);
	pointReturn.y = (double) y + radius*sin(theta);
	return pointReturn;
}

CPointF Coordinates::MovePolarDouble(double x, double y, double radius, double theta){	
	CPointF pointReturn;
	pointReturn.x = x + radius*cos(theta);
	pointReturn.y = y + radius*sin(theta);
	return pointReturn;
}


double Coordinates::DegreeToRad(double degree){	
	return (2*PI/360)*degree;
}
