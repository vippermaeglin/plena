// Bands.h: interface for the Bands class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_BANDS_H__9DA76978_5BDD_4E82_B83A_9B08D1CC443A__INCLUDED_)
#define AFX_BANDS_H__9DA76978_5BDD_4E82_B83A_9B08D1CC443A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class CNavigator;
class CField;
class CRecordset;

class CBands  
{
public:
	CBands();
	virtual ~CBands();
	CRecordset* HighLowBands(CNavigator* pNav, CField* HighPrice,
            CField* LowPrice, CField* ClosePrice, int Periods);
	CRecordset* CBands::HILOBands(CNavigator* pNav, CField* HighPrice,
            CField* LowPrice, CField* ClosePrice, int PeriodsHigh, int PeriodsLow, int Shift, double Scale);
	CRecordset* MovingAverageEnvelope(CNavigator* pNav, CField* pSource,
            int Periods, int MAType, double Shift);
	CRecordset* BollingerBands(CNavigator* pNav, CField* pSource,
            int Periods, double StandardDeviations, int MAType);
	CRecordset* FractalChaosBands(CNavigator* pNav, CRecordset* pOHLCV,
            int Periods);
   CRecordset* PrimeNumberBands(CNavigator* pNav, 
			CField* HighPrice, CField* LowPrice);

};

#endif // !defined(AFX_BANDS_H__9DA76978_5BDD_4E82_B83A_9B08D1CC443A__INCLUDED_)
