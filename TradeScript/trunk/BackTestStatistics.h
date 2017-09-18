#pragma once

class CBackTestStatistics
{
public:
	CBackTestStatistics(void);
	~CBackTestStatistics(void);

	double ValueAddedMonthlyIndex( vector<double> monthlyPL );
	double CompoundMonthlyROR( vector<double> monthlyPL );
	double CompoundQuarterlyROR( vector<double> monthlyPL );
	double CompoundAnnualizedROR( vector<double> monthlyPL );
	double StandardDeviation( vector<double> monthlyPL );
	double AnnualizedStandardDeviation( vector<double> monthlyPL );
	double DownsideDeviation( vector<double> monthlyPL, double MinimumAcceptanceReturn );
	double SharpeRatio( vector<double> monthlyPL, double periodRiskFreeReturn );
	double AnnualizedSharpeRatio( vector<double> monthlyPL, double periodRiskFreeReturn  );
	double SortinoRatio( vector<double> monthlyPL, double MinimumAcceptanceReturn );
	double AnnualizedSortinoRatio( vector<double> monthlyPL, double MinimumAcceptanceReturn  );
	double MaximumDrawdown( vector<double> values );
	double MaximumDrawdownMonteCarlo( vector<double> values );
	double CalmarRatio( vector<double> monthlyPL );
	double SterlingRatio( vector<double> monthlyPL, double MinimumAcceptanceReturn );
	vector<double> ConvertToMonthly( vector<double> trades, vector<double> dates );
	int m_avgTradesPerMonth;

private:
	vector <double> m_shuffle;
	__inline void CBackTestStatistics::swap(int i, int j);
	__inline void CBackTestStatistics::shuffle(void);

};