/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "Index.h"
// for .net use:
#include <iostream>
// for 6.0 use:
//#include <iostream.h>

//#define _CONSOLE_DEBUG

CIndex::CIndex()
{

}

CIndex::~CIndex()
{

}

  CRecordset* CIndex::MoneyFlowIndex(CNavigator* pNav, CRecordset* pOHLCV,
            int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    double Price1 = 0;
    double Price2 = 0;
    double V = 0;
    double Today = 0;
    double PosFlow = 0;
    double NegFlow = 0;
    double MoneyIndex = 0;
    double MoneyRatio = 0;

    RecordCount = pNav->getRecordCount();

    if(Periods < 1 || Periods > RecordCount){
        //cout << "Index->MoneyFlowIndex: Invalid Periods";
        return NULL;
    }

    Field1 = new CField(RecordCount, Alias);

    Start = Periods + 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 2; Record++){

        PosFlow = 0;
        NegFlow = 0;

        pNav->setPosition(Record - Periods);

        for(Period = 1; Period < Periods + 1; Period++){

            pNav->MovePrevious();
            Price1 = (pOHLCV->getField("High")->getValue(pNav->getPosition()) +
                    pOHLCV->getField("Low")->getValue(pNav->getPosition()) +
                    pOHLCV->getField("Close")->getValue(pNav->getPosition())) / 3;
            pNav->MoveNext();
            V = pOHLCV->getField("Volume")->getValue(pNav->getPosition());
            if(V < 1){V = 1;}
            Price2 = (pOHLCV->getField("High")->getValue(pNav->getPosition()) +
                    pOHLCV->getField("Low")->getValue(pNav->getPosition()) +
                    pOHLCV->getField("Close")->getValue(pNav->getPosition())) / 3;

            if(Price2 > Price1){
                PosFlow += Price2 * V;
            }
            else if(Price2 < Price1){
                NegFlow += Price2 * V;
            }

            pNav->MoveNext();

        }//Period

        pNav->MovePrevious();

        if(PosFlow != 0 && NegFlow != 0){
            MoneyRatio = PosFlow / NegFlow;
            MoneyIndex = 100 - (100 / (1 + MoneyRatio));
            Field1->setValue(pNav->getPosition(), MoneyIndex);
        }

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::TradeVolumeIndex(CNavigator* pNav, CField* pSource,
              CField* Volume, double MinTickValue, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    int Direction = 0;
    int LastDirection = 0;
    double Change = 0;
    double TVI = 0;

    RecordCount = pNav->getRecordCount();

    if(MinTickValue < 0){
        //cout << "Index->TradeVolumeIndex: Invalid MinTickValue";
    }

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        Change = pSource->getValue(pNav->getPosition()) -
                pSource->getValue(pNav->getPosition() - 1);

        if(Change > MinTickValue){
            Direction = 1;
        }
        else if(Change < -MinTickValue){
            Direction = -1;
        }

        if(Change <= MinTickValue && Change >= -MinTickValue){
            Direction = LastDirection;
        }

        LastDirection = Direction;

        if(Direction == 1){
            TVI = TVI + Volume->getValue(pNav->getPosition());
        }
        else if(Direction == -1){
            TVI = TVI - Volume->getValue(pNav->getPosition());
        }

        Field1->setValue(pNav->getPosition(), TVI);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::SwingIndex(CNavigator* pNav, CRecordset* pOHLCV,
            double LimitMoveValue, LPCTSTR Alias){

	TASDK* TASDK1 = new TASDK();
	
    CRecordset* Results = new CRecordset();

    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    double Cy = 0;
    double Ct = 0;
    double Oy = 0;
    double Ot = 0;
    double Hy = 0;
    double Ht = 0;
    double Ly = 0;
    double Lt = 0;
    double K = 0;
    double R = 0;
    double A = 0;
    double B  = 0;
    double C = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    if(LimitMoveValue <= 0){
        //cout << "Index->SwingIndex: Invalid LimitMoveValue";
        return NULL;
    }

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        Oy = pOHLCV->getField("Open")->getValue(pNav->getPosition() - 1);
        Ot = pOHLCV->getField("Open")->getValue(pNav->getPosition());
        Hy = pOHLCV->getField("High")->getValue(pNav->getPosition() - 1);
        Ht = pOHLCV->getField("High")->getValue(pNav->getPosition());
        Ly = pOHLCV->getField("Low")->getValue(pNav->getPosition() - 1);
        Lt = pOHLCV->getField("Low")->getValue(pNav->getPosition());
        Cy = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
        Ct = pOHLCV->getField("Close")->getValue(pNav->getPosition());

        K = TASDK1->maxVal(fabs(Ht - Cy), fabs(Lt - Cy));

        A = fabs(Ht - Cy);
        B = fabs(Lt - Cy);
        C = fabs(Ht - Lt);

        if(A > B && A > C){
            R = fabs(Ht - Cy) - 0.5 * fabs(Lt - Cy) + 0.25 * fabs(Cy - Oy);
        }
        else if(B > A && B > C){
            R = fabs(Lt - Cy) - 0.5 * fabs(Ht - Cy) + 0.25 * fabs(Cy - Oy);
        }
        else if(C > A && C > B){
            R = fabs(Ht - Lt) + 0.25 * fabs(Cy - Oy);
        }

		if(R > 0 && LimitMoveValue > 0)
		{
			Value = 50 * ((Ct - Cy) + 0.5 * (Ct - Ot) + 0.25 *
				    (Cy - Oy)) / R * K / LimitMoveValue;
		}
		else
		{
			Value = 0;
		}

        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

	delete TASDK1;
    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::AccumulativeSwingIndex(CNavigator* pNav, CRecordset* pOHLCV,
            double LimitMoveValue, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CRecordset* RawSI;
    CField* Field1;
    CIndex* SI = new CIndex();
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    if(LimitMoveValue <= 0){
        //cout << "Index->AccumulativeSwingIndex: Invalid LimitMoveValue";
    }

    RawSI = SI->SwingIndex(pNav, pOHLCV, LimitMoveValue, "SI");

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){
        Value = RawSI->getValue("SI", pNav->getPosition()) +
                Field1->getValue(pNav->getPosition() - 1);
        Field1->setValue(pNav->getPosition(), Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
	
	delete RawSI;
	delete SI;
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::RelativeStrengthIndex(CNavigator* pNav, CField* pSource,
            int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    CField* AU;
    CField* AD;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    double UT = 0;
    double DT = 0;
    double UpSum = 0;
    double DownSum = 0;
    double RS = 0;
    double RSI = 0;
	double value = 0;

    RecordCount = pNav->getRecordCount();

    if(Periods < 1 || Periods > RecordCount){
        //cout << "Index->RelativeStrengthIndex: Invalid Periods";
    }

    Field1 = new CField(RecordCount, Alias);
    AU = new CField(RecordCount, "AU");
    AD = new CField(RecordCount, "AD");
	
    pNav->setPosition(2);
    for(Period = 1; Period < Periods + 1; Period++){
        UT = 0;
        DT = 0;
			
		if(value != NULL_VALUE){
			if(value > pSource->getValue(pNav->getPosition() - 1)){
				UT = pSource->getValue(pNav->getPosition()) -
						pSource->getValue(pNav->getPosition() - 1);
				UpSum += UT;
			}
			else if(pSource->getValue(pNav->getPosition()) <
						pSource->getValue(pNav->getPosition() - 1)){
				DT = pSource->getValue(pNav->getPosition() - 1) -
						pSource->getValue(pNav->getPosition());
				DownSum += DT;
			}
		}
        pNav->MoveNext();
    }//Period

    pNav->MovePrevious();

    UpSum = UpSum / Periods;
    AU->setValue(pNav->getPosition(), UpSum);
    DownSum = DownSum / Periods;
    AD->setValue(pNav->getPosition(), DownSum);

    RS = UpSum / DownSum;

    RSI = 100 - (100 / (1 + RS));

    Start = Periods + 3;

    for(Record = Start; Record < RecordCount + 2; Record++){

        pNav->setPosition(Record - Periods);

        UpSum = 0;
        DownSum = 0;

        for(Period = 1; Period < Periods + 1; Period++){
            UT = 0;
            DT = 0;
			value = pSource->getValue(pNav->getPosition());
			if(value != NULL_VALUE){
				if(value > pSource->getValue(pNav->getPosition() - 1)){
					UT = pSource->getValue(pNav->getPosition()) -
							pSource->getValue(pNav->getPosition() - 1);
					UpSum += UT;
				}
				else if(pSource->getValue(pNav->getPosition()) <
							pSource->getValue(pNav->getPosition() - 1)){
					DT = pSource->getValue(pNav->getPosition() - 1) -
							pSource->getValue(pNav->getPosition());
					DownSum += DT;
				}
			}
            pNav->MoveNext();
        }//Period

        pNav->MovePrevious();

        UpSum = (((AU->getValue(pNav->getPosition() - 1) * (Periods - 1)) + UT)) / Periods;
        DownSum = (((AD->getValue(pNav->getPosition() - 1) * (Periods - 1)) + DT)) / Periods;

        AU->setValue(pNav->getPosition(), UpSum);
        AD->setValue(pNav->getPosition(), DownSum);

        if(DownSum == 0) DownSum = UpSum;
        if(UpSum == 0){
            RS = 0;
        }
		else{
            RS = UpSum / DownSum;
        }

        RS = (UpSum / DownSum);
        RSI = 100 - (100 / (1 + RS));

        Field1->setValue(pNav->getPosition(), RSI);

    }//Record

	delete AU;
	delete AD;
    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::ComparativeRelativeStrength(CNavigator* pNav, CField* pSource1,
            CField* pSource2, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    for(Record = 1; Record < RecordCount + 1; Record++){

        Value = pSource1->getValue(pNav->getPosition()) / pSource2->getValue(pNav->getPosition());
		if(Value == 1) Value = NULL_VALUE;
        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::PriceVolumeTrend(CNavigator* pNav, CField* pSource,
            CField* Volume, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){
        Value = (((pSource->getValue(pNav->getPosition()) -
                pSource->getValue(pNav->getPosition() - 1)) /
                pSource->getValue(pNav->getPosition() - 1)) *
                Volume->getValue(pNav->getPosition())) +
                Field1->getValue(pNav->getPosition() - 1);
        Field1->setValue(pNav->getPosition(), Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::PositiveVolumeIndex(CNavigator* pNav,
            CField* pSource, CField* Volume, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    Field1->setValue(1, 1);

    for(Record = Start; Record < RecordCount + 1; Record++){

        if(Volume->getValue(pNav->getPosition()) >
                Volume->getValue(pNav->getPosition() - 1)){
            Value = Field1->getValue(pNav->getPosition() - 1) +
                    (pSource->getValue(pNav->getPosition()) -
                    pSource->getValue(pNav->getPosition() - 1)) /
                    pSource->getValue(pNav->getPosition() - 1) *
                    Field1->getValue(pNav->getPosition() - 1);
        }
        else{
            Value = Field1->getValue(pNav->getPosition() - 1);
        }

        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::NegativeVolumeIndex(CNavigator* pNav,
            CField* pSource, CField* Volume, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();	
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    Field1->setValue(1, 1);

    for(Record = Start; Record < RecordCount + 1; Record++){

        if(Volume->getValue(pNav->getPosition()) <
                Volume->getValue(pNav->getPosition() - 1)){
            Value = Field1->getValue(pNav->getPosition() - 1) +
                    (pSource->getValue(pNav->getPosition()) -
                    pSource->getValue(pNav->getPosition() - 1)) /
                    pSource->getValue(pNav->getPosition() - 1) *
                    Field1->getValue(pNav->getPosition() - 1);
        }
        else{
            Value = Field1->getValue(pNav->getPosition() - 1);
        }

        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::Performance(CNavigator* pNav, CField* pSource, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Start  = 0;
    double FirstPrice = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    FirstPrice = pSource->getValue(1);

    for(Record = Start; Record < RecordCount + 1; Record++){

        Value = ((pSource->getValue(pNav->getPosition()) -
                FirstPrice) / FirstPrice) * 100;
        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::OnBalanceVolume(CNavigator* pNav, CField* pSource,
            CField* Volume, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        if(pSource->getValue(pNav->getPosition() - 1) <
            pSource->getValue(pNav->getPosition())){
            Value = Field1->getValue(pNav->getPosition() - 1) +
                Volume->getValue(pNav->getPosition());
        }
        else if(pSource->getValue(pNav->getPosition()) <
            pSource->getValue(pNav->getPosition() - 1)){
            Value = Field1->getValue(pNav->getPosition() - 1) -
                Volume->getValue(pNav->getPosition());
        }
        else{
            Value = Field1->getValue(pNav->getPosition() - 1);
        }

        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::MassIndex(CNavigator* pNav, CRecordset* pOHLCV, int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    CGeneral* GE = new CGeneral();
    CMovingAverage* MA = new CMovingAverage();
    CField* Temp;
    CRecordset* HML;
    CRecordset* EMA1;
    CRecordset* EMA2;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    int Period = 0;
    double Sum = 0;

    RecordCount = pNav->getRecordCount();

    if(Periods < 1 || Periods > RecordCount){
        //cout << "Index->MassIndex: Invalid Periods";
        return NULL;
    }

    Field1 = new CField(RecordCount, Alias);

    HML = GE->HighMinusLow(pNav, pOHLCV, "HML");
    Temp = HML->getField("HML");
    EMA1 = MA->ExponentialMovingAverage(pNav, Temp, 9, "EMA");
    Temp = EMA1->getField("EMA");
    EMA2 = MA->ExponentialMovingAverage(pNav, Temp, 9, "EMA");

    Start = (Periods * 2) + 1;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 2; Record++){

        Sum = 0;

        pNav->setPosition(Record - Periods);
        for(Period = 1; Period < Periods + 1; Period++){
            Sum = Sum + (EMA1->getValue("EMA", pNav->getPosition()) /
            EMA2->getValue("EMA", pNav->getPosition()));
            pNav->MoveNext();
        }//Period
        pNav->MovePrevious();

        Field1->setValue(pNav->getPosition(), Sum);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();

	delete GE;
	delete MA;
	delete HML;
	delete EMA1;
	delete EMA2;

    Results->addField(Field1);
    return Results;

  }

  CRecordset* CIndex::ChaikinMoneyFlow(CNavigator* pNav, CRecordset* pOHLCV, 
	  int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    
	int RecordCount = 0;
    int Record = 0;
    double Value = 0;
    double Sum = 0, SumV = 0;
	double a = 0, b = 0;
    
    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    
	for(Record = Periods ; Record < RecordCount + 1; Record++){
	
        Sum = 0;
        SumV = 0;

		double test = pOHLCV->getValue("Close", Record - 0);

		for(int n = 0; n != Periods; ++n){
			//Chaikin original formula:
			/*a = ((pOHLCV->getValue("Close", Record - n) - pOHLCV->getValue("Low", Record - n)) - 
            (pOHLCV->getValue("High", Record - n) - pOHLCV->getValue("Close", Record - n)));*/

			//Barchart formula:
			a = pOHLCV->getValue("Close", Record - n) - pOHLCV->getValue("Open", Record - n);
			b = (pOHLCV->getValue("High", Record - n) - pOHLCV->getValue("Low", Record - n));
            //if(a != 0 && b != 0) Sum += (a / b);
            if(a != 0 && b != 0) Sum += (a / b)*pOHLCV->getValue("Volume", Record - n);


            SumV += pOHLCV->getValue("Volume", Record - n);
        }

        //Value = (Sum / SumV) * pow(SumV,2);
		Value = (Sum / SumV) ;
     
        Field1->setValue(Record, Value);
        
    } //Record

    pNav->MoveFirst();
    
    Results->addField(Field1);
    return Results;

  }
    
  CRecordset* CIndex::AccumulationDistribution(CNavigator* pNav, CRecordset* pOHLCV, 
	  LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    
	int RecordCount = 0;
    int Record = 0;
    double Value = 0;
    double Sum = 0, SumV = 0;
	double a = 0, b = 0;
    
    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Field1->setValue(Record, 0);

	for(Record = 1 ; Record < RecordCount + 1; Record++){
	
        Value = 0;
		
		//Barchart formula 1: http://www.barchart.com/education/studies.php?what=accdis
		a = ((pOHLCV->getValue("Close", Record) - pOHLCV->getValue("Low", Record)) - 
            (pOHLCV->getValue("High", Record) - pOHLCV->getValue("Close", Record)));
		b = (pOHLCV->getValue("High", Record) - pOHLCV->getValue("Low", Record));
		if(a != 0 && b != 0) Value = a/b*pOHLCV->getValue("Volume", Record) + Field1->getValue(Record-1);


		//Bar chart Formula 2: http://www.barchart.com/trader/help/studies/adi.php
		/*if (Record > 1){
			if (pOHLCV->getValue("Close", Record) > pOHLCV->getValue("Close", Record - 1)) Value = Field1->getValue(Record - 1) + pOHLCV->getValue("Close", Record) - pOHLCV->getValue("Low", Record);
			else if....
		}*/

        Field1->setValue(Record, Value);

    } //Record

    pNav->MoveFirst();
    
    Results->addField(Field1);
    return Results;

  }

CRecordset* CIndex::CommodityChannelIndex(CNavigator* pNav, CRecordset* pOHLCV, 
	  int Periods, LPCTSTR Alias){

	CGeneral* GN = new CGeneral();
    CMovingAverage* MA = new CMovingAverage();
	CRecordset* Results = new CRecordset();
	CRecordset* TPrs;
	CRecordset* MArs;
	CField* Field1;
    
    double dMeanDeviation = 0;
    double dTmp = 0;
    long Count = 0;
	int RecordCount = 0;
    int Record = 0;
    
    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

#ifdef _CONSOLE_DEBUG
	printf("\n\tCommodityChannelIndex()\n\t");
#endif
    TPrs = GN->TypicalPrice(pNav, pOHLCV, "TP");
    MArs = MA->SimpleMovingAverage(pNav, TPrs->getField("TP"), Periods, "TPMA");
    for(Record = 1; Record != (1 * Periods) + 1; ++Record){
		Field1->setValue(Record, 0);
#ifdef _CONSOLE_DEBUG
		printf("TP[%d]=%f", Record, TPrs->getField("TP")->getValue(Record));
#endif
    }

	for(Record = (1 * Periods); Record != RecordCount + 1; ++Record){
        dMeanDeviation = 0;
		for (Count = (Record - Periods) + 1; Count != Record + 1; ++Count){
            dTmp = fabs(TPrs->getField("TP")->getValue(Count) - 
				MArs->getField("TPMA")->getValue(Record));
            dMeanDeviation = dMeanDeviation + dTmp;
        } //Count
		dMeanDeviation = dMeanDeviation / Periods;
        dTmp = (TPrs->getField("TP")->getValue(Record) - 
			MArs->getField("TPMA")->getValue(Record)) / (dMeanDeviation * 0.015);
		Field1->setValue(Record, dTmp);
		if (Record < 5){
#ifdef _CONSOLE_DEBUG
			printf("\n\tMA[%d]=%f", Record, MArs->getField("TPMA")->getValue(Record));
#endif
#ifdef _CONSOLE_DEBUG
			printf(" TP[%d]=%f", Record, TPrs->getField("TP")->getValue(Record));
#endif
#ifdef _CONSOLE_DEBUG
			printf(" Dev[%d]=%f", Record, dMeanDeviation);
#endif
		}
    } //Record

    pNav->MoveFirst();
    Results->addField(Field1);
    
	delete TPrs;
	delete MArs;
	delete GN;
	delete MA;

	return Results;

}


CRecordset* CIndex::StochasticMomentumIndex(CNavigator* pNav, CRecordset* pOHLCV,
            int KPeriods, int KSmooth, int KDoubleSmooth, 
			int DPeriods, int MAType, int PctD_MAType){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
	CRecordset* Temp = NULL;
	CGeneral* GN = new CGeneral();
    int Record = 0;
    int RecordCount = 0;
    int Period = 0;
    int Start = 0;
	double Value = 0;	
	CField* LLV = NULL;
    CField* HHV = NULL;
	CField* CHHLL = NULL;
	CField* HHLL = NULL;
    CField* Field1 = NULL;
	CField* Field2 = NULL;
	    
    KSmooth += 1;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, "%K");

	
	Temp = GN->HHV(pNav, pOHLCV->getField("High"), KPeriods, "HHV");
	HHV = new CField(RecordCount, "HHV");
	Temp->copyField(HHV, "HHV");
	delete Temp;

	Temp = GN->LLV(pNav, pOHLCV->getField("Low"), KPeriods, "LLV");
	LLV = new CField(RecordCount, "LLV");
	Temp->copyField(LLV, "LLV");
	delete Temp;


	HHLL = new CField(RecordCount, "HHLL");
    for(Record = 1; Record != RecordCount + 1; ++Record){
        Value = HHV->getValue(Record) - LLV->getValue(Record);
        HHLL->setValue(Record, Value);        
    }


    CHHLL = new CField(RecordCount, "CHHLL");
    for(Record = 1; Record != RecordCount + 1; ++Record){
        Value = pOHLCV->getValue("Close", Record) - 
            (0.5f * (HHV->getValue(Record) + LLV->getValue(Record)));
        CHHLL->setValue(Record, Value);
    }

	if(KSmooth > 1){
        switch(MAType){
        case indSimpleMovingAverage:
            Temp = MA->SimpleMovingAverage(pNav, CHHLL, KSmooth, "CHHLL");
            break;
        case indExponentialMovingAverage:			
            Temp = MA->ExponentialMovingAverage(pNav, CHHLL, KSmooth, "CHHLL");			
            break;
        case indTimeSeriesMovingAverage:
            Temp = MA->TimeSeriesMovingAverage(pNav, CHHLL, KSmooth, "CHHLL");
			break;
        case indTriangularMovingAverage:
            Temp = MA->TriangularMovingAverage(pNav, CHHLL, KSmooth, "CHHLL");
            break;
        case indVariableMovingAverage:
            Temp = MA->VariableMovingAverage(pNav, CHHLL, KSmooth, "CHHLL");
            break;
        case indWeightedMovingAverage:
            Temp = MA->WeightedMovingAverage(pNav, CHHLL, KSmooth, "CHHLL");
            break;
        case indVIDYA:
            Temp = MA->VIDYA(pNav, CHHLL, KSmooth, 0.65, "CHHLL");
            break;
        }
		
		Temp->copyField(CHHLL, "CHHLL");
		delete Temp;

    }


	if(KDoubleSmooth > 1){
        switch(MAType){
        case indSimpleMovingAverage:
            Temp = MA->SimpleMovingAverage(pNav, CHHLL, KDoubleSmooth, "CHHLL");
            break;
        case indExponentialMovingAverage:			
            Temp = MA->ExponentialMovingAverage(pNav, CHHLL, KDoubleSmooth, "CHHLL");		
            break;
        case indTimeSeriesMovingAverage:
            Temp = MA->TimeSeriesMovingAverage(pNav, CHHLL, KDoubleSmooth, "CHHLL");
			break;
        case indTriangularMovingAverage:
            Temp = MA->TriangularMovingAverage(pNav, CHHLL, KDoubleSmooth, "CHHLL");
            break;
        case indVariableMovingAverage:
            Temp = MA->VariableMovingAverage(pNav, CHHLL, KDoubleSmooth, "CHHLL");
            break;
        case indWeightedMovingAverage:
            Temp = MA->WeightedMovingAverage(pNav, CHHLL, KDoubleSmooth, "CHHLL");
            break;
        case indVIDYA:
            Temp = MA->VIDYA(pNav, CHHLL, KDoubleSmooth, 0.65, "CHHLL");
            break;
        }
				
		Temp->copyField(CHHLL, "CHHLL");
		delete Temp;

    }

	if(KSmooth > 1){
        switch(MAType){
        case indSimpleMovingAverage:
            Temp = MA->SimpleMovingAverage(pNav, HHLL, KSmooth, "HHLL");
            break;
        case indExponentialMovingAverage:
            Temp = MA->ExponentialMovingAverage(pNav, HHLL, KSmooth, "HHLL");
            break;
        case indTimeSeriesMovingAverage:
            Temp = MA->TimeSeriesMovingAverage(pNav, HHLL, KSmooth, "HHLL");
			break;
        case indTriangularMovingAverage:
            Temp = MA->TriangularMovingAverage(pNav, HHLL, KSmooth, "HHLL");
            break;
        case indVariableMovingAverage:
            Temp = MA->VariableMovingAverage(pNav, HHLL, KSmooth, "HHLL");
            break;
        case indWeightedMovingAverage:
            Temp = MA->WeightedMovingAverage(pNav, HHLL, KSmooth, "HHLL");
            break;
        case indVIDYA:
            Temp = MA->VIDYA(pNav, HHLL, KSmooth, 0.65, "HHLL");
            break;
        }
		
		Temp->copyField(HHLL, "HHLL");
		delete Temp;

    }
		

	if(KDoubleSmooth > 1){
        switch(MAType){
        case indSimpleMovingAverage:
            Temp = MA->SimpleMovingAverage(pNav, HHLL, KDoubleSmooth, "HHLL");
            break;
        case indExponentialMovingAverage:
            Temp = MA->ExponentialMovingAverage(pNav, HHLL, KDoubleSmooth, "HHLL");
            break;
        case indTimeSeriesMovingAverage:
            Temp = MA->TimeSeriesMovingAverage(pNav, HHLL, KDoubleSmooth, "HHLL");
			break;
        case indTriangularMovingAverage:
            Temp = MA->TriangularMovingAverage(pNav, HHLL, KDoubleSmooth, "HHLL");
            break;
        case indVariableMovingAverage:
            Temp = MA->VariableMovingAverage(pNav, HHLL, KDoubleSmooth, "HHLL");
            break;
        case indWeightedMovingAverage:
            Temp = MA->WeightedMovingAverage(pNav, HHLL, KDoubleSmooth, "HHLL");
            break;
        case indVIDYA:
            Temp = MA->VIDYA(pNav, HHLL, KDoubleSmooth, 0.65, "HHLL");
            break;
        }
		
		Temp->copyField(HHLL, "HHLL");
		delete Temp;
		
    }

	
    for(Record = KPeriods + 1; Record != RecordCount + 1; ++Record){
		double x = CHHLL->getValue(Record);
		double y = HHLL->getValue(Record);
		
		TRACE("%f\t%f\n", x,y);
		

		double a = CHHLL->getValue(Record);
		double b = (0.5f * HHLL->getValue(Record));
        if(a != b && b != 0) Value = 100.0f * (a / b);
        Field1->setValue(Record, Value);        
    }
	

	if(DPeriods > 1){
		switch(PctD_MAType){
        case indSimpleMovingAverage:
            Temp = MA->SimpleMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indExponentialMovingAverage:
            Temp = MA->ExponentialMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indTimeSeriesMovingAverage:
            Temp = MA->TimeSeriesMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indTriangularMovingAverage:
            Temp = MA->TriangularMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indVariableMovingAverage:
            Temp = MA->VariableMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indWeightedMovingAverage:
            Temp = MA->WeightedMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indVIDYA:
            Temp = MA->VIDYA(pNav, Field1, DPeriods, 0.65, "%D");
            break;
        }
		
		Field2 = new CField(RecordCount, "%D");
		Temp->copyField(Field2, "%D");
		Results->addField(Field2);
		delete Temp;
	}
    

	Results->addField(Field1);
 
	if(LLV) delete LLV;
	if(HHV) delete HHV;
	if(HHLL) delete HHLL;
	if(CHHLL) delete CHHLL;
	if(MA) delete MA;
	if(GN) delete GN;

    return Results;

  }



CRecordset* CIndex::HistoricalVolatility(CNavigator* pNav, CField* pSource,
              int Periods /*= 30*/, int BarHistory /*= 365*/, int MAType, 
			  double StandardDeviations /*=2*/, LPCTSTR Alias /*=Historical Volatility*/)
{


#ifdef _CONSOLE_DEBUG
	printf("\nHistoricalVolatility() Per=%d Stand=%f",Periods,StandardDeviations);
#endif

    CRecordset* Results = new CRecordset();
	CGeneral* Stdv = new CGeneral();
    CField* Field1;
	CRecordset* Field2;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    int Direction = 0;
    double Value = 0;	

    RecordCount = pNav->getRecordCount();
    
    Field1 = new CField(RecordCount, "PRICERETURNS");
	
	Start = 2;
	
	for(Record = Start; Record < RecordCount + 1; Record++)
	{
		Value = log10(pSource->getValue(Record) / pSource->getValue(Record - 1)); 
		Field1->setValue(Record, Value);
		if (Record<5){
#ifdef _CONSOLE_DEBUG
			//printf("\n\tPR[%d]=%f",Record,Value);
#endif
		}
	}
	
	Field2 = Stdv->StandardDeviation(pNav, Field1, Periods, StandardDeviations, MAType, "STDV");
	delete Stdv;

	for(Record = Periods+1; Record < RecordCount + 1; Record++)
	{
		if (Record<5){
#ifdef _CONSOLE_DEBUG
			//printf("\n\tSTDV[%d]=%f", Record, Field2->getValue("STDV", Record));
#endif
		}
		Value = Field2->getValue("STDV", Record)   * sqrt((double)BarHistory) * 100;
		Field1->setValue(Record, Value);
		if (Record<5){
#ifdef _CONSOLE_DEBUG
			//printf("\n\tHV[%d]=%f", Record, Value);
#endif
		}
	}
	
	delete Field2;
	Field1->setName(Alias);
    Results->addField(Field1);
    return Results;

 }




