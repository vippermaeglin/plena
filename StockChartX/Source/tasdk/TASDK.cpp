/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */
  
#include "stdafx.h"
#include <iostream>
#include <string>
#include "TASDK.h"

/*
int main(int argc, LPCTSTR argv[])
{


    CNavigator* pNav = new CNavigator();

    //Open the CNavigator
    pNav->OpenDatabase("AA.txt", "\t");

    CRecordset* RS = new CRecordset();
    CRecordset* pOHLCV = pNav->getRecordset();

    CField* O = pOHLCV->getField("Open");
    CField* C = pOHLCV->getField("Close");
    CField* H = pOHLCV->getField("High");
    CField* L = pOHLCV->getField("Low");
    CField* D = pOHLCV->getField("Date");
    CField* V = pOHLCV->getField("Volume");
	
	
	COscillator os;
	CRecordset* pOut;
	pOut =  os.RainbowOscillator(pNav, pOHLCV, 5, indSimpleMovingAverage);
	cout << pOut->getField("Rainbow Oscillator")->getValue(1000) << endl;

	cout << pOHLCV->getValue("Close", 123);

	return 0;

}
*/

TASDK::TASDK(){

}

TASDK::~TASDK(){

}


    //Returns max of two values
	double TASDK::maxVal(double Value1, double Value2){
    if(Value1 > Value2){
        return Value1;
    }
    else if(Value2 > Value1){
        return Value2;
    }
    else{
      return 0;
    }
  }

  //Returns min of two values
  double TASDK::minVal(double Value1, double Value2){
    if(Value1 < Value2){
        return Value1;
    }
    else if(Value2 < Value1){
        return Value2;
    }
    else{
      return 0;
    }
  }

  //Returns max of two values
  int TASDK::maxVal(int Value1, int Value2){
    if(Value1 > Value2){
        return Value1;
    }
    else if(Value2 > Value1){
        return Value2;
    }
    else{
      return 0;
    }
  }

  //Returns min of two values
  int TASDK::minVal(int Value1, int Value2){
    if(Value1 < Value2){
        return Value1;
    }
    else if(Value2 < Value1){
        return Value2;
    }
    else{
      return 0;
    }
  }

  //Normalizes a value
  double TASDK::normalize(double Max, double Min, double Value){
    if (Max == Min){
      return 0;
    }
    else{
      return (Value - Min) / (Max - Min);
    }
  }

  static bool CompareNoCase(CString string1, CString string2){
		return string1.CompareNoCase(string2) == 0;
  }
