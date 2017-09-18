#include "StdAfx.h"
#include ".\backteststatistics.h"

#define CONSOLE_DEBUG

CBackTestStatistics::CBackTestStatistics(void)
{
	m_avgTradesPerMonth = 0;
}

CBackTestStatistics::~CBackTestStatistics(void)
{
}




/*
 * For Monte Carlo simulations
 */
vector< double > m_shuffle;

__inline void CBackTestStatistics::swap(int i, int j) { 
    double x = m_shuffle[i];
    m_shuffle[i] = m_shuffle[j]; 
    m_shuffle[j] = x;    
} 

__inline void CBackTestStatistics::shuffle(void) { 
    int left, right; 
    for (left = 0; left < (int)m_shuffle.size() - 1; ++left) { 
        right = left + (rand() / (RAND_MAX / ((int)m_shuffle.size() - left - 1))); 

       if (left != right) 
          swap(left, right); 
	}
}

/*
 * Value Added Monthly Index (VAMI)
 */
double CBackTestStatistics::ValueAddedMonthlyIndex( vector<double> monthlyPL )
{
	double returnValue = 1000;
	int size = (int)monthlyPL.size();
	for( int i = 0; i < size; i++)
		returnValue += (1 + monthlyPL[i]) * returnValue;
	return returnValue;
}

/*
 * Compound (Geometric) Monthly ROR
 */
double CBackTestStatistics::CompoundMonthlyROR( vector<double> monthlyPL  )
{
	int size = (int)monthlyPL.size();
	if (size < 1) return 0;
	double returnValue = ValueAddedMonthlyIndex(monthlyPL) / 1000;
	returnValue = pow( returnValue, (1.0 / size)) - 1;
	return returnValue;
}

/*
 * Compound Quarterly ROR
 */
double CBackTestStatistics::CompoundQuarterlyROR( vector<double> monthlyPL )
{
	int size = (int)monthlyPL.size();
	if (size < 1) return 0;
	double returnValue = 1 + CompoundMonthlyROR(monthlyPL);
	returnValue = pow( returnValue, 3) - 1;
	return returnValue;
}

/*
 * Compound Annualized ROR
 */
double CBackTestStatistics::CompoundAnnualizedROR( vector<double> monthlyPL  )
{
	int size = (int)monthlyPL.size();
	if (size < 1) return 0;
	double returnValue = 1 + CompoundMonthlyROR(monthlyPL);
	returnValue = pow(returnValue, size) - 1;
	return returnValue;
}

/*
 * Monthly Standard Deviation
 */
double CBackTestStatistics::StandardDeviation( vector<double> monthlyPL  )
{
	int size = (int)monthlyPL.size();
	if (size < 2) return 0;
    double returnValue = 0;
	double mean = 0;
	int i = 0;
	for (i = 0; i < size; i++)
		mean += monthlyPL[i];
	for (i = 0; i < size; i++)
		returnValue += pow( monthlyPL[i] - mean, 2);
	returnValue /= size - 1;
	returnValue = pow( returnValue, 0.5);
	return returnValue;
}

/*
 *  Annualized Standard Deviation
 */
double CBackTestStatistics::AnnualizedStandardDeviation( vector<double> monthlyPL  )
{
	int size = (int)monthlyPL.size();
	double returnValue = StandardDeviation(monthlyPL) * pow( (double)size, (double)0.5);
	return returnValue;
}

/*
 * 10% DownsideDeviation
 */
double CBackTestStatistics::DownsideDeviation( vector<double> monthlyPL, double MinimumAcceptanceReturn )
{
	int size = (int)monthlyPL.size();
	double returnValue = 0;
	if (size < 1) return 0;
	for (int i = 0; i < size; i++)
	{
		if (monthlyPL[i] < MinimumAcceptanceReturn)
			returnValue += pow( monthlyPL[i] - MinimumAcceptanceReturn, 2);
	}
	returnValue /= size;
	returnValue = pow( (double)returnValue, (double)0.5);
	return returnValue;
}

/*
 * Sharpe Ratio 0.5
 */
double CBackTestStatistics::SharpeRatio( vector<double> monthlyPL, double periodRiskFreeReturn )
{
	int size = (int)monthlyPL.size();
	double returnValue = 0;
	if (size < 2) return 0;
  	double mean = 0;
	for (int i = 0; i < size; i++)
		mean += monthlyPL[i];
	double standardDeviation = StandardDeviation(monthlyPL );
	if (standardDeviation > 0)
		returnValue = (mean - periodRiskFreeReturn) / standardDeviation;
	return returnValue;
}

/*
 * Annualized Sharpe Ratio 0.5
 */
double CBackTestStatistics::AnnualizedSharpeRatio( vector<double> monthlyPL , double periodRiskFreeReturn  )
{
	int size = (int)monthlyPL.size();
	double returnValue = SharpeRatio(monthlyPL, periodRiskFreeReturn);
	returnValue *= pow((double)size, (double)0.5);
	return returnValue;
}

/*
 * Sortino Ratio
 */
double CBackTestStatistics::SortinoRatio( vector<double> monthlyPL, double MinimumAcceptanceReturn )
{
	double returnValue = 0;

	int size = (int)monthlyPL.size();
	double DDMAR = 0;
	for (int i = 0; i < size; i++)
	{
		if (monthlyPL[i] < MinimumAcceptanceReturn)
			DDMAR += pow( monthlyPL[i] - MinimumAcceptanceReturn, 2);
	}
	if (size > 0)
	{
		DDMAR /= size;
		DDMAR = pow( (double)DDMAR, (double)0.5);
	}

	if (DDMAR > 0)
		returnValue = (CompoundMonthlyROR(monthlyPL) - MinimumAcceptanceReturn) / DDMAR;

	return returnValue;
}

/*
 * Annualized Sortino Ratio
 */
double CBackTestStatistics::AnnualizedSortinoRatio( vector<double> monthlyPL, double MinimumAcceptanceReturn  )
{
	int size = (int)monthlyPL.size();
	double returnValue = SortinoRatio(monthlyPL, MinimumAcceptanceReturn);
	returnValue *= pow( (double)size, (double)0.5);
	return returnValue;
}

/*
 * Maximum drawdown 
 * You can use trade-to-trade values to get a dollar drawdown
 * or you can use month-to-month PL% values to get a percentage.
 */
double CBackTestStatistics::MaximumDrawdown( vector<double> values )
{	
	int size = (int)values.size();
	double mdd = 0;
	double sum = 0;
	for (int i = 0; i < size; i++)
	{
		sum += values[i];
		if(sum < mdd) mdd = sum;
	}
	return mdd;
}

/*
 * Maximum drawdown with Monte Carlo simulation
 * Same as above, but shuffles trades 10000 times 
 */
double CBackTestStatistics::MaximumDrawdownMonteCarlo( vector<double> values )
{
	int size = (int)values.size();

	m_shuffle.clear();
	m_shuffle.resize(size);
	vector<double> mmd;

	for(int n = 0; n < 10000; ++n)
	{
		
		for(int j = 0; j < size; ++j)
			m_shuffle[j] = values[j];

		shuffle();

		double sum = 0;
		double val = 0;
		for (int i = 0; i < size; i++)
		{
			sum += m_shuffle[i];
			if(sum < val) val = sum;
		}
		mmd.push_back(val);
	}

	double min = 0;
	for(int n = 0; n < 10000; ++n)
		if(mmd[n] < min) min = mmd[n];

	return min;
}

/*
 * Calmar Ratio
 */
double CBackTestStatistics::CalmarRatio( vector<double> monthlyPL )
{
	int size = (int)monthlyPL.size();
	double  max = fabs(MaximumDrawdown(monthlyPL));
	if (max == 0) return 0;
	double returnValue = CompoundAnnualizedROR(monthlyPL ) / max;
	if(returnValue > 1000000000000000 || returnValue < -1000000000000000) returnValue = 0;
	return returnValue;
}

/*
 * Sterling Ratio
 */
double CBackTestStatistics::SterlingRatio( vector<double> monthlyPL, double MinimumAcceptanceReturn )
{
	int size = (int)monthlyPL.size();
	double monthlyROR = CompoundMonthlyROR(monthlyPL);
	double returnValue = 0;

	double DDMAR = 0;
	for (int i = 0; i < size; i++)
	{
		if (monthlyPL[i] < MinimumAcceptanceReturn)
			DDMAR += pow( monthlyPL[i] - MinimumAcceptanceReturn, 2);
	}
	if (size > 0)
	{
		DDMAR /= size;
		DDMAR = pow( (double)DDMAR, (double)0.5);
	}

	if (DDMAR > 0)
		returnValue = (monthlyROR - MinimumAcceptanceReturn) / DDMAR;

	return returnValue;
}





//int main(int argc, char* argv[])
//{
	
	//double monthlyPL[6] = {-0.075, 0.15,0.12,-1,0.8,0.66};

	//double r = MaximumDrawdownMonteCarlo(monthlyPL, 5);
	
 	//printf("Hello World!\n");
	//return 0;
//}

/*

	How to use these functions:

	First, normalize all individual trades to a percentage:
	Bought at 24, sold at 23
	Loss = 1.04%
	Sold at 23, bought at 24
	Profit = 1.04%

	So now we have an array of values that looks like:
	trades[0] = 1.04
	trades[1] = -1.04
	
	Calculate MaxDrawDown, Sharpe, an DownsideDeviation using those values.

	Next, IF there are more than 3 month's worth of values, that is if:
	trades[end].jdate - trades[0].jdate >= (JMONTH * 3)
	then do this:

	Create a new array with monthly values:

	monthlyPL is the sum of all values between each JMONTH.

	Finally, calculate the rest of the functions using those values.

*/




/*
 * Convert
 * Converts from daily periods to monthly periods
 */
vector<double> CBackTestStatistics::ConvertToMonthly( vector<double> trades, vector<double> dates )
{
	// How many months are between the start and end date?
	double months = 1 + floor((dates[dates.size()-1] - dates[0]) / JMONTH);
	vector<double> monthly((size_t)months);	
	int size = (int)trades.size();
	double month = 0;
	m_avgTradesPerMonth = 0;
	for(int n = 0; n < size; ++n)
	{
		// Which month does this trade fit into?
		month = (dates[n] - dates[0]) / JMONTH;
		if((int)month < (int)monthly.size())
		{
			monthly[(int)month] += trades[n];
			m_avgTradesPerMonth++;
		}
	}
	m_avgTradesPerMonth /= (int)months;
	m_avgTradesPerMonth /= 2;
	return monthly;
}
