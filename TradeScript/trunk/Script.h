// Script.h: interface for the CScript class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SCRIPT_H__99347DCF_2DFC_4149_AADA_0F070136AE06__INCLUDED_)
#define AFX_SCRIPT_H__99347DCF_2DFC_4149_AADA_0F070136AE06__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "resource.h"
#include "TASDK.h"
class CTASDK;

#include "BackTestStatistics.h"

// Script
class variable
{
	public:
		variable()
		{
			name = "";
			value = 0;			
		}
		string name;
		double value;
		vector<double> field;
};

class Script	
{
	

static __inline int instr(std::string &s, const std::string &to_find)
{	
	std::string::size_type pos;
	if((pos = s.find(to_find)) != string::npos)
	{
		return pos;
	}
	return -1;
}

static __inline void replace(std::string &s, const std::string &to_find, const std::string& repl_with) 
{
   for(int p = s.find( to_find.c_str() ); p >= 0; p=s.find( to_find.c_str(), p ))
   {
      s.replace( p, to_find.length(), repl_with );
      p += repl_with.length();
   }
}


static __inline void makeupper(std::string &str) 
{ 
	std::string::iterator it = str.begin(); 
	std::string::iterator end = str.end(); 

	for( ; it != end; ++it) 
	{ 
		*it = toupper(*it); 
	}
}

__inline bool isNumeric( const string &s )
{ 
    return (s.find_first_not_of("0123456789.-+eE") == string::npos) && s != "e" && s != "E";
} 


__inline string trim( string const& s ) 
{ 
	std::string::size_type beg = std::distance(s.begin(), 
		std::find_if(s.begin(),s.end(), 
        std::not1(std::ptr_fun(isspace))));
    std::string::size_type end = std::distance(std::find_if(s.rbegin(),s.rend(), 
			std::not1(std::ptr_fun(isspace))), s.rend()); 
    return s.substr(beg, end-beg);

	return s;
} 

__inline double roundToDec(double x, int decimals)
{ 
	double ten = 10;
    double s = x*pow(ten,decimals); 
    if ( s-int(s)<0.5 ) 
        return double(int(s))/pow(ten,decimals); 
    else 
        return double(int(s+1))/pow(ten,decimals);
} 

__inline string toString( double x, int precision = 20 )
{
 	ostringstream o;
	o.str("");
	o << setprecision(precision) << fixed << x;
	int found = o.str().find("#");
	if(found > 1) return "0";
	string ret = o.str();

	return ret;
}

__inline string toStringL( long x, int precision = 20 ) 
{ 
 	ostringstream o;
	o.str("");
	o << setprecision(precision) << fixed << x;
	int found = o.str().find("#");
	if(found > 1) return "0";
	string ret = o.str();
	found = instr(ret, ".");
	if(found > 0) ret = ret.substr(0, found);	

	return ret;
}

public:
	Script::Script()
	{
		m_validating = false;
		m_runningIfFunction = false;
		m_isAssignment = false;
		m_watchVars = false;
		m_accessLevel = 0;
		m_maxBars = 0;				
		m_startTimer = 0;
		m_maxPositionOpen = "";
		m_market = "STOCKS";
		m_records.clear();
	}
	Script::~Script()
	{
	}

public:
	void LoadTestData(recordset& r);
	void Test();

	__inline void AddPrimVar(string name, double value);
	__inline void AddVar(string name, double value);
	string RunScript(string& script, bool iterate, bool validate = false);
	string BackTest(string& BuyScript, string& SellScript, string& ExitLongScript, string& ExitShortScript, double SlipPct);
	string RunBackTest(string& Script, int buySellExit);
	string GetData(string& script);
	string Wrap(string script);
	variable Eval(string expr);
	variable Token(string expr, long& pos);
	variable EvalFunction(string function, string argString, variable var, string evalString = "");
	string GetArg(string argString, int argNum, int minArgsError);
	__inline int SetVar(string& name, variable value);
	__inline variable GetVar(string name);	
	variable GetFunctionVector(string evalString, string functionName);
	variable ThrowMissingArgError(string function, string argument);
	variable ThrowInvalidArgError(string function, string argument);	
	variable ThrowError(string error);
	string CreateVarName(string& expr, string prefix = "");
	int GetMaxBars(string& script);
	__inline void noArgs(string& script, string func);
	__inline bool GetVectorFromArg(string arg, variable& var);
	__inline bool GetValueFromArg(string func, string arg, double& value, bool isRef = true, bool isMAType = false);

	string FromJulianDate(double JDate);
	double ToJulianDate(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet);

	bool m_watchVars;

	vector<variable> variables;
	vector<variable> primitiveVariables;

	string m_error, m_helpString, m_lastFunctionName;	
	string m_exchange, m_sector, m_industry, m_symbol;
		
	int m_maxBars;

	void GetArg(string argString, int minArgsError, 
		string* arg1 = NULL, string* arg2 = NULL, 
		string* arg3 = NULL, string* arg4 = NULL, 
		string* arg5 = NULL, string* arg6 = NULL, 
		string* arg7 = NULL, string* arg8 = NULL);

	recordset m_recordset; // Current symbol's data while scanning
	bool LoadSymbolData(int record);
	bool LoadSymbolDataForBackTest(int record);
	void ResetVariables();
	vector<record> m_records;
	vector<record> m_temp;

	vector<trade> m_trades;

	string m_header;
	int m_accessLevel;
	long m_startTimer;
	vector<string> m_outputs;
	vector<string> m_assignments;
	bool m_isAssignment;
	bool m_runningIfFunction;
	string m_maxPositionOpen;
	string m_market;
	bool m_validating;
	string m_scriptHelp;
	string GetScriptHelp();

};


#endif // !defined(AFX_SCRIPT_H__99347DCF_2DFC_4149_AADA_0F070136AE06__INCLUDED_)
