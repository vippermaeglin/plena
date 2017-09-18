//***************************************************************************
//								STOCKCHART
//***************************************************************************

//1) Criar classes no StockChart:

	"StockChartX\Header\IndicatorVolume.h"
	"StockChartX\Source Files\IndicatorVolume.cpp"
	
//2) Definir métodos da classe:
	
<FILE: IndicatorVolume.h | IndicatorVolume.cpp>
	
	#include "Indicator.h"
	class CIndicatorVolume : public CIndicator  
	{
	public:
		void SetParamInfo();
		CIndicatorVolume(LPCTSTR name, int type, int members, CChartPanel* owner);
		virtual ~CIndicatorVolume();
		BOOL Calculate();
	};
	
//3) Adicionar tipo do indicador em StockChartX:

<FILE: StockChartX.odl>
	
	enum eIndicator
	{
		(...)
		[helpstring("Volume Indicator")]	indVolume,
		[hidden] LastIndicator
	}
	Indicator;
	
//4) Definir a classe do indicador: 

	"Bands | General | Index | LinearRegression | Oscillator | MovingAverage | Custom Indicators"
	
	Volume = General

//5) Adicionar protótipo e tipo do indicador em StdAfx:

<FILE: StdAfx.h>
	
	// CGeneral
	#include "IndicatorVolume.h"
	
	(...)
	
	enum eIndicator
	{
		(...)
		indVolume	= (...) + 1,
		LastIndicator	= indVolume + 1
	}INDICATORS;
	
//6) Acrescentar o Recordset no cabeçalho da classe escolhida:
	
<FILE: CGeneral.h>
	
	class CGeneral  
	{
	public:
		CGeneral();
		virtual ~CGeneral();
		(....)
		CRecordset* Volume(CNavigator* pNav, CField* Volume, int Periods, LPCTSTR Alias = "Volume");
	}

//7) Implementar o Recordset no fonte da classe escolhida:
	
<FILE: CGeneral.cpp>
	
	CRecordset* CGeneral::Volume(CNavigator* pNav, CField* Volume, int Periods, LPCTSTR Alias)
	{
		(...)
	}

//8) Adicionar indicador no método externo de inserção:

<FILE: StockChartXCtl.cpp>

	BOOL CStockChartXCtrl::AddIndicatorSeries(long IndicatorType, LPCTSTR Key, long Panel, BOOL UserParams) 
	{
		(...)	
		switch(IndicatorType){		
			(...)
			case  indVolume:
				panels[Panel]->series.push_back(new CIndicatorVolume(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
				valid = true;
				break;
		}
	}

//9) Criar métodos externos para Inserir e Atualizar indicador:

<FILE: StockChartX.cpp>
	
	VARIANT_BOOL CStockChartXCtrl::AddIndicatorVolume(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
	{
		(...)
	}

	VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVolume(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
	{
		(...)
	}
	



//***************************************************************************
//								PLENA M4
//***************************************************************************

//1) Adicionar indicador na estrutura do MOCK:

<FILE: Mock.cs>
	
	public void LoadIndicator()
    {
            Indicators = new List<Indicator>
                            {
								(...)
								new Indicator
								{
									 Code = "VOL",
									 Description = "Volume",
									 IndicatorsMocks = new List<IndicatorMock>(),
									 Window = Enums.Window.NewWindow,
                                }
							}
	}
	
	//2) Adicionar os métodos de inserção, atualização, delete em FrmSelectIndicator:
	
	<FILE: FrmSelectIndicator.cs>
		
			(...)