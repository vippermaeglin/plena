#include "stdafx.h"
#include "Candle.h"


Candle::Candle(void)
{
}

Candle::Candle(double p_open, double p_close, double p_max, double p_min)
{
	open = p_open;
	close = p_close;
	max = p_max;
	min = p_min;
}

Candle::~Candle(void)
{
}

double Candle::GetOpen(){
	return open;
}
double Candle::GetClose(){
	return close;
}
double Candle::GetMax(){
	return max;
}
double Candle::GetMin(){
	return min;
}

void Candle::SetOpen(double p_open){
	open = p_open;
}
void Candle::SetClose(double p_close){
	close = p_close;
}
void Candle::SetMax(double p_max){
	max = p_max;
}
void Candle::SetMin(double p_min){
	min = p_min;
}

void Candle::PrintCandle(){
	printf("Open, Close, Max, Min (%f, %f, %f %f) \n", open, close, max, min);
}