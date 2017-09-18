/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "Bands.h"
// for .net use:
#include <iostream>
// for 6.0 use:
//#include <iostream.h>
#include <math.h>

CBands::CBands()
{

}

CBands::~CBands()
{

}

	CRecordset* CBands::BollingerBands(CNavigator* pNav, CField* pSource,
            int Periods, double StandardDeviations, int MAType){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = NULL;	
    CField* Field1 = NULL;
    CField* Field2 = NULL;

    int Period = 0;
    int RecordCount = 0;
    int Record = 0;
    int Start = 0;
    double Sum = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    if(MAType < MA_START || MAType > MA_END){
        //cout << "Bands->BollingerBands: Invalid Moving Average Type";
        return NULL;
    }

    if(Periods < 1 || Periods > RecordCount){
        //cout << "Bands->BollingerBands: Invalid Periods";
        return NULL;
    }

    if(StandardDeviations < 0 || StandardDeviations > 100){
        //cout << "Bands->BollingerBands: Invalid Standard Deviations";
        return NULL;
    }

    Field1 = new CField(RecordCount, "Bollinger Band Bottom");
    Field2 = new CField(RecordCount, "Bollinger Band Top");

    switch(MAType){

    case indSimpleMovingAverage:
        Results = MA->SimpleMovingAverage(pNav, pSource, Periods, "Bollinger Band Median");
        break;
    case indExponentialMovingAverage:
        Results = MA->ExponentialMovingAverage(pNav, pSource, Periods, "Bollinger Band Median");
        break;
    case indTimeSeriesMovingAverage:
        Results = MA->TimeSeriesMovingAverage(pNav, pSource, Periods, "Bollinger Band Median");
        break;
    case indTriangularMovingAverage:
        Results = MA->TriangularMovingAverage(pNav, pSource, Periods, "Bollinger Band Median");
        break;
    case indVariableMovingAverage:
        Results = MA->VariableMovingAverage(pNav, pSource, Periods, "Bollinger Band Median");
        break;
    case indWeightedMovingAverage:
        Results = MA->WeightedMovingAverage(pNav, pSource, Periods, "Bollinger Band Median");
        break;
    case indVIDYA:
        Results = MA->VIDYA(pNav, pSource, Periods, 0.65, "Bollinger Band Median");
        break;
    }

    Start = Periods + 1;
    pNav->setPosition(Start);

    for(Record = Start; Record < RecordCount + 1; Record++){

        Sum = 0;

        Value = Results->getValue("Bollinger Band Median", pNav->getPosition());

        for(Period = 1; Period < Periods + 1; Period++){
            Sum += (pSource->getValue(pNav->getPosition()) - Value) *
            (pSource->getValue(pNav->getPosition()) - Value);
            pNav->MovePrevious();
        }//Period

        pNav->setPosition(pNav->getPosition() + Periods);

        Value = StandardDeviations * sqrt(Sum / Periods);
        Field1->setValue(pNav->getPosition(),
        Results->getValue("Bollinger Band Median", pNav->getPosition()) - Value);
        Field2->setValue(pNav->getPosition(),
        Results->getValue("Bollinger Band Median", pNav->getPosition()) + Value);

        pNav->MoveNext();

    }//Record

    //Append fields to recordset
	if(Results){
		Results->addField(Field1);
		Results->addField(Field2);
	}

	delete MA;
    return Results;

  }

  CRecordset* CBands::MovingAverageEnvelope(CNavigator* pNav, CField* pSource,
            int Periods, int MAType, double Shift){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = NULL;
    CField* Field1 = NULL;
    CField* Field2 = NULL;
    int Period = 0;
    int RecordCount = 0;
    int Record = 0;
    double Value = 0;

    RecordCount = pNav->getRecordCount();

    if(MAType < MA_START || MAType > MA_END){
//      cout << "Bands->MovingAverageEnvelope: Invalid Moving Average Type";
      return NULL;
    }

    if(Periods < 1 || Periods > RecordCount){
//      cout << "Bands->MovingAverageEnvelope: Invalid Periods";
      return NULL;
    }

    if(Shift < 0 || Shift > 100){
      //cout << "Bands->MovingAverageEnvelope: Invalid Shift";
      return NULL;
    }

    Field1 = new CField(RecordCount, "Envelope Top");
    Field2 = new CField(RecordCount, "Envelope Bottom");

    switch(MAType){

    case indSimpleMovingAverage:
        Results = MA->SimpleMovingAverage(pNav, pSource, Periods, "Temp");
        break;
    case indExponentialMovingAverage:
        Results = MA->ExponentialMovingAverage(pNav, pSource, Periods, "Temp");
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
    }

    pNav->MoveFirst();

	Shift = Shift / 100;

    for(Record = 1; Record < RecordCount + 1; Record++){

        Value = Results->getValue("Temp", pNav->getPosition());
        Field1->setValue(pNav->getPosition(), Value + (Value * Shift));

        Value = Results->getValue("Temp", pNav->getPosition());
        Field2->setValue(pNav->getPosition(), Value - (Value * Shift));

        pNav->MoveNext();

    }//Record

    //Append fields to recordset
	if(Results){
		Results->addField(Field1);
		Results->addField(Field2);
	}

    delete MA;

    return Results;

  }

  CRecordset* CBands::HighLowBands(CNavigator* pNav, CField* HighPrice,
            CField* LowPrice, CField* ClosePrice, int Periods){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* RS1;
    CRecordset* RS2;
    CRecordset* RS3;

    if(Periods < 6 || Periods > pNav->getRecordCount()){
        //cout << "Bands->HighLowBands: Invalid Periods";
        return NULL;
    }

    RS1 = MA->VIDYA(pNav, HighPrice, Periods, 0.8, "High Low Bands Top");
    RS2 = MA->VIDYA(pNav, ClosePrice, (int)(Periods / 2), 0.8, "High Low Bands Median");
    RS3 = MA->VIDYA(pNav, LowPrice, Periods, 0.8, "High Low Bands Bottom");

    Results->addField(RS1->getField("High Low Bands Top"));
    Results->addField(RS2->getField("High Low Bands Median"));
    Results->addField(RS3->getField("High Low Bands Bottom"));

	// Remove fields so recordset can be deleted
	RS1->removeField("High Low Bands Top");
	RS2->removeField("High Low Bands Median");
	RS3->removeField("High Low Bands Bottom");

	delete RS1;
	delete RS2;
	delete RS3;

	delete MA;
    pNav->MoveFirst();
    return Results;

  }

    CRecordset* CBands::HILOBands(CNavigator* pNav, CField* HighPrice,
            CField* LowPrice, CField* ClosePrice, int PeriodsHigh, int PeriodsLow, int Shift, double Scale){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
    CRecordset* RS1;
    CRecordset* RS2;
    CRecordset* RS3;

    if(PeriodsHigh > pNav->getRecordCount() || PeriodsLow > pNav->getRecordCount()){
        //cout << "Bands->HILOBands: Invalid Periods";
        return NULL;
	}
	if (Scale <0 || Scale>100){
		//cout << "Bands->HILOBands: Invalid Scale";
		return NULL;
	}

    RS1 = MA->SimpleMovingAverage(pNav, HighPrice, PeriodsHigh, "HILO Bands Top",Shift);
    RS2 = MA->SimpleMovingAverage(pNav, ClosePrice, (int)((PeriodsHigh+PeriodsLow) / 2), "HILO Bands Median",Shift);
    RS3 = MA->SimpleMovingAverage(pNav, LowPrice, PeriodsLow, "HILO Bands Bottom",Shift);


	pNav->MoveFirst();

	/*Scale = Scale / 100;
	double Value = 0;
	for (int Record = 1; Record < pNav->getRecordCount() + 1; Record++){

		Value = RS1->getValue("HILO Bands Top", pNav->getPosition());
		RS1->setValue("HILO Bands Top", pNav->getPosition(), Value + (Value * Scale));

		Value = RS2->getValue("HILO Bands Median", pNav->getPosition());
		RS2->setValue("HILO Bands Median", pNav->getPosition(), Value + (Value * Scale));

		Value = RS3->getValue("HILO Bands Bottom", pNav->getPosition());
		RS3->setValue("HILO Bands Bottom", pNav->getPosition(), Value + (Value * Scale));

		pNav->MoveNext();

	}*/

    Results->addField(RS1->getField("HILO Bands Top"));
    Results->addField(RS2->getField("HILO Bands Median"));
    Results->addField(RS3->getField("HILO Bands Bottom"));

	// Remove fields so recordset can be deleted
	RS1->removeField("HILO Bands Top");
	RS2->removeField("HILO Bands Median");
	RS3->removeField("HILO Bands Bottom");

	delete RS1;
	delete RS2;
	delete RS3;

	delete MA;
    pNav->MoveFirst();
    return Results;

  }

  CRecordset* CBands::FractalChaosBands(CNavigator* pNav, CRecordset* pOHLCV,
            int Periods){

    CMovingAverage* MA = new CMovingAverage();
    CRecordset* Results = new CRecordset();
        
    int RecordCount = pNav->getRecordCount();
	int Record = 0;
    
    if(Periods < 1) Periods = 100;
    
    CRecordset* rsFractals = NULL;

    CField* fHiFractal = new CField(RecordCount, "Fractal High");
    CField* fLoFractal = new CField(RecordCount, "Low High");
    CField* fHLM = new CField(RecordCount, "HLM");
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
    
    for(Record = 1; Record < RecordCount + 1; ++Record){
        fHiFractal->setValue(Record, (fH->getValue(Record) + fL->getValue(Record)) / 3);		
    }
    
    rsFractals = MA->SimpleMovingAverage(pNav, fHiFractal, Periods, "Fractal High");   
	delete fHiFractal;
	fHiFractal = rsFractals->getField("Fractal High");
	rsFractals->removeField("Fractal High");
	delete rsFractals;

    rsFractals = MA->SimpleMovingAverage(pNav, fLoFractal, Periods, "Fractal Low");
    delete fLoFractal;
	fLoFractal = rsFractals->getField("Fractal Low");
	rsFractals->removeField("Fractal Low");
	delete rsFractals;

    for(Record = 1; Record < RecordCount + 1; ++Record){
        fHiFractal->setValue(Record, fH3->getValue(Record) + fHiFractal->getValue(Record));
        fLoFractal->setValue(Record, fL3->getValue(Record) - fLoFractal->getValue(Record));		
    }
    
    for(Record = 2; Record < RecordCount + 1; ++Record){

        if((fH3->getValue(Record) > fH1->getValue(Record)) &&
            (fH3->getValue(Record) > fH2->getValue(Record)) &&
            (fH3->getValue(Record) >= fH4->getValue(Record)) &&
            (fH3->getValue(Record) >= fH->getValue(Record))){
            fFR->setValue(Record, fHiFractal->getValue(Record));
        }
		else{
			fFR->setValue(Record, 0);
        }
                
        if(fFR->getValue(Record) == 0){
            if((fL3->getValue(Record) < fL1->getValue(Record)) &&
                (fL3->getValue(Record) < fL2->getValue(Record)) &&
                (fL3->getValue(Record) <= fL4->getValue(Record)) &&
                (fL3->getValue(Record) <= fL->getValue(Record))){
                fFR->setValue(Record, fLoFractal->getValue(Record));
            }
			else{
				fFR->setValue(Record, 0);
            }
        }
        
        if(fHiFractal->getValue(Record) == fFR->getValue(Record)){
            fHiFractal->setValue(Record, fH3->getValue(Record));
        }
		else{
            fHiFractal->setValue(Record, fHiFractal->getValue(Record - 1));
        }
        
        if(fLoFractal->getValue(Record) == fFR->getValue(Record)){
            fLoFractal->setValue(Record, fL3->getValue(Record));
        }
		else{
            fLoFractal->setValue(Record, fLoFractal->getValue(Record - 1));
        }
        
    }    

	// Added 12/19/2005 TW
    for(Record = 2; Record < RecordCount + 1; ++Record)
	{
		if(fLoFractal->getValue(Record) == 0)		
			fLoFractal->setValue(Record, NULL_VALUE);
		if(fHiFractal->getValue(Record) == 0)		
			fHiFractal->setValue(Record, NULL_VALUE);
	}

    Results->addField(fHiFractal);
    Results->addField(fLoFractal);

	delete MA;
	delete fHLM;
	delete fFR;
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


   CRecordset* CBands::PrimeNumberBands(CNavigator* pNav, 
	   CField* HighPrice, CField* LowPrice){

    CRecordset* Results = new CRecordset();
        
    int RecordCount = pNav->getRecordCount();
	int Record = 0;    
    
    CField* fTop = new CField(RecordCount, "Prime Bands Top");
	CField* fBottom = new CField(RecordCount, "Prime Bands Bottom");
  
	CGeneral* GN = new CGeneral();	

    long N = 0;
    long Value = 0;
    long Top = 0, Bottom = 0;
    
	for(Record = 1; Record != RecordCount + 1; ++Record){    
    
        Value = (long)(LowPrice->getValue(Record));
		if(Value < 10) Value = Value * 10;
        
		for(N = Value; N != 1; --N){        
            if(GN->IsPrime(N)){
                Bottom = N;
                break;
            }
        }
		fBottom->setValue(Record, (double)Bottom);

		
		Value = (long)(HighPrice->getValue(Record));
		if(Value < 10) Value = Value * 10;

        for(N = Value; N != Value * 2; ++N){
            if(GN->IsPrime(N)){
                Top = N;
                break;
            }
        }
		fTop->setValue(Record, (double)Top);
        
        
    } // Record
    

	delete GN;

    Results->addField(fTop);
	Results->addField(fBottom);
    
    return Results;
      
}