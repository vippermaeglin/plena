/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "General.h"
// for .net use:
#include <iostream>

//#define _CONSOLE_DEBUG

// for 6.0 use:
//#include <iostream.h>

CGeneral::CGeneral()
{

}

CGeneral::~CGeneral()
{

}

  CRecordset* CGeneral::HighMinusLow(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    pNav->MoveFirst();

    for(Record = 1; Record < RecordCount + 1; Record++){
        Value = (pOHLCV->getField("High")->getValue(Record) -
        pOHLCV->getField("Low")->getValue(Record));
        Field1->setValue(Record,Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CGeneral::MedianPrice(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    pNav->MoveFirst();

    for(Record = 1; Record < RecordCount + 1; Record++){
        Value = (pOHLCV->getField("High")->getValue(Record) +
        pOHLCV->getField("Low")->getValue(Record)) / 2;
        Field1->setValue(Record, Value);
        pNav->MoveNext();
    } //Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CGeneral::TypicalPrice(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    pNav->MoveFirst();

    for(Record = 1; Record < RecordCount + 1; Record++){
        Value = (pOHLCV->getField("High")->getValue(Record) +
                pOHLCV->getField("Low")->getValue(Record) +
                pOHLCV->getField("Close")->getValue(Record)) / 3;
        Field1->setValue(Record, Value);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CGeneral::WeightedClose(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    pNav->MoveFirst();

    for(Record = 1; Record < RecordCount + 1; Record++){
       Value = (pOHLCV->getField("High")->getValue(Record) +
                pOHLCV->getField("Low")->getValue(Record) +
                (pOHLCV->getField("Close")->getValue(Record) * 2)) / 4;
        Field1->setValue(Record, Value);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CGeneral::VolumeROC(CNavigator* pNav, CField* Volume, int Periods, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;
    int Start = 0;
    double PrevVolume = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = Periods + 1;

    pNav->setPosition(Start);

     for(Record = Start; Record < RecordCount + 1; Record++){
        pNav->setPosition(pNav->getPosition() - Periods);
        PrevVolume = Volume->getValue(pNav->getPosition());
        pNav->setPosition(pNav->getPosition() + Periods);
        if(PrevVolume != 0){
            Value = ((Volume->getValue(pNav->getPosition()) - PrevVolume) / PrevVolume) * 100;
        }
        Field1->setValue(Record, Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CGeneral::Volume(CNavigator* pNav, CField* Volume, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;
    int Start = 0;
    double PrevVolume = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    //Start = Periods + 1;

    pNav->setPosition(Start);

     for(Record = Start; Record < RecordCount + 1; Record++){
        pNav->setPosition(pNav->getPosition()/* - Periods*/);
        PrevVolume = Volume->getValue(pNav->getPosition());
        pNav->setPosition(pNav->getPosition()/* + Periods*/);
        if(PrevVolume != 0){
            Value = Volume->getValue(pNav->getPosition()); //((Volume->getValue(pNav->getPosition()) - PrevVolume) / PrevVolume) * 100;
        }
        Field1->setValue(Record, Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CGeneral::PriceROC(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;
    int Start = 0;
    double PrevPrice = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = Periods + 1;

    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){
        PrevPrice = pSource->getValue(pNav->getPosition() - Periods);
        Value = ((pSource->getValue(pNav->getPosition()) - PrevPrice) / PrevPrice) * 100;
        Field1->setValue(Record, Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  double CGeneral::CorrelationAnalysis(CField* pSource1, CField* pSource2){

    int Record = 0;
    int RecordCount = 0;
    double Total = 0;
    double A = 0;
    double B = 0;

    RecordCount = pSource1->getRecordCount();

    for (Record = 2; Record < RecordCount + 1; Record++){
        A = (pSource1->getValue(Record) - pSource1->getValue(Record - 1));
        B = (pSource2->getValue(Record) - pSource2->getValue(Record - 1));
		if (A < 0){A = -1 * A;}
		if (B < 0){B = -1 * B;}
        Total += (A * B);
    }//Record

    Total = Total / (RecordCount - 2);

    return 1 - Total;

  }

CRecordset* CGeneral::StandardDeviation(CNavigator* pNav, CField* pSource,
            int Periods, double StandardDeviations,
            int MAType, LPCTSTR Alias){

#ifdef _CONSOLE_DEBUG
	printf("StandardDeviation() Periods=%d Stand=%f",Periods,StandardDeviations);
#endif
    CRecordset* Results = NULL;

    CMovingAverage* MA = new CMovingAverage();
    CField* Field1;
    int Period = 0;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Sum = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    switch (MAType)
    {
    case indExponentialMovingAverage:
		Results = MA->ExponentialMovingAverage(pNav, pSource, Periods, "Temp");
#ifdef _CONSOLE_DEBUG
		printf("\n\t\tindExponentialMovingAverage");
#endif
        break;
    case indTimeSeriesMovingAverage:
        Results = MA->TimeSeriesMovingAverage(pNav, pSource, Periods, "Temp");
        break;
    case indTriangularMovingAverage:
        Results = MA->TriangularMovingAverage(pNav, pSource, Periods, "Temp");
        break;
    case indVariableMovingAverage:
        Results = MA->VariableMovingAverage(pNav, pSource, Periods, "Temp");
        break;
    case indWeightedMovingAverage:
        Results = MA->WeightedMovingAverage(pNav, pSource, Periods, "Temp");
        break;
    case indVIDYA:
        Results = MA->VIDYA(pNav, pSource, Periods, 0.65, "Temp");
        break;
	default:
		Results = MA->SimpleMovingAverage(pNav, pSource, Periods, "Temp");
#ifdef _CONSOLE_DEBUG
		printf("\n\t\tindSimpleMovingAverage");
#endif
		break;
    }

    Start = Periods + 1;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        Sum = 0;
        Value = Results->getValue("Temp", pNav->getPosition());

		if (Record<5){
#ifdef _CONSOLE_DEBUG
			//printf("\n\t\tPRMA[%d]=%f", Record, Value);
#endif
		}
        for(Period = 1; Period < Periods + 1; Period++){
            Sum += (pSource->getValue(pNav->getPosition()) - Value) *
            (pSource->getValue(pNav->getPosition()) - Value);
            pNav->MovePrevious();
        }//Period

        pNav->setPosition(pNav->getPosition() + Periods);

		Value = StandardDeviations *  sqrt(Sum / Periods);
		if (Record<5){
#ifdef _CONSOLE_DEBUG
			//printf("\n\t\tSTDV[%d]=%f", Record, Value);
#endif
		}
        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    } //Record

	if(Results){
		Results->addField(Field1);
    }

	delete MA;
    return Results;

  }

  CNote* CGeneral::MaxValue(CField* pSource, int StartPeriod, int EndPeriod){

    int Record = 0;
    double HH = 0;
    int HHP = 0;
    CNote* Value = new CNote();

    if(StartPeriod > EndPeriod){
        return Value;
    }

    for(Record = StartPeriod; Record < EndPeriod + 1; Record++){
        if(pSource->getValue(Record) > HH){
            HH = pSource->getValue(Record);
            HHP = Record;
        }
    }//Record

    Value->setPeriod(HHP);
    Value->setValue(HH);

    return Value;

  }

  CNote* CGeneral::MinValue(CField* pSource, int StartPeriod, int EndPeriod){

    int Record = 0;
    double LL = 0;
    int LLP = 0;
    CNote* Value = new CNote();

    if(StartPeriod > EndPeriod){
        return Value;
    }

    LL = pSource->getValue(StartPeriod);
    LLP = StartPeriod;

    for(Record = StartPeriod; Record < EndPeriod + 1; Record++){
        if(pSource->getValue(Record) < LL){
            LL = pSource->getValue(Record);
            LLP = Record;
        }
    }//Record

    Value->setPeriod(LLP);
    Value->setValue(LL);

    return Value;

  }


  // 9/6/04 RDG
  bool CGeneral::IsPrime(long number){
    
    long divisor = 0;
    long increment = 0;
    long maxDivisor = 0;
    
    if(number > 3){
        if(number % 2 == 0) return false;
        if(number % 3 == 0) return false;
    }
    
    divisor = 5;
    increment = 2;
    maxDivisor = sqrt( (double)number) + 1;
    
    while(divisor <= maxDivisor){
        if(number % divisor == 0) return false;
        divisor += increment;
        increment = 6 - increment;
    }
    
    return true;

  }



   


CRecordset* CGeneral::HHV(CNavigator* pNav, CField* High,
					 int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();

    CMovingAverage* MA = new CMovingAverage();
    CField* Field1;
    int Period = 0;
    int RecordCount = 0;
    int Record = 0;
	double Max = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);
     
	for(Record = Periods + 1; Record != RecordCount + 1; ++Record){	
        Max = High->getValue(Record);
        for(int N = Record; N != (Record - Periods) - 1; --N){
            if(High->getValue(N) > Max){
                Max = High->getValue(N);
            }
        }        
        Field1->setValue(Record, Max);        
    }

	Results->addField(Field1);

	delete MA;

    return Results;

 }


CRecordset* CGeneral::LLV(CNavigator* pNav, CField* Low,
					 int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();

    CMovingAverage* MA = new CMovingAverage();
    CField* Field1;
    int Period = 0;
    int RecordCount = 0;
    int Record = 0;
	double Min = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);	
     
	for(Record = Periods + 1; Record != RecordCount + 1; ++Record){
        Min = Low->getValue(Record);
        for(int N = Record; N != (Record - Periods) - 1; --N){
            if(Low->getValue(N) < Min){
                Min = Low->getValue(N);
            }
        }        
        Field1->setValue(Record, Min);        
    }  

	Results->addField(Field1);
    
	delete MA;

	return Results;

 }


