/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "MovingAverage.h"
// for .net use:
#include <iostream>
// for 6.0 use:
//#include <iostream.h>

//#define _CONSOLE_DEBUG

CMovingAverage::CMovingAverage()
{
	
}

CMovingAverage::~CMovingAverage()
{

}

  CRecordset* CMovingAverage::SimpleMovingAverage(CNavigator* pNav,
                   CField* pSource, int Periods, LPCTSTR Alias, int Shift /*=0*/){

    double Avg = 0;
    int Period  = 0;
    int Record = 0;
    int RecordCount = 0;
    int Start = 0;
	int Pos = 0;

    CField* Field1;

    CRecordset* Results = new CRecordset();

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

	int nullOffset = 0;
	bool stop = false;
	for(int i = 1; !stop && i<pSource->getRecordCount() ;i++){
		if(pSource->getValue(i) == NULL_VALUE || pSource->getValue(i) == 0) nullOffset++;
		else stop = true;
	}

    Pos = Periods + nullOffset;

    //Loop through each record in recordset
    for(Record = Pos; Record < RecordCount + 1; Record++){

        Avg = 0;
        
        for(Period = Pos; Period > (Pos - Periods); Period--)
            Avg += pSource->getValue(Period);		

        //Calculate average
        Avg = Avg / Periods;
		if(((Pos+Shift)>=0) && ((Pos+Shift)<RecordCount + 1))Field1->setValue(Pos+Shift, Avg);
		Pos++;

    }

    Results->addField(Field1);
    return Results;

  }
  
/*
  CRecordset* CMovingAverage::GenericMovingAverage(CNavigator* pNav,
                   CField* pSource, int Periods, int Shift, int Type, double R2Scale, LPCTSTR Alias){

    double Avg = 0;
    int Period  = 0;
    int Record = 0;
    int RecordCount = 0;
    int Start = 0;
	int Pos = 0;

    CField* Field1;

    CRecordset* Results = new CRecordset();

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Pos = Periods;

    //Loop through each record in recordset
    for(Record = Periods; Record < RecordCount + 1; Record++){

        Avg = 0;
        
        for(Period = Pos; Period > (Pos - Periods); Period--)
            Avg += pSource->getValue(Period);		

        //Calculate average
        Avg = Avg / Periods;
		Field1->setValue(Pos, Avg);
		Pos++;

    }

    Results->addField(Field1);
    return Results;

  }
  */

  CRecordset* CMovingAverage::ExponentialMovingAverage(CNavigator* pNav,
                   CField* pSource, int Periods, LPCTSTR Alias, int Shift /*=0*/){

    double Avg = 0;
	double Prime = 0;
    int Period  = 0;
    int Record = 0;
    int RecordCount = 0;
    double Exp = 0;
    double PrevAvg = 0;
    double Value = 0;
    CField* Field1;
    CField* Field2;

    CRecordset* Results = new CRecordset();

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Exp = 2 / (double)(Periods + 1);

	int nullOffset = 1;
	bool stop = false;
	for(int i = 1; !stop && i<pSource->getRecordCount() ;i++){
		if(pSource->getValue(i) == NULL_VALUE || pSource->getValue(i) == 0) nullOffset++;
		else stop = true;
	}

	// To prime the EMA, get an average for the first n periods
	for(Record = nullOffset; Record < nullOffset + Periods; Record++){
		Prime += pSource->getValue(Record);
	}

	Prime = Prime / Periods;

	Value = Prime; // (pSource->getValue(Record) * (1 - Exp)) + (Prime * Exp);
	
	Field1->setValue(nullOffset + Periods - 1, Value);


    //Loop through each record in recordset
    for(Record = Periods + nullOffset; Record < RecordCount + 1; Record++){

        Avg = 0;
		
		//Value = (Field1->getValue(Record - 1) * (1 - Exp)) + 
			//(pSource->getValue(Record) * Exp);

		
		Value = Field1->getValue(Record - 1)  + 
			((pSource->getValue(Record)-Field1->getValue(Record - 1)) * Exp);
		
        //if(((Record+Shift)>=0) && ((Record+Shift)<RecordCount + 1)) Field1->setValue(Record+Shift, Value);
		Field1->setValue(Record, Value);
        

    }

    Field2 = new CField(RecordCount, Alias);
	//Shift++;

	for(int i=0;i<=RecordCount;i++){
		if(((i+Shift)>=0) && ((i+Shift)<RecordCount + 1))Field2->setValue(i+Shift,Field1->getValue(i));
	}

    Results->addField(Field2);
    return Results;

  }

  CRecordset* CMovingAverage::TimeSeriesMovingAverage(CNavigator* pNav,
                   CField* pSource, int Periods, LPCTSTR Alias, int Shift /*=0*/){

    CLinearRegression* LR = new CLinearRegression();    
    int Record = 0;
    int RecordCount = 0;
    double Value = 0;
    CField* Field1;
    CRecordset* Results;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Results = LR->Regression(pNav, pSource, Periods);
    
    pNav->MoveFirst();
    for(Record = pNav->getPosition(); Record < RecordCount + 1; Record++){
        Value = Results->getValue("Forecast", pNav->getPosition());
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1)) Field1->setValue(pNav->getPosition()+Shift, Value);
        pNav->MoveNext();
    }

    pNav->MoveFirst();
    Results->addField(Field1);

	delete LR;

    return Results;

  }

  CRecordset* CMovingAverage::VariableMovingAverage(CNavigator* pNav,
                   CField* pSource, int Periods, LPCTSTR Alias, int Shift /*=0*/){

	COscillator* OS = new COscillator();
    int Period  = 0;
    int Record = 0;
    int RecordCount = 0;
    int Start = 0;
    double CMO = 0;
    double VMA = 0;
    double PrevVMA = 0;
    double Price = 0;
    CField* Field1;

	CRecordset* Results;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    //CMO must be overwritten    
    Results = OS->ChandeMomentumOscillator(pNav, pSource, 9, "CMO");

    Start = 2;
    pNav->setPosition(Start);
    for(Record = Start; Record < RecordCount + 1; Record++){
        pNav->MovePrevious();
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1))PrevVMA =  Field1->getValue(pNav->getPosition()+Shift);
		else continue;
        pNav->MoveNext();
        CMO = Results->getValue("CMO", pNav->getPosition()) / 100;
        Price = pSource->getValue(pNav->getPosition());
		if(CMO < 0){CMO = -1 * CMO;}
        VMA = (CMO * Price) + (1 - CMO) * PrevVMA;
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1)) Field1->setValue(pNav->getPosition()+Shift, VMA);
        pNav->MoveNext();
    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);    

	delete OS;

    return Results;

  }

  CRecordset* CMovingAverage::TriangularMovingAverage(CNavigator* pNav, CField* pSource,
          int Periods, LPCTSTR Alias, int Shift /*=0*/){

    int Record = 0;
    int RecordCount = 0;
    int Start = 0;
    int Period = 0;
    double MA1 = 0;
    double MA2 = 0;
    double Avg = 0;
    CField* Field1;
    CField* Field2;
    CRecordset* Results = new CRecordset();

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, "MA1");
    Field2 = new CField(RecordCount, Alias);

    if ((Periods % 2) > 0 ){ //Odd number
        MA1 = (int) ((double) Periods / 2) + 1;
        MA2 = MA1;
    }
    else{ //Even number
        MA1 = (double)Periods / 2;
        MA2 = MA1 + 1;
    }

    Start = Periods + 1;
    pNav->setPosition(Start);

    //Loop through each record in recordset
    for(Record = Start; Record < RecordCount + 1; Record++){

        Avg = 0;

        //Loop backwards through each period
        for(Period = 1; Period < MA1 + 1; Period++){
            Avg += pSource->getValue(pNav->getPosition());
            pNav->MovePrevious();
        }//Period

        //Jump forward to last position
        pNav->setPosition(pNav->getPosition() + (int)MA1);

        //Calculate moving average
        Avg = Avg / MA1;
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1))Field1->setValue(pNav->getPosition()+Shift, Avg);

        pNav->MoveNext();

    }//Record

    pNav->setPosition(Start);

    //Loop through each record in recordset
    for(Record = Start; Record < RecordCount + 1; Record++){

        Avg = 0;

        //Loop backwards through each period
        for(Period = 1; Period < MA2 + 1; Period++){
            Avg += Field1->getValue(pNav->getPosition());
            pNav->MovePrevious();
        }//Period

        //Jump forward to last position
        pNav->setPosition(pNav->getPosition() + (int)MA2);

        //Calculate moving average
        Avg = Avg / MA2;
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1)) Field2->setValue(pNav->getPosition()+Shift, Avg);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field2);
	delete Field1;
    return Results;

  }

  CRecordset* CMovingAverage::WeightedMovingAverage(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias, int Shift /*=0*/){

    double Total = 0;
    double Weight = 0;
    int Period = 0;
    int Start = 0;
    int Record = 0;
    int RecordCount = 0;
    CField* Field1;
    CRecordset* Results = new CRecordset();

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    for(Period = 1; Period < Periods + 1; Period++){
      Weight += Period;
    }//Period

    Start = Periods + 1;
    pNav->setPosition(Start);

    //Loop through each record in recordset
    for(Record = Start; Record < RecordCount + 1; Record++){

        Total = 0;

        //Loop backwards through each period
        for(Period = Periods; Period > 0; Period--){
            Total += Period * pSource->getValue(pNav->getPosition());
            pNav->MovePrevious();
        }//Period

        //Jump forward to last position
        pNav->setPosition(pNav->getPosition() + Periods);

        //Calculate moving average
        Total = Total / Weight;
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1)) Field1->setValue(pNav->getPosition()+Shift, Total);

        pNav->MoveNext();

    }//Record

    pNav->MoveFirst();
    Results->addField(Field1);
    return Results;

  }

  CRecordset* CMovingAverage::VIDYA(CNavigator* pNav, CField* pSource,
            int Periods, double R2Scale, LPCTSTR Alias, int Shift /*=0*/){

    bool CleanUp = false;
    int Record = 0;
    int RecordCount = 0;
    int Start = 0;
    double R2Scaled = 0;
    double PreviousValue = 0;
    CField* Field1;
    CLinearRegression* LR = new CLinearRegression();
    CRecordset* Results;

    RecordCount = pNav->getRecordCount();

    Field1 = new CField(RecordCount, Alias);

    Results = LR->Regression(pNav, pSource, Periods);
     
    Start = 2;
    pNav->setPosition(Start);
    for(Record = Start; Record < RecordCount + 1; Record++){
        pNav->MovePrevious();
        PreviousValue = pSource->getValue(pNav->getPosition());
        pNav->MoveNext();
        R2Scaled = Results->getValue("RSquared", pNav->getPosition()) * R2Scale;
        if(((pNav->getPosition()+Shift)>=0) && ((pNav->getPosition()+Shift)<RecordCount + 1)) Field1->setValue(pNav->getPosition()+Shift, R2Scaled *
        pSource->getValue(pNav->getPosition()) + (1 - R2Scaled) * PreviousValue);
        pNav->MoveNext();
    }//Record

	delete LR;    
    Results->addField(Field1);
    return Results;

  }

CRecordset* CMovingAverage::WellesWilderSmoothing(CField* pSource, 
												 int Periods, LPCTSTR Alias, int Shift /*=0*/,int ignoreOffset /*=0*/){
	
	CField* Field1;
    CRecordset* Results = new CRecordset();

	int Record = 0;
    int RecordCount = 0;
    double Value = 0;    

    RecordCount = pSource->getRecordCount();

    Field1 = new CField(RecordCount, Alias);


	int nullOffset = 1;
	bool stop = false;
	for(int i = 1;ignoreOffset==0 && !stop && i<pSource->getRecordCount() ;i++){
		if(pSource->getValue(i) == NULL_VALUE || pSource->getValue(i) == 0) nullOffset++;
		else stop = true;
	}
	if(ignoreOffset!=0) nullOffset = ignoreOffset;
	Value = 0;
	for(Record = nullOffset; Record<Periods+nullOffset;Record++)
	{
		Value += pSource->getValue(Record);
	}
	Record--;
#ifdef _CONSOLE_DEBUG
	printf("\nWWS(%s)\nSOURCE:",Alias);
	for(int i= 0;i<10;i++) printf("\ns[%d]=%f",i,pSource->getValue(i));
	printf("\nnullOffset = %d",nullOffset);
	printf("\nRecord = %d sumValue = %f Value = %f",Record,Value,Value/Periods);
#endif
	if(((Record+Shift)>=0) && ((Record+Shift)<RecordCount + 1)) Field1->setValue(Record+Shift, Value/Periods);

	for(Record = Periods+nullOffset; Record < RecordCount + 1; ++Record){

		Value = Field1->getValue(Record - 1) + 1 / (double)Periods *
           (pSource->getValue(Record) - Field1->getValue(Record - 1));

        if(((Record+Shift)>=0) && ((Record+Shift)<RecordCount + 1)) Field1->setValue(Record+Shift, Value);
  
    } //Record

    Results->addField(Field1);
    
	return Results;
    
}

