/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "LinearRegression.h"

CLinearRegression::CLinearRegression()
{

}

CLinearRegression::~CLinearRegression()
{

}

  CRecordset* CLinearRegression::Regression(CNavigator* pNav, CField* pSource, int Periods){

    int X = 0; //Period
	std::vector<double> Y; //Value
    int N = 0;
    double q1 = 0;
    double q2 = 0;
    double q3 = 0;
    double XY = 0;
    double XSquared = 0;
    double YSquared = 0;
    double XSum = 0;
    double YSum = 0;
    double XSquaredSum = 0;
    double YSquaredSum = 0;
    double XYSum = 0;
    double Slope = 0;
    double Intercept = 0;
    double Forecast = 0;
    double RSquared = 0;

    CRecordset* Results = new CRecordset();
    double Value = 0;
    int Record = 0;
    int RecordCount = 0;
    int Period = 0;
    int Position = 0;

    RecordCount = pNav->getRecordCount();

    CField* Field1 = new CField(RecordCount, "Slope");
    CField* Field2 = new CField(RecordCount, "Intercept");
    CField* Field3 = new CField(RecordCount, "Forecast");
    CField* Field4 = new CField(RecordCount, "RSquared");

    pNav->MoveFirst();

    for (Record = Periods; Record < RecordCount + 1; Record++){

        X = Periods;
        Y.resize(X + 1);

        //Move back n periods
        Position = Record;
        pNav->setPosition (Record - Periods + 1);

        for (Period = 1; Period < Periods + 1; Period++){
            Value = pSource->getValue(pNav->getPosition());
            Y[Period] = Value;
            pNav->MoveNext();
        } //Period

        //Return to original position and reset
        pNav->setPosition(Position);
        XSum = 0;
        YSum = 0;
        XSquaredSum = 0;
        YSquaredSum = 0;
        XYSum = 0;

        //Square
        for (N = 1; N  < X + 1; N++){
            XSum += N;
            YSum += Y[N];
            XSquaredSum += (N * N);
            YSquaredSum += (Y[N] * Y[N]);
            XYSum += (Y[N] * N);
        }//N

        N = X; //Number of periods in calculation
        q1 = (XYSum - ((XSum * YSum) / N));
        q2 = (XSquaredSum - ((XSum * XSum) / N));
        q3 = (YSquaredSum - ((YSum * YSum) / N));

        Slope = (q1 / q2); //Slope
        Intercept = (((1 / (double)N) * YSum) - (((int)((double)N / 2)) * Slope)); //Intercept
        Forecast = ((N * Slope) + Intercept); //Forecast
        
		
		if((q1 * q1) != 0 && (q2 * q3) != 0){
			RSquared = (q1 * q1) / (q2 * q3); //Coefficient of determination (R-Squared)
		}

        if (Record > Periods){
            Field1->setValue(Record, Slope);
            Field2->setValue(Record, Intercept);
            Field3->setValue(Record, Forecast);
            Field4->setValue(Record, RSquared);
        }

        pNav->MoveNext();

    }//Record

    //Append fields to CNavigator
    Results->addField(Field1);
    Results->addField(Field2);
    Results->addField(Field3);
    Results->addField(Field4);

    pNav->MoveFirst();
    return Results;
  }

  CRecordset* CLinearRegression::TimeSeriesForecast(CNavigator* pNav,
                   CField* pSource, int Periods, LPCTSTR Alias){

    CRecordset* Results;

    Results = Regression(pNav, pSource, Periods);
    Results->renameField("Forecast", Alias);
    Results->removeField("Slope");
    Results->removeField("Intercept");
    Results->removeField("RSquared");

    pNav->MoveFirst();
    return Results;

  }
