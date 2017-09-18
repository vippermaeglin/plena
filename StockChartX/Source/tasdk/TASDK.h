/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 2002
 * Company:      Modulus Financial Engineering
 * @author 	 Modulus FE
 * @version 	 1.0
 */

#if !defined(TASDK_INCLUDED_)
#define TASDK_INCLUDED_

#include <cmath>

#include "Note.h"
#include "Field.h"
#include "Recordset.h"
#include "Navigator.h"
#include "General.h"
#include "LinearRegression.h"
#include "MovingAverage.h"
#include "Oscillator.h"
#include "Index.h"
#include "Bands.h"


#define TASDK_INCLUDED_



class TASDK
{
public:
	TASDK();
	virtual ~TASDK();
	static bool CompareNoCase(CString string1, CString string2);
	double maxVal(double Value1, double Value2);
	double minVal(double Value1, double Value2);
	int maxVal(int Value1, int Value2);
	int minVal(int Value1, int Value2);
	double normalize(double Max, double Min, double Value);
	

};

#endif;