/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "Oscillator.h"
// for .net use:
#include <iostream>
// for 6.0 use:
//#include <iostream.h>

#include <math.h>

//#define _CONSOLE_DEBUG

COscillator::COscillator()
{

}

COscillator::~COscillator()
{

}

  CRecordset* COscillator::ChandeMomentumOscillator(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    int Period = 0;
    double Today = 0;
    double Yesterday = 0;
    double UpSum = 0;
    double DownSum = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    pNav->MoveFirst();

    for(Record = Periods + 2; Record < RecordCount + 2; Record++){

        //Move back n periods
        pNav->setPosition (Record - Periods);
        UpSum = 0;
        DownSum = 0;

        for(Period = 1; Period < Periods + 1; Period++){

            pNav->MovePrevious();
            Yesterday = pSource->getValue(pNav->getPosition());
            pNav->MoveNext();
            Today = pSource->getValue(pNav->getPosition());

            if(Today > Yesterday){
                UpSum += (Today - Yesterday);
            }
            else if(Today < Yesterday){
                DownSum += (Yesterday - Today);
            }

            pNav->MoveNext();

        }//Period

        pNav->MovePrevious();
		if(UpSum + DownSum != 0){
			Value = 100 * (UpSum - DownSum) / (UpSum + DownSum);
		}
		else{
			Value = NULL_VALUE;
		}
        Field1->setValue(pNav->getPosition(), Value);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();

    Results->addField(Field1);

    return Results;

  }

  CRecordset* COscillator::Momentum(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = Periods + 1;


    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        pNav->setPosition(pNav->getPosition() - Periods);
        Value = pSource->getValue(pNav->getPosition());
        pNav->setPosition (pNav->getPosition() + Periods);

		//Original Formula shows range around 100:
        //Value = 100 + ((pSource->getValue(pNav->getPosition()) - Value) / Value) * 100;

		Value = pSource->getValue(pNav->getPosition()) - Value;

        Field1->setValue(pNav->getPosition(), Value);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::TRIX(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* RS;
    CField* EMA;
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Start = 0;
    double Value = 0;

#ifdef _CONSOLE_DEBUG
	printf("\n\nTRIX()\nSOURCE:");
	for(int i=0;i<10;i++){
		printf("\n%f",pSource->getValue(i));
	}
#endif

    RS = MA->ExponentialMovingAverage(pNav, pSource, Periods, "EMA1");
	EMA = RS->getField("EMA1");
	RS->removeField("EMA1");
	delete RS;
	
#ifdef _CONSOLE_DEBUG
	printf("\n\t EMA1 = %f %f %f",EMA->getValue(3),EMA->getValue(4),EMA->getValue(5));
#endif

    RS = MA->ExponentialMovingAverage(pNav, EMA, Periods, "EMA2");
	delete EMA;
	EMA = RS->getField("EMA2");
	RS->removeField("EMA2");	
	delete RS;	

#ifdef _CONSOLE_DEBUG
	printf("\n\t EMA2 = %f %f %f",EMA->getValue(5),EMA->getValue(6),EMA->getValue(7));
#endif
	
    RS = MA->ExponentialMovingAverage(pNav, EMA, Periods, "EMA3");
	delete EMA;
	EMA = RS->getField("EMA3");
	RS->removeField("EMA3");	
	delete RS;
	
#ifdef _CONSOLE_DEBUG
	printf("\n\t EMA3 = %f %f %f",EMA->getValue(7),EMA->getValue(8),EMA->getValue(9));
#endif

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

		pNav->MovePrevious();
        Value = EMA->getValue(pNav->getPosition());
        pNav->MoveNext();
        if(Value != 0){
            Value = ((EMA->getValue(pNav->getPosition()) - Value) / Value) * 100;
            Field1->setValue(pNav->getPosition(), Value);
        }
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);

	delete EMA;	
	delete MA;

    return Results;

  }

  CRecordset* COscillator::UltimateOscillator(CNavigator* pNav, CRecordset* pOHLCV,
            int Cycle1, int Cycle2, int Cycle3, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
    int Periods = 0;
    int Start = 0;
    double Value = 0;
    double TL = 0;
    double BP = 0;
    double TR = 0;
    double BPSum1 = 0;
    double BPSum2 = 0;
    double BPSum3 = 0;
    double TRSum1 = 0;
    double TRSum2 = 0;
    double TRSum3 = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Periods = Cycle1;
    if(Cycle2 > Periods){Periods = Cycle2;}
    if(Cycle3 > Periods){Periods = Cycle3;}

    Start = Periods + 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 2; Record++){

        BPSum1 = 0;
        BPSum2 = 0;
        BPSum3 = 0;
        TRSum1 = 0;
        TRSum2 = 0;
        TRSum3 = 0;


        pNav->setPosition(Record - Cycle1);
        for(Period = 1; Period < Cycle1 + 1; Period++){
          if(pOHLCV->getField("Low")->getValue(pNav->getPosition()) <
            pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
            TL = pOHLCV->getField("Low")->getValue(pNav->getPosition());
          }
          else{
                TL = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
          }
            BP = pOHLCV->getField("Close")->getValue(pNav->getPosition()) - TL;
            TR = pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                    pOHLCV->getField("Low")->getValue(pNav->getPosition());
            if(TR < pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                    pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
                TR = pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                        pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
            }
            if(TR < pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1) -
                    pOHLCV->getField("Low")->getValue(pNav->getPosition())){
                TR = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1) -
                        pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }
            BPSum1 += BP;
            TRSum1 += TR;
            pNav->MoveNext();
        }//Period

        pNav->setPosition(pNav->getPosition() - Cycle2);
        for(Period = 1; Period < Cycle2 + 1; Period++){
            if(pOHLCV->getField("Low")->getValue(pNav->getPosition()) <
                    pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
                TL = pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }
            else{
                TL = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
            }
            BP = pOHLCV->getField("Close")->getValue(pNav->getPosition()) - TL;
            TR = pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                    pOHLCV->getField("Low")->getValue(pNav->getPosition());
            if(TR < pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                    pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
                TR = pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                        pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
            }
            if(TR < pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1) -
                    pOHLCV->getField("Low")->getValue(pNav->getPosition())){
                TR = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1) -
                        pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }
            BPSum2 += BP;
            TRSum2 += TR;
            pNav->MoveNext();
        } //Period

        pNav->setPosition(pNav->getPosition() - Cycle3);
        for(Period = 1; Period < Cycle3 + 1; Period++){
            if(pOHLCV->getField("Low")->getValue(pNav->getPosition()) <
                    pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
                TL = pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }
            else{
                TL = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
            }
            BP = pOHLCV->getField("Close")->getValue(pNav->getPosition()) - TL;
            TR = pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                    pOHLCV->getField("Low")->getValue(pNav->getPosition());
            if(TR < pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                    pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
                TR = pOHLCV->getField("High")->getValue(pNav->getPosition()) -
                        pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
            }
            if(TR < pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1) -
                    pOHLCV->getField("Low")->getValue(pNav->getPosition())){
                TR = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1) -
                        pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }
            BPSum3 += BP;
            TRSum3 += TR;
            pNav->MoveNext();
        }//Period

        pNav->MovePrevious();
        Value = (4 * (BPSum1 / TRSum1) + 2 * (BPSum2 / TRSum2) +
        (BPSum3 / TRSum3)) / (4 + 2 + 1) * 100;
        Field1->setValue(pNav->getPosition(), Value);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::VerticalHorizontalFilter(CNavigator* pNav, CField* pSource,
         int Periods, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    int Period = 0;
    int Start = 0;
    double HCP = 0;
    double LCP = 0;
    double Sum = 0;
	double Abs = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = Periods + 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 2; Record++){

        HCP = 0;
        LCP = pSource->getValue(pNav->getPosition());

        pNav->setPosition(Record - Periods);

        for(Period = 1; Period < Periods + 1; Period++){
            if(pSource->getValue(pNav->getPosition()) < LCP){
                LCP = pSource->getValue(pNav->getPosition());
            }
            else if(pSource->getValue(pNav->getPosition()) > HCP){
                HCP = pSource->getValue(pNav->getPosition());
            }
            pNav->MoveNext();
        }//Period

        Sum = 0;
        pNav->setPosition(Record - Periods);
        for(Period = 1; Period < Periods + 1; Period++){
			Abs = (pSource->getValue(pNav->getPosition()) - 
				pSource->getValue(pNav->getPosition() - 1));
			if(Abs < 0){Abs = -1 * Abs;}
            Sum += Abs;
            pNav->MoveNext();
        }//Period

        pNav->MovePrevious();
		Abs = (HCP - LCP) / Sum;
		if(Abs < 0){Abs = -1 * Abs;}
        Field1->setValue(pNav->getPosition(), Abs);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::WilliamsPctR(CNavigator* pNav,
          CRecordset* pOHLCV, int Periods, LPCTSTR Alias){

    CField* Field1;
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    int Period = 0;
    int Start = 0;
    double HH = 0;
    double LL = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = Periods + 1;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 2; Record++){

        HH = pOHLCV->getField("High")->getValue(pNav->getPosition());

        LL = pOHLCV->getField("Low")->getValue(pNav->getPosition());

        pNav->setPosition (Record - Periods);

        for(Period = 1; Period < Periods + 1; Period++){
            if(pOHLCV->getField("High")->getValue(pNav->getPosition()) > HH){
               HH = pOHLCV->getField("High")->getValue(pNav->getPosition());
            }
            if(pOHLCV->getField("Low")->getValue(pNav->getPosition()) < LL){
               LL = pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }
            pNav->MoveNext();
        }//Period

        pNav->MovePrevious();
        Field1->setValue(pNav->getPosition(),
        ((HH - pOHLCV->getField("Close")->getValue(pNav->getPosition())) / (HH - LL)) * -100);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::WilliamsAccumulationDistribution(CNavigator* pNav, 
				CRecordset* pOHLCV, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double TRH = 0;
    double TRL = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = 1; Record < RecordCount; Record++){

        TRH = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
        if(pOHLCV->getField("High")->getValue(pNav->getPosition()) > TRH){
            TRH = pOHLCV->getField("High")->getValue(pNav->getPosition());
        }

        TRL = pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1);
        if(pOHLCV->getField("Low")->getValue(pNav->getPosition()) < TRL){
            TRL = pOHLCV->getField("Low")->getValue(pNav->getPosition());
        }

        if(pOHLCV->getField("Close")->getValue(pNav->getPosition()) >
                pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
            Value = pOHLCV->getField("Close")->getValue(pNav->getPosition()) - TRL;
        }
        else if(pOHLCV->getField("Close")->getValue(pNav->getPosition()) <
                    pOHLCV->getField("Close")->getValue(pNav->getPosition() - 1)){
            Value = pOHLCV->getField("Close")->getValue(pNav->getPosition()) - TRH;
        }
        else{
            Value = 0;
        }

        Field1->setValue(pNav->getPosition(), Value +
                Field1->getValue(pNav->getPosition() - 1));

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::VolumeOscillator(CNavigator* pNav, CField* Volume,
            int ShortTerm, int LongTerm,
            int PointsOrPercent, LPCTSTR Alias){

    CField* Field1;
    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* MA1;
    CRecordset* MA2;
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    MA1 = MA->SimpleMovingAverage(pNav, Volume, ShortTerm, "MA1");
    MA2 = MA->SimpleMovingAverage(pNav, Volume, LongTerm, "MA2");
    
	for(Record = 1; Record < RecordCount + 1; Record++){

        if(PointsOrPercent == 1){
            Value = MA1->getField("MA1")->getValue(pNav->getPosition()) -
                    MA2->getField("MA2")->getValue(pNav->getPosition());
        }
        else if(PointsOrPercent == 2){
            if(MA2->getField("MA2")->getValue(pNav->getPosition()) > 0){
                Value = ((MA1->getField("MA1")->getValue(pNav->getPosition()) -
                        MA2->getField("MA2")->getValue(pNav->getPosition())) /
                        MA2->getField("MA2")->getValue(pNav->getPosition())) * 100;
            }
        }

        Field1->setValue(Record, Value);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);

	delete MA;
	delete MA1;
	delete MA2;

    return Results;

  }

  CRecordset* COscillator::ChaikinVolatility(CNavigator* pNav, CRecordset* pOHLCV,
            int Periods, int ROC, int MAType, LPCTSTR Alias){

    CField* Field1;
    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* HLMA = NULL;
    CField* HL;
    int Record = 0;
    int RecordCount = 0;
    int Start = 0;
    double Value = 0;
    double MA1 = 0;
    double MA2 = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    HL = new CField(RecordCount, "HL");

    pNav->MoveFirst();
    for(Record = 1; Record < RecordCount + 1; Record++){
        HL->setValue(pNav->getPosition(), pOHLCV->getField("High")->getValue(pNav->getPosition()) -
        pOHLCV->getField("Low")->getValue(pNav->getPosition()));
        pNav->MoveNext();
    }

    switch(MAType){
    case indSimpleMovingAverage:
        HLMA = MA->SimpleMovingAverage(pNav, HL, Periods, "HLMA");
        break;
    case indExponentialMovingAverage:
        HLMA = MA->ExponentialMovingAverage(pNav, HL, Periods, "HLMA");
        break;
    case indTimeSeriesMovingAverage:
        HLMA = MA->TimeSeriesMovingAverage(pNav, HL, Periods, "HLMA");
        break;
    case indTriangularMovingAverage:
        HLMA = MA->TriangularMovingAverage(pNav, HL, Periods, "HLMA");
        break;
    case indVariableMovingAverage:
        HLMA = MA->VariableMovingAverage(pNav, HL, Periods, "HLMA");
        break;
    case indWeightedMovingAverage:
        HLMA = MA->WeightedMovingAverage(pNav, HL, Periods, "HLMA");
        break;
    case indVIDYA:
        HLMA = MA->VIDYA(pNav, HL, Periods, 0.65, "HLMA");
        break;
    }

    Start = ROC + 1;
    pNav->setPosition(Start);
    for(Record = Start; Record < RecordCount + 1; Record++){
        MA1 = HLMA->getField("HLMA")->getValue(pNav->getPosition() - ROC);
        MA2 = HLMA->getField("HLMA")->getValue(pNav->getPosition());
        if(MA1 != 0 && MA2 != 0){Value = ((MA1 - MA2) / MA1) * -100;}
        Field1->setValue(Record, Value);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
	
	delete HL;
	delete MA;
	if(HLMA) delete HLMA;

    Results->addField(Field1);
    return Results;

  }

 
  CRecordset* COscillator::StochasticOscillator(CNavigator* pNav, CRecordset* pOHLCV,
            int KPeriods, int KSlowingPeriods, int DPeriods, int MAType){

    CMovingAverage* MA = new CMovingAverage();
    CField* Field1 = NULL;
    CRecordset* PctD = NULL;	
    CRecordset* Results = new CRecordset();
    int Record = 0;
    int RecordCount = 0;
    int Period = 0;
    int Start = 0;
    double LL = 0;
    double HH = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, "%K");

    Start = KPeriods + 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 2; Record++){

        pNav->setPosition(Record - KPeriods);
        HH = pOHLCV->getField("High")->getValue(pNav->getPosition());
        LL = pOHLCV->getField("Low")->getValue(pNav->getPosition());

        for(Period = 1; Period < KPeriods + 1; Period++){

            if(pOHLCV->getField("High")->getValue(pNav->getPosition()) > HH){
                HH = pOHLCV->getField("High")->getValue(pNav->getPosition());
            }

            if(pOHLCV->getField("Low")->getValue(pNav->getPosition()) < LL){
                LL = pOHLCV->getField("Low")->getValue(pNav->getPosition());
            }

            pNav->MoveNext();

        } //Period

        pNav->MovePrevious();
        Value = (pOHLCV->getField("Close")->getValue(pNav->getPosition()) - LL) /
                (HH - LL) * 100;
        Field1->setValue(pNav->getPosition(), Value);
        pNav->MoveNext();

    }//Record

    if(KSlowingPeriods > 0){
		delete Results;
        switch(MAType){
        case indSimpleMovingAverage:
            Results = MA->SimpleMovingAverage(pNav, Field1, KSlowingPeriods, "%K");
            break;
        case indExponentialMovingAverage:
            Results = MA->ExponentialMovingAverage(pNav, Field1, KSlowingPeriods, "%K");
            break;
        case indTimeSeriesMovingAverage:
            Results = MA->TimeSeriesMovingAverage(pNav, Field1, KSlowingPeriods, "%K");
			break;
        case indTriangularMovingAverage:
            Results = MA->TriangularMovingAverage(pNav, Field1, KSlowingPeriods, "%K");
            break;
        case indVariableMovingAverage:
            Results = MA->VariableMovingAverage(pNav, Field1, KSlowingPeriods, "%K");
            break;
        case indWeightedMovingAverage:
            Results = MA->WeightedMovingAverage(pNav, Field1, KSlowingPeriods, "%K");
            break;
        case indVIDYA:
            Results = MA->VIDYA(pNav, Field1, KSlowingPeriods, 0.65, "%K");
            break;
        }
    }
    else{
        Results->addField(Field1);
    }

	delete Field1;
    Field1 = Results->getField("%K");

     switch(MAType){
        case indSimpleMovingAverage:
            PctD = MA->SimpleMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indExponentialMovingAverage:
            PctD = MA->ExponentialMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indTimeSeriesMovingAverage:
            PctD = MA->TimeSeriesMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indTriangularMovingAverage:
            PctD = MA->TriangularMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indVariableMovingAverage:
            PctD = MA->VariableMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indWeightedMovingAverage:
            PctD = MA->WeightedMovingAverage(pNav, Field1, DPeriods, "%D");
            break;
        case indVIDYA:
            PctD = MA->VIDYA(pNav, Field1, DPeriods, 0.65, "%D");
            break;
        }

    pNav->MoveFirst();

	if(PctD){
		Results->addField(PctD->getField("%D"));	
		PctD->removeField("%D");
		delete PctD;
	}

	delete MA;

    return Results;

  }


  CRecordset* COscillator::PriceOscillator(CNavigator* pNav, CField* pSource,
            int LongCycle, int ShortCycle, int MAType, LPCTSTR Alias){

	TASDK* TASDK1 = new TASDK();
    CRecordset* Results = new CRecordset();
    CField* Field1;
    CMovingAverage* MA = new CMovingAverage();
    CRecordset* LongMA = NULL;
    CRecordset* ShortMA = NULL;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Value = 0;

    if(LongCycle <= ShortCycle){
        //cout << ("ShortCycle must be less than LongCycle");
        return Results;
    }

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    switch(MAType){
    case indSimpleMovingAverage:
        LongMA = MA->SimpleMovingAverage(pNav, pSource, LongCycle, "MA");
        ShortMA = MA->SimpleMovingAverage(pNav, pSource, ShortCycle, "MA");
        break;
    case indExponentialMovingAverage:
        LongMA = MA->ExponentialMovingAverage(pNav, pSource, LongCycle, "MA");
        ShortMA = MA->ExponentialMovingAverage(pNav, pSource, ShortCycle, "MA");
        break;
    case indTimeSeriesMovingAverage:
        LongMA = MA->TimeSeriesMovingAverage(pNav, pSource, LongCycle, "MA");
        ShortMA = MA->TimeSeriesMovingAverage(pNav, pSource, ShortCycle, "MA");
        break;
    case indTriangularMovingAverage:
        LongMA = MA->TriangularMovingAverage(pNav, pSource, LongCycle, "MA");
        ShortMA = MA->TriangularMovingAverage(pNav, pSource, ShortCycle, "MA");
        break;
    case indVariableMovingAverage:
        LongMA = MA->VariableMovingAverage(pNav, pSource, LongCycle, "MA");
        ShortMA = MA->VariableMovingAverage(pNav, pSource, ShortCycle, "MA");
        break;
    case indWeightedMovingAverage:
        LongMA = MA->WeightedMovingAverage(pNav, pSource, LongCycle, "MA");
        ShortMA = MA->WeightedMovingAverage(pNav, pSource, ShortCycle, "MA");
        break;
    case indVIDYA:
        LongMA = MA->VIDYA(pNav, pSource, LongCycle, 0.65, "MA");
        ShortMA = MA->VIDYA(pNav, pSource, ShortCycle, 0.65, "MA");
        break;
    }

    Start = TASDK1->maxVal(LongCycle, ShortCycle) + 1;

    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        Value = ((ShortMA->getValue("MA", pNav->getPosition()) -
        LongMA->getValue("MA", pNav->getPosition())) /
        LongMA->getValue("MA", pNav->getPosition())) * 100;
        Field1->setValue(pNav->getPosition(), Value);
        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();

	delete TASDK1;
    delete MA;    
    if(LongMA) delete LongMA;
    if(ShortMA) delete ShortMA;    

    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::MACD(CNavigator* pNav, CRecordset* pOHLCV,
            int SignalPeriods, int LongCycle, int ShortCycle, LPCTSTR Alias){
	
    CRecordset* Results = new CRecordset();
    CField* Field1;
    CField* Field2;
    CMovingAverage* MA = new CMovingAverage();
    CRecordset* EMA1;
    CRecordset* EMA2;
    int RecordCount = 0;
    int Record = 0;    
    int Period = 0;
    double Value = 0;
	CString a = Alias;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);    

    EMA1 = MA->ExponentialMovingAverage(pNav, pOHLCV->getField("Close"), LongCycle, "EMA");
    EMA2 = MA->ExponentialMovingAverage(pNav, pOHLCV->getField("Close"), ShortCycle, "EMA");
    

    for(Record = 1; Record < RecordCount + 1; Record++){
        Value = EMA2->getValue("EMA", Record) - EMA1->getValue("EMA", Record);
        Field1->setValue(Record, Value);        
    }

	delete EMA1;
	EMA1 = MA->ExponentialMovingAverage(pNav, Field1, SignalPeriods, "EMA");

    Field2 = EMA1->getField("EMA");
    Field2->setName(a + "Signal");

    pNav->MoveFirst();

    Results->addField(Field1);
    Results->addField(Field2);
	
	EMA1->removeField(a + "Signal");
	delete MA;
	delete EMA1;
	delete EMA2;

    return Results;

  }

   CRecordset* COscillator::MACDHistogram(CNavigator* pNav, CRecordset* pOHLCV,
            int SignalPeriods, int LongCycle, int ShortCycle, LPCTSTR Alias){
	
    CRecordset* Results = new CRecordset();
    CField* Field1;
    CField* Field2;
    CMovingAverage* MA = new CMovingAverage();
    CRecordset* EMA1;
    CRecordset* EMA2;
    int RecordCount = 0;
    int Record = 0;    
    int Period = 0;
    double Value = 0;
	CString a = Alias;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);    

    EMA1 = MA->ExponentialMovingAverage(pNav, pOHLCV->getField("Close"), LongCycle, "EMA");
    EMA2 = MA->ExponentialMovingAverage(pNav, pOHLCV->getField("Close"), ShortCycle, "EMA");
    

    for(Record = 1; Record < RecordCount + 1; Record++){
        Value = EMA2->getValue("EMA", Record) - EMA1->getValue("EMA", Record);
        Field1->setValue(Record, Value);        
    }

	delete EMA1;
	EMA1 = MA->ExponentialMovingAverage(pNav, Field1, SignalPeriods, "EMA");

    Field2 = EMA1->getField("EMA");
    Field2->setName(a + "Signal");



	CField* histogram = new CField(RecordCount, Alias);
	for(Record = 1; Record < RecordCount + 1; Record++){		
        Value = Field1->getValue(Record) - Field2->getValue(Record);
        histogram->setValue(Record, Value);
    }


    pNav->MoveFirst();

    Results->addField(histogram);
   
	EMA1->removeField(a + "Signal");
	delete Field1;
    delete Field2;
	delete MA;
	delete EMA1;
	delete EMA2;

    return Results;

  }

  CRecordset* COscillator::EaseOfMovement(CNavigator* pNav, CRecordset* pOHLCV,
            int Periods, int MAType, LPCTSTR Alias){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* EMVMA = NULL;
    CField* Field1 = NULL;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double MPM = 0;
    double EMV = 0;
    double BoxRatio = 0;
	double bd = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        MPM = ((pOHLCV->getValue("High", pNav->getPosition()) +
        pOHLCV->getValue("Low", pNav->getPosition())) / 2) -
        ((pOHLCV->getValue("High", pNav->getPosition() - 1) +
        pOHLCV->getValue("Low", pNav->getPosition() - 1)) / 2);

        bd = (pOHLCV->getValue("High", pNav->getPosition()) -
        pOHLCV->getValue("Low", pNav->getPosition()));
			
		if(bd != 0){
			BoxRatio = pOHLCV->getValue("Volume", pNav->getPosition()) / bd;
        }

        EMV = MPM / BoxRatio;

        Field1->setValue(pNav->getPosition(), EMV * 10000);

        pNav->MoveNext();

    }//Record

    switch(MAType){
    case indSimpleMovingAverage:
        EMVMA = MA->SimpleMovingAverage(pNav, Field1, Periods, "MA");
        break;
    case indExponentialMovingAverage:
        EMVMA = MA->ExponentialMovingAverage(pNav, Field1, Periods, "MA");
        break;
    case indTimeSeriesMovingAverage:
        EMVMA = MA->TimeSeriesMovingAverage(pNav, Field1, Periods, "MA");
        break;
    case indTriangularMovingAverage:
        EMVMA = MA->TriangularMovingAverage(pNav, Field1, Periods, "MA");
        break;
    case indVariableMovingAverage:
        EMVMA = MA->VariableMovingAverage(pNav, Field1, Periods, "MA");
        break;
    case indWeightedMovingAverage:
        EMVMA = MA->WeightedMovingAverage(pNav, Field1, Periods, "MA");
        break;
    case indVIDYA:
        EMVMA = MA->VIDYA(pNav, Field1, Periods, 0.65, "MA");
        break;
    }

	delete Field1;
    
	if(EMVMA){
		Field1 = EMVMA->getField("MA");
		Field1->setName(Alias);
		pNav->MoveFirst();
		EMVMA->removeField(Alias);
		delete EMVMA;
	}

	delete MA;

    Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::DetrendedPriceOscillator(CNavigator* pNav, CField* pSource,
            int Periods, int MAType, LPCTSTR Alias){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* DPOMA = NULL;
    CField* Field1 = NULL;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    switch(MAType){
    case indSimpleMovingAverage:
        DPOMA = MA->SimpleMovingAverage(pNav, pSource, Periods, "MA");
        break;
    case indExponentialMovingAverage:
        DPOMA = MA->ExponentialMovingAverage(pNav, pSource, Periods, "MA");
        break;
    case indTimeSeriesMovingAverage:
        DPOMA = MA->TimeSeriesMovingAverage(pNav, pSource, Periods, "MA");
        break;
    case indTriangularMovingAverage:
        DPOMA = MA->TriangularMovingAverage(pNav, pSource, Periods, "MA");
    case indVariableMovingAverage:
        DPOMA = MA->VariableMovingAverage(pNav, pSource, Periods, "MA");
        break;
    case indWeightedMovingAverage:
        DPOMA = MA->WeightedMovingAverage(pNav, pSource, Periods, "MA");
        break;
    case indVIDYA:
        DPOMA = MA->VIDYA(pNav, pSource, Periods, 0.65, "MA");
        break;
    }

    Start = Periods + 1;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){
        Field1->setValue(pNav->getPosition(), pSource->getValue(pNav->getPosition()) -
        DPOMA->getValue("MA", pNav->getPosition() - ((Periods / 2) + 1)));
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
	
	if(DPOMA) delete DPOMA;
	delete MA;
    
	Results->addField(Field1);
    return Results;

  }

  CRecordset* COscillator::ParabolicSAR(CNavigator* pNav, CField* HighPrice,
            CField* LowPrice, double MinAF, double MaxAF, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
    CField* Field1;

    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
	int Start = 0;
	int Position = 0;
    double Max  = 0;
    double Min  = 0;
    double pSAR = 0;
    double pEP = 0;
    double pAF = 0;
    double SAR = 0;
    double AF = 0;
    double Hi = 0;
    double Lo = 0;
    double pHi = 0;
    double pLo = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

    Max = HighPrice->getValue(1);
    Min = LowPrice->getValue(1);
    
    if(HighPrice->getValue(2) - HighPrice->getValue(1) < 
		LowPrice->getValue(2) - LowPrice->getValue(1)){
        pSAR = Max;
        Position = -1;
    }
	else{
        pSAR = Min;
        Position = 1;
    }

    pAF = MinAF;
    SAR = pSAR;
    Hi = Max;
    Lo = Min;
    pHi = Hi;
    pLo = Lo;
    AF = MinAF;

    for(Record = Start; Record < RecordCount + 1; ++Record){

		if(Position == 1){

			if(HighPrice->getValue(Record) > Hi){
                Hi = HighPrice->getValue(Record);
				if(AF < MaxAF) AF = AF + MinAF;
			}
                SAR = pSAR + pAF * (pHi - pSAR);
                if(LowPrice->getValue(Record) < SAR){
                    Position = -1;
                    AF = MinAF;
                    SAR = pHi;
                    Hi = 0;
                    Lo = LowPrice->getValue(Record);
                }

		}
		else if(Position == -1){
			if(LowPrice->getValue(Record) < Lo){
                Lo = LowPrice->getValue(Record);
				if(AF < MaxAF) AF = AF + MinAF;
			}
				SAR = pSAR + pAF * (pLo - pSAR);
                if(HighPrice->getValue(Record) > SAR){
                    Position = 1;
                    AF = MinAF;
                    SAR = pLo;
                    Lo = 0;
                    Hi = HighPrice->getValue(Record);
				}
		}

        pHi = Hi;
        pLo = Lo;
        pSAR = SAR;
        pAF = AF;
            
        Field1->setValue(Record, SAR);

        pNav->MoveNext();
        

    } // Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

}

/*CRecordset* COscillator::DirectionalMovementSystem(CNavigator* pNav, 
												   CRecordset* pOHLCV, int Periods){

	CMovingAverage* MA = new CMovingAverage();

    CRecordset* Results = new CRecordset();
    CRecordset* ADX = new CRecordset();

    CField* DX;
    CField* ADXR;
    CField* ADXF;
    CField* UpDMI;
    CField* DnDMI;
    CField* DIN;
    CField* DIP;
    CField* TR;
    
	CRecordset* wSumTR;
    CRecordset* wSumUpDMI;
    CRecordset* wSumDnDMI;
	CRecordset* rsTemp;
    
	double HDIF  = 0;
    double LDIF  = 0;

    int RecordCount = 0;
    int Record = 0;
	int Period = 0;
	double Value = 0;
    
	RecordCount = pNav->getRecordCount();
        
	rsTemp = TrueRange(pNav, pOHLCV, "TR");
	TR = rsTemp->getField("TR");
	rsTemp->removeField("TR");

    wSumTR = MA->WellesWilderSmoothing(TR, Periods, "TRSum");

    UpDMI = new CField(RecordCount, "UpDMI");
    DnDMI = new CField(RecordCount, "DnDMI");
    DIN = new CField(RecordCount, "DI-");
    DIP = new CField(RecordCount, "DI+");
    
	for(Record = 2; Record != RecordCount + 1; ++ Record){
        
        HDIF = pOHLCV->getValue("High", Record) - pOHLCV->getValue("High", Record - 1);
        LDIF = pOHLCV->getValue("Low", Record - 1) - pOHLCV->getValue("Low", Record);
        
        if((HDIF < 0 && LDIF < 0) || (HDIF == LDIF)){
            UpDMI->setValue(Record, 0);
            DnDMI->setValue(Record, 0);
		}
        else if(HDIF > LDIF){
            UpDMI->setValue(Record, HDIF);
            DnDMI->setValue(Record, 0);
		}
        else if(HDIF < LDIF){
            UpDMI->setValue(Record, 0);
            DnDMI->setValue(Record, LDIF);
        }

    } //Record
   
    wSumUpDMI = MA->WellesWilderSmoothing(UpDMI, Periods, "DM+Sum");
    wSumDnDMI = MA->WellesWilderSmoothing(DnDMI, Periods, "DM-Sum");
    
    for(Record = 2; Record != RecordCount + 1; ++ Record){
    
        DIP->setValue(Record, floor(100 * wSumUpDMI->getValue("DM+Sum", Record) / 
            wSumTR->getValue("TRSum", Record)));
        DIN->setValue(Record, floor(100 * wSumDnDMI->getValue("DM-Sum", Record) / 
            wSumTR->getValue("TRSum", Record)));

    } //Record
   
    DX = new CField(RecordCount, "DX");

	for(Record = 2; Record != RecordCount + 1; ++ Record){
    
		//double a = (double)(fabs((DIP->getValue(Record) - DIN->getValue(Record))));
		double a = (double)(abs((DIP->getValue(Record) - DIN->getValue(Record))));

		double b = DIP->getValue(Record) + DIN->getValue(Record);
		if(a > 0 && b > 0){
			DX->setValue(Record, floor(100 * (a / b)));
		}

    } //Record
    
    ADXF = new CField(RecordCount, "ADX");
    ADX->addField(ADXF);

	for(Record = Periods + 1; Record != RecordCount + 1; ++Record){
        
        Value = floor(((ADX->getValue("ADX", Record - 1) * (Periods - 1)) + 
                DX->getValue(Record)) / Periods + 0.5);
        ADX->setValue("ADX", Record, Value);
    
    } //Record

    ADXR = new CField(RecordCount, "ADXR");
    
	for(Record = Periods + 1; Record != RecordCount + 1; ++Record){
		
        ADXR->setValue(Record, floor((ADX->getValue("ADX", Record) + 
                ADX->getValue("ADX", Record - 1)) / 2) + 0.5);
    
    } //Record
    
    pNav->MoveFirst();	
       
    Results->addField(ADX->getField("ADX"));
    Results->addField(ADXR);
    Results->addField(DX);
    Results->addField(wSumTR->getField("TRSum"));
    Results->addField(DIN);
    Results->addField(DIP);

	ADX->removeField("ADX");
	ADX->removeField("ADXF");
	wSumTR->removeField("TRSum");	

	delete rsTemp;
	delete wSumTR;
    delete wSumUpDMI;
    delete wSumDnDMI;
	delete ADX;
	delete UpDMI;
	delete DnDMI;
	delete TR;
	delete MA;

    return Results;
        
}
*/

	CRecordset* COscillator::DirectionalMovementSystem(CNavigator* pNav, 
												   CRecordset* pOHLCV, int Periods){

	CMovingAverage* MA = new CMovingAverage();

    CRecordset* Results = new CRecordset();
    CRecordset* ADX = new CRecordset();

    CField* DX;
    CField* ADXR;
    CField* ADXF;
    CField* UpDMI;
    CField* DnDMI;
    CField* DIN;
    CField* DIP;
    CField* TR;
    
	CRecordset* wSumTR;
    CRecordset* wSumUpDMI;
    CRecordset* wSumDnDMI;
	CRecordset* rsTemp;
    
	double HDIF  = 0;
    double LDIF  = 0;

    int RecordCount = 0;
    int Record = 0;
	int Period = 0;
	double Value = 0;
    
	RecordCount = pNav->getRecordCount();
        
	rsTemp = TrueRange(pNav, pOHLCV, "TR");
	TR = rsTemp->getField("TR");
	rsTemp->removeField("TR");

    wSumTR = MA->WellesWilderSmoothing(TR, Periods, "TRSum");

    UpDMI = new CField(RecordCount, "UpDMI");
    DnDMI = new CField(RecordCount, "DnDMI");
    DIN = new CField(RecordCount, "DI-");
    DIP = new CField(RecordCount, "DI+");
    
	for(Record = 2; Record != RecordCount + 1; ++ Record){
        
        HDIF = pOHLCV->getValue("High", Record) - pOHLCV->getValue("High", Record - 1);
        LDIF = pOHLCV->getValue("Low", Record - 1) - pOHLCV->getValue("Low", Record);
        
		if((HDIF < 0 && LDIF < 0) || (HDIF == LDIF)){
            UpDMI->setValue(Record, 0);
            DnDMI->setValue(Record, 0);
		}
        else if(HDIF > LDIF){
            UpDMI->setValue(Record, HDIF);
            DnDMI->setValue(Record, 0);
		}
        else if(HDIF < LDIF){
            UpDMI->setValue(Record, 0);
            DnDMI->setValue(Record, LDIF);
        }

    } //Record
   
    wSumUpDMI = MA->WellesWilderSmoothing(UpDMI, Periods, "DM+Sum",0,2);
    wSumDnDMI = MA->WellesWilderSmoothing(DnDMI, Periods, "DM-Sum",0,2);
    
	//Calculating +Di and -DI:
    for(Record = 1; Record != RecordCount + 1; ++ Record){
    
		DIP->setValue(Record, 100 * wSumUpDMI->getValue("DM+Sum", Record) / 
            wSumTR->getValue("TRSum", Record));
        DIN->setValue(Record, 100 * wSumDnDMI->getValue("DM-Sum", Record) / 
            wSumTR->getValue("TRSum", Record));

		//DIP->setValue(Record, 100 * wSumUpDMI->getValue("DM+Sum", Record) );
        //DIN->setValue(Record, 100 * wSumDnDMI->getValue("DM-Sum", Record) );

		//DIP->setValue(Record, UpDMI->getValue( Record) );
        //DIN->setValue(Record, DnDMI->getValue( Record));
		
		//DIP->setValue(Record, TR->getValue( Record) );
        //DIN->setValue(Record, wSumTR->getValue("TRSum", Record));

    } //Record
   
    DX = new CField(RecordCount, "DX");

	//Calculating DX:
	for(Record = 1; Record != RecordCount + 1; ++ Record){
    
		double a = (double)(fabs((DIP->getValue(Record) - DIN->getValue(Record))));
		double b = DIP->getValue(Record) + DIN->getValue(Record);
		if(a > 0 && b > 0){
			DX->setValue(Record, 100 * (a / b));
		}

    } //Record
    
    ADXF = new CField(RecordCount, "ADX");
    ADX->addField(ADXF);
	
	//Calculating ADX: (ignore null values from DX!)
	int nullOffset = 1;
	bool stop = false;
	for(int i = 1;!stop && i<DX->getRecordCount() ;i++){
		if(DX->getValue(i) == NULL_VALUE || DX->getValue(i) == 0) nullOffset++;
		else stop = true;
	}

	for(Record=nullOffset;Record<Periods+nullOffset;Record++)
	{
		Value += DX->getValue(Record);		
	}
	Record--;
    ADX->setValue("ADX", Record, Value/Periods);
	//ADX->setValue("ADX", Record, DX->getValue(Record));

	for(Record = Periods + nullOffset; Record != RecordCount + 1; ++Record){
        
       Value = ((ADX->getValue("ADX", Record - 1) * (Periods - 1)) + 
                DX->getValue(Record)) / Periods;
        ADX->setValue("ADX", Record, Value);

		//ADX->setValue("ADX", Record, DX->getValue(Record));
    
    } //Record

    ADXR = new CField(RecordCount, "ADXR");
    
	for(Record = Periods + 1; Record != RecordCount + 1; ++Record){
		
        ADXR->setValue(Record, (ADX->getValue("ADX", Record) + 
                ADX->getValue("ADX", Record - 1)) / 2 + 0.5);
    
    } //Record
    
    pNav->MoveFirst();	
       
    Results->addField(ADX->getField("ADX"));
    Results->addField(ADXR);
    Results->addField(DX);
    Results->addField(wSumTR->getField("TRSum"));
    Results->addField(DIN);
    Results->addField(DIP);

	ADX->removeField("ADX");
	ADX->removeField("ADXF");
	wSumTR->removeField("TRSum");	

	delete rsTemp;
	delete wSumTR;
    delete wSumUpDMI;
    delete wSumDnDMI;
	delete ADX;
	delete UpDMI;
	delete DnDMI;
	delete TR;
	delete MA;

    return Results;
        
}


  CRecordset* COscillator::TrueRange(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();	
    CField* Field1;

    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
	int Start = 0;
    double T1 = 0;
    double T2 = 0;
    double T3 = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Start = 2;
    pNav->setPosition(Start);

	for(Record = Start; Record != RecordCount + 1; ++Record){

        T1 = pOHLCV->getValue("High", Record) - pOHLCV->getValue("Low", Record);
		
		// GR 3/22/08 replaced abs((int)(pOHLCV... with fabs((pOHLCV...
		// ints would give way-invalid results for currency!
        T2 = fabs((pOHLCV->getValue("High", Record) - pOHLCV->getValue("Close", Record - 1)));
		//if (T2 < 0) T2 = -1 * T2;  // use fabs?
        T3 = fabs((pOHLCV->getValue("Close", Record - 1) - pOHLCV->getValue("Low", Record)));
		//if (T3 < 0) T3 = -1 * T3;

		// True Range is the greater of |High-Low|, |High-PrevClose|, and |Close-PrevLow|
		Value = 0;
        if(T1 > Value) Value = T1;
        if(T2 > Value) Value = T2;
        if(T3 > Value) Value = T3;
        
        Field1->setValue(Record, Value);
        
    } //Record

    Field1->setName(Alias);
    pNav->MoveFirst();
	Results->addField(Field1);
    
	return Results;

  }

  CRecordset* COscillator::Aroon(CNavigator* pNav, CRecordset* pOHLCV, int Periods){

    CRecordset* Results = new CRecordset();
	
	CField* AUp;
	CField* ADn;
	CField* AOs;

    int RecordCount = 0;
    int Record = 0;
    int Period = 0;
	
	double HighestHigh = 0;
    double LowestLow = 0;
	int HighPeriod = 0;
	int LowPeriod = 0;

    RecordCount = pNav->getRecordCount();

    AUp = new CField(RecordCount, "Aroon Up");
	ADn = new CField(RecordCount, "Aroon Down");
	AOs = new CField(RecordCount, "Aroon Oscillator");

    for(Record = Periods + 1; Record != RecordCount + 1; ++Record){
    
        HighestHigh = pOHLCV->getValue("High", Record);
        LowestLow = pOHLCV->getValue("Low", Record);
        HighPeriod = Record;
        LowPeriod = Record;
        
        for(Period = Record - Periods; Period != Record; ++Period){
            
            if(pOHLCV->getValue("High", Period) > HighestHigh){
                HighestHigh = pOHLCV->getValue("High", Period);
                HighPeriod = Period;
            }
            
            if(pOHLCV->getValue("Low", Period) < LowestLow){
                LowestLow = pOHLCV->getValue("Low", Period);
                LowPeriod = Period;
            }
            
        } // Period
        
		AUp->setValue(Record, (double)(((double)Periods - (double)(Record - HighPeriod)) / Periods) * 100);
        ADn->setValue(Record, (double)(((double)Periods - (double)(Record - LowPeriod)) / Periods) * 100);
        
        AOs->setValue(Record, (AUp->getValue(Record) - ADn->getValue(Record)));
        
    } // Record
    
    Results->addField(AUp);
    Results->addField(ADn);
	Results->addField(AOs);
    
    return Results;
  
 }

 CRecordset* COscillator::RainbowOscillator(CNavigator* pNav, 
	 CField* pSource, int Levels, int MAType, LPCTSTR Alias){

	CMovingAverage* MA = new CMovingAverage();

    CRecordset* Results = new CRecordset();
	CRecordset* rsMA = NULL;
	
	CField* Field1;

    int RecordCount = 0;
    int Record = 0;
	int Level = 0;
	double Value = 0;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);
	    
    for(Level = 2; Level != Levels + 1; ++Level){

        switch(MAType){
        case indSimpleMovingAverage:
            rsMA = MA->SimpleMovingAverage(pNav, 
				pSource, Levels, "MA");
			break;
        case indExponentialMovingAverage:
            rsMA = MA->ExponentialMovingAverage(pNav,
                    pSource, Levels, "MA");
			break;
        case indTimeSeriesMovingAverage:
            rsMA = MA->TimeSeriesMovingAverage(pNav,
                    pSource, Levels, "MA");
			break;
        case indTriangularMovingAverage:
            rsMA = MA->TriangularMovingAverage(pNav,
                    pSource, Levels, "MA");
			break;
        case indVariableMovingAverage:
            rsMA = MA->VariableMovingAverage(pNav,
                    pSource, Levels, "MA");
			break;
        case indWeightedMovingAverage:
            rsMA = MA->WeightedMovingAverage(pNav, 
                    pSource, Levels, "MA");
			break;
        case indVIDYA:
            rsMA = MA->VIDYA(pNav, pSource,
                    Level, 0.65, "MA");
			break;
		}
        
        for(Record = 1; Record != RecordCount + 1; ++Record){
            Value = rsMA->getValue("MA", Record);
            Field1->setValue(Record, (pSource->getValue(Record) - Value) + 
				Field1->getValue(Record));
        } // Record
        
        if(rsMA) delete rsMA;

    } // Level
    
    for(Record = 1; Record != RecordCount + 1; ++Record){
        Value = Field1->getValue(Record);
        Field1->setValue(Record, (Field1->getValue(Record) / (double)Levels));
    } // Record

	delete MA;

    Results->addField(Field1);
    return Results;

 }

   CRecordset* COscillator::FractalChaosOscillator(CNavigator* pNav, CRecordset* pOHLCV,
            int Periods, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
        
    int RecordCount = pNav->getRecordCount();
	int Record = 0;
    
    if(Periods < 1) Periods = 100;
    
    CField* fH = pOHLCV->getField("High");
    CField* fL = pOHLCV->getField("Low");
    CField* fFR = new CField(RecordCount, "FR");

    CField* fH1 = new CField(RecordCount, "High 1");
    CField* fH2 = new CField(RecordCount, "High 2");
    CField* fH3 = new CField(RecordCount, "High 3");
    CField* fH4 = new CField(RecordCount, "High 4");

    CField* fL1 = new CField(RecordCount, "Low 1");
    CField* fL2 = new CField(RecordCount, "Low 2");
    CField* fL3 = new CField(RecordCount, "Low 3");
    CField* fL4 = new CField(RecordCount, "Low 4");
    
	for(Record = 5; Record < RecordCount + 1; ++Record){    
    
        fH1->setValue(Record, fH->getValue(Record - 4));
        fL1->setValue(Record, fL->getValue(Record - 4));
        
        fH2->setValue(Record, fH->getValue(Record - 3));
        fL2->setValue(Record, fL->getValue(Record - 3));
        
        fH3->setValue(Record, fH->getValue(Record - 2));
        fL3->setValue(Record, fL->getValue(Record - 2));
        
        fH4->setValue(Record, fH->getValue(Record - 1));
        fL4->setValue(Record, fL->getValue(Record - 1));
        
    }

    for(Record = 2; Record < RecordCount + 1; ++Record){

        if((fH3->getValue(Record) > fH1->getValue(Record)) &&
            (fH3->getValue(Record) > fH2->getValue(Record)) &&
            (fH3->getValue(Record) >= fH4->getValue(Record)) &&
            (fH3->getValue(Record) >= fH->getValue(Record))){
            fFR->setValue(Record, 1);
        }
    
        if((fL3->getValue(Record) < fL1->getValue(Record)) &&
            (fL3->getValue(Record) < fL2->getValue(Record)) &&
            (fL3->getValue(Record) <= fL4->getValue(Record)) &&
            (fL3->getValue(Record) <= fL->getValue(Record))){
            fFR->setValue(Record, -1);
        }
   
    }    

	fFR->setName(Alias);
    Results->addField(fFR);

	delete fH1;
	delete fH2;
	delete fH3;
	delete fH4;
	delete fL1;
	delete fL2;
	delete fL3;
	delete fL4;
    
    return Results;
    
}



   CRecordset* COscillator::PrimeNumberOscillator(CNavigator* pNav, 
	   CField* pSource, LPCTSTR Alias){

    CRecordset* Results = new CRecordset();
        
    int RecordCount = pNav->getRecordCount();
	int Record = 0;    
    
    CField* fPrime = new CField(RecordCount, Alias);
  
	CGeneral* GN = new CGeneral();	

    long N = 0;
    long Value = 0;
    long Top = 0, Bottom = 0;
    
	for(Record = 1; Record != RecordCount + 1; ++Record){    
    
        Value = (long)(pSource->getValue(Record));
		if(Value < 10) Value = Value * 10;
        
		for(N = Value; N != 1; --N){        
            if(GN->IsPrime(N)){
                Bottom = N;
                break;
            }
        }
    
        for(N = Value; N != Value * 2; ++N){
            if(GN->IsPrime(N)){
                Top = N;
                break;
            }
        }
        
        if(fabs((double)(Value - Top)) < fabs((double)(Value - Bottom))){
            fPrime->setValue(Record, Value - Top);
        }
		else{
            fPrime->setValue(Record, Value - Bottom);
        }
        
    } // Record
    

	delete GN;

    Results->addField(fPrime);
    
    return Results;
      
}