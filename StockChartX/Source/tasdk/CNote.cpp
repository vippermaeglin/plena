/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */
  
#include "stdafx.h"
#include "Note.h"
// for .net use:
#include <iostream>
// for 6.0 use:
//#include <iostream.h>

CNote::CNote(){
	m_Note = "";
	m_Value = 0;
	m_Period = 0;
}

CNote::~CNote(){

}

void CNote::setPeriod(int Period){
    m_Period = Period;
}

void CNote::setValue(double Value){
    m_Value = Value;
}

void CNote::setNote(LPCTSTR Note){
    m_Note = Note;
}

int CNote::getPeriod(){
    return m_Period;
}

double CNote::getValue(){
    return m_Value;
}

LPCTSTR CNote::getNote(){
    return m_Note;
}
