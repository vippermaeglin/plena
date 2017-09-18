// SerializerX.h: interface for the CSerializerX class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SERIALIZERX_H__FDA375EF_16E6_44D1_BE76_5DBC27331579__INCLUDED_)
#define AFX_SERIALIZERX_H__FDA375EF_16E6_44D1_BE76_5DBC27331579__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "IOStructures.h"

class CStockChartXSerializer;
class CStockChartXCtrl;

#define SIGN_TMPL_GENERAL     0x0001
#define SIGN_TMPL_OBJECTS     0x0002

class CSerializerX  
{
public:
	CSerializerX(CStockChartXCtrl &ChartControl);
	virtual ~CSerializerX();

  long Save(LPCTSTR FileName,CStockChartXSerializer *Saver);
  long Load(LPCTSTR FileName,CStockChartXSerializer *Loader);

  void Write (const void* lpBuf, UINT nCount);
  UINT Read (void* lpBuf, UINT nCount);

  template<class T>
  friend CSerializerX& operator << (CSerializerX &Serializer,const T &data);
  template<class T>
  friend CSerializerX& operator >> (CSerializerX &Serializer,T &data);
private:
  CStockChartXCtrl &m_ChartControl;
  CFile            *m_file;
};

class CStockChartXSerializer{
public:
  CStockChartXSerializer(CStockChartXCtrl &ChartControl,CSerializerX &Serializer);

  virtual long Save();
  virtual long Load();
protected:
  //data members
  CStockChartXCtrl  &m_ChartControl;
  CSerializerX      &m_Serializer;

  IOWorkspace	m_workspace;
  int               m_iVersion;
  int               m_iPanelsCount;
  int               m_iPanelCountLeft;//how many panels(wchi have data series) are left after after ClearChart

  int m_startIndex;
  int m_endIndex;

protected:
  //saving methods
  virtual void SaveSignature();
  virtual void SaveVersion();
  virtual void SaveWorkSpace();
  virtual void SaveChartPanels();
  virtual void SavePanelsSeries(const IOPanel &ioPanel,const CChartPanel&	panel,int iPanelIdx);
  virtual void SaveTextAreas(const CChartPanel&	panel);
  virtual void SaveObjects(const CChartPanel&	panel);
  virtual void SaveLineObjects(const CChartPanel&	panel);
  virtual void SaveHorizontalLines(const CChartPanel&	panel);
  virtual void SaveSeriesData(const IOSeries &ioSeries,const CSeries &series);
  virtual void SavePanelIndex(int iPanelIdx);//needed by GeneralTemplate

  //loading methods
  virtual void ReadSignature();
  virtual void ClearChart();
  virtual void ReadVersion();
  virtual void ReadWorkSpace();
  virtual void ReadChartPanels();
  virtual CChartPanel* ReadPanelsSeries(const IOPanel &ioPanel);
  virtual void ReadTextAreas(CChartPanel &panel);
  virtual void ReadObjects(CChartPanel &panel);
  virtual void ReadLineStudyObjects(CChartPanel &panel);
  virtual void ReadHorizontalLines(CChartPanel &panel);
  virtual void UpdateScreen(bool SetStartEndIndex = true);
  virtual void ReadSeriesData(const IOSeries &ioSeries,CSeries &series);
  virtual CChartPanel* ReadPanelIndex();//needed by GeneralTemplate

  //some common methods
  virtual long GetPanelsCount();
  virtual bool IgnorePanel(const CChartPanel &panel);
  virtual bool IgnoreSeries(const CSeries &series);
  virtual CSeries* AddSeries(const IOSeries &ioSeries,int iPanelIdx);
  virtual long GetSeriesCount(const CChartPanel &panel);
};

class CStockChartXSerializer_All : public CStockChartXSerializer{
public:
  CStockChartXSerializer_All(CStockChartXCtrl &ChartControl,CSerializerX &Serializer)
    : CStockChartXSerializer( ChartControl, Serializer ){
    }

  virtual long Save();
  virtual long Load();
protected:
};

class CStockChartXSerializer_General : public CStockChartXSerializer{
public:
  CStockChartXSerializer_General(CStockChartXCtrl &ChartControl,CSerializerX &Serializer)
    : CStockChartXSerializer( ChartControl, Serializer ){
  }

  virtual long Save();
  virtual long Load(bool Temp=false);
protected:
  virtual void SaveSignature();
  virtual void SaveSeriesData(const IOSeries &ioSeries,const CSeries &series);
  virtual void SavePanelIndex(int iPanelIdx);
  virtual void SaveTextAreas(const CChartPanel&	panel);
  virtual void SaveObjects(const CChartPanel&	panel);
  virtual void SaveLineObjects(const CChartPanel&	panel);
  virtual void SaveHorizontalLines(const CChartPanel&	panel);

  virtual void ClearChart();
  virtual void ReadWorkSpace(bool Temp=false);
  virtual void ReadSignature();
  virtual void ReadChartPanels();
  virtual void ReadTextAreas(CChartPanel &panel);
  virtual void ReadObjects(CChartPanel &panel);
  virtual void ReadLineStudyObjects(CChartPanel &panel);
  virtual void ReadHorizontalLines(CChartPanel &panel);
  virtual void ReadSeriesData(const IOSeries &ioSeries,CSeries &series);
  virtual CChartPanel* ReadPanelIndex();

  virtual void UpdateScreen(bool SetStartEndIndex = true);
  virtual long GetPanelsCount();
  virtual bool IgnorePanel(const CChartPanel &panel);
  virtual bool IgnoreSeries(const CSeries &series);
  virtual CSeries* AddSeries(const IOSeries &ioSeries,int iPanelIdx);
  virtual long GetSeriesCount(const CChartPanel &panel);
};

class CStockChartXSerializer_Objects : public CStockChartXSerializer{
public:
  CStockChartXSerializer_Objects(CStockChartXCtrl &ChartControl,CSerializerX &Serializer)
    : CStockChartXSerializer( ChartControl, Serializer ){
  }

  virtual long Save();
  virtual long Load();
protected:
  virtual void SaveSignature();

  virtual void UpdateScreen(bool SetStartEndIndex = true);
  virtual void ReadChartPanels();
  virtual void ReadSignature();
public:
	void SaveStudyControl(void);
	void LoadStudyControl(void);
};


#endif // !defined(AFX_SERIALIZERX_H__FDA375EF_16E6_44D1_BE76_5DBC27331579__INCLUDED_)
