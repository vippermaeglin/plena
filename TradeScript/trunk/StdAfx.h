// stdafx.h : include file for standard system include files,
//      or project specific include files that are used frequently,
//      but are changed infrequently

#pragma warning(disable:4244) 
#pragma warning(disable:4018) 

#if !defined(AFX_STDAFX_H__C9F0639C_7C2A_4508_A12B_491DCBC2FBDD__INCLUDED_)
#define AFX_STDAFX_H__C9F0639C_7C2A_4508_A12B_491DCBC2FBDD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define STRICT
#ifndef _WIN32_WINNTs
#define _WIN32_WINNT 0x5100
#endif
#define _ATL_APARTMENT_THREADED

#define DEMO_MODE
//#define PERSONAL_LICENSE

#include <atlbase.h>
//You may derive a class from CComModule and use it if you want to override
//something, but do not change the name of _Module
extern CComModule _Module;
#include <atlcom.h>

#include <string>
#include <vector>
#include <stdio.h>
#include <math.h>
#include <map>
#include <set>
#include <algorithm> 
#include <sstream> 
#include <functional> 
#include <iomanip>

using std::fixed;
using namespace std;

 

 
class trade
{
	public:
		trade()
		{
			record = 0;
			signal = 0;
			jdate = 0;
			price = 0;
		}
		int record;
		int signal;
		double jdate;
		double price;

	bool operator < (trade const& rhs) 
    {
        return this->jdate < rhs.jdate; 
    }
	bool operator > (trade const& rhs) 
    {
        return this->jdate > rhs.jdate; 
    }
	bool operator == (trade const& rhs) 
    {
        return this->jdate == rhs.jdate; 
    }
};



class record
{
public:
	record::record()
	{
		jDateTime = 0;
		open = 0;
		high = 0;
		low = 0;
		close = 0;
		volume = 0;
	}
	double 	jDateTime;
	double	open;
	double	high;
	double	low;
	double	close;
	long	volume;
};


class SymbolData
{
public:
	SymbolData::SymbolData()
	{		
		szSymbol = "";
	}
	string szSymbol;
	vector<record> data;
};



#define MAX_STRING 10000

// Buy/sell/exit
#define BUY		1
#define SELL	2
#define EXIT	3
#define EXIT_LONG	4
#define EXIT_SHORT	5
#define NO_TRADE	6

#define MAX_STOCK_PRICE	9999

// Julian date constants
#define JSECOND	1.1574011296034e-005f
#define	JMINUTE	0.00069444440305233f
#define	JHOUR	0.041666666511446f
#define	JDAY	1.0000000000000f
#define	JWEEK	7.0000000000000f
#define	JMONTH	30.000000002328f
#define	JYEAR	365.00000022864f

/////////////////////////////////////////////////////////////////////////////
// Copyright 2006-2008 Modulus Financial Engineering, Inc.
// TradeScript is a trademark of Modulus Financial Engineering, Inc.
// http://www.modulusfe.com
// Use of this source code is subject to the licensing
// agreement found at http://www.modulusfe.com/download/license.asp
// and other license agreements. Contact sales@modulusfe.com for details.
/////////////////////////////////////////////////////////////////////////////
#include "License.h"


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__C9F0639C_7C2A_4508_A12B_491DCBC2FBDD__INCLUDED)
