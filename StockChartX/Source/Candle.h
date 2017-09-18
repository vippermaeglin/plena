#if !defined(CANDLE_H)
#define CANDLE_H

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class Candle
{
private:
	double open;
	double close;
	double max;
	double min;
	
public:
	Candle(void);
	Candle(double p_open, double p_close, double p_max, double p_min);
	~Candle(void);
	double GetOpen();
	double GetClose();
	double GetMax();
	double GetMin();
	
	void SetOpen(double);
	void SetClose(double);
	void SetMax(double);
	void SetMin(double);
	void PrintCandle();
};

#endif