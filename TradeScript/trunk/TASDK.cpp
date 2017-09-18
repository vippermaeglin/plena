
///////////////////////////////////////////////////////////////
// TA-SDK - Technical Analysis Software Development Kit
// Original version 2/14/2002 to 7/15/2002
// Modified and enhanced 2003 to 2004
// Re-write 8/1/2005 to 9/1/2005
// Copyright 2002 to 2008 Modulus Financial Engineering, Inc.
///////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "TASDK.h"

using namespace std;

CTASDK::CTASDK()
{

}

CTASDK::~CTASDK()
{

}

void CTASDK::LogItem(string logitem, string file)
{
	fstream filestr;
	filestr.open (file.c_str(), fstream::in | fstream::out | fstream::app);
	filestr<<logitem<<endl;
}

// Rounding function.
// computers don't denote a perpetual repetition of 9s, but 
// something which starts with 9s and eventually breaks off. 
// 0.499... has an infinite string of 9s in reality, and is 
// equal to 0.5, and consequently needs to be rounded accordingly.
// Therefore we make an approximation by using as many 9s as possible.
static __inline double round(double x)
{ 
    return (double)(int)(x > 0.0 ? x + 0.499999999999 : x - 0.499999999999);
} 

static __inline double normalize(double max, double min, double value)
{
    if(max == min) return 0;
    return (value - min) / (max - min);
}

static __inline double minVal(double a, double b)
{
	if(a < b) 
		return a;
	else
		return b;
}

static __inline double maxVal(double a, double b)
{
	if(a > b) 
		return a;
	else
		return b;
}

// Returns true if all series are the same length
__inline bool CTASDK::IsEqual( recordset& recSet )
{	
	if(recSet.size() < 2) return true;
	int size = recSet[0].data.size();
	for(int r = 1; r != recSet.size() - 1; ++r)
	{
		if(recSet[r].data.size() != size) return false;
	}
	return true;
}

// Adds a new field to a recordset
field* CTASDK::AddField( const string& name, int size, recordset& recSet )
{
	field ret;
	ret.data.resize(size + 1);
	ret.name = name;
	recSet.push_back(ret);		
	return &recSet[recSet.size() - 1];
}

// Returns a field pointer by name lookup in a recordset
__inline field* CTASDK::GetField( const string& name, recordset& recSet ) 
{
	field* ret = NULL;
	for(int n = 0; n != recSet.size(); ++n)
	{		
		if(recSet[n].name.compare(name) == 0)
			return &recSet[n];
	}
	return ret;
}

// Returns the size of the close series
__inline int CTASDK::GetSize( recordset& ohlcv )
{
	field* close = GetField("C", ohlcv);
	if(close->data.size() < 1) 
		return 0;
	else
		return close->data.size();
}


// Copys a field from a recordset into a new field
__inline field CTASDK::CopyField( const string& name, recordset& recSet ) 
{
	field ret;
	for(int n = 0; n != recSet.size(); ++n)
		if(recSet[n].name.compare(name) == 0)
			return recSet[n];	
	return ret;
}




// Public functions




void CTASDK::Test()
{

	// Create a new field and use it
	field f;
	f.data.resize(2);
	f.name = "TEST";

	// Add the field to a recordset
	recordset r;
	r.push_back(f);


	// Or you could just do this
	AddField("Testing", 2, r);


	// Get the values
	for(int fields = 0; fields != r.size(); ++fields)
	{
		for(int row = 0; row != r[fields].data.size(); ++row)
		{
			printf("X");
		}
	}

}





// TA-SDK C++ 2008 Edition
// Copyright 2005-2008 by Modulus Financial Engineering, Inc.
// This code may not be redistributed.

//////////////////////////////////////////////////////////////////////////////
//	General functions used by most indicators
//
//////////////////////////////////////////////////////////////////////////////


// Returns the high minus the low: (high - low)
field CTASDK::HighMinusLow( recordset& ohlcv, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;
	
	if(!IsEqual(ohlcv)) return field1;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);	
	
	for(int record = 1; record != size; ++record)
	    field1.data[record] = (high->data[record] - low->data[record]);    

    return field1;
}


// Returns the median price: (high + low) / 2
field CTASDK::MedianPrice( recordset& ohlcv, const string& name )
{

	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	if(!IsEqual(ohlcv)) return field1;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	
	for(int record = 1; record != size; ++record)	
	    field1.data[record] = (high->data[record] + low->data[record]) / 2;	
 
	return field1;
}


// Returns the typical price: (high + low + close) / 3
field CTASDK::TypicalPrice( recordset& ohlcv, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	if(!IsEqual(ohlcv)) return field1;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	
	for(int record = 1; record != size; ++record)	
	    field1.data[record] = (high->data[record] + low->data[record] + close->data[record]) / 3;
 
	return field1;
}


// Returns the weighted close: (high + low + (close * 2)) / 4
field CTASDK::WeightedClose( recordset& ohlcv, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	if(!IsEqual(ohlcv)) return field1;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	
	for(int record = 1; record != size; ++record)	
	    field1.data[record] = 
			(high->data[record] + low->data[record] + (close->data[record] * 2)) / 4;
 
	return field1;
}


// Price Rate of Change
field CTASDK::PriceROC( field* source, int periods, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
	
	double prev = 0;
	const int start = periods + 1;	
    for(int record = start; record != size; ++record)
	{
		prev = source->data[record - periods];
		if(prev > 0 )
			field1.data[record] = ((source->data[record] - prev) / prev) * 100;
	}

	return field1;
}


// Volume Rate of Change
field CTASDK::VolumeROC( field* volume, int periods, const string& name )
{
	return PriceROC( volume, periods, name );
}


// Correlation analysis of two fields
double CTASDK::CorrelationAnalysis( field* source1, field* source2 )
{
    double total = 0, a = 0, b = 0; 

	int size = source1->data.size();
	if(size < 1) return 0;
	if(size != source2->data.size()) return 0;
    
	for(int record = 2; record != size; ++record)
	{
        a = (source1->data[record] - source1->data[record - 1]);
        b = (source2->data[record] - source2->data[record - 1]);
		if (a < 0){a = -1 * a;}
		if (b < 0){b = -1 * b;}
        total += (a * b);
    }

    total = total / (size - 2);
    return 1 - total;
}


// Returns the maximum and minimum values for a given range by reference
void CTASDK::MaxMinValue( field* source, const int start_period, const int end_period, double& max_value, double& min_value)
{
	if(start_period > end_period) return;    
	int size = source->data.size();
	if(size < end_period + 1) return;	
    
	max_value = source->data[1];
	min_value = max_value;

	for(int record = start_period; record != end_period + 1; ++record)
	{
		if(source->data[record] > max_value) max_value = source->data[record];
		if(source->data[record] < min_value) min_value = source->data[record];
    }
}


// Returns true if the specified number is a prime
bool CTASDK::IsPrime( const long number )
{
    long divisor = 0;
    long increment = 0;
    long maxDivisor = 0;
    
    if(number > 3){
        if(number % 2 == 0) return false;
        if(number % 3 == 0) return false;
    }
    
    divisor = 5;
    increment = 2;
    maxDivisor = (long)sqrt( (double)number) + 1;
    
    while(divisor <= maxDivisor){
        if(number % divisor == 0) return false;
        divisor += increment;
        increment = 6 - increment;
    }
    
    return true;
}


// Returns a continuous series of the highest high values over a specified period
field CTASDK::HHV( field* high, int periods, const string& name )
{
	field field1;
	int size = high->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
	
	double max = 0;
	const int start = periods + 1;
    for(int record = start; record != size; ++record)
	{
		max = high->data[record];
		for(int p = record; p != (record - periods) - 1; --p)
		{
			if(high->data[p] > max)
				max = high->data[p];
		}
		field1.data[record] = max;
	}

	return field1;
}


// Returns a continuous series of the lowest low values over a specified period
field CTASDK::LLV( field* low, int periods, const string& name )
{
	field field1;
	int size = low->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
	
	double min = 0;
	const int start = periods + 1;
    for(int record = start; record != size; ++record)
	{
		min = low->data[record];
		for(int p = record; p != (record - periods) - 1; --p)
		{
			if(low->data[p] < min)
				min = low->data[p];
		}
		field1.data[record] = min;
	}

	return field1;
}


// Standard deviation
field CTASDK::StandardDeviation( field* source, int periods, int standardDeviations, int maType, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
	
    double sum = 0;
    double value = 0;

	field temp = GetMAByType(source, periods, maType);

    const int start = periods + 1;
    int position = start;

    for(int record = start; record != size; ++record)
	{

        sum = 0;
        value = temp.data[position];

        for(int period = 1; period < periods + 1; period++)
		{
            sum += (source->data[position] - value) * (source->data[position] - value);
            position--;
        }

		position+= periods;        
		
        field1.data[position] = standardDeviations *  sqrt(sum / periods);        

        position++;

    }

	return field1;

}









//////////////////////////////////////////////////////////////////////////////
//	Linear regression functions
//
//////////////////////////////////////////////////////////////////////////////

// Returns slope, intercept, forecast, and r2 in one recordset
recordset CTASDK::Regression( field* source, int periods )
{

    int x = 0; // Period
	std::vector<double> y; // Value
    int n = 0;
    double q1 = 0;
    double q2 = 0;
    double q3 = 0;
    double xy = 0;
    double xSquared = 0;
    double ySquared = 0;
    double xSum = 0;
    double ySum = 0;
    double xSquaredSum = 0;
    double ySquaredSum = 0;
    double xYSum = 0;
    double slope = 0;
    double intercept = 0;
    double forecast = 0;
    double rSquared = 0;

    recordset ret;
    double value = 0;      
    int period = 0;
    int position = 0, oldPosition = 0;

	int size = source->data.size();
	if(periods < 1 || periods > size) return ret;
    
	field field1;
	field1.name = "SLOPE";	
	field1.data.resize(size);

	field field2;
	field2.name = "INTERCEPT";	
	field2.data.resize(size);

	field field3;
	field3.name = "FORECAST";	
	field3.data.resize(size);

	field field4;
	field4.name = "RSQUARED";	
	field4.data.resize(size);

	for(int record = periods; record != size; ++record)
	{
    
        x = periods;
        y.resize(x + 1);

        //Move back n periods        
		oldPosition = record;
		position = record - periods + 1;        

        for(period = 1; period != periods + 1; ++period)
		{
            value = source->data[position];
            y[period] = value;
            position++;
        } // Period

        // Return to original position and reset
        position = oldPosition;
        xSum = 0;
        ySum = 0;
        xSquaredSum = 0;
        ySquaredSum = 0;
        xYSum = 0;

        //Square
        for(n = 1; n != x + 1; ++n){
            xSum += n;
            ySum += y[n];
            xSquaredSum += (n * n);
            ySquaredSum += (y[n] * y[n]);
            xYSum += (y[n] * n);
        } // n

        n = x; // Number of periods in calculation
        q1 = (xYSum - ((xSum * ySum) / n));
        q2 = (xSquaredSum - ((xSum * xSum) / n));
        q3 = (ySquaredSum - ((ySum * ySum) / n));

        slope = (q1 / q2); // Slope
        intercept = (((1 / (double)n) * ySum) - (((int)((double)n / 2)) * slope)); // Intercept
        forecast = ((n * slope) + intercept); // Forecast
        
		if((q2 * q3) != 0){
			rSquared = (q1 * q1) / (q2 * q3); // Coefficient of determination (R-Squared)
		}

        if (record >= periods){
			field1.data[record] = slope;
			field2.data[record] = intercept;
			field3.data[record] = forecast;
			field4.data[record] = rSquared;            
        }

        position++;

    } // Record

    // Append fields to recordset
	ret.push_back(field1);
	ret.push_back(field2);
	ret.push_back(field3);
	ret.push_back(field4);
    
    return ret;

}


// Returns the linear regression forecast and discards the other three fields
field CTASDK::TimeSeriesForecast( field* source, int periods, const string& name )
{
    recordset ret;
    ret = Regression(source, periods);
	field f = CopyField("FORECAST", ret);
	f.name = name;
    return f;
}






// Trend function
field CTASDK::Trend( field* source, int periods, const string& name )
{	

	field field1;
	field1.name = name;
	field1 = SimpleMovingAverage(source, periods, name);
	int size = source->data.size();
	
	field field2;
	field2.data.resize(size);
	field2.name = name;

	int trend = 0; //1 = UP, 2 = DOWN, 3 = SIDEWAYS

	// Prime
	int record = 0;
	for(record = 1; record != periods + 1; record++)
	{
		if(source->data[record] > source->data[1] * 1.005)
			trend = 1;
		else if(source->data[record] < source->data[1] * 0.995)
			trend = 2;
		else
			trend = 3;
		field2.data[record] = trend;
	}
	
    for(record = periods + 1; record != size; record++)
	{
		if(field1.data[record] > field1.data[record - periods] * 1.005)
			trend = 1;
		else if(field1.data[record] < field1.data[record - periods] * 0.995)
			trend = 2;
		else
			trend = 3;
		field2.data[record] = trend;
	}

	return field2;

}








//////////////////////////////////////////////////////////////////////////////
//	Moving Average functions
//
//////////////////////////////////////////////////////////////////////////////

// Returns the moving average specified by maType
field CTASDK::GetMAByType( field* source, int periods, int maType, const string& name )
{
	field field1;
	field1.name = name;

	switch(maType)
	{
	case 1:
		field1 = SimpleMovingAverage(source, periods, name);
		break;
	case 2:
		field1 = ExponentialMovingAverage(source, periods, name);
		break;
	case 3:
		field1 = TimeSeriesMovingAverage(source, periods, name);
		break;
	case 4:
		field1 = VariableMovingAverage(source, periods, name);
		break;
	case 5:
		field1 = TriangularMovingAverage(source, periods, name);
		break;
	case 6:
		field1 = WeightedMovingAverage(source, periods, name);
		break;
	case 7:
		field1 = VIDYA(source, periods, 0.65, name);
		break;
	case 8:
		field1 = WellesWilderSmoothing(source, periods, name);
		break;

	default:
		field1 = SimpleMovingAverage(source, periods, name);
	}

	return field1;
}

//Generic moving average (it replaces GetMAByType above)
field CTASDK::GenericMovingAverage(field* source, int periods, int shift, int maType, double r2scale, const string& name)
{
#ifdef _CONSOLE_DEBUG
	string msg = "GenericMovingAverage()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
	field field1;
	field1.name = name;

	switch (maType)
	{
	case 0:
		field1 = SimpleMovingAverage(source, periods, name);
		break;
	case 1:
		field1 = ExponentialMovingAverage(source, periods, name);
		break;
	case 2:
		field1 = TimeSeriesMovingAverage(source, periods, name);
		break;
	case 3:
		field1 = VariableMovingAverage(source, periods, name);
		break;
	case 4:
		field1 = TriangularMovingAverage(source, periods, name);
		break;
	case 5:
		field1 = WeightedMovingAverage(source, periods, name);
		break;
	case 6:
		field1 = VIDYA(source, periods, 0.65, name);
		break;
	case 7:
		field1 = WellesWilderSmoothing(source, periods, name);
		break;

	default:
		field1 = SimpleMovingAverage(source, periods, name);
	}

	return field1;
}

// Simple moving average (SMA)
field CTASDK::SimpleMovingAverage( field* source, int periods, const string& name, int shift /*=0*/ )
{
#ifdef _CONSOLE_DEBUG
	string msg = "SimpleMovingAverage()n="+name;
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
    double avg = 0;
    int period  = 0;
    int record = 0;
    int pos = 0;    
    
	field field1;		
	int recordCount = source->data.size();
	field1.data.resize(recordCount);
	field1.name = name;

	if (periods < 1 || periods >= recordCount/2) return field1;
    
	int nullOffset = 0;
	bool stop = false;
	for (int i = 1; !stop && i<recordCount; i++){
		if (source->data[i] == NULL_VALUE || source->data[i] == 0) nullOffset++;
		else stop = true;
	}

	pos = periods + nullOffset;

    // Loop through each record
	for (record = pos; record < recordCount /*+ 1*/; record++)
	{

        avg = 0;

		for (period = pos; period >(pos - periods);period--)
		{
            avg += source->data[period];
        } // Period
		
        // Calculate moving average
        avg = avg / periods;
		if (((pos + shift) >= 0) && ((pos + shift)<recordCount + 1))field1.data[pos+shift] = avg;

        pos++;

    } // record

    return field1;

	
	/*
    double avg = 0;
    int period  = 0;
    int record = 0;
    int position = 0;    
    
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    
    const int start = periods + 1;
    position = start;

    // Loop through each record
    for(record = start; record != size; record++)
	{

        avg = 0;

        // Loop backwards through each period
        for(period = 1; period < periods + 1; period++)
		{
            avg += source->data[position];
            position--;
        } // Period

        // Jump forward to last position        
		position += periods;

        // Calculate moving average
        avg = avg / periods;
        field1.data[position] = avg;		

        position++;

    } // record

    return field1;*/
	

}


// Exponential moving average (EMA)
field CTASDK::ExponentialMovingAverage( field* source, int periods, const string& name )
{
	
	double prime = 0;    
    int record = 0;    
    double exp = 0;        

	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    
    exp = 2 / (double)(periods + 1);	

	// To prime the EMA, get an average for the first n periods	
	int cnt = 0;
	for(record = 1; record < periods + 1; record++)
	{
		if(source->data[record] != 0)
		{
			prime += source->data[record];
			cnt++;
		}
	}

	if(prime > 0 && cnt > 0)
	{
		prime = prime / cnt;
	}
	else
	{
		if((int)source->data.size() > periods)
			prime = source->data[periods];
	}

	field1.data[periods] = (source->data[record] * (1 - exp)) + (prime * exp);	 

    // Loop through each record
    for(record = periods + 1; record != size; ++record)
	{
		field1.data[record] = (field1.data[record - 1] * 
					(1 - exp)) + (source->data[record] * exp);    
	}

    return field1;

}


// Time series moving average TSA (same is Time Series Forecast)
field CTASDK::TimeSeriesMovingAverage( field* source, int periods, const string& name )
{
    recordset ret;
    ret = Regression(source, periods);
	field f = CopyField("FORECAST", ret);
	f.name = name;
    return f;
}


// Variable moving average (VMA) uses the Chande Momentum Oscillator (CMO)
field CTASDK::VariableMovingAverage( field* source, int periods, const string& name )
{
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    	
	double cmo = 0;
    double vma = 0;
    double prevVMA = 0;
    double price = 0;
    
    field fcmo = ChandeMomentumOscillator(source, 9);

    const int start = 2;
    int position = start;

	for(int record = start; record != size; ++record)
	{
        prevVMA = field1.data[position - 1];        
        cmo = fcmo.data[position] / 100;
        price = source->data[position];
		if(cmo < 0) cmo = -1 * cmo;
        vma = (cmo * price) + (1 - cmo) * prevVMA;
        field1.data[position] = vma;		
        position++;
    }

    return field1;
}
 

// Triangular moving average (TMA)
field CTASDK::TriangularMovingAverage( field* source, int periods, const string& name )
{
	field field1, field2;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;
    field2.data.resize(size);
	field2.name = name;

	if(periods < 1 || periods >= size) return field1;

	double ma1 = 0;
    double ma2 = 0;
    double avg = 0;
	
    if ((periods % 2) > 0 ){ // Odd number
        ma1 = (int) ((double) periods / 2) + 1;
        ma2 = ma1;
    }
    else{ // Even number
        ma1 = (double)periods / 2;
        ma2 = ma1 + 1;
    }

    const int start = periods + 1;
    int position = start;

    // Loop through each record
	int record;
    for(record = start; record != size; ++record)
	{
        avg = 0;

        // Loop backwards through each period
        for(int period = 1; period < ma1 + 1; period++)
		{
            avg += source->data[position];
            position--;
        } // period

        // Jump forward to last position        
		position += (int)ma1;

        // Calculate moving average
        avg = avg / ma1;
        
		field1.data[position] = avg;		

        position++;

    } // record

    position = start;

    // Loop through each record
    for(record = start; record != size; ++record)
	{
        avg = 0;

        // Loop backwards through each period
        for(int period = 1; period < ma2 + 1; period++)
		{
            avg += field1.data[position];
            position--;
        } // period

        // Jump forward to last position        
		position += (int)ma2;

        // Calculate moving average
        avg = avg / ma2;
        field2.data[position] = avg;

        position++;

    } // record

    return field2;
}
 

 

// Weighted moving average (WMA)
field CTASDK::WeightedMovingAverage( field* source, int periods, const string& name )
{
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    
	double weight = 0;
    for(int period = 1; period != periods + 1; ++period)
	  weight += period;

	double total = 0;
    const int start = periods + 1;
    int position = start;
    
    for(int record = start; record != size; ++record)
	{

        total = 0;
        
        for(int period = periods; period > 0; period--)
		{
            total += period * source->data[position];
            position--;
        }

        // Jump forward to last position
		position += periods;        

        // Calculate moving average
        total = total / weight;
        field1.data[position] = total;		

        position++;
    }

    return field1;
}


// Volatlity Index Dynamic moving Average (VIDYA) by Chande
field CTASDK::VIDYA( field* source, int periods, double r2scale, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    
    double r2scaled = 0;
    double prevValue = 0;
    
	recordset linear = Regression(source, periods);
	field* r2 = GetField("RSQUARED", linear);
     
    const int start = 2;
    int position = start;

    for(int record = start; record != size; ++record)
	{  
        prevValue = source->data[position - 1];
        
        r2scaled = r2->data[position] * r2scale;
        
		field1.data[position] = r2scaled *
        source->data[position] + (1 - r2scaled) * prevValue;
		
		position++;
    }

	return field1;

}
	

// Welles Wilder Smoothing (WWS)
field CTASDK::WellesWilderSmoothing( field* source, int periods, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    
	for(int record = 2; record != size; ++record)	
		field1.data[record] = field1.data[record - 1] + 1 / (double)periods *
           (source->data[record] - field1.data[record - 1]);
    
	return field1;

}










//////////////////////////////////////////////////////////////////////////////
//	Oscillator functions
//
//////////////////////////////////////////////////////////////////////////////


// Chande Momentum Oscillator (CMO)
field CTASDK::ChandeMomentumOscillator( field* source, int periods, const string& name )
{
	
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
     
    double today = 0;
    double yesterday = 0;
    double upSum = 0;
    double downSum = 0;
    double value = 0;
	int position = 0;
	const int start = periods + 2;

    for(int record = start; record != size + 1; ++record)
	{
        
        position = record - periods;
        upSum = 0;
        downSum = 0;

        for(int period = 1; period != periods + 1; ++period)
		{
            
            yesterday = source->data[position - 1];            
			today = source->data[position];

            if(today > yesterday){
                upSum += (today - yesterday);
            }
            else if(today < yesterday){
                downSum += (yesterday - today);
            }

            position++;

        } // period

        position--;
		if(upSum + downSum != 0)
		{
			value = 100 * (upSum - downSum) / (upSum + downSum);
		}
		else{
			value = 0;
		}
        
		field1.data[position] = value;        

    } // record

    return field1;

}


// Momentum Oscillator (MO)
field CTASDK::MomentumOscillator( field* source, int periods, const string& name )
{
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;
    
	if(periods < 1 || periods >= size) return field1;
	const int start = periods + 2;	
	double value = 0;

    for(int record = start; record != size; ++record)
	{
        value = source->data[record - periods];
		if(value != 0)
			value = 100 + ((source->data[record] - value) / value) * 100;
        field1.data[record] = value;
    }

	return field1;
}


// Momentum Oscillator (MO)
field CTASDK::TRIX( field* source, int periods, const string& name )
{
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

    if(periods < 1 || periods >= size) return field1;

	field ema = ExponentialMovingAverage(source, periods);
	ema = ExponentialMovingAverage(&ema, periods);
	ema = ExponentialMovingAverage(&ema, periods);
	
	double value = 0;
	const int start = periods + 2;
	
    for(int record = start; record != size; ++record)
	{
		value = ema.data[record - 1];		
        if(value != 0) field1.data[record] = ((ema.data[record] - value) / value) * 100;
    }

	return field1;

  }


// Ultimate Oscillator (UO)
field CTASDK::UltimateOscillator( recordset& ohlcv, int cycle1, int cycle2, int cycle3, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	field* open = GetField("O", ohlcv);
	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
    
    double value = 0;
    double tl = 0;
    double bp = 0;
    double tr = 0;
    double bpSum1 = 0;
    double bpSum2 = 0;
    double bpSum3 = 0;
    double trSum1 = 0;
    double trSum2 = 0;
    double trSum3 = 0;

    int periods = cycle1;
    if(cycle2 > periods) periods = cycle2;
    if(cycle3 > periods) periods = cycle3;

    const int start = periods + 2;
    int position = start;

    for(int record = start; record != size + 1; ++record)
	{

		if(record > size) break;

        bpSum1 = 0;
        bpSum2 = 0;
        bpSum3 = 0;
        trSum1 = 0;
        trSum2 = 0;
        trSum3 = 0;
				
		position = record - cycle1;
		int period = 0;
        for(period = 1; period < cycle1 + 1; period++)
		{
			if(low->data[position] < close->data[position - 1])		  
				tl = low->data[position];
			else
				tl = close->data[position - 1];
          
            bp = close->data[position] - tl;
            tr = high->data[position] - low->data[position];
            
			if(tr < high->data[position] - close->data[position - 1])
			    tr = high->data[position] - close->data[position - 1];
            
            if(tr < close->data[position - 1] - low->data[position])			
                tr = close->data[position - 1] - low->data[position];            

            bpSum1 += bp;
            trSum1 += tr;

            position++;
        }
        
		position -= cycle2;
        for(period = 1; period < cycle2 + 1; period++)
		{
            if(low->data[position] < close->data[position - 1])
				tl = low->data[position];
            else
				tl = close->data[position - 1];
            
            bp = close->data[position] - tl;
            tr = high->data[position] - low->data[position];

            if(tr < high->data[position] - close->data[position - 1])
                tr = high->data[position] - close->data[position - 1];
            
			if(tr < close->data[position - 1] - low->data[position])
                tr = close->data[position - 1] - low->data[position];
            
            bpSum2 += bp;
            trSum2 += tr;

            position++;
        }

        position -= cycle3;
        for(period = 1; period < cycle3 + 1; period++)
		{
            if(low->data[position] < close->data[position - 1])
				tl = low->data[position];
            else
				tl = close->data[position - 1];
            
            bp = close->data[position] - tl;
            tr = high->data[position] - low->data[position];

            if(tr < high->data[position] - close->data[position - 1])
                tr = high->data[position] - close->data[position - 1];
            
			if(tr < close->data[position - 1] - low->data[position])
                tr = close->data[position - 1] - low->data[position];
            
            bpSum3 += bp;
            trSum3 += tr;

            position++;

        }
        
		if(trSum1 != 0 && trSum2 != 0 && trSum3 != 0)
			field1.data[position - 1] = (4 * (bpSum1 / trSum1) + 2 * 
				(bpSum2 / trSum2) + (bpSum3 / trSum3)) / (4 + 2 + 1) * 100;
       
    }

    return field1;

}


// Vertical Horizontal Filter (VHF)
field CTASDK::VerticalHorizontalFilter( field* source, int periods, const string& name )
{
	field field1;		
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;
    
	if(periods < 1 || periods >= size) return field1;

	double hcp = 0;
    double lcp = 0;
    double sum = 0;
	double abs = 0;

    const int start = periods + 2;
	int position = start;	
	int period = 0;

    for(int record = start; record != size + 1; ++record)
	{

        hcp = 0;
        lcp = source->data[position];        
		position = record - periods;

        for(period = 1; period != periods + 1; ++period)
		{
            if(source->data[position] < lcp)
			{
                lcp = source->data[position];
            }
            else if(source->data[position] > hcp)
			{
                hcp = source->data[position];
            }
            position++;
        }

        sum = 0;
		position = record - periods;
        
        for(period = 1; period != periods + 1; ++period)
		{
			abs = (source->data[position] - source->data[position - 1]);
			if(abs < 0) abs = -1 * abs;
            sum += abs;
            position++;
        }
        
		if(sum != 0) abs = (hcp - lcp) / sum;
		if(abs < 0) abs = -1 * abs;
        field1.data[position - 1] = abs;

    }

	return field1;

}


// Williams %R (WPR)
field CTASDK::WilliamsPctR( recordset& ohlcv, int periods, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;
    
	if(periods < 1 || periods >= size) return field1;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);

    double hh = 0;
    double ll = 0;

    const int start = periods + 2;
    int position = start;

    for(int record = start; record != size + 1; ++record)
	{

        hh = 0;
        ll = low->data[position];
        
		position = record - periods;

        for(int period = 1; period != periods + 1; ++period)
		{
            if(high->data[position] > hh) hh = high->data[position];            
			if(low->data[position] < ll) ll = low->data[position];            
            position++;
        }
        
		if(hh - ll != 0)
			field1.data[position - 1] = ((hh - close->data[position - 1]) / (hh - ll)) * -100;        

    }

	return field1;
}


// Williams Accumulation Distribution (WAD)
field CTASDK::WilliamsAccumulationDistribution( recordset& ohlcv, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;
    
	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);

    double trh = 0;
    double trl = 0;
    double value = 0;
    
	const int start = 2;
    int position = start;

    for(int record = start; record != size; ++record)
	{

        trh = close->data[position - 1];
        if(high->data[position] > trh)
            trh = high->data[position];

        trl = close->data[position - 1];
        if(low->data[position] < trl)
            trl = low->data[position];        

        if(close->data[position] > close->data[position - 1])
	        value = close->data[position] - trl;
	    else if(close->data[position] < close->data[position - 1])
	        value = close->data[position] - trh;
	    else
		    value = 0;
	    
		field1.data[position] = value + field1.data[position - 1];

		position++;

    }

	return field1;

}


// Volume Oscillator (VO)
field CTASDK::VolumeOscillator( field* volume, int shortTerm, int longTerm, int maType, int pointsOrPercent, const string& name )
{
	field field1;		
	int size = volume->data.size();
	field1.data.resize(size);
	field1.name = name;
    
	if(shortTerm > longTerm){
		int term = shortTerm;
		shortTerm = longTerm;
		longTerm = term;
	}

	field ma1 = GetMAByType(volume, shortTerm, maType);
	field ma2 = GetMAByType(volume, longTerm, maType);

    for(int record = 1; record != size; ++record)
	{
        if(pointsOrPercent == 1)
		{
            field1.data[record] = ma1.data[record] - ma2.data[record];                    
        }
        else if(pointsOrPercent == 2)
		{
            if(ma2.data[record] != 0)
                field1.data[record] = ((ma1.data[record] - ma2.data[record]) / 
						ma2.data[record]) * 100;
        }

    }
    
	return field1;
}


// Chaikin Volatility (CV)
field CTASDK::ChaikinVolatility( recordset& ohlcv, int periods, int roc, int maType, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    
	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);

    double ma1 = 0;
    double ma2 = 0;

    field hl;
	hl.data.resize(size);
	int record = 0;
    for(record = 1; record != size; ++record)
		hl.data[record] = high->data[record] - low->data[record];

	field hlma = GetMAByType(&hl, periods, maType);

	const int start = roc + 1;	
    for(record = start; record != size; ++record)
	{
        ma1 = hlma.data[record - roc];
		ma2 = hlma.data[record];        
        if(ma1 != 0 && ma2 != 0) field1.data[record] = ((ma1 - ma2) / ma1) * -100;
	}

	return field1;

}


// StochasticOscillator (SO)
recordset CTASDK::StochasticOscillator( recordset& ohlcv, int kPeriods, int kSlowingPeriods, int dPeriods, int maType )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = "D";
	
	recordset ret;

	if(kPeriods < 1 || kPeriods > size) return ret;
	if(kSlowingPeriods < 1 || kSlowingPeriods > size) return ret;
	if(dPeriods < 1 || dPeriods > size) return ret;
    
	field field2;	
	field2.data.resize(size);
	field2.name = "K";

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);	

    double ll = 0;
    double hh = 0;
    double value = 0;

    const int start = kPeriods + 2;
    int position = start;

    for(int record = start; record != size + 1; ++record)
	{
    
		position = record - kPeriods;
        hh = high->data[position];
        ll = low->data[position];

        for(int period = 1; period != kPeriods + 1; ++period)
		{
            if(high->data[position] > hh)
                hh = high->data[position];            

            if(low->data[position] < ll)
                ll = low->data[position];

			position++;
        }

		if(hh - ll != 0)
			field1.data[position - 1] = (close->data[position - 1] - ll) / (hh - ll) * 100;

    }

	//double a = field1.data[50];

    if(kSlowingPeriods > 1) field1 = GetMAByType(&field1, kSlowingPeriods, maType, "K");
    
	//double b = field1.data[50];

	field2 = GetMAByType(&field1, dPeriods, maType, "D");	

	ret.push_back(field1);
	ret.push_back(field2);

	return ret;

}


// Price Oscillator (PO)
field CTASDK::PriceOscillator( field* source, int shortTerm, int longTerm, int maType, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

    if(longTerm <= shortTerm)
	{
		int temp = shortTerm;
		shortTerm = longTerm;
		longTerm = temp;		
	}
   
	field shortMA = GetMAByType(source, shortTerm, maType);
	field longMA = GetMAByType(source, longTerm, maType);
	
    const int start = longTerm + 1;

    for(int record = start; record != size; ++record)
	{
		if(longMA.data[record] != 0)
			field1.data[record] = ((shortMA.data[record] - 
				longMA.data[record]) / longMA.data[record]) * 100;
	}

	return field1;

}


// Moving Average Convergence Divergence (MACD)
recordset CTASDK::MACD( recordset& ohlcv, int shortCycle, int longCycle, int signalPeriods, int maType, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);

	recordset ret;

	if(maType < 1) maType = 2; // EMA
	if(signalPeriods == shortCycle) signalPeriods++;

	field ema1 = GetMAByType(close, longCycle, maType);
    field ema2 = GetMAByType(close, shortCycle, maType);
    
	int record = 0;

    for(record = 1; record != size; ++record)        
		field1.data[record] = ema2.data[record] - ema1.data[record];    

	field field2 = GetMAByType(&field1, signalPeriods, maType);	
	field2.name = name;
	field2.name.append(" SIGNAL");

	for(record = 1; record != longCycle + 1; ++record)
		field1.data[record] = 0;
	
	for(record = 1; record != longCycle + 1 + shortCycle; ++record)
		field2.data[record] = 0;	

	ret.push_back(field1);
	ret.push_back(field2);

	return ret;

}


// Ease Of Movement (EOM)
field CTASDK::EaseOfMovement( recordset& ohlcv, int periods, int maType, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	field* volume = GetField("V", ohlcv);

	recordset ret;

	if(periods < 1 || periods >= size) return field1;

    double mpm = 0;
    double emv = 0;
    double boxRatio = 0;
	double bd = 0;

    const int start = 2;
    for(int record = start; record != size; ++record)
	{

        mpm = ((high->data[record] + low->data[record]) / 2) -
        ((high->data[record - 1] + low->data[record - 1]) / 2);

        bd = high->data[record] - low->data[record];
			
		if(bd != 0) boxRatio = volume->data[record] / bd;

        if(boxRatio != 0) emv = mpm / boxRatio;

		field1.data[record] = emv * 100000;

    }
	
	field emva = SimpleMovingAverage(&field1, periods);
    
	return field1;

}


// Detrended Price Oscillator (DPO)
field CTASDK::DetrendedPriceOscillator( field* source, int periods, int maType, const string& name )
{

	field field1;
	int size = source->data.size();

	field1.data.resize(size);
	field1.name = name;
	if(periods < 1 || periods >= size) return field1;
	field dpoma = GetMAByType(source, periods, maType);

	int record = 0;
    for(record = 1; record != size; ++record)
	{

		int r = record - (periods / 2) + 1;
		double value = 0;
		if(r < dpoma.data.size())
			value = dpoma.data[record - (periods / 2) + 1];
        field1.data[record] = source->data[record] - value;

	}
	for(record = 1; record != periods * 2; ++record)
		field1.data[record] = 0;

	return field1;

}


// Parabolic SAR (PSAR)
field CTASDK::ParabolicSAR( field* highPrice, field* lowPrice, double minAF, double maxAF, const string& name )
{
	field field1;
	int size = highPrice->data.size();
	field1.data.resize(size);
	field1.name = name;
		
    double max  = 0;
    double min  = 0;
    double pSAR = 0;
    double pEP = 0;
    double pAF = 0;
    double sar = 0;
    double af = 0;
    double hi = 0;
    double lo = 0;
    double pHi = 0;
    double pLo = 0;

    const int start = 2;
    int position = 0;

    max = highPrice->data[1];
    min = lowPrice->data[1];
	
    if(highPrice->data[2] - highPrice->data[1] < lowPrice->data[2] - lowPrice->data[1]) 
	{
        pSAR = max;
        position = -1;
    }
	else
	{
        pSAR = min;
        position = 1;
    }

    pAF = minAF;
    sar = pSAR;
    hi = max;
    lo = min;
    pHi = hi;
    pLo = lo;
    af = minAF;

    for(int record = start; record != size; ++record)
	{

		if(position == 1)
		{

			if(highPrice->data[record] > hi)
			{
                hi = highPrice->data[record];
				if(af < maxAF) af += minAF;
			}
            sar = pSAR + pAF * (pHi - pSAR);
            if(lowPrice->data[record] < sar){
				position = -1;
                af = minAF;
                sar = pHi;
				hi = 0;
                lo = lowPrice->data[record];
            }

		}
		else if(position == -1)
		{
			if(lowPrice->data[record] < lo){
                lo = lowPrice->data[record];
				if(af < maxAF) af = af + minAF;
			}
			sar = pSAR + pAF * (pLo - pSAR);
			if(highPrice->data[record] > sar)
			{
				position = 1;
                af = minAF;
                sar = pLo;
                lo = 0;
                hi = highPrice->data[record];
			}
		}

        pHi = hi;
        pLo = lo;
        pSAR = sar;
        pAF = af;
		
        field1.data[record] = sar;

    }

	return field1;

}


// True Range (TR)
field CTASDK::TrueRange( recordset& ohlcv, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = name;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);

    double t1 = 0;
    double t2 = 0;
    double t3 = 0;    
	double value = 0;

	for(int record = 2; record != size; ++record)
	{

        t1 = high->data[record] - low->data[record];
		
        t2 = fabs(high->data[record] - close->data[record - 1]);
        t3 = fabs(close->data[record - 1] - low->data[record]);

        value = 0;
        if(t1 > value) value = t1;
        if(t2 > value) value = t2;
        if(t3 > value) value = t3;
        
		field1.data[record] = value;
        
    }

	return field1;

}


// Average True Range (ATR)
field CTASDK::AverageTrueRange( recordset& ohlcv, int periods, int maType, const string& name )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	
	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);

    double truehigh = 0;
    double truelow = 0;

	for(int record = 2; record != size; ++record)
	{

        if(close->data[record-1] > high->data[record])
			truehigh = close->data[record-1];
		else
			truehigh = high->data[record];
		
        if(close->data[record-1] < low->data[record])
			truelow = close->data[record-1];
		else
			truelow = low->data[record];
        
		field1.data[record] = truehigh - truelow;
        
    }

	field1 = GetMAByType(&field1, periods, maType, name);

	return field1;


}


// Directional Movement System (ADX, ADXR, DI+, DI-)
recordset CTASDK::DirectionalMovementSystem( recordset& ohlcv, int periods )
{

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);	

	recordset ret;
	int size = GetSize(ohlcv);

	if(periods < 1 || periods > size) return ret;

	double hdif  = 0;
    double ldif  = 0;
   	double value = 0;
    
	field tr = TrueRange(ohlcv, "TR");

    field wSumTR = WellesWilderSmoothing( &tr, periods, "TRSUM");

	field upDMI;
	upDMI.data.resize(size);
	upDMI.name = "UPDMI";

	field dnDMI;
	dnDMI.data.resize(size);
	dnDMI.name = "DNDMI";

	field din;
	din.data.resize(size);
	din.name = "DI-";

	field dip;
	dip.data.resize(size);
	dip.name = "DI+";

	int record = 0;
	for(record = 2; record != size; ++record)
	{        
        hdif = high->data[record] - high->data[record - 1];
        ldif = low->data[record - 1] - low->data[record];
        
        if((hdif < 0 && ldif < 0) || (hdif == ldif))
		{
            upDMI.data[record] = 0;
            dnDMI.data[record] = 0;
		}
        else if(hdif > ldif)
		{
            upDMI.data[record] = hdif;
            dnDMI.data[record] = 0;
		}
        else if(hdif < ldif)
		{
            upDMI.data[record] = 0;
            dnDMI.data[record] = ldif;
        }

    }
   
    field wSumUpDMI = WellesWilderSmoothing(&upDMI, periods, "DM+SUM");
    field wSumDnDMI = WellesWilderSmoothing(&dnDMI, periods, "DM-SUM");
    
    for(record = 2; record != size; ++record)
	{
		if(wSumTR.data[record] != 0)
		{
			dip.data[record] = round(100 * wSumUpDMI.data[record]  / wSumTR.data[record]);
			din.data[record] = round(100 * wSumDnDMI.data[record] / wSumTR.data[record]);
		}
    }
   
    field dx;
	dx.data.resize(size);
	dx.name = "DX";

	for(record = 2; record != size; ++record)
	{
		double a = fabs(dip.data[record] - din.data[record]);
		double b = dip.data[record] + din.data[record];
		if(b != 0)
		{
			dx.data[record] = round(100 * (a / b));
		}	
    }
    
    field adxf;
	adxf.data.resize(size);
	adxf.name = "ADX";
    
	for(record = periods + 1; record != size; ++record)
	{     
        adxf.data[record] = round(((adxf.data[record - 1] * 
			(periods - 1)) + dx.data[record]) / periods);		
    }

    field adxr;
	adxr.data.resize(size);
	adxr.name = "ADXR";
    
	for(record = periods + 1; record != size; ++record)
	{	
        adxr.data[record] = round((adxf.data[record] + adxf.data[record - 1]) / 2);
    }
    
	ret.push_back(adxf); // ADX
	ret.push_back(adxr); // ADXR
	ret.push_back(dx);	// DX
	ret.push_back(wSumTR); // TRSUM
	ret.push_back(din);   // DI-
	ret.push_back(dip);   // DI+

	return ret;
	
}


// Aroon (ARN)
recordset CTASDK::Aroon( recordset& ohlcv, int periods )
{

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);	

	recordset ret;
	int size = GetSize(ohlcv);

	if(periods < 1 || periods > size) return ret;

	double highestHigh = 0;
    double lowestLow = 0;
	int highPeriod = 0;
	int lowPeriod = 0;

    field aUp;
	aUp.data.resize(size);
	aUp.name = "AROON UP";

	field aDn;
	aDn.data.resize(size);
	aDn.name = "AROON DOWN";

	field aOs;
	aOs.data.resize(size);
	aOs.name = "AROON OSCILLATOR";

    for(int record = periods + 1; record != size; ++record)
	{
    
        highestHigh = high->data[record];
        lowestLow = low->data[record];
        highPeriod = record;
        lowPeriod = record;
        
        for(int period = record - periods; period != record; ++period)
		{
            
            if(high->data[period] > highestHigh)
			{
                highestHigh = high->data[period];
                highPeriod = period;
            }
            
            if(low->data[period] < lowestLow)
			{
                lowestLow = low->data[period];
                lowPeriod = period;
            }
            
        }
        
		aUp.data[record] = (double)(((double)periods - (double)(record - highPeriod)) / periods) * 100;
        aDn.data[record] = (double)(((double)periods - (double)(record - lowPeriod)) / periods) * 100;        
        aOs.data[record] = aUp.data[record] - aDn.data[record];
        
    }    

	ret.push_back(aUp);
	ret.push_back(aDn);
	ret.push_back(aOs);
    
    return ret;
  
}


// Rainbow Oscillator (RO)
field CTASDK::RainbowOscillator( field* source, int levels, int maType, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	field fma;

	if(levels < 1 || levels > 50) return field1;

    for(int level = 2; level != levels + 1; ++level)
	{
		fma = GetMAByType(source, levels, maType, name);

		for(int record = 1; record != size; ++record)
		{
            field1.data[record] = (source->data[record] - 
				fma.data[record]) + field1.data[record];
        }
    }
    
    for(int record = 1; record != size; ++record)	
         field1.data[record] = field1.data[record] / (double)levels;

	return field1;

}


// Fractal Chaos Oscillator (FCO)
field CTASDK::FractalChaosOscillator( recordset& ohlcv, int periods, const string& name )
{

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);	

	recordset ret;
	int size = GetSize(ohlcv);

	field field1;
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods > size) periods = 100;	

    field fH1;
	fH1.data.resize(size);
	field fH2;
	fH2.data.resize(size);
	field fH3;
	fH3.data.resize(size);
	field fH4;
	fH4.data.resize(size);
	field fL1;
	fL1.data.resize(size);
	field fL2;
	fL2.data.resize(size);
	field fL3;
	fL3.data.resize(size);
	field fL4;
	fL4.data.resize(size);	

	int record = 0;
	for(record = 5; record != size; ++record)
	{
    
		fH1.data[record] = high->data[record - 4];
        fL1.data[record] = low->data[record - 4];
        
        fH2.data[record] = high->data[record - 3];
        fL2.data[record] = low->data[record - 3];
        
        fH3.data[record] = high->data[record - 2];
        fL3.data[record] = low->data[record - 2];
        
        fH4.data[record] = high->data[record - 1];
        fL4.data[record] = low->data[record - 1];
        
    }

    for(record = 2; record != size; ++record)
	{

        if((fH3.data[record] > fH1.data[record]) &&
            (fH3.data[record] > fH2.data[record]) &&
            (fH3.data[record] >= fH4.data[record]) &&
            (fH3.data[record] >= high->data[record])){
            field1.data[record] = 1;
        }
    
        if((fL3.data[record] < fL1.data[record]) &&
            (fL3.data[record] < fL2.data[record]) &&
            (fL3.data[record] <= fL4.data[record]) &&
            (fL3.data[record] <= low->data[record])){
            field1.data[record] = -1;
        }
   
    }    

	return field1;
    
}


// Prime Number Oscillator (PNO)
field CTASDK::PrimeNumberOscillator( field* source, const string& name )
{

	int size = source->data.size();
	field field1;
	field1.data.resize(size);
	field1.name = name;

	long n = 0, value = 0;
    long top = 0, bottom = 0;
    
	for(int record = 1; record != size; ++record)
	{    
    
        value = (long)source->data[record];
		if(value < 10) value = value * 10;
        
		for(n = value; n != 1; --n)
		{
            if(IsPrime(n))
			{
                bottom = n;
                break;
            }
        }
    
        for(n = value; n != value * 2; ++n)
		{
            if(IsPrime(n))
			{
                top = n;
                break;
            }
        }
        
        if(fabs((double)(value - top)) < fabs((double)(value - bottom)))
		{
             field1.data[record] = value - top;
        }
		else{
            field1.data[record] = value - bottom;
        }
        
    }   

	return field1;

}










//////////////////////////////////////////////////////////////////////////////
//	Index functions
//
//////////////////////////////////////////////////////////////////////////////


// Money Flow Index (MFI)
field CTASDK::MoneyFlowIndex( recordset& ohlcv, int periods, const string& name )
{

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	field* volume = GetField("V", ohlcv);

	int size = GetSize(ohlcv);
	field field1;
	field1.data.resize(size);
	field1.name = name;

    double price1 = 0, price2 = 0;
    double v = 0;
    double today = 0;
    double posFlow = 0;
    double negFlow = 0;
    double moneyIndex = 0;
    double moneyRatio = 0;

    if(periods < 1 || periods >= size) return field1;
    
    const int start = periods + 2;
    int position = start;

    for(int record = start; record != size + 1; ++record)
	{

        posFlow = negFlow = 0;

        position = record - periods;		

        for(int period = 1; period != periods + 1; ++period)
		{

            position--;
            price1 = (high->data[position] + low->data[position] + close->data[position]) / 3;
            position++;

            v = volume->data[position];
            if(v < 1) v = 1;
            price2 = (high->data[position] + low->data[position] + close->data[position]) / 3;

            if(price2 > price1)
				posFlow += price2 * v;            
            else if(price2 < price1)
                negFlow += price2 * v;
            
            position++;

        }

        position--;

        if(posFlow != 0 && negFlow != 0)
		{
            moneyRatio = posFlow / negFlow;
            moneyIndex = 100 - (100 / (1 + moneyRatio));
            field1.data[position] = moneyIndex;
        }

    }

	return field1;

}


// Trade Volume Index (TPO)
field CTASDK::TradeVolumeIndex( field* source, field* volume, double minTickValue, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	int direction = 0;
    int lastDirection = 0;
    double change = 0;
    double tvi = 0;

    if(minTickValue < 0) return field1;

    for(int record = 2; record != size; ++record)
	{

        change = source->data[record] - source->data[record - 1];

        if(change > minTickValue)
		    direction = 1;
        else if(change < -minTickValue)
		    direction = -1;
        
        if(change <= minTickValue && change >= -minTickValue)
            direction = lastDirection;

        lastDirection = direction;

        if(direction == 1)
            tvi += volume->data[record];
        else if(direction == -1)
            tvi -= volume->data[record];
        
        field1.data[record] = tvi;
	}

    return field1;
}


// Swing Index (SI)
field CTASDK::SwingIndex( recordset& ohlcv, double limitMoveValue, const string& name )
{

	field* open = GetField("O", ohlcv);
	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	field* volume = GetField("V", ohlcv);

	int size = GetSize(ohlcv);
	field field1;
	field1.data.resize(size);
	field1.name = name;

	if(limitMoveValue == 0) return field1;
	
    double cy = 0;
    double ct = 0;
    double oy = 0;
    double ot = 0;
    double hy = 0;
    double ht = 0;
    double ly = 0;
    double lt = 0;
    double k = 0;
    double r = 0;
    double a = 0;
    double b  = 0;
    double c = 0;
    double value = 0;

    for(int record = 2; record != size; ++record)
	{		
        oy = open->data[record - 1];
        ot = open->data[record];
        hy = high->data[record - 1];
        ht = high->data[record];
        ly = low->data[record - 1];
        lt = low->data[record];
        cy = close->data[record - 1];
        ct = close->data[record];

		if(fabs(ht - cy) > fabs(lt - cy))
			k = fabs(ht - cy);
		else
			k = fabs(lt - cy);

        a = fabs(ht - cy);
        b = fabs(lt - cy);
        c = fabs(ht - lt);

        if(a > b && a > c)
		    r = fabs(ht - cy) - 0.5 * fabs(lt - cy) + 0.25 * fabs(cy - oy);        
        else if(b > a && b > c)
            r = fabs(lt - cy) - 0.5 * fabs(ht - cy) + 0.25 * fabs(cy - oy);        
        else if(c > a && c > b)
            r = fabs(ht - lt) + 0.25 * fabs(cy - oy);        

		if(r != 0 && limitMoveValue != 0)
			value = 50 * ((ct - cy) + 0.5 * (ct - ot) + 0.25 * (cy - oy)) / r * k / limitMoveValue;

        field1.data[record] = value;

	}

	return field1;

}


// Accumulative Swing Index (ASI)
field CTASDK::AccumulativeSwingIndex( recordset& ohlcv, double limitMoveValue, const string& name )
{

	int size = GetSize(ohlcv);
	field field1;
	field1.data.resize(size);
	field1.name = name;

	if(limitMoveValue == 0) return field1;
	
    field rawSI = SwingIndex(ohlcv, limitMoveValue);
    
    for(int record = 2; record != size; ++record)	
		field1.data[record] = rawSI.data[record] + field1.data[record - 1];
    
	return field1;

}


// Relative Strength Index (RSI)
field CTASDK::RelativeStrengthIndex( field* source, int periods, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;

    double ut = 0;
    double dt = 0;
    double upSum = 0;
    double downSum = 0;
    double rs = 0;
    double rsi = 0;
	double value = 0;

	field au;
	au.data.resize(size);
	field ad;
	ad.data.resize(size);

    int position = 2;

    for(int period = 1; period != periods + 1; ++period)
	{
        ut = 0;
        dt = 0;
		value = source->data[position];
		if(value != NULL_VALUE)
		{
			if(value > source->data[position - 1])
			{
				ut = value - source->data[position - 1];
				upSum += ut;
			}
			else if(value < source->data[position - 1])
			{
				dt = source->data[position - 1] - value;
				downSum += dt;
			}
		}
        position++;
    }

    position--;

    upSum = upSum / periods;
    au.data[position] = upSum;
    downSum = downSum / periods;
    ad.data[position] = downSum;

	if(downSum != 0) rs = upSum / downSum;
    
	rsi = 100 - (100 / (1 + rs));

    const int start = periods + 3;

	for(int record = start; record != size + 1; ++record)
	{
        position = record - periods;

        upSum = 0;
        downSum = 0;

        for(int period = 1; period != periods + 1; ++period)
		{
            ut = 0;
            dt = 0;
			value = source->data[position];
			if(value != NULL_VALUE)
			{
				if(value > source->data[position - 1])
				{
					ut = value - source->data[position - 1];							
					upSum += ut;
				}
				else if(value < source->data[position - 1])
				{
					dt = source->data[position - 1] - value;
					downSum += dt;
				}
			}
            position++;
		}

        position--;

        upSum = (((au.data[position - 1] * (periods - 1)) + ut)) / periods;
        downSum = (((ad.data[position - 1] * (periods - 1)) + dt)) / periods;

		au.data[position] = upSum;
		ad.data[position] = downSum;
        
        if(downSum == 0) downSum = upSum;
        if(upSum == 0)
		{
			rs = 0;     
		}
		else
		{
            if(downSum != 0) rs = upSum / downSum;
		}
        
        if(downSum != 0) rs = (upSum / downSum);
        
		rsi = 100 - (100 / (1 + rs));

        field1.data[position] = rsi;

	}

	return field1;

}


// Comparative Relative Strength Index (CRSI)
field CTASDK::ComparativeRelativeStrengthIndex( field* source1, field* source2, const string& name )
{
	field field1;
	int size = source1->data.size();
	field1.data.resize(size);
	field1.name = name;
	
	if(source2->data.size() != source1->data.size()) return field1;

    for(int record = 1; record != size; ++record)
	{
		if(source2->data[record] != 0){
			field1.data[record] = source1->data[record] / source2->data[record];
		}
		if(field1.data[record] == 1) field1.data[record] = NULL_VALUE;
	}

    return field1;

}


// Price Volume Trend (PVT)
field CTASDK::PriceVolumeTrend( field* source, field* volume, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(source->data.size() != volume->data.size()) return field1;

    for(int record = 2; record != size; ++record)
	{
        field1.data[record] = (((source->data[record] -
                source->data[record - 1]) /
                source->data[record - 1]) *
                volume->data[record]) +
                field1.data[record - 1];
	}

	return field1;

}


// Positive Volume Index (PVI)
field CTASDK::PositiveVolumeIndex( field* source, field* volume, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(source->data.size() != volume->data.size()) return field1;

	field1.data[1] = 1;
    for(int record = 2; record != size; ++record)
	{
        if(volume->data[record] > volume->data[record - 1])
		{
            field1.data[record] = field1.data[record - 1] +
                    (source->data[record] -
                    source->data[record - 1]) /
                    source->data[record - 1] *
                    field1.data[record - 1];
        }
        else
		{
            field1.data[record] = field1.data[record - 1];
        }
	}

	return field1;

}


// Negative Volume Index (NVI)
field CTASDK::NegativeVolumeIndex( field* source, field* volume, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(source->data.size() != volume->data.size()) return field1;

	field1.data[1] = 1;
    for(int record = 2; record != size; ++record)
	{
        if(volume->data[record] < volume->data[record - 1])
		{
            field1.data[record] = field1.data[record - 1] +
                    (source->data[record] -
                    source->data[record - 1]) /
                    source->data[record - 1] *
                    field1.data[record - 1];
        }
        else
		{
            field1.data[record] = field1.data[record - 1];
        }
	}

	return field1;

}


// Performance Index (PI)
field CTASDK::PerformanceIndex( field* source, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	double firstPrice = 0;
    double value = 0;

    firstPrice = source->data[1];
	if(firstPrice == 0) return field1;

    for(int record = 2; record != size; ++record)	
        field1.data[record] = ((source->data[record] - firstPrice) / firstPrice) * 100;
        
	return field1;

}


// On Balance Volume (OBV)
field CTASDK::OnBalanceVolume( field* source, field* volume, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	if(source->data.size() != volume->data.size()) return field1;

	for(int record = 2; record != size; ++record)
	{
		if(source->data[record - 1] < source->data[record])		
            field1.data[record] = field1.data[record - 1] + volume->data[record];
        else if(source->data[record] < source->data[record - 1])
            field1.data[record] = field1.data[record - 1] - volume->data[record];
        else
            field1.data[record] = field1.data[record - 1];
	}	
	
	return field1;

}


// Mass Index (MI)
field CTASDK::MassIndex( recordset& ohlcv, int periods, const string& name )
{
	int size = GetSize(ohlcv);
	field field1;
	field1.data.resize(size);
	field1.name = name;

	if(periods < 1 || periods >= size) return field1;
    double sum = 0;

	field hml = HighMinusLow(ohlcv);	
	field ema1 = ExponentialMovingAverage(&hml, 9);
	field ema2 = ExponentialMovingAverage(&ema1, 9);	
    
	const int start = (periods * 2) + 1;
	int position = start;
	for(int record = start; record != size + 1; ++record)
	{

        sum = 0;
		position = record - periods;
        
		for(int period = 1; period != periods + 1; ++period)
		{
			double test1 = ema1.data[position];
			double test2 = ema2.data[position];
            sum += ema1.data[position]  / ema2.data[position];
            position++;
        }

		field1.data[position - 1] = sum;

    }

	return field1;

}



// Historical Volatility Index (HVI)
field CTASDK::HistoricalVolatility( field* source, 
              int periods /*= 30*/, int barHistory /*=365*/, 
			  int standardDeviations /*=2*/, const string& name )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = name;

	field field2;	
	field2.data.resize(size);
	field2.name = "STDEV";

	field ret;

	if(barHistory < 0) return ret;
	if(periods < 1 || periods > size) return ret;    
    if(standardDeviations < 0 || standardDeviations > 100) return ret;

    const int start = 2;

	int record = 0;
    for(record = start; record != size; ++record)
		field1.data[record] = log10(source->data[record] / source->data[record - 1]);
	
	field2 = StandardDeviation(&field1, periods, standardDeviations, 1, "STDV");
		
    for(record = start; record != size; ++record)
		field1.data[record] = 100 * (field2.data[record] * sqrt((double)barHistory));

	return field1;

}


// Chaikin Money Flow (CMF)
field CTASDK::ChaikinMoneyFlow( recordset& ohlcv, int periods, const string& name )
{
	int size = GetSize(ohlcv);
	field field1;
	field1.data.resize(size);
	field1.name = name;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	field* volume = GetField("V", ohlcv);

	if(periods < 1 || periods >= size) return field1;

	double value = 0;
	double sum = 0, sumV = 0;
	double a = 0, b = 0;
    
	const int start = periods + 1;
	for(int record = start; record != size; ++record)
	{
        sum = 0;
        sumV = 0;

		for(int n = 0; n != periods; ++n)
		{
			if(high->data[record - n] != 0 && volume->data[record - n] != 0)
			{
                sum += ((close->data[record - n] - low->data[record - n]) - 
                (high->data[record - n] - close->data[record - n])) / 
                (high->data[record - n] - low->data[record - n]) * 
                volume->data[record - n];
                sumV += volume->data[record - n];
			}        
        }

		if(sumV != 0) value = (sum / sumV) * pow(sumV,2);
     
        field1.data[record] = value;
	}

	return field1;

}


// Commodity Channel Index (CCI)
field CTASDK::CommodityChannelIndex( recordset& ohlcv, int periods, int maType, const string& name )
{
	int size = GetSize(ohlcv);
	field field1;
	field1.data.resize(size);
	field1.name = name;

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);
	field* volume = GetField("V", ohlcv);

	if(periods < 1 || periods >= size) return field1;

    double dMeanDeviation = 0;
    double dTmp = 0;
    long count = 0;
        
    field tp = TypicalPrice(ohlcv);
	field ma = GetMAByType(&tp, periods, maType);
    
	const int start = (int)(1.5 * periods);
	for(int record = start; record != size; ++record)
	{
        dMeanDeviation = 0;
        for(count = (record - periods); count != record +1; ++count)
		{
            dTmp = fabs(tp.data[count] - ma.data[count]);
            dMeanDeviation += dTmp;
        }

        dMeanDeviation = dMeanDeviation / periods;
        if(dMeanDeviation != 0) dTmp = (tp.data[record] - ma.data[record]) / (dMeanDeviation * 0.015);
        field1.data[record] = dTmp;
    }

    return field1;

}


// Stochastic Momentum Index (SMI)
recordset CTASDK::StochasticMomentumIndex( recordset& ohlcv, int kPeriods, int kSmooth, 
										  int kDoubleSmooth, int dPeriods, int maType, int pctD_maType )
{
	field field1;
	int size = GetSize(ohlcv);
	field1.data.resize(size);
	field1.name = "K";

	field field2;	
	field2.data.resize(size);
	field2.name = "D";

	field* open = GetField("O", ohlcv);
	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);
	field* close = GetField("C", ohlcv);

	recordset ret;

	if(maType < 1) maType = 2; // EMA
	if(pctD_maType < 1) pctD_maType = 2; // EMA
	if(kPeriods < 1 || kPeriods > size) return ret;
	if(kSmooth < 1 || kSmooth > size) return ret;
	if(kDoubleSmooth < 1 || kDoubleSmooth > size) return ret;
	if(dPeriods < 1 || dPeriods > size) return ret;
	    
    kSmooth += 1;
	
	field temp = HHV(high, kPeriods, "HHV");
	field hhv = temp;

	temp = LLV(low, kPeriods, "LLV");
	field llv = temp;

	field hhll;
	hhll.data.resize(size);
	hhll.name = "HHLL";
	int record = 0;
    for(record = 1; record != size; ++record)
        hhll.data[record] = hhv.data[record] - llv.data[record];

    field chhll;
	chhll.data.resize(size);
	chhll.name = "CHHLL";
        for(record = 1; record != size; ++record)
			chhll.data[record] = close->data[record] - (0.5f * (hhv.data[record] + llv.data[record]));
    
	if(kSmooth > 1)	chhll = GetMAByType(&chhll, kSmooth, maType);
	if(kDoubleSmooth > 1) chhll = GetMAByType(&chhll, kDoubleSmooth, maType);

	if(kSmooth > 1)	hhll = GetMAByType(&hhll, kSmooth, maType);
	if(kDoubleSmooth > 1) hhll = GetMAByType(&hhll, kDoubleSmooth, maType);
		
    for(record = kPeriods + 1; record != size; ++record)
	{
		double a = chhll.data[record];
		double b = (0.5f * hhll.data[record]);
        if(b != 0) field1.data[record] = 100.0f * (a / b);         
    }
		
	if(dPeriods > 1) field2 = GetMAByType(&field1, dPeriods, pctD_maType, "D");

	ret.push_back(field1);
	ret.push_back(field2);

	return ret;

}










//////////////////////////////////////////////////////////////////////////////
//	Band functions
//
//////////////////////////////////////////////////////////////////////////////



// Keltner Channel (KC)
recordset CTASDK::KeltnerChannel( recordset& ohlcv, int periods, int maType, double multiplier )
{
	int size = GetSize(ohlcv);
	
	field field1;
	field1.data.resize(size);
	field1.name = "KC_TOP";

	field field2;	
	field2.data.resize(size);
	field2.name = "KC_BOTTOM";

	recordset ret;

    double sum = 0;
    double value = 0;
    
	if(maType < 0) return ret;
	if(periods < 1 || periods > size) return ret;    
    if(multiplier == 0) return ret;

	field atr = AverageTrueRange(ohlcv, periods, maType);

	field* close = GetField("C", ohlcv);
	field avg = GetMAByType(close, periods, maType, "KC_MEDIAN");

    const int start = periods + 1;
    int position = start;

    for(int record = start; record != size; ++record)
	{		
		double shift = multiplier * atr.data[position];
		field1.data[position] = avg.data[position] + shift;
		field2.data[position] = avg.data[position] - shift;
        
		position++;

	}

	ret.push_back(field1);
	ret.push_back(avg);
	ret.push_back(field2);

	return ret;

}


// Bollinger Bands (BB)
recordset CTASDK::BollingerBands( field* source, int periods, int standardDeviations, int maType )
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = "BB_TOP";

	field field2;	
	field2.data.resize(size);
	field2.name = "BB_BOTTOM";

	recordset ret;

    double sum = 0;
    double value = 0;
    
	if(maType < 0) return ret;
	if(periods < 1 || periods > size) return ret;    
    if(standardDeviations < 0 || standardDeviations > 100) return ret;

	field median = GetMAByType(source, periods, maType, "BB_MEDIAN");

    const int start = periods + 1;
    int position = start;

    for(int record = start; record != size; ++record)
	{

        sum = 0;

        value = median.data[position];
        for(int period = 1; period != periods + 1; ++period)
		{
            sum += (source->data[position] - value) * (source->data[position] - value);
            position--;
        }
		position += periods;        

        value = standardDeviations * sqrt(sum / periods);
        
		field1.data[position] = median.data[position] + value;
		field2.data[position] = median.data[position] - value;
        
		position++;

	}

	ret.push_back(field1);
	ret.push_back(median);
	ret.push_back(field2);

	return ret;

}


// Moving Average Envelope (MAE)
recordset CTASDK::MovingAverageEnvelope( field* source, int periods, int maType, double shift)
{
	field field1;
	int size = source->data.size();
	field1.data.resize(size);
	field1.name = "MAE_TOP";

	field field2;	
	field2.data.resize(size);
	field2.name = "MAE_BOTTOM";

	recordset ret;
	if(maType < 0) return ret;
	if(periods < 1 || periods > size) return ret;    
    if(shift < 0 || shift > 100) return ret;

	field temp = GetMAByType(source, periods, maType);
	
	shift = shift / 100;

    for(int record = 1; record != size; ++record)
	{
		field1.data[record] = temp.data[record] + (temp.data[record] * shift);
		field2.data[record] = temp.data[record] - (temp.data[record] * shift);
	}

	ret.push_back(field1);
	ret.push_back(field2);

	return ret;

}


// Moving Average Envelope (PNB)
recordset CTASDK::PrimeNumberBands( recordset& ohlcv )
{
	int size = GetSize(ohlcv);

	field* high = GetField("H", ohlcv);
	field* low = GetField("L", ohlcv);

	recordset ret;

	field ftop;
	ftop.name = "PNB_TOP";
	ftop.data.resize(size);

	field fbottom;
	fbottom.name = "PNB_BOTTOM";
	fbottom.data.resize(size);

	double value = 0;
    long top = 0, bottom = 0, n = 0;
    
	for(int record = 1; record != size; ++record)
	{    
    
        value = (long)(low->data[record]);
		if(value < 10) value = value * 10;
        
		for(n = (long)value; n != 1; --n)
		{        
            if(IsPrime(n)){
                bottom = n;
                break;
            }
        }
		fbottom.data[record] = (double)bottom;
		
		value = (long)high->data[record];
		if(value < 10) value = value * 10;

        for(n = (long)value; n != value * 2; ++n)
		{
            if(IsPrime(n))
			{
                top = n;
                break;
            }
        }
		ftop.data[record] = (double)top;
                
	}
    
	ret.push_back(ftop);
	ret.push_back(fbottom);

	return ret;

}










//////////////////////////////////////////////////////////////////////////////
//	Candlestick recognition
//
//////////////////////////////////////////////////////////////////////////////


// Original algorithm by Richard Gardner 4/4/2002
// Candlestick recognition last revised 5/2/05 by Richard Gardner
int CTASDK::CandleStickPattern( recordset& ohlcv )
{
	int size = GetSize(ohlcv);

	if(size <= 3) return 0; // requires 3 candles

	field* fjdate = GetField("D", ohlcv);
	field* fopen = GetField("O", ohlcv);
	field* fhigh = GetField("H", ohlcv);
	field* flow = GetField("L", ohlcv);
	field* fclose = GetField("C", ohlcv);
	field* fvolume = GetField("C", ohlcv);
	
	// Build the last three candlesticks (normalized) and identify the pattern
	int cnt = 2;
	int bar = 0;
	int start = 0;
	double open[3] = {0,0,0};
	double close[3] = {0,0,0};

	m_Candlesticks.resize(3);
	int n = 0;
	for(n = size - 1; n != size - 4; --n){		

		if(fclose->data[n] != 0 && 
		   fhigh->data[n] != 0 &&
		   flow->data[n] != 0 && 
		   fvolume->data[n] != 0){
					
			m_Candlesticks[cnt].jdateTime = fjdate->data[n];
			m_Candlesticks[cnt].bullBear = 0;
			m_Candlesticks[cnt].direction = 0;
			m_Candlesticks[cnt].pattern = 0;
			m_Candlesticks[cnt].openPrice = fopen->data[n];
			m_Candlesticks[cnt].highPrice = fhigh->data[n];
			m_Candlesticks[cnt].lowPrice = flow->data[n];
			m_Candlesticks[cnt].closePrice = fclose->data[n];
			m_Candlesticks[cnt].volume = fvolume->data[n];

			cnt--;
		}

	}
	

	// Get the normalized open and close prices
	for(n = 0; n != 3; ++n)
	{
		// normalized open
        open[n] = normalize(m_Candlesticks[n].highPrice, m_Candlesticks[n].lowPrice, m_Candlesticks[n].openPrice);
		// normalized close
        close[n] = normalize(m_Candlesticks[n].highPrice, m_Candlesticks[n].lowPrice, m_Candlesticks[n].closePrice);
	}


	// Get max and min candle height
    double Max = fabs(m_Candlesticks[0].highPrice - m_Candlesticks[0].lowPrice);
    double Min = fabs(m_Candlesticks[0].highPrice - m_Candlesticks[0].lowPrice);
	for(n = 0; n != 3; ++n)
	{
        if(fabs(m_Candlesticks[n].highPrice - m_Candlesticks[n].lowPrice) > Max)
			Max = fabs(m_Candlesticks[n].highPrice - m_Candlesticks[n].lowPrice);
		if(fabs(m_Candlesticks[n].highPrice - m_Candlesticks[n].lowPrice) < Min)
			Min = fabs(m_Candlesticks[n].highPrice - m_Candlesticks[n].lowPrice);
    }

	// Get the normalized values of High-Low (normalized height of candles)
	// in proportion to the max and min values from above:
	double wickHeight[3] = {0,0,0};
	for(n = 0; n != 3; ++n)
	{
        if(Max == Min)	
            wickHeight[n] = 1;
        else
            wickHeight[n] = normalize(Max, Min, fabs(m_Candlesticks[n].highPrice - m_Candlesticks[n].lowPrice));
    }

	// Get max and min actual prices
	Max = m_Candlesticks[0].highPrice;
	Min = m_Candlesticks[0].lowPrice;
	for(n = 1; n != 3; ++n)
	{
		if(m_Candlesticks[n].highPrice > Max) Max = m_Candlesticks[n].highPrice;
		if(m_Candlesticks[n].lowPrice < Min) Min = m_Candlesticks[n].lowPrice;
	}

	for(n = 0; n != 3; ++n)
	{
		m_Candlesticks[n].openPrice = normalize(Max, Min, m_Candlesticks[n].openPrice);
		m_Candlesticks[n].highPrice = normalize(Max, Min, m_Candlesticks[n].highPrice);
		m_Candlesticks[n].lowPrice = normalize(Max, Min, m_Candlesticks[n].lowPrice);
		m_Candlesticks[n].closePrice = normalize(Max, Min, m_Candlesticks[n].closePrice);
    }


	// OK, now we have the last three candles normalized between 0 and 1




	// Now, identify individual candles (the basic components for patterns)	
    for(n = 0; n != 3; ++n)
	{

        // Long body------------------------------------
		// If candle height > 50% and wick height > 40%
        if(fabs(open[n] - close[n]) > 0.5 && wickHeight[n] >= 0.4)
		{
		    m_Candlesticks[n].pattern = LONG_BODY;
            if(m_Candlesticks[n].closePrice > m_Candlesticks[n].openPrice)
                m_Candlesticks[n].bullBear = 1;
            else if(m_Candlesticks[n].closePrice < m_Candlesticks[n].openPrice)
                m_Candlesticks[n].bullBear = -1;
		}

        // Doji------------------------------------------
		// If candle body height < 5%
        else if(fabs(open[n] - close[n]) < 0.05)
		{
		    m_Candlesticks[n].pattern = DOJI;         
			m_Candlesticks[n].bullBear = 0;
		}

        // Hammer----------------------------------------
		// If candle height < 45% and either the open > 65% or close < 35%
        else if(fabs(open[n] - close[n]) < 0.45 && ((open[n] > 0.65) || (close[n] < 0.35)))
		{
            if(open[n] > 0.65) // If open > 65%
			{
                m_Candlesticks[n].pattern = HAMMER;
                if(m_Candlesticks[n].closePrice >= m_Candlesticks[n].openPrice)
                    m_Candlesticks[n].direction = 1;
				else
                    m_Candlesticks[n].direction = -1;

				// Hammer head is on top, so its bullish
                m_Candlesticks[n].bullBear = 1;
			}
            else if(close[n] < 0.35) // If close < 35%
			{
                m_Candlesticks[n].pattern = HAMMER;
                m_Candlesticks[n].bullBear = -1;
                if(m_Candlesticks[n].closePrice >= m_Candlesticks[n].openPrice)
                    m_Candlesticks[n].direction = 1;
                else
                    m_Candlesticks[n].direction = -1;

				// Hammer head is on bottom, so its bearish
                m_Candlesticks[n].bullBear = -1;
            }
		}

        // Star OR Harami Cross---------------------------------
		// If candle height < 66% and open < 80% and close > 20% then
		// this could be a Star OR a Harami Cross
        else if(fabs(open[n] - close[n]) < 0.66 && open[n] < 0.8 && close[n] > 0.2)
		{
            m_Candlesticks[n].pattern = HARAMI; // Default
            m_Candlesticks[n].bullBear = 0;
            
			// If the candle height < 10%
			if(fabs(open[n] - close[n]) < 0.1)
			{
                m_Candlesticks[n].pattern = HARAMI;
                m_Candlesticks[n].bullBear = 0;
			}
            else if(n > 0)
			{	
				// If open > previous high and close > previous high
				// (if candle body is above the previous high)
                if(m_Candlesticks[n].openPrice >= m_Candlesticks[n-1].highPrice &&
                    m_Candlesticks[n].closePrice >= m_Candlesticks[n-1].highPrice)
				{
                    m_Candlesticks[n].pattern = STAR;
                    m_Candlesticks[n].direction = 1;
				}
				// If open < previous low and close < previous low
				// (if candle body is below the previous low)
                else if(m_Candlesticks[n].openPrice <= m_Candlesticks[n-1].lowPrice &&
                    m_Candlesticks[n].closePrice <= m_Candlesticks[n-1].lowPrice)
				{
                    m_Candlesticks[n].pattern = STAR;
                    m_Candlesticks[n].direction = -1;
                }
            }
        }




	} // End of loop identifying individual bars






	// First, try to identify a pattern with 3 candles
    // (Morning/Evening Star). If this pattern doesn't exist
    // then look for patterns of 2 candles. If none
    // exist then look for a single candle pattern.
    
    // Morning Star-------------------------------------------------
    if(m_Candlesticks[0].pattern == LONG_BODY && m_Candlesticks[0].bullBear == -1 &&
        (m_Candlesticks[1].pattern == STAR || m_Candlesticks[1].pattern == DOJI_STAR ||
        m_Candlesticks[1].pattern == HAMMER) && m_Candlesticks[2].pattern == LONG_BODY &&
        m_Candlesticks[2].bullBear == 1)
	{
            m_Candlesticks[2].pattern = MORNING_STAR;
            m_Candlesticks[2].bullBear = 1;
	}


	// Evening Star---------------------------------------------------	
    else if(m_Candlesticks[0].pattern == LONG_BODY && m_Candlesticks[0].bullBear == 1 && 
        (m_Candlesticks[1].pattern == STAR || m_Candlesticks[1].pattern == DOJI_STAR || 
        m_Candlesticks[1].pattern == HAMMER) && 
		m_Candlesticks[2].pattern == LONG_BODY && m_Candlesticks[2].bullBear == -1)
	{
            m_Candlesticks[2].pattern = EVENING_STAR;
            m_Candlesticks[2].bullBear = -1;
	}



	// Piercing Line------------------------------------------------
	// 1st day is a long black body.
	// 2nd day is a white body which opens below the low of the 1st day.
	// 2nd day closes within, but above the midpoint of the 1st day's body.
    else if(m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == -1 && 
		m_Candlesticks[2].openPrice < m_Candlesticks[1].lowPrice &&
         m_Candlesticks[2].closePrice >= ((m_Candlesticks[1].highPrice + m_Candlesticks[1].lowPrice) / 2))
	{
     		m_Candlesticks[2].pattern = PIERCING_LINE;
            m_Candlesticks[2].bullBear = 1;
	}


    // Bullish Engulfing Line----------------------------------------
	// 1st day is black.
	// 2nd day's body engulfs the 1st day's body.
    else if(m_Candlesticks[2].closePrice > maxVal(m_Candlesticks[1].highPrice, m_Candlesticks[1].lowPrice) && 
         m_Candlesticks[2].openPrice < minVal(m_Candlesticks[1].lowPrice, m_Candlesticks[1].highPrice) && 
         m_Candlesticks[2].bullBear == 1 && m_Candlesticks[1].highPrice < m_Candlesticks[0].closePrice )
	{         
		m_Candlesticks[2].pattern = BULLISH_ENGULFING_LINE;
        m_Candlesticks[2].bullBear = 1;
	}


    // Bullish Doji Star---------------------------------------------
	// 1st day is a long black day. 
	// 2nd day is a doji day that gaps below the 1st day. 
	// The doji shadows shouldn't be excessively long.
    else if(m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == -1 && 
		 (m_Candlesticks[2].pattern == DOJI_STAR || m_Candlesticks[2].pattern == STAR) && 
		 m_Candlesticks[2].highPrice < m_Candlesticks[1].highPrice)
         
	{
		m_Candlesticks[2].pattern = DOJI_STAR;
        m_Candlesticks[2].bullBear = 1;		
    }


    // Hanging Man---------------------------------------------------
    else if(m_Candlesticks[2].pattern == HAMMER && m_Candlesticks[2].bullBear == 1 && 
		 m_Candlesticks[2].direction == 1 && 
		 m_Candlesticks[2].openPrice > m_Candlesticks[1].closePrice && 
		 m_Candlesticks[2].closePrice > m_Candlesticks[1].closePrice && 
		 m_Candlesticks[1].closePrice > m_Candlesticks[0].closePrice)
	{
		m_Candlesticks[2].pattern = HANGING_MAN;
        m_Candlesticks[2].bullBear = -1;
	}


    // Dark Cloud----------------------------------------------------
	// 1st day is a long white day. 
	// 2nd day is a black day which opens above the 1st day's high. 
	// 2nd day closes within the 1st day, but below the midpoint.
    else if(m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == 1 && 
		m_Candlesticks[2].closePrice < m_Candlesticks[2].openPrice && 
		m_Candlesticks[2].openPrice > m_Candlesticks[1].highPrice && 
		m_Candlesticks[2].closePrice <= m_Candlesticks[1].highPrice && 
		m_Candlesticks[2].closePrice >= m_Candlesticks[1].lowPrice && 
		m_Candlesticks[2].closePrice <= 
		((m_Candlesticks[1].highPrice + m_Candlesticks[1].lowPrice) / 2))		
	{
		m_Candlesticks[2].pattern = DARK_CLOUD_COVER;
        m_Candlesticks[2].bullBear = -1;
	}


    // Bearish Engulfing Line----------------------------------------
    // The color of the 1st day's body reflects the trend, however could be a doji. 
	// The 2nd day's real body engulfs the 1st day's body.
     else if(m_Candlesticks[1].closePrice > m_Candlesticks[1].openPrice && 
		 m_Candlesticks[1].closePrice > m_Candlesticks[0].highPrice && 
		 m_Candlesticks[2].openPrice > m_Candlesticks[1].closePrice && 
		 m_Candlesticks[2].closePrice < m_Candlesticks[1].openPrice && 
		 m_Candlesticks[1].lowPrice > m_Candlesticks[0].closePrice)

	{
		m_Candlesticks[2].pattern = BEARISH_ENGULFING_LINE;
        m_Candlesticks[2].bullBear = -1;	 
	}


    // Bearish Doji Star---------------------------------------------
	// 1st day is a long white day. 
	// 2nd day is a doji day that gaps above the 1st day. 
	// The doji shadows shouldn't be excessively long.
     else if(m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == 1 && 
		 m_Candlesticks[2].closePrice == m_Candlesticks[2].openPrice && m_Candlesticks[2].lowPrice > 
		 m_Candlesticks[1].highPrice && m_Candlesticks[1].lowPrice > m_Candlesticks[0].highPrice && 
		 m_Candlesticks[2].volume > 0)

	{
		m_Candlesticks[2].pattern = BEARISH_DOJI_STAR;
        m_Candlesticks[2].bullBear = -1;	 
	}

	
	 // Shooting Star-------------------------------------------------
	// Price gap open to the upside. 
	// Small real body formed near the bottom of the price range. 
	// The upper shadow at least three times as long as the body. 
	// The lower shadow is small or nonexistent.
     else if(m_Candlesticks[2].lowPrice > m_Candlesticks[1].highPrice && 
		 fabs(open[2] - close[2]) < 0.4 && m_Candlesticks[2].closePrice <= 
		 (m_Candlesticks[2].lowPrice + (m_Candlesticks[2].lowPrice * 0.03)) &&
		 m_Candlesticks[2].highPrice > m_Candlesticks[2].openPrice + 
		 (fabs(m_Candlesticks[2].openPrice - m_Candlesticks[2].closePrice)))

	{
		m_Candlesticks[2].pattern = BEARISH_SHOOTING_STAR;
        m_Candlesticks[2].bullBear = -1;
	}


	// Spinning Tops-------------------------------------------------
     else if(m_Candlesticks[1].pattern == STAR && m_Candlesticks[2].pattern == STAR)
	{
		m_Candlesticks[2].pattern = SPINNING_TOPS;
        m_Candlesticks[2].bullBear = 0;
	}


	// Harami Cross--------------------------------------------------
	// The 1st day is a long white day. 
	// The 2nd day is a doji day that is engulfed by the 1st day's body.
    else if(m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == 1 && 
		m_Candlesticks[2].pattern == DOJI && m_Candlesticks[2].highPrice < 
		m_Candlesticks[1].closePrice && m_Candlesticks[2].lowPrice > 
		m_Candlesticks[1].openPrice && m_Candlesticks[1].lowPrice > 
		m_Candlesticks[0].highPrice)
	{
		m_Candlesticks[2].pattern = HARAMI_CROSS;
        m_Candlesticks[2].bullBear = -1;
	}


	// Bullish TriStar--------------------------------------------------
	// All three days are doji days. 
	// 2nd day gaps below the 1st and 3rd days.
    else if(m_Candlesticks[0].pattern == DOJI && 
		m_Candlesticks[1].pattern == DOJI && 
		m_Candlesticks[2].pattern == DOJI &&
		m_Candlesticks[1].highPrice < m_Candlesticks[0].lowPrice && 
		m_Candlesticks[1].highPrice < m_Candlesticks[2].lowPrice)

	{
		m_Candlesticks[2].pattern = BULLISH_TRISTAR;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bullish Three White Soldiers--------------------------------------
	// Three consecutive long white days with higher closes each day. 
	// Each day opens within the previous body.
    else if(m_Candlesticks[0].pattern == LONG_BODY && m_Candlesticks[0].bullBear == 1 && 
		m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == 1 && 
		m_Candlesticks[2].pattern == LONG_BODY && m_Candlesticks[2].bullBear == 1 && 
		m_Candlesticks[0].closePrice < m_Candlesticks[1].closePrice && 
		m_Candlesticks[1].closePrice < m_Candlesticks[2].closePrice && 
		m_Candlesticks[1].openPrice > m_Candlesticks[0].openPrice && 
		m_Candlesticks[1].openPrice < m_Candlesticks[0].closePrice && 
		m_Candlesticks[2].openPrice > m_Candlesticks[1].openPrice && 
		m_Candlesticks[2].openPrice < m_Candlesticks[1].closePrice)
	{
		m_Candlesticks[2].pattern = THREE_WHITE_SOLDIERS;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bearish Three Black Crows--------------------------------------
	// Three consecutive black days with lower closes each day. 
	// Each day opens within the body of the previous day.
    else if(m_Candlesticks[0].pattern == LONG_BODY && m_Candlesticks[0].bullBear == -1 && 
		m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == -1 && 
		m_Candlesticks[2].pattern == LONG_BODY && m_Candlesticks[2].bullBear == -1 && 
		m_Candlesticks[1].openPrice < m_Candlesticks[0].openPrice && 
		m_Candlesticks[1].openPrice > m_Candlesticks[0].closePrice && 
		m_Candlesticks[2].openPrice < m_Candlesticks[1].openPrice && 
		m_Candlesticks[2].openPrice > m_Candlesticks[1].closePrice) 

	{
		m_Candlesticks[2].pattern = THREE_BLACK_CROWS;
        m_Candlesticks[2].bullBear = -1;
	}


	// Bullish Abandoned Baby----------------------------------------
	// 1st day is a black day. 
	// 2nd day is a doji whose shadows gaps below the 1st day's close. 
	// 3rd day is a white day with no overlapping shadows.
    else if(m_Candlesticks[0].closePrice < m_Candlesticks[0].openPrice && 		
		m_Candlesticks[1].pattern == DOJI && 
		m_Candlesticks[2].closePrice > m_Candlesticks[2].openPrice &&
		m_Candlesticks[1].highPrice < m_Candlesticks[0].lowPrice && 
		m_Candlesticks[1].highPrice < m_Candlesticks[2].lowPrice)

	{
		m_Candlesticks[2].pattern = ABANDONED_BABY;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bearish Abandoned Baby----------------------------------------
	else if(m_Candlesticks[0].closePrice > m_Candlesticks[0].openPrice && 		
		m_Candlesticks[1].pattern == DOJI && 
		m_Candlesticks[2].closePrice < m_Candlesticks[2].openPrice &&
		m_Candlesticks[1].lowPrice > m_Candlesticks[0].highPrice && 
		m_Candlesticks[1].lowPrice > m_Candlesticks[2].highPrice)

	{
		m_Candlesticks[2].pattern = ABANDONED_BABY;
        m_Candlesticks[2].bullBear = -1;
	}


	// Bullish Upside Gap  -------------------------------------------
	// 1st two day are long white days with a gap between them. 
	// 3rd day is a black day that fills the gap of the 1st two days.
    else if(m_Candlesticks[0].pattern == LONG_BODY && m_Candlesticks[0].bullBear == 1 && 
		m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == 1 && 
		m_Candlesticks[2].closePrice < m_Candlesticks[2].openPrice && 
		m_Candlesticks[2].lowPrice < m_Candlesticks[0].highPrice && 
		m_Candlesticks[1].lowPrice > m_Candlesticks[0].highPrice)
	{
		m_Candlesticks[2].pattern = BULLISH_UPSIDE_GAP;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bullish Hammer -------------------------------------------------
	// Small real body at the upper trading range. 
	// Color of the body is not important. 
	// Long lower shadow at least twice the length of the body. 
	// Little or no upper shadow. 
	// Previous trend should be bearish.
    else if(m_Candlesticks[2].pattern == HAMMER && m_Candlesticks[2].bullBear == 1 && 
		m_Candlesticks[2].direction == 1 && m_Candlesticks[2].closePrice < 
		m_Candlesticks[1].closePrice && m_Candlesticks[1].closePrice > 
		m_Candlesticks[0].closePrice && 
		fabs(m_Candlesticks[2].openPrice - m_Candlesticks[2].closePrice) < 
		fabs(m_Candlesticks[2].highPrice - m_Candlesticks[2].lowPrice) / 8)
	{
		m_Candlesticks[2].pattern = BULLISH_HAMMER;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bullish Kicking--------------------------------------------------
	// 1st day is a black marubozu. 
	// 2nd day is a white marubozu and gaps open above the 1st day's close.
	else if(m_Candlesticks[1].highPrice == m_Candlesticks[1].openPrice && 
		m_Candlesticks[1].closePrice == m_Candlesticks[1].lowPrice && 
		m_Candlesticks[2].closePrice == m_Candlesticks[2].highPrice && 
		m_Candlesticks[2].openPrice == m_Candlesticks[2].lowPrice && 
		m_Candlesticks[2].lowPrice > m_Candlesticks[1].highPrice && 
		m_Candlesticks[2].lowPrice != m_Candlesticks[2].highPrice && 
		m_Candlesticks[1].lowPrice != m_Candlesticks[1].highPrice && 
		m_Candlesticks[2].volume > 0 && m_Candlesticks[1].volume > 1000)
	
	{
		m_Candlesticks[2].pattern = BULLISH_KICKING;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bearish Kicking--------------------------------------------------
	// 1st day is a white marubozu. 
	// 2nd day is a black marubozu and gaps open below the 1st day's open.
	else if(m_Candlesticks[1].highPrice == m_Candlesticks[1].closePrice && 
		m_Candlesticks[1].openPrice == m_Candlesticks[1].lowPrice && 
		m_Candlesticks[2].closePrice == m_Candlesticks[2].lowPrice && 
		m_Candlesticks[2].openPrice == m_Candlesticks[2].highPrice && 
		m_Candlesticks[2].highPrice < m_Candlesticks[1].lowPrice && 
		m_Candlesticks[2].lowPrice != m_Candlesticks[2].highPrice && 
		m_Candlesticks[1].lowPrice != m_Candlesticks[1].highPrice && 
		m_Candlesticks[2].volume > 0 && m_Candlesticks[1].volume > 1000)
	
	{
		m_Candlesticks[2].pattern = BEARISH_KICKING;
        m_Candlesticks[2].bullBear = -1;
	}

	
	// Bearish Belt Hold--------------------------------------------------
	// Long black day where the open is equal to the high. 
	// No upper shadow.
	else if(m_Candlesticks[2].pattern == LONG_BODY && m_Candlesticks[2].bullBear == -1 && 
		m_Candlesticks[2].openPrice == m_Candlesticks[2].highPrice && 
		m_Candlesticks[2].closePrice < m_Candlesticks[1].lowPrice && 
		m_Candlesticks[1].closePrice < m_Candlesticks[0].lowPrice)
	
	{
		m_Candlesticks[2].pattern = BEARISH_BELT_HOLD;
        m_Candlesticks[2].bullBear = -1;
	}


	// Bullish Belt Hold--------------------------------------------------
	// Long white day where the close is equal to the high. 
	// No lower shadow.
	else if(m_Candlesticks[2].pattern == LONG_BODY && m_Candlesticks[2].bullBear == 1 && 
		m_Candlesticks[2].closePrice == m_Candlesticks[2].highPrice && 
		m_Candlesticks[2].lowPrice > m_Candlesticks[1].highPrice && 
		m_Candlesticks[1].closePrice > m_Candlesticks[0].highPrice)
	
	{
		m_Candlesticks[2].pattern = BULLISH_BELT_HOLD;
        m_Candlesticks[2].bullBear = 1;
	}


	// Bearish Two Crows--------------------------------------------------
	// 1st day is a long white day.
	// 2nd day gaps up and is black.
	// 3rd day is black and opens inside the body of the 2nd day, then closes inside the body of the 1st day.
	else if(m_Candlesticks[0].pattern == LONG_BODY && m_Candlesticks[0].bullBear == 1 && 
		m_Candlesticks[1].openPrice > m_Candlesticks[0].highPrice && 
		m_Candlesticks[1].closePrice < m_Candlesticks[1].openPrice && 
		m_Candlesticks[2].closePrice < m_Candlesticks[2].openPrice && 
		m_Candlesticks[2].openPrice > m_Candlesticks[1].closePrice && 
		m_Candlesticks[2].openPrice < m_Candlesticks[1].openPrice && 
		m_Candlesticks[2].closePrice < m_Candlesticks[0].closePrice && 
		m_Candlesticks[2].closePrice > m_Candlesticks[0].openPrice)	
	{
		m_Candlesticks[2].pattern = BEARISH_TWO_CROWS;
        m_Candlesticks[2].bullBear = -1;
	}



	// Bullish Matching Low--------------------------------------------------
	// 1st day is a long black day. 
	// 2nd day is a black day with a close equal to the 1st day.
	else if(m_Candlesticks[1].pattern == LONG_BODY && m_Candlesticks[1].bullBear == -1 && 
		m_Candlesticks[2].closePrice < m_Candlesticks[2].openPrice && 
		m_Candlesticks[2].closePrice == m_Candlesticks[1].closePrice)		
	{
		m_Candlesticks[2].pattern = BULLISH_MATCHING_LOW;
        m_Candlesticks[2].bullBear = 1;
	}


	// No multi-bar pattern found
	else
	{
		if(m_Candlesticks[0].pattern != 0)
		{
			m_Candlesticks[2].pattern = m_Candlesticks[0].pattern;
			m_Candlesticks[2].bullBear = m_Candlesticks[0].bullBear;
		}
		else if(m_Candlesticks[1].pattern != 0)
		{
			m_Candlesticks[2].pattern = m_Candlesticks[1].pattern;
			m_Candlesticks[2].bullBear = m_Candlesticks[1].bullBear;
		}
		else
		{
			m_Candlesticks[2].pattern = 0;
			m_Candlesticks[2].bullBear = 0;
		}
	}


	return m_Candlesticks[2].pattern;	

}