// SerializerX.cpp: implementation of the CSerializerX class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "SerializerX.h"
#include "StockChartXCtl.h"

#include "memory" //for auto_ptr

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CSerializerX::CSerializerX(CStockChartXCtrl &ChartControl)
  : m_ChartControl( ChartControl )
{

}

CSerializerX::~CSerializerX()
{

}

long CSerializerX::Save(LPCTSTR FileName,CStockChartXSerializer *Saver)
{
  try
  {
	  
#ifdef _CONSOLE_DEBUG
  printf("\nCSerializerX::Save() FileName=%s",FileName);
#endif
    std::auto_ptr<CFile> ptrFile( new CFile( FileName, CFile::modeWrite | CFile::modeCreate ) );
    m_file = ptrFile.get();
	
    Saver->Save();

    m_file->Close();//useless
  }
  catch( CFileException* )
  {
    CString	msg;
    msg.Format( "IO error saving file %s", (const TCHAR*)FileName );
    m_ChartControl.ThrowErr(1010, msg);
    return -1;
  }
  return 1;
}

long CSerializerX::Load(LPCTSTR FileName,CStockChartXSerializer *Loader)
{
 try
  {
	  
#ifdef _CONSOLE_DEBUG
  printf("\nCSerializerX::Load()");
#endif
    std::auto_ptr<CFile> ptrFile( new CFile( FileName, CFile::modeRead ) );
    m_file = ptrFile.get();
	CString cFileName = FileName;
	if(cFileName.Find(".sct",0)>0){
		if(cFileName.Find("TempTemplate",0)>0){
			((CStockChartXSerializer_General*)Loader)->Load(true);
		}
		else ((CStockChartXSerializer_General*)Loader)->Load();
	}
	else Loader->Load();
    m_file->Close();//useless
  }
  catch( CFileException* )
  {
    /*CString	msg;
    msg.Format( "IO error loading file %s", (const TCHAR*)FileName );
    m_ChartControl.ThrowErr(1010, msg);*/
    return -1;
  }
  return 1;
}

void CSerializerX::Write(const void* lpBuf, UINT nCount)
{
  m_file->Write( lpBuf, nCount );
}

UINT CSerializerX::Read(void* lpBuf, UINT nCount)
{
  return m_file->Read( lpBuf, nCount );
}

template<class T>
CSerializerX& operator << (CSerializerX &Serializer,const T &data)
{
  Serializer.m_file->Write( &data, sizeof( T ) );
  return Serializer;
}

template<class T>
CSerializerX& operator >> (CSerializerX &Serializer,T &data)
{
  Serializer.m_file->Read( &data, sizeof( T ) );
  return Serializer;
}
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
CStockChartXSerializer::CStockChartXSerializer(CStockChartXCtrl &ChartControl,CSerializerX &Serializer)
  : m_ChartControl( ChartControl ), m_Serializer( Serializer )
{
}

long CStockChartXSerializer::Save()
{
#ifdef _CONSOLE_DEBUG
	printf("\nStockChartXSerializer::Save()");
	//system("pause");
#endif
 SaveSignature();
  SaveVersion();
  SaveWorkSpace();
  SaveChartPanels();

  return 1;
}

long CStockChartXSerializer::Load()
{
	
m_startIndex = m_ChartControl.startIndex;
  m_endIndex = m_ChartControl.endIndex;

  ClearChart();
  ReadSignature();
  ReadVersion();
  ReadWorkSpace();

  
  // Added 11/7/05
  m_ChartControl.startIndex = m_startIndex;
  m_ChartControl.endIndex = m_endIndex;
  

  ReadChartPanels();
  UpdateScreen();


  // Reset color properties
  m_ChartControl.backGradientTop = m_workspace.m_backGradientTop;
  m_ChartControl.backGradientBottom = m_workspace.m_backGradientBottom;
  m_ChartControl.m_backGradientTop = m_workspace.m_backGradientTop;
  m_ChartControl.m_backGradientBottom = m_workspace.m_backGradientBottom;
  m_ChartControl.valueViewGradientTop = m_workspace.m_valueViewGradientTop;
  m_ChartControl.valueViewGradientBottom = m_workspace.m_valueViewGradientBottom;
  m_ChartControl.m_valueViewGradientTop = m_workspace.m_valueViewGradientTop;
  m_ChartControl.m_valueViewGradientBottom = m_workspace.m_valueViewGradientBottom;
  m_ChartControl.backColor = m_workspace.m_backColor;
  m_ChartControl.foreColor = m_workspace.m_foreColor;
  m_ChartControl.upColor = m_workspace.m_upColor;
  m_ChartControl.downColor = m_workspace.m_downColor;
  m_ChartControl.gridColor = m_workspace.m_gridColor;  
  m_ChartControl.SetSeriesColor(CString(m_ChartControl.m_symbol) + ".close", m_workspace.m_wickColor);
  
  return 1;
}
//implement here common methods
void CStockChartXSerializer::SaveSignature()
{
  //no signature
}

void CStockChartXSerializer::SaveVersion()
{
  m_Serializer << SCX_FILE_VERSION;
}

void CStockChartXSerializer::SaveWorkSpace()
{
  IOWorkspace workspace;

  setIOString( (TCHAR*)&(workspace.m_symbol), (const TCHAR*)m_ChartControl.m_symbol );

  workspace.m_backGradientTop		        = m_ChartControl.backGradientTop;
  workspace.m_backGradientBottom		    = m_ChartControl.backGradientBottom;
  workspace.m_valueViewGradientTop		  = m_ChartControl.valueViewGradientTop;
  workspace.m_valueViewGradientBottom		= m_ChartControl.valueViewGradientBottom;
  workspace.m_backColor		              = m_ChartControl.backColor;
  workspace.m_foreColor		              = m_ChartControl.foreColor;
  workspace.m_gridColor		              = m_ChartControl.gridColor;
  workspace.m_upColor			              = m_ChartControl.upColor;
  workspace.m_downColor		              = m_ChartControl.downColor;
  workspace.m_infoPanelColor	          = m_ChartControl.valuePanelColor;
  workspace.m_displayTitles	            = m_ChartControl.displayTitles;
  workspace.m_horizontalSeparators	    = m_ChartControl.m_horzLines;
  workspace.m_horizontalSeparatorColor  = m_ChartControl.horzLineColor;
  workspace.m_threeDStyle		            = m_ChartControl.threeDStyle;
  workspace.m_startX			              = m_ChartControl.startIndex;
  workspace.m_endX			                = m_ChartControl.endIndex;
  workspace.m_extendedX		              = m_ChartControl.extendedXPixels;
  workspace.m_scalingType		            = m_ChartControl.scalingType;
  workspace.m_priceStyle		            = m_ChartControl.priceStyle;
  workspace.m_showXGrid		              = m_ChartControl.showXGrid;  
  workspace.m_showYGrid		              = m_ChartControl.showYGrid;
  workspace.m_YGridAlignment	          = m_ChartControl.yAlignment;
  workspace.m_realtimeXLabels	          = m_ChartControl.realTime;
  workspace.m_darvasBoxes		            = m_ChartControl.darvasBoxes;
  workspace.m_panelsCount		            = GetPanelsCount();
  workspace.m_lineColor					= m_ChartControl.lineColor;
  workspace.m_lineThickness				= m_ChartControl.lineThickness;
  
  // File version 8
  workspace.m_textAreaFontSize          = m_ChartControl.textAreaFontSize;	
  setIOString( (TCHAR*)&(workspace.m_textAreaFontName), (const TCHAR*)m_ChartControl.m_textAreaFontName );
  
  // File version 9
  workspace.m_wickColor						= m_ChartControl.wickColor;

  // Version 10
  workspace.m_useVolumeUpDownColors 		= m_ChartControl.useVolumeUpDownColors;	 

  m_Serializer << workspace;
}

void CStockChartXSerializer::SaveChartPanels()
{
#ifdef _CONSOLE_DEBUG
	printf("\nSaveChartPanels()");
	//system("pause");
#endif
  IOPanel ioPanel;
  int panelsCount = m_ChartControl.GetVisiblePanelCount();

  for( int panelIdx = 0; panelIdx < panelsCount; ++panelIdx )
  {
    CChartPanel&	panel	= * m_ChartControl.panels[panelIdx];
    
    if( IgnorePanel( panel ) ){
      continue;
	}
    //	Save x,y of the panel
    ioPanel.reset();
    ioPanel.m_Y1	= (double)panel.y1 / m_ChartControl.height;
    ioPanel.m_Y2	= (double)panel.y2 / m_ChartControl.height;
    
    ioPanel.m_seriesCount	= GetSeriesCount( panel );//panel.series.size();
    
	
    m_Serializer << ioPanel;

    SavePanelsSeries( ioPanel, panel, panelIdx );

    SaveTextAreas( panel );

    SaveObjects( panel );

    SaveLineObjects( panel );

    SaveHorizontalLines( panel );
  }
}

long CStockChartXSerializer::GetSeriesCount(const CChartPanel &panel)
{
  return panel.series.size();
}

void CStockChartXSerializer::SavePanelIndex(int iPanelIdx)
{
  //dont save any indexes
}

void CStockChartXSerializer::SavePanelsSeries(const IOPanel &ioPanel,const CChartPanel&	panel,int iPanelIdx)
{
#ifdef _CONSOLE_DEBUG
	printf("\nStockChartXSerializer::SavePanelsSeries()");
#endif
	IOSeries	ioSeries;
	int	seriesCount	= panel.series.size();
  
  //save the index of the panel
  SavePanelIndex( iPanelIdx );
  
  for( int seriesIdx = 0; seriesIdx <  seriesCount; seriesIdx++ )
  {
    CSeries&	series	= *(panel.series[seriesIdx]);
    if( IgnoreSeries( series ) )//cause on one panel might be data and indicator series. general template must ignore data series
      continue;

    ioSeries.reset();
    setIOString( (TCHAR*)(&ioSeries.m_name),  series.szName  );
    setIOString( (TCHAR*)(&ioSeries.m_title), series.szTitle );
    ioSeries.m_seriesType		= series.seriesType;
    ioSeries.m_indicatorType	= series.indicatorType;
    setIOString( (TCHAR*)(&ioSeries.m_link), series.linkTo );
    ioSeries.m_userParams		= series.userParams;
    ioSeries.m_lineStyle		= series.lineStyle;
    ioSeries.m_lineWeight		= series.lineWeight;
    ioSeries.m_lineColor		= series.lineColor;
    ioSeries.m_upColor			= series.upColor;
    ioSeries.m_downColor		= series.downColor;
    ioSeries.m_selected			= series.selected;
    ioSeries.m_shareScale		= series.shareScale;
    ioSeries.m_selectable		= series.selectable;	
    
	//TWIN
	ioSeries.m_lineColorSignal	= series.lineColorSignal;
	ioSeries.m_lineStyleSignal	= series.lineStyleSignal;
	ioSeries.m_lineWeightSignal	= series.lineWeightSignal;

    int	indParamCount	= (series.seriesType == OBJECT_SERIES_INDICATOR ? series.paramStr.size() : 0);
    ioSeries.m_indParamCount	= indParamCount;
    ioSeries.m_valuesCount		= series.data_master.size();
    
    m_Serializer << ioSeries;
    
    IOIndicator	indicator;


	// Save series names 11/1/07
	std::vector<CString> oldParamStr;
	int i = 0;
	for( i = 0; i < indParamCount; i++ )
		oldParamStr.push_back(series.paramStr[i]);

    for( int ind = 0; ind < indParamCount; ind++ )
    {
      indicator.reset();
	  if(series.paramStr[ind] == panel.pCtrl->m_symbol && panel.pCtrl->m_symbol != "")
	  {
		  series.paramStr[ind] = "GROUP";
	  }
      setIOString( (TCHAR*)indicator.m_str, series.paramStr[ind] );
      indicator.m_double	= series.paramDbl[ind];
      indicator.m_int		= series.paramInt[ind];
      
#ifdef _CONSOLE_DEBUG
	  printf("\nIndicator %s saved str=%s dbl=%f int=%d",series.szName,series.paramStr[ind],series.paramDbl[ind],series.paramInt[ind]);
#endif
      m_Serializer << indicator;
    }
    
    SaveSeriesData( ioSeries, series );

	// Restore series names 11/1/07
	for( i = 0; i < indParamCount; i++ )
		series.paramStr[i] = oldParamStr[i];

  }	//	End of Series


}

void CStockChartXSerializer::SaveSeriesData(const IOSeries &ioSeries,const CSeries &series)
{
  IOValue	ioValue;
  int	valuesCount		= ioSeries.m_valuesCount;
  for( int vidx = 0; vidx < valuesCount; vidx++ )
  {
    ioValue.reset();
    ioValue.m_value	= series.data_master[vidx].value;
    ioValue.m_jDate	= series.data_master[vidx].jdate;
    
    m_Serializer << ioValue;
  }
}

void CStockChartXSerializer::SaveTextAreas(const CChartPanel&	panel)
{
	IOTextArea	ioTextArea;
  int	textAreasCount	= panel.textAreas.size();

  m_Serializer << textAreasCount;
  
  for( int taIdx = 0; taIdx != textAreasCount; taIdx++ )
  {
    CTextArea&	textArea	= *(panel.textAreas[taIdx]);
    
    ioTextArea.reset();
    setIOString( (TCHAR*)(&ioTextArea.m_key),  textArea.key  );
    setIOString( (TCHAR*)(&ioTextArea.m_text), textArea.Text );
    ioTextArea.m_fontColor	= textArea.fontColor;
    ioTextArea.m_fontSize	= textArea.fontSize;
    ioTextArea.m_startX		= textArea.startX;
    ioTextArea.m_startY		= textArea.startY;
    ioTextArea.m_X1			= textArea.x1Value;
    ioTextArea.m_Y1			= textArea.y1Value;
    ioTextArea.m_X2			= textArea.x2Value;
    ioTextArea.m_Y2			= textArea.y2Value;
    ioTextArea.m_selectable	= textArea.selectable;
    ioTextArea.m_selected	= textArea.selected;
	ioTextArea.m_zOrder		= textArea.zOrder;
    
    m_Serializer << ioTextArea;
  }
}

void CStockChartXSerializer::SaveObjects(const CChartPanel&	panel)
{
  IOObject	ioObject;
	int	objectsCount	= panel.objects.size();

  m_Serializer << objectsCount;
  
  for( int oIdx = 0; oIdx != objectsCount; oIdx++ )
  {
    CCO&	object	= *(panel.objects[oIdx]);
    
    ioObject.reset();
    setIOString( (TCHAR*)(&ioObject.m_objectType), object.objectType  );
    setIOString( (TCHAR*)(&ioObject.m_key)       , object.key         );
    setIOString( (TCHAR*)(&ioObject.m_fileName)  , object.fileName    );
    setIOString( (TCHAR*)(&ioObject.m_text)      , object.text        );
    ioObject.m_backColor	= object.backColor;
    ioObject.m_foreColor	= object.foreColor;
    ioObject.m_lineStyle	= object.lineStyle;
    ioObject.m_lineWeight	= object.lineWeight;
    ioObject.m_X1			= object.x1Value;
    ioObject.m_Y1			= object.y1Value;
    ioObject.m_X2			= object.x2Value;
    ioObject.m_Y2			= object.y2Value;

    ioObject.m_selectable	= object.selectable;
    ioObject.m_selected		= object.selected;
    ioObject.m_visible		= object.visible;
    ioObject.m_zOrder		= object.zOrder;	
    
    m_Serializer << ioObject;
  }
}

void CStockChartXSerializer::SaveLineObjects(const CChartPanel&	panel)
{
  IOLineObject	ioLine;
	int	linesCount	= panel.lines.size();

  m_Serializer << linesCount;
  
  for( int lIdx = 0; lIdx != linesCount; lIdx++ )
  {
    CCOL&	line	= *(panel.lines[lIdx]);
    
    ioLine.reset();
    setIOString( (TCHAR*)(&ioLine.m_objectType), line.objectType  );
    setIOString( (TCHAR*)(&ioLine.m_key)       , line.key         );
    ioLine.m_lineStyle	= line.lineStyle;
    ioLine.m_lineWeight	= line.lineWeight;
    ioLine.m_lineColor	= line.lineColor;
    ioLine.m_X1			= line.x1Value;
    ioLine.m_Y1			= line.y1Value;
    ioLine.m_X2			= line.x2Value;
    ioLine.m_Y2			= line.y2Value;
	ioLine.m_X1_2		= line.x1Value_2;
    ioLine.m_Y1_2		= line.y1Value_2;
    ioLine.m_X2_2		= line.x2Value_2;
    ioLine.m_Y2_2		= line.y2Value_2;
	ioLine.m_x1JDate	= line.x1JDate;
	ioLine.m_x2JDate	= line.x2JDate;
	ioLine.m_x1JDate_2	= line.x1JDate_2;
	ioLine.m_x2JDate_2	= line.x2JDate_2;
	ioLine.m_selectable	= line.selectable;
    ioLine.m_selected	= line.selected;
    ioLine.m_zOrder		= line.zOrder;
	ioLine.m_fillStyle	= line.fillStyle;
	ioLine.m_vFixed		= line.vFixed;
	ioLine.m_hFixed		= line.hFixed;
	ioLine.m_isFirstPointArrow = line.isFirstPointArrow;
	ioLine.m_radiusExtension = line.radiusExtension;
	ioLine.m_rightExtension = line.rightExtension;
	ioLine.m_leftExtension = line.leftExtension;
	ioLine.m_valuePosition = line.valuePosition;

#ifdef _CONSOLE_DEBUG
	printf("SerializerX::SaveLineObjects() valuePosition = %f",ioLine.m_valuePosition);
#endif

    m_Serializer << ioLine;
  }
}

void CStockChartXSerializer::SaveHorizontalLines(const CChartPanel&	panel)
{
  IOHorizontalLine	ioHLine;
	int	hLinesCount	= panel.horizontalLines.size();

  m_Serializer << hLinesCount;
  
  for( int hlIdx = 0; hlIdx != hLinesCount; hlIdx++ )
  {
    double	hLine	= panel.horizontalLines[hlIdx];
    
    ioHLine.reset();
    ioHLine.m_Y	= hLine;
    
    m_Serializer << ioHLine;
  }
}

long CStockChartXSerializer::GetPanelsCount()
{
  return m_ChartControl.GetVisiblePanelCount();
}

bool CStockChartXSerializer::IgnorePanel(const CChartPanel &panel)
{
  return false;//save all panels
}

bool CStockChartXSerializer::IgnoreSeries(const CSeries &series)
{
  return false;//do not ignore any panels   
}

void CStockChartXSerializer::ReadSignature()
{
  //no signature
}

void CStockChartXSerializer::ClearChart()
{
  m_ChartControl.RemoveAllSeries();
  m_iPanelCountLeft = 0;
}

void CStockChartXSerializer::ReadVersion()
{
  m_iVersion = 0;
  m_Serializer >> m_iVersion;
  
  // 2/25/05
  if(m_iVersion < 7)
  {
    m_ChartControl.ThrowError( CUSTOM_CTL_SCODE(1009), "File version is incompatible" );
  }
}

void CStockChartXSerializer::ReadWorkSpace()
{
  //	Load the workspace information
  IOWorkspace	workspace;
  
  // Backwards compatibility structure
  UINT size =  0;
  switch( m_iVersion ){
  case 6:
    size = 290;
    break;
  case 7:
    size = 292;
    break;
  case 8:
	size = 500;
	break;
  case 9:
	size = 508;
	break;
  case SCX_FILE_VERSION:
    size = sizeof(workspace);
    break;
  }

  long test = sizeof(workspace) - sizeof(bool);

  m_Serializer.Read( &workspace, size );
 
  m_ChartControl.backGradientTop		      = workspace.m_backGradientTop;
  m_ChartControl.backGradientBottom	      = workspace.m_backGradientBottom;	
  m_ChartControl.valueViewGradientTop		  = workspace.m_valueViewGradientTop;
  m_ChartControl.valueViewGradientBottom  = workspace.m_valueViewGradientBottom;
  m_ChartControl.m_symbol	                = (TCHAR*)&(workspace.m_symbol);
  m_ChartControl.backColor			          = workspace.m_backColor;
  m_ChartControl.foreColor			          = workspace.m_foreColor;
  m_ChartControl.gridColor			          = workspace.m_gridColor;
  m_ChartControl.upColor			            = workspace.m_upColor;
  m_ChartControl.downColor			          = workspace.m_downColor;
  m_ChartControl.valuePanelColor	        = workspace.m_infoPanelColor;
  m_ChartControl.displayTitles		        = workspace.m_displayTitles;
  m_ChartControl.m_horzLines		          = workspace.m_horizontalSeparators;
  m_ChartControl.horzLineColor		        = workspace.m_horizontalSeparatorColor;
  m_ChartControl.threeDStyle		          = workspace.m_threeDStyle;
  m_ChartControl.startIndex		            = workspace.m_startX;
  m_ChartControl.endIndex			            = workspace.m_endX;
  m_ChartControl.extendedXPixels	        = workspace.m_extendedX;
  m_ChartControl.scalingType		          = workspace.m_scalingType;
  m_ChartControl.priceStyle		            = workspace.m_priceStyle;
  m_ChartControl.showXGrid			          = workspace.m_showXGrid;
  m_ChartControl.showYGrid			          = workspace.m_showYGrid;
  m_ChartControl.yAlignment		            = workspace.m_YGridAlignment;
  m_ChartControl.realTime			            = workspace.m_realtimeXLabels;
  m_ChartControl.darvasBoxes					=	workspace.m_darvasBoxes;
  m_ChartControl.lineColor						= workspace.m_lineColor;
  m_ChartControl.lineThickness					= workspace.m_lineThickness;
  
  m_ChartControl.m_horzLines		          = workspace.m_horizontalSeparators;
  m_ChartControl.m_horizontalSeparators		= workspace.m_horizontalSeparators;  
  m_ChartControl.m_horizontalSeparatorColor  = workspace.m_horizontalSeparatorColor;  
  m_ChartControl.m_threeDStyle		          = workspace.m_threeDStyle;
  m_ChartControl.m_priceStyle 	            = workspace.m_priceStyle;

  
  // Newer file version features
  if( m_iVersion >= 8)
  {		
    m_ChartControl.textAreaFontSize       = workspace.m_textAreaFontSize;
    m_ChartControl.m_textAreaFontSize     = m_ChartControl.textAreaFontSize;
    m_ChartControl.textAreaFontName       = (TCHAR*)&(workspace.m_textAreaFontName);
    m_ChartControl.m_textAreaFontName     = m_ChartControl.textAreaFontName;
  
	// Version 9
	if( m_iVersion >= 9) m_ChartControl.wickColor			  = workspace.m_wickColor;

	// Version 10
	if( m_iVersion >= 10) m_ChartControl.useVolumeUpDownColors 	= workspace.m_useVolumeUpDownColors;	 

  }

  m_workspace = workspace;

  m_iPanelsCount = workspace.m_panelsCount;
}

void CStockChartXSerializer::ReadChartPanels()
{
  IOPanel	ioPanel;
  int panelIdx;
  
  std::vector<IOPanel> panelSizes;
  
  for( panelIdx	= 0; panelIdx < m_iPanelsCount; panelIdx++ )
  {
    //	Load the panel
    ioPanel.reset();
	
    m_Serializer >> ioPanel;
    
    //CChartPanel*	panel	= *m_ChartControl.panels[ iFilePanelIndex ];
    //panel.Connect( &m_ChartControl );
    
    //panel.y1	= (double)m_ChartControl.height * ioPanel.m_Y1;
    //panel.y2	= (double)m_ChartControl.height * ioPanel.m_Y2;
    
    CChartPanel &panel = *ReadPanelsSeries( ioPanel );
    panel.Connect( &m_ChartControl );
    
   // panel.y1	= (double)m_ChartControl.height * ioPanel.m_Y1;
    //panel.y2	= (double)m_ChartControl.height * ioPanel.m_Y2;

	panelSizes.push_back(ioPanel);

    ReadTextAreas( panel );

    ReadObjects( panel );

    ReadLineStudyObjects( panel );

    ReadHorizontalLines( panel );
	
  }

  
  // 5-2-05 RDG
  // Resize the panels (this can't be done until they are all added)
  for( panelIdx	= 0; panelIdx < m_iPanelsCount; panelIdx++ )
  {
    CChartPanel&	panel	= *m_ChartControl.panels[ panelIdx ];
    panel.Connect( &m_ChartControl );
    panel.y1	= (double)m_ChartControl.height * panelSizes[panelIdx].m_Y1;
    panel.y2	= (double)m_ChartControl.height * panelSizes[panelIdx].m_Y2;
  }
	

}

CChartPanel* CStockChartXSerializer::ReadPanelIndex()
{
  long iPanelIdx = m_ChartControl.AddChartPanelSerialized();
  if( iPanelIdx == -1 )
    m_ChartControl.ThrowErr( 1010, "Too many panels" );
  return m_ChartControl.panels[ iPanelIdx ];
}


CChartPanel* CStockChartXSerializer::ReadPanelsSeries(const IOPanel &ioPanel)
{
	//	Load the panel series
	IOSeries	ioSeries;
  CChartPanel *panel = NULL;
  
  panel = ReadPanelIndex( );
  
  for( int seriesIdx = 0; seriesIdx <  ioPanel.m_seriesCount; seriesIdx++ )
  {
    ioSeries.reset();
    
    m_Serializer >> ioSeries;
   //TWIN
	if(ioSeries.m_seriesType==0) continue;
    CSeries&	series	= *AddSeries( ioSeries, panel->index );//*(panel->series[seriesIdx]);
    series.Connect( &m_ChartControl );

    series.szName		= ioSeries.m_name;
    series.szTitle		= ioSeries.m_title;
    series.seriesType	= ioSeries.m_seriesType;
    series.indicatorType= ioSeries.m_indicatorType;
    series.linkTo		  = ioSeries.m_link;
    series.userParams	= ioSeries.m_userParams;
    series.lineStyle	= ioSeries.m_lineStyle;
    series.lineWeight	= ioSeries.m_lineWeight;
    series.lineColor	= ioSeries.m_lineColor;
    series.upColor		= ioSeries.m_upColor;
    series.downColor	= ioSeries.m_downColor;
    series.selected		= ioSeries.m_selected;
    series.shareScale	= ioSeries.m_shareScale;
    series.selectable	= ioSeries.m_selectable;


	//TWIN
	series.lineColorSignal	= ioSeries.m_lineColorSignal;
	series.lineStyleSignal	= ioSeries.m_lineStyleSignal;
	series.lineWeightSignal = ioSeries.m_lineWeightSignal;
    
    int	indParamCount	= ioSeries.m_indParamCount;
    int	valuesCount		= ioSeries.m_valuesCount;
    
    //	Load Indicator parameters if any
    IOIndicator	indicator;
    series.paramStr.resize( indParamCount );
    series.paramDbl.resize( indParamCount );
    series.paramInt.resize( indParamCount );
    
    for( int ind = 0; ind < indParamCount; ind++ )
    {
      indicator.reset();
      m_Serializer >> indicator;
      

	  //5-3-05 RG
	  // We must convert the original .open, .high, .low, and .close to the current symbol
	  CString symbol = panel->pCtrl->m_symbol;
	  CString source = indicator.m_str;
	  source.MakeUpper();
	  symbol.MakeUpper();
	  long found = source.Find(".OPEN");
	  if(found == -1) found = source.Find(".HIGH");
	  if(found == -1) found = source.Find(".LOW");
	  if(found == -1) found = source.Find(".CLOSE");
	  if(found == -1) found = source.Find(".VOL");
	  if(found > -1)
	  {
		 // if condition removed 9/12/05
		 // REPLACED 11/7/05
		 if(symbol == "") symbol = source.Mid(0, found); // If the main symbol wasn't set
		 //symbol = source.Mid(0, found); // If the main symbol wasn't set
	     source = source.Mid(found + 1);
		 source = symbol + "." + source;
	  }
	  else if(source == "GROUP") // This is a symbol group, not a series source
	  {
		 source = symbol;
	  }

      series.paramStr[ind]	= source;
      series.paramDbl[ind]	= indicator.m_double;
      series.paramInt[ind]	= indicator.m_int;
    }
    
    ReadSeriesData( ioSeries, series );

    m_ChartControl.barColors.resize( valuesCount, -1 );
  
  }	//	End of Series
  
  m_ChartControl.UpdateScreen(true);
  
  return panel;
  
}

CSeries* CStockChartXSerializer::AddSeries(const IOSeries &ioSeries,int iPanelIdx)
{
  
  if( ioSeries.m_seriesType == OBJECT_SERIES_INDICATOR )
  {
    m_ChartControl.AddIndicatorSeries( ioSeries.m_indicatorType, ioSeries.m_name, iPanelIdx, FALSE );
  }
  else
  {
    m_ChartControl.AddNewSeriesType( iPanelIdx, ioSeries.m_seriesType, ioSeries.m_name );
  }
  return m_ChartControl.panels[ iPanelIdx ]->series[ m_ChartControl.panels[ iPanelIdx ]->series.size() - 1 ];
}

void CStockChartXSerializer::ReadSeriesData(const IOSeries &ioSeries,CSeries &series)
{
	SeriesPoint sp;
	IOValue		ioValue;
	int iValuesCount = ioSeries.m_valuesCount;

	for( int vidx = 0; vidx < iValuesCount; vidx++ )
	{
	ioValue.reset();
	m_Serializer >> ioValue;
    
	sp.value	= ioValue.m_value;
	sp.jdate	= ioValue.m_jDate;
    
	series.data_master.push_back( sp );
	}
}

void CStockChartXSerializer::ReadTextAreas(CChartPanel &panel)
{
  IOTextArea	ioTextArea;
	int	textAreasCount	= 0;

  m_Serializer >> textAreasCount;
  
  for( int taIdx = 0; taIdx != textAreasCount; taIdx++ )
  {
    ioTextArea.reset();
    m_Serializer >> ioTextArea;
    
    panel.textAreas.push_back( new CTextArea( 0,0, &panel ) );
    CTextArea&	textArea	= *(panel.textAreas[panel.textAreas.size() - 1]);
    
    textArea.gotUserInput	= true;
    
    textArea.key		= ioTextArea.m_key;
    textArea.Text		= ioTextArea.m_text;
    textArea.fontColor	= ioTextArea.m_fontColor;
    textArea.fontSize	= ioTextArea.m_fontSize;
    textArea.startX		= ioTextArea.m_startX;
    textArea.startY		= ioTextArea.m_startY;
    textArea.x1Value	= ioTextArea.m_X1;
    textArea.y1Value	= ioTextArea.m_Y1;
    textArea.x2Value	= ioTextArea.m_X2;
    textArea.y2Value	= ioTextArea.m_Y2;
    textArea.selectable	= ioTextArea.m_selectable;
    textArea.selected	= ioTextArea.m_selected;
	textArea.zOrder		= ioTextArea.m_zOrder;
  }	//	End of TextAreas 
}

void CStockChartXSerializer::ReadObjects(CChartPanel &panel)
{
	int	objectsCount	= 0;
  IOObject	ioObject;

  m_Serializer >> objectsCount;
  
  for( int oIdx = 0; oIdx != objectsCount; oIdx++ )
  {
    ioObject.reset();
    m_Serializer >> ioObject;
    
    CString	objType	= ioObject.m_objectType;
    
    if( objType == "Buy Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, IDB_BUY, "", "", "Buy Symbol", &panel ) );
    }
    else if( objType == "Sell Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, IDB_SELL, "", "", "Sell Symbol", &panel ) );
    }
    else if( objType == "Exit Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, IDB_EXIT, "", "", "Exit Symbol", &panel ) );
    }
    else if( objType == "Exit Long Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, IDB_EXIT_LONG, "", "", "Exit Long Symbol", &panel ) );
    }
    else if( objType == "Exit Short Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, IDB_EXIT_SHORT, "", "", "Exit Short Symbol", &panel ) );
    }
    else if( objType == "Signal Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, IDB_SIGNAL, "", "", "Signal Symbol", &panel ) );
    }
    else if( objType == "Custom Symbol" )
    {
      panel.objects.push_back( new CSymbolObject( 0, 0, "", "", "", "Custom Symbol", &panel ) );
    }
    else
    {
      panel.objects.push_back( new CCO() );
    }
    
    CCO&	object	= *(panel.objects[panel.objects.size() - 1]);
    
    object.objectType	= ioObject.m_objectType;
    object.key			= ioObject.m_key;
    object.fileName		= ioObject.m_fileName;
    object.text			= ioObject.m_text;
    object.backColor	= ioObject.m_backColor;
    object.foreColor	= ioObject.m_foreColor;
    object.lineStyle	= ioObject.m_lineStyle;
    object.lineWeight	= ioObject.m_lineWeight;
    object.x1Value		= ioObject.m_X1;
    object.y1Value		= ioObject.m_Y1;
    object.x2Value		= ioObject.m_X2;
    object.y2Value		= ioObject.m_Y2;
    object.selectable	= ioObject.m_selectable;
    object.selected		= ioObject.m_selected;
    object.visible		= ioObject.m_visible;
    object.zOrder		= ioObject.m_zOrder;
    
    object.Connect( &m_ChartControl ); 
    object.SetXY();       
    object.UpdatePoints();
  }	//	End of Objects
}

void CStockChartXSerializer::ReadLineStudyObjects(CChartPanel &panel)
{
	int	linesCount	= 0;
  IOLineObject	ioLine;

  m_Serializer >> linesCount;
  
  for( int lIdx = 0; lIdx != linesCount; lIdx++ )
  {
    ioLine.reset();
    m_Serializer >> ioLine;    
    
    if( CString(ioLine.m_objectType) == "TrendLine" ){
      panel.lines.push_back( new CLineStandard( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "Ellipse" ){
      panel.lines.push_back( new CLineStudyEllipse( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "Rectangle" ){
      panel.lines.push_back( new CLineStudyRectangle( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "Triangle" ){
      panel.lines.push_back( new CLineStudyTriangle( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "Polyline" ){
      panel.lines.push_back( new CLineStudyPolyline( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "SpeedLines" ){
      panel.lines.push_back( new CLineStudySpeedLines( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "GannFan" ){
      panel.lines.push_back( new CLineStudyGannFan( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "FibonacciArcs" ){
      panel.lines.push_back( new CLineStudyFibonacciArcs( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "FibonacciFan" ){
      panel.lines.push_back( new CLineStudyFibonacciFan( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "FibonacciRetracements" ){
      panel.lines.push_back( new CLineStudyFibonacciRetracements( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
	else if( CString(ioLine.m_objectType) == "FibonacciProgression" ){
		panel.lines.push_back( new CLineStudyFibonacciProgression( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "FibonacciTimeZones" ){
      panel.lines.push_back( new CLineStudyFibonacciTimeZones( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "TironeLevels" ){
      panel.lines.push_back( new CLineStudyTironeLevels( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "QuadrantLines" ){
      panel.lines.push_back( new CLineStudyQuadrantLines( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "RaffRegression" ){
      panel.lines.push_back( new CLineStudyRaffRegression( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    else if( CString(ioLine.m_objectType) == "ErrorChannel" ){
      panel.lines.push_back( new CLineStudyErrorChannel( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }			
	else if( CString(ioLine.m_objectType) == "Freehand" ){
      panel.lines.push_back( new CLineStudyFreehand( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    } 
	else if( CString(ioLine.m_objectType) == "Channel" ){
		panel.lines.push_back( new CLineStudyChannel( ioLine.m_lineColor, ioLine.m_key, &panel ) );
    }
    
    // --- Add new line types here ---
    
    CCOL&	line	= *(panel.lines[panel.lines.size() - 1]);
    
    line.objectType	= ioLine.m_objectType;
    line.key		= ioLine.m_key;
    line.lineStyle	= ioLine.m_lineStyle;
    line.lineWeight	= ioLine.m_lineWeight;
    line.lineColor	= ioLine.m_lineColor;
    line.x1Value	= ioLine.m_X1;
    line.y1Value	= ioLine.m_Y1;
    line.x2Value	= ioLine.m_X2;
    line.y2Value	= ioLine.m_Y2;
	line.x1Value_2	= ioLine.m_X1_2;
    line.y1Value_2	= ioLine.m_Y1_2;
    line.x2Value_2	= ioLine.m_X2_2;
    line.y2Value_2	= ioLine.m_Y2_2;
	line.x1JDate	= ioLine.m_x1JDate;
	line.x2JDate	= ioLine.m_x2JDate;
	line.x1JDate_2	= ioLine.m_x1JDate_2;
	line.x2JDate_2	= ioLine.m_x2JDate_2;
    line.selectable	= ioLine.m_selectable;
    line.selected	= ioLine.m_selected;
    line.zOrder		= ioLine.m_zOrder;
	line.fillStyle	= ioLine.m_fillStyle;
	line.vFixed		= ioLine.m_vFixed;
	line.hFixed		= ioLine.m_hFixed;
    line.isFirstPointArrow = ioLine.m_isFirstPointArrow;
	line.radiusExtension = ioLine.m_radiusExtension;
	line.rightExtension = ioLine.m_rightExtension;
	line.leftExtension = ioLine.m_leftExtension;
	line.valuePosition = ioLine.m_valuePosition;
	

#ifdef _CONSOLE_DEBUG
	printf("SerializerX::ReadLineStudyObjects() valuePosition = %f", ioLine.m_valuePosition);
#endif

    line.Connect( &m_ChartControl );
  }	//	End of line objects
}

void CStockChartXSerializer::ReadHorizontalLines(CChartPanel &panel)
{
	int	hLinesCount	= 0;
  IOHorizontalLine	ioHLine;

  m_Serializer >> hLinesCount;
  
  for( int hlIdx = 0; hlIdx != hLinesCount; hlIdx++ )
  {
    ioHLine.reset();
    m_Serializer >> ioHLine;
    
    panel.AddHorzLine( ioHLine.m_Y );
  }	//	End of Horizontal Lines
}

void CStockChartXSerializer::UpdateScreen(bool SetStartEndIndex /*=true*/)
{

  m_ChartControl.OnSize( -1, m_ChartControl.width, m_ChartControl.height + CALENDAR_HEIGHT );
  
  // Calculate all indicators
  m_ChartControl.loading = false;

  int	panelsCount = m_ChartControl.GetVisiblePanelCount();
  
  /*
  for( int panelIdx = 0; panelIdx < panelsCount; panelIdx++ )
  {
    int	seriesCount	= m_ChartControl.panels[panelIdx]->series.size();
    for( int seriesIdx = 0; seriesIdx < seriesCount; seriesIdx++ )
    {
      if( m_ChartControl.panels[panelIdx]->series[seriesIdx]->seriesType == OBJECT_SERIES_INDICATOR )
      {
         //m_ChartControl.panels[panelIdx]->series[seriesIdx]->Calculate();
      }
    }
  */

  if(SetStartEndIndex)
  {
	// Set the start and end index
	m_ChartControl.startIndex = m_workspace.m_startX;
	m_ChartControl.endIndex = m_workspace.m_endX;
  }
  else{
	m_ChartControl.startIndex = m_startIndex;
	m_ChartControl.endIndex = m_endIndex;
  }
  
    
  //m_ChartControl.CalculateScaleInfo();
  m_ChartControl.Update();
  m_ChartControl.changed = false;
}
//------------------------------------------------------------------------
//just call the defaults
long CStockChartXSerializer_All::Save()
{
  return CStockChartXSerializer::Save();
}
//just call the defaults without Workspace
long CStockChartXSerializer_All::Load()
{
  return CStockChartXSerializer::Load();
}
//------------------------------------------------------------------------
//when saving general template, will not save panels which has at least one data series
//must change SaveChartPanels method.
long CStockChartXSerializer_General::Save()
{
#ifdef _CONSOLE_DEBUG
	printf("\nStockChartXSerializer_General::Save()");
	//system("pause");
#endif
  return CStockChartXSerializer::Save();
}

long CStockChartXSerializer_General::Load(bool Temp /*=false*/)
{
#ifdef _CONSOLE_DEBUG
	printf("\nStockChtXSerializer::Load()");
#endif
  m_startIndex = m_ChartControl.startIndex;
  m_endIndex = m_ChartControl.endIndex;

  
#ifdef _CONSOLE_DEBUG
	printf("\n\tClearChart()"); //  <---- ERRO AKI
#endif

  ClearChart();
#ifdef _CONSOLE_DEBUG
	printf("\n\tReadSignature()");
#endif
  ReadSignature();
#ifdef _CONSOLE_DEBUG
	printf("\n\tReadVersion()");
#endif
  ReadVersion();
#ifdef _CONSOLE_DEBUG
	printf("\n\tReadWorkSpace()");
#endif
  ReadWorkSpace(Temp);

  
  // Added 11/7/05
  m_ChartControl.startIndex = m_startIndex;
  m_ChartControl.endIndex = m_endIndex;
  
  
#ifdef _CONSOLE_DEBUG
	printf("\n\tReadChartPanels()");
#endif
  ReadChartPanels();
#ifdef _CONSOLE_DEBUG
	printf("\n\t UpdateScreen()");
#endif
  UpdateScreen();


  // Reset color properties
  /*m_ChartControl.backGradientTop = m_workspace.m_backGradientTop;
  m_ChartControl.backGradientBottom = m_workspace.m_backGradientBottom;
  m_ChartControl.m_backGradientTop = m_workspace.m_backGradientTop;
  m_ChartControl.m_backGradientBottom = m_workspace.m_backGradientBottom;
  m_ChartControl.valueViewGradientTop = m_workspace.m_valueViewGradientTop;
  m_ChartControl.valueViewGradientBottom = m_workspace.m_valueViewGradientBottom;
  m_ChartControl.m_valueViewGradientTop = m_workspace.m_valueViewGradientTop;
  m_ChartControl.m_valueViewGradientBottom = m_workspace.m_valueViewGradientBottom;
  m_ChartControl.backColor = m_workspace.m_backColor;
  m_ChartControl.foreColor = m_workspace.m_foreColor;
  m_ChartControl.upColor = m_workspace.m_upColor;
  m_ChartControl.downColor = m_workspace.m_downColor;
  m_ChartControl.gridColor = m_workspace.m_gridColor;  
  m_ChartControl.SetSeriesColor(CString(m_ChartControl.m_symbol) + ".close", m_workspace.m_wickColor);*/
  
  return 1;
}

void CStockChartXSerializer_General::ReadChartPanels()
{
  IOPanel	ioPanel;
  int panelIdx;

  std::vector<IOPanel> panelSizes;
  
  for( panelIdx	= 0; panelIdx < m_iPanelsCount; panelIdx++ )
  {
    //	Load the panel
    ioPanel.reset();
    m_Serializer >> ioPanel;
    
    //CChartPanel*	panel	= *m_ChartControl.panels[ iFilePanelIndex ];
    //panel.Connect( &m_ChartControl );
    //panel.y1	= (double)m_ChartControl.height * ioPanel.m_Y1;
    //panel.y2	= (double)m_ChartControl.height * ioPanel.m_Y2;
    
    CChartPanel &panel = *ReadPanelsSeries( ioPanel );
    panel.Connect( &m_ChartControl );
	
#ifdef _CONSOLE_DEBUG
	printf("\nReadChartPanels() %d",panel.index);
#endif
	// Are there panels above this one?
	double lastY2 = 0;
	if(panel.index > 0) 
	{
		CChartPanel &above = *m_ChartControl.panels[panel.index - 1];
		lastY2 = above.y2;		
	}
	
	//panel.y1	= ((double)m_ChartControl.height * ioPanel.m_Y1) + lastY2;
	//panel.y2	= ((double)m_ChartControl.height * ioPanel.m_Y2) + lastY2;

	// By design, general templates should NOT resize panels!
	
    panel.y1	= (double)m_ChartControl.height * ioPanel.m_Y1;
    panel.y2	= (double)m_ChartControl.height * ioPanel.m_Y2;
	

	panelSizes.push_back(ioPanel);

    ReadTextAreas( panel );

    ReadObjects( panel );

    ReadLineStudyObjects( panel );

    ReadHorizontalLines( panel );
	
  }
  

  
}

long CStockChartXSerializer_General::GetPanelsCount()
{
  int iResult, iPanelsCount;
  bool bHasIndicator;
  
  iPanelsCount = iResult = CStockChartXSerializer::GetPanelsCount();
  //remove the ones which have data series
  for( int panelIdx = 0; panelIdx < iPanelsCount; ++panelIdx )
  {
    CChartPanel &panel = *m_ChartControl.panels[ panelIdx ];
    int iSeriesCount = panel.series.size();

    bHasIndicator = false;//assume no indicators
    for( int seriesIdx = 0; seriesIdx < iSeriesCount && !bHasIndicator; ++seriesIdx )
      if( panel.series[ seriesIdx ]->seriesType == OBJECT_SERIES_INDICATOR )
        bHasIndicator = true;

    if( !bHasIndicator )
      iResult--;
  }
  return iResult;
}

long CStockChartXSerializer_General::GetSeriesCount(const CChartPanel &panel)
{
  long lCount = 0;
  long lSeriesCount = panel.series.size();

  for( int seriesIdx = 0; seriesIdx < lSeriesCount; ++seriesIdx )
    if( panel.series[ seriesIdx ]->seriesType == OBJECT_SERIES_INDICATOR )
      lCount++;

  return lCount;
}

void CStockChartXSerializer_General::SaveSignature()
{
  m_Serializer << SIGN_TMPL_GENERAL;
}

void CStockChartXSerializer_General::SaveTextAreas(const CChartPanel&	panel)
{
  //do not sav nothing for general template
}

void CStockChartXSerializer_General::SaveObjects(const CChartPanel&	panel)
{
  //do not sav nothing for general template
}

void CStockChartXSerializer_General::SaveLineObjects(const CChartPanel&	panel)
{
  //do not sav nothing for general template
}

void CStockChartXSerializer_General::SaveHorizontalLines(const CChartPanel&	panel)
{
  //do not sav nothing for general template
}


void CStockChartXSerializer_General::ReadSignature()
{
  int iSign;

  m_Serializer >> iSign;
  if( iSign != SIGN_TMPL_GENERAL )
  {
    m_ChartControl.ThrowErr( 1010 , "Wrong signature for general template." );
  }
}

void CStockChartXSerializer_General::SaveSeriesData(const IOSeries &ioSeries,const CSeries &series)
{
  //does not save any data
}

void CStockChartXSerializer_General::SavePanelIndex(int iPanelIdx)
{
  m_Serializer << iPanelIdx;
}

CChartPanel* CStockChartXSerializer_General::ReadPanelIndex()
{
  int iPanelsCount, iFilePanelIndex;
  CString sVolume;

  iPanelsCount = m_ChartControl.GetVisiblePanelCount();
  m_Serializer >> iFilePanelIndex;

  //got the index of panel.

  //check if such an index exists. if not just create new panels and return it
  if( iFilePanelIndex >= iPanelsCount )
    CStockChartXSerializer::ReadPanelIndex();//call the default behavior
  //if such a panel exists. must check if it has any volume series
  //if true then add a new chart panel and return it, if not don't create a new panel
  CChartPanel &panel = *m_ChartControl.panels[ iFilePanelIndex ];
  /*for(int seriesIdx = 0; seriesIdx < (int)panel.series.size(); ++seriesIdx )
  {
    CSeries &series = *panel.series[ seriesIdx ];
    
    sVolume = series.szName;
    sVolume.MakeLower();

    if( ( sVolume.Find(".volume") != -1 ) || 
        ( sVolume == "volume" ) || 
        ( sVolume.Find(".vol") != -1 ) )//volume series exists. add a new panel
      return CStockChartXSerializer::ReadPanelIndex();
  }*/
  return m_ChartControl.panels[ iFilePanelIndex ];
}





void CStockChartXSerializer_General::ReadWorkSpace(bool Temp /*=false*/)
{
   //	Load the workspace information
  IOWorkspace	workspace;
  
  // Backwards compatibility structure
  UINT size =  0;
  switch( m_iVersion ){
  case 6:
    size = 290;
    break;
  case 7:
    size = 292;
    break;
  case 9:
	size = 508;
	break;
  case SCX_FILE_VERSION:
    size = sizeof(workspace);
    break;
  }


  m_Serializer.Read( &workspace, size );
    
  /*m_ChartControl.backGradientTop		      = workspace.m_backGradientTop;
  m_ChartControl.backGradientBottom	      = workspace.m_backGradientBottom;	
  m_ChartControl.valueViewGradientTop		  = workspace.m_valueViewGradientTop;
  m_ChartControl.valueViewGradientBottom  = workspace.m_valueViewGradientBottom;
  //m_ChartControl.m_symbol	                = (TCHAR*)&(workspace.m_symbol);
  m_ChartControl.backColor			          = workspace.m_backColor;
  m_ChartControl.foreColor			          = workspace.m_foreColor;
  m_ChartControl.gridColor			          = workspace.m_gridColor;
  m_ChartControl.upColor			            = workspace.m_upColor;
  m_ChartControl.downColor			          = workspace.m_downColor;  
  m_ChartControl.valuePanelColor	        = workspace.m_infoPanelColor;  
  m_ChartControl.horzLineColor		        = workspace.m_horizontalSeparatorColor;
  m_ChartControl.m_horizontalSeparatorColor  = workspace.m_horizontalSeparatorColor;
  m_ChartControl.threeDStyle		          = workspace.m_threeDStyle;
  m_ChartControl.extendedXPixels	        = workspace.m_extendedX
  m_ChartControl.priceStyle		            = workspace.m_priceStyle;
  m_ChartControl.m_priceStyle 	            = workspace.m_priceStyle;
  m_ChartControl.yAlignment		            = workspace.m_YGridAlignment;  
  m_ChartControl.realTime			            = workspace.m_realtimeXLabels;
  m_ChartControl.m_threeDStyle		          = workspace.m_threeDStyle;*/
  if(Temp){
	  m_ChartControl.m_horzLines		          = workspace.m_horizontalSeparators;
	  m_ChartControl.m_horizontalSeparators		= workspace.m_horizontalSeparators;
	  m_ChartControl.scalingType		          = workspace.m_scalingType;
	  m_ChartControl.showXGrid			          = workspace.m_showXGrid;
	  m_ChartControl.m_xGrid					=  workspace.m_showXGrid;
	  m_ChartControl.showYGrid			          = workspace.m_showYGrid;
	  m_ChartControl.m_yGrid			          = workspace.m_showYGrid;
	  m_ChartControl.darvasBoxes		          = workspace.m_darvasBoxes;
	  m_ChartControl.displayTitles		        = workspace.m_displayTitles;
	  m_ChartControl.priceStyle		            = workspace.m_priceStyle;
	  m_ChartControl.m_priceStyle 	            = workspace.m_priceStyle;

	  m_ChartControl.lineColor = workspace.m_lineColor;
	  m_ChartControl.lineThickness = workspace.m_lineThickness;
  }
  m_ChartControl.startIndex		            = workspace.m_startX;
  m_ChartControl.endIndex			        = workspace.m_endX; 
  /*
  // Newer file version features
  if( m_iVersion >= 8)
  {	
    m_ChartControl.textAreaFontSize       = workspace.m_textAreaFontSize;
    m_ChartControl.m_textAreaFontSize     = m_ChartControl.textAreaFontSize;
    m_ChartControl.textAreaFontName       = (TCHAR*)&(workspace.m_textAreaFontName);
    m_ChartControl.m_textAreaFontName     = m_ChartControl.textAreaFontName;	
  
	// Version 9
	if( m_iVersion >= 9) m_ChartControl.wickColor = workspace.m_wickColor;  

	// Version 10
	if( m_iVersion >= 10) m_ChartControl.useVolumeUpDownColors 	= workspace.m_useVolumeUpDownColors;

  }

  m_workspace = workspace;
  */
  m_iPanelsCount = workspace.m_panelsCount;
}




void CStockChartXSerializer_General::ClearChart()
{
	m_ChartControl.LoadUserStudyLine();
	m_ChartControl.SaveUserStudies();
	
#ifdef _CONSOLE_DEBUG
	printf("\nClearChart()");
#endif
  //remove just indicators
  //collect the name of indicators
  std::vector<CString> vIndicators;
  int n, j;
  int iPanelsCount;
  
  iPanelsCount = m_ChartControl.GetVisiblePanelCount();

  for( n = 0; n < iPanelsCount; ++n )
    for( j = 0; j < (int)m_ChartControl.panels[n]->series.size(); ++j )	
	{
		const CSeries& series = *m_ChartControl.panels[n]->series[j];
			if( series.seriesType == OBJECT_SERIES_INDICATOR || 
					(series.seriesType == OBJECT_SERIES_LINE/* && series.isTwin*/) )//12/9/2009 ER Added isTwin
				vIndicators.push_back( series.szName );	  
	}


  for( n = 0; n != vIndicators.size(); ++n )
    m_ChartControl.RemoveSeries( vIndicators[ n ] );

  m_iPanelCountLeft = iPanelsCount - n;//left the panels with data
  //remove drawings
}

bool CStockChartXSerializer_General::IgnorePanel(const CChartPanel &panel)
{  
  int seriesCount = panel.series.size();
 	for( int seriesIdx = 0; seriesIdx < seriesCount; ++seriesIdx )
		if( panel.series[ seriesIdx ]->seriesType == OBJECT_SERIES_INDICATOR/* || panel.series[ seriesIdx ]->szName.Find("volume")>0*/)
      return false;
 
  return true;
}

bool CStockChartXSerializer_General::IgnoreSeries(const CSeries &series)
{
  return series.seriesType != OBJECT_SERIES_INDICATOR;//ignore any non-indicator series
}

void CStockChartXSerializer_General::ReadTextAreas(CChartPanel &panel)
{
  //do not read nothing
}

void CStockChartXSerializer_General::ReadObjects(CChartPanel &panel)
{
  //do not read nothing
}

void CStockChartXSerializer_General::ReadLineStudyObjects(CChartPanel &panel)
{
  //do not read nothing
}

void CStockChartXSerializer_General::ReadHorizontalLines(CChartPanel &panel)
{
  //do not read nothing
}

void CStockChartXSerializer_General::ReadSeriesData(const IOSeries &ioSeries,CSeries &series)
{
  //do not read any data
}

CSeries* CStockChartXSerializer_General::AddSeries(const IOSeries &ioSeries,int iPanelIdx)
{  
  //"if" is useles, but it has a chance to survive :)
  if( ioSeries.m_seriesType == OBJECT_SERIES_INDICATOR )
  {
    m_ChartControl.AddIndicatorSeries( ioSeries.m_indicatorType, ioSeries.m_name, iPanelIdx, FALSE );	
    return m_ChartControl.panels[ iPanelIdx ]->series[ m_ChartControl.panels[ iPanelIdx ]->series.size() - 1 ];
  }
  m_ChartControl.ThrowErr( 1010, "Non-indicator series found while loading general template.");
  return NULL;
}

void CStockChartXSerializer_General::UpdateScreen(bool SetStartEndIndex /*=true*/)
{	
	CStockChartXSerializer::UpdateScreen(false);
}

//------------------------------------------------------------------------

long CStockChartXSerializer_Objects::Save()
{	
	SaveStudyControl();
	/*
  int iPanelsCount;
  
  SaveSignature();
  SaveVersion();
  iPanelsCount = m_ChartControl.GetVisiblePanelCount();

  m_Serializer << iPanelsCount;
  for( int panelIdx = 0; panelIdx < iPanelsCount; panelIdx++ )
  {
    CChartPanel &panel = *m_ChartControl.panels[ panelIdx ];

    SaveTextAreas( panel );
    SaveObjects( panel );
    SaveLineObjects( panel );
    SaveHorizontalLines( panel );
  }
  */
  
  return 1;
}

long CStockChartXSerializer_Objects::Load()
{
	
#ifdef _CONSOLE_DEBUG
  printf("\nCSXSerializer_Objects::Load()");
#endif
	LoadStudyControl();
	/*
  int iPanelsCount, iPanelsCountCurrent;

  m_startIndex = m_ChartControl.startIndex; // 9/29/05
  m_endIndex = m_ChartControl.endIndex;		// 9/29/05

  ReadSignature();
  ReadVersion();
  iPanelsCountCurrent = m_ChartControl.GetVisiblePanelCount();
  m_Serializer >> iPanelsCount;

  iPanelsCount = iPanelsCount < iPanelsCountCurrent ? iPanelsCount : iPanelsCountCurrent;//read as much as we have panels at the moment
  for( int panelIdx = 0; panelIdx < iPanelsCount; panelIdx++ )
  {
    CChartPanel &panel = *m_ChartControl.panels[ panelIdx ];

    ReadTextAreas( panel );
    ReadObjects( panel );
    ReadLineStudyObjects( panel );
    ReadHorizontalLines( panel );
    UpdateScreen();
  }
  */

  return 1;
}

void CStockChartXSerializer_Objects::SaveSignature()
{
  m_Serializer << SIGN_TMPL_OBJECTS;
}

void CStockChartXSerializer_Objects::ReadSignature()
{
  int iSign;
  
  m_Serializer >> iSign;
  if( iSign != SIGN_TMPL_OBJECTS )
  {
    m_ChartControl.ThrowErr( 1010 , "Wrong signature for object template." );
  }
}


void CStockChartXSerializer_Objects::ReadChartPanels()
{
  IOPanel	ioPanel;
  int panelIdx;

  std::vector<IOPanel> panelSizes;

  for( panelIdx	= 0; panelIdx < m_iPanelsCount; panelIdx++ )
  {
    //	Load the panel
    ioPanel.reset();
    m_Serializer >> ioPanel;
    
    //CChartPanel*	panel	= *m_ChartControl.panels[ iFilePanelIndex ];
    //panel.Connect( &m_ChartControl );
    
    //panel.y1	= (double)m_ChartControl.height * ioPanel.m_Y1;
    //panel.y2	= (double)m_ChartControl.height * ioPanel.m_Y2;
    
    CChartPanel &panel = *ReadPanelsSeries( ioPanel );
    panel.Connect( &m_ChartControl );
    
   // panel.y1	= (double)m_ChartControl.height * ioPanel.m_Y1;
   // panel.y2	= (double)m_ChartControl.height * ioPanel.m_Y2;

	panelSizes.push_back(ioPanel);

    ReadTextAreas( panel );

    ReadObjects( panel );

    ReadLineStudyObjects( panel );

    ReadHorizontalLines( panel );
  }

  
}

void CStockChartXSerializer_Objects::UpdateScreen(bool SetStartEndIndex /*=true*/)
{	
	CStockChartXSerializer::UpdateScreen(false);
}

// Save StockChartXCtl::UserStudyLine<struct>
void CStockChartXSerializer_Objects::SaveStudyControl(void)
{
	
#ifdef _CONSOLE_DEBUG
							printf("\nCStockChartXSerializer_Objects::SaveStudyControl() objectsCount = %d",m_ChartControl.userStudyLine.size());
#endif
  IOStudyControl	ioStudyControl;
  int	objectsCount	= m_ChartControl.userStudyLine.size();

  m_Serializer << objectsCount;
  
  for( int oIdx = 0; oIdx != objectsCount; oIdx++ )
  {    
	 //if( /*CString(m_ChartControl.userStudyLine[oIdx].objectType)=="Polyline"||*/CString(m_ChartControl.userStudyLine[oIdx].objectType).GetLength()<3) continue;
	 ioStudyControl.reset();

	setIOString( (TCHAR*)( &ioStudyControl.m_SerieName ),m_ChartControl.userStudyLine[oIdx].SerieName);
	setIOString( (TCHAR*)( &ioStudyControl.m_objectType ),m_ChartControl.userStudyLine[oIdx].objectType);
	setIOString( (TCHAR*)( &ioStudyControl.m_key ),m_ChartControl.userStudyLine[oIdx].key);//
	setIOString( (TCHAR*)( &ioStudyControl.m_Text ),m_ChartControl.userStudyLine[oIdx].Text);//
	setIOString( (TCHAR*)( &ioStudyControl.m_text ),m_ChartControl.userStudyLine[oIdx].text);
	setIOString( (TCHAR*)( &ioStudyControl.m_fileName ),m_ChartControl.userStudyLine[oIdx].fileName);
	//setIOVectorInt((LONG*)(&ioStudyControl.m_xValues),(LONG*)&m_ChartControl.userStudyLine[oIdx].xValues);
	//ioStudyControl.m_xValues=m_ChartControl.userStudyLine[oIdx].xValues;
	//memmove(&ioStudyControl.m_xValues,&m_ChartControl.userStudyLine[oIdx].xValues,((int)m_ChartControl.userStudyLine[oIdx].xValues.size()) * sizeof(LONG));
#ifdef _CONSOLE_DEBUG
	/*if(ioStudyControl.m_objectType == "Polyline"){
							printf("\nStudy trying to serialize with \n");
							for(int i=0;i<10;i++){
								printf("\tparams[%d]=%f\n",i,ioStudyControl.m_params[i]);
							}
		}*/
#endif
	//if(m_ChartControl.userStudyLine[oIdx].xValues.size()!=m_ChartControl.userStudyLine[oIdx].yValues.size() || m_ChartControl.userStudyLine[oIdx].xValues.size() != m_ChartControl.userStudyLine[oIdx].xJDates.size())continue;
	for(int i=0;i<m_ChartControl.userStudyLine[oIdx].xValues.size();i++){
		ioStudyControl.m_xValues[i]=m_ChartControl.userStudyLine[oIdx].xValues[i];
		ioStudyControl.m_xJDates[i]=m_ChartControl.userStudyLine[oIdx].xJDates[i];
		ioStudyControl.m_yValues[i]=m_ChartControl.userStudyLine[oIdx].yValues[i];
	}
	for(int i=0;(i<m_ChartControl.userStudyLine[oIdx].params.size())&&(i<10);i++){
		ioStudyControl.m_params[i]=m_ChartControl.userStudyLine[oIdx].params[i];
	}
	ioStudyControl.m_lineColor= m_ChartControl.userStudyLine[oIdx].lineColor;
	ioStudyControl.m_fontColor= m_ChartControl.userStudyLine[oIdx].fontColor;//
	ioStudyControl.m_backColor= m_ChartControl.userStudyLine[oIdx].backColor;
	ioStudyControl.m_foreColor= m_ChartControl.userStudyLine[oIdx].foreColor;
	ioStudyControl.m_x1=m_ChartControl.userStudyLine[oIdx].x1;
	ioStudyControl.m_y1=m_ChartControl.userStudyLine[oIdx].y1;
	ioStudyControl.m_x2=m_ChartControl.userStudyLine[oIdx].x2;
	ioStudyControl.m_y2=m_ChartControl.userStudyLine[oIdx].y2;
	ioStudyControl.m_x1Value=m_ChartControl.userStudyLine[oIdx].x1Value;//									
	ioStudyControl.m_x2Value=m_ChartControl.userStudyLine[oIdx].x2Value;//
	ioStudyControl.m_y1Value=m_ChartControl.userStudyLine[oIdx].y1Value;//
	ioStudyControl.m_y2Value=m_ChartControl.userStudyLine[oIdx].y2Value;//
	ioStudyControl.m_x1JDate=m_ChartControl.userStudyLine[oIdx].x1JDate;//
	ioStudyControl.m_x2JDate=m_ChartControl.userStudyLine[oIdx].x2JDate;//
	ioStudyControl.m_selectable=m_ChartControl.userStudyLine[oIdx].selectable;//
	ioStudyControl.m_selected=m_ChartControl.userStudyLine[oIdx].selected;//
    ioStudyControl.m_zOrder=m_ChartControl.userStudyLine[oIdx].zOrder;//
	ioStudyControl.m_lineStyle=m_ChartControl.userStudyLine[oIdx].lineStyle;
	ioStudyControl.m_lineWeight=m_ChartControl.userStudyLine[oIdx].lineWeight;
	ioStudyControl.m_x1Value_2=m_ChartControl.userStudyLine[oIdx].x1Value_2;
	ioStudyControl.m_y1Value_2=m_ChartControl.userStudyLine[oIdx].y1Value_2;
	ioStudyControl.m_x2Value_2=m_ChartControl.userStudyLine[oIdx].x2Value_2;
	ioStudyControl.m_y2Value_2=m_ChartControl.userStudyLine[oIdx].y2Value_2;
	ioStudyControl.m_x1JDate_2=m_ChartControl.userStudyLine[oIdx].x1JDate_2;
	ioStudyControl.m_x2JDate_2=m_ChartControl.userStudyLine[oIdx].x2JDate_2;
	ioStudyControl.m_upOrDown=m_ChartControl.userStudyLine[oIdx].upOrDown;
	ioStudyControl.m_fillStyle=m_ChartControl.userStudyLine[oIdx].fillStyle;
	ioStudyControl.m_vFixed=m_ChartControl.userStudyLine[oIdx].vFixed;
	ioStudyControl.m_hFixed=m_ChartControl.userStudyLine[oIdx].hFixed;
    ioStudyControl.m_isFirstPointArrow=m_ChartControl.userStudyLine[oIdx].isFirstPointArrow;
	ioStudyControl.m_radiusExtension=m_ChartControl.userStudyLine[oIdx].radiusExtension;
	ioStudyControl.m_rightExtension=m_ChartControl.userStudyLine[oIdx].rightExtension;
	ioStudyControl.m_leftExtension = m_ChartControl.userStudyLine[oIdx].leftExtension;
	ioStudyControl.m_valuePosition = m_ChartControl.userStudyLine[oIdx].valuePosition;
	ioStudyControl.m_startX=m_ChartControl.userStudyLine[oIdx].startX;//
	ioStudyControl.m_startY=m_ChartControl.userStudyLine[oIdx].startY;//
	ioStudyControl.m_fontSize=m_ChartControl.userStudyLine[oIdx].fontSize;//
	ioStudyControl.m_bitmapID=m_ChartControl.userStudyLine[oIdx].bitmapID;
	ioStudyControl.m_nType=m_ChartControl.userStudyLine[oIdx].nType;
	ioStudyControl.m_typing = m_ChartControl.userStudyLine[oIdx].typing;//
	ioStudyControl.m_gotUserInput = m_ChartControl.userStudyLine[oIdx].gotUserInput;//

#ifdef _CONSOLE_DEBUG
	/*if(ioStudyControl.m_objectType == "Polyline"){
							printf("\nStudy serialized with \n");
							for(int i=0;i<10;i++){
								printf("\tparams[%d]=%f\n",i,ioStudyControl.m_params[i]);
							}
		}*/
#endif

    m_Serializer << ioStudyControl;
  }
#ifdef _CONSOLE_DEBUG
	//VINCIUS
  printf("\n\nSerializerX_Objects:SaveUserControl userStudyLine=%d m_symbol=%s",m_ChartControl.userStudyLine.size(),m_ChartControl.m_symbol);
#endif
}
// Load StockChartXCtl::UserStudyLine<struct>
void CStockChartXSerializer_Objects::LoadStudyControl(void)
{
	
  int	objectsCount	= 0;
  IOStudyControl	ioStudyControl;

  m_Serializer >> objectsCount;

  m_ChartControl.userStudyLine.clear();
  
#ifdef _CONSOLE_DEBUG
  printf("\nSerializerX_Objects:LoadUserControl() userStudyLine=%d m_symbol=%s",objectsCount,m_ChartControl.m_symbol);
#endif

  for( int oIdx = 0; oIdx != objectsCount; oIdx++ )
  {
	  //if( /*CString(ioStudyControl.m_objectType)=="Polyline" || */CString(ioStudyControl.m_objectType).GetLength()<3) continue;
	UserStudyLine studyControl;
	
    ioStudyControl.reset();
    m_Serializer >> ioStudyControl;
	
	

	studyControl.SerieName = ioStudyControl.m_SerieName;
	studyControl.objectType = ioStudyControl.m_objectType;
	studyControl.key = ioStudyControl.m_key;//
	studyControl.Text = ioStudyControl.m_Text;//
	studyControl.text = ioStudyControl.m_text;
	studyControl.fileName = ioStudyControl.m_fileName;
	studyControl.lineColor = ioStudyControl.m_lineColor;
	studyControl.fontColor = ioStudyControl.m_fontColor;//
	studyControl.backColor = ioStudyControl.m_backColor;
	studyControl.foreColor = ioStudyControl.m_foreColor;
	studyControl.x1 = ioStudyControl.m_x1;
	studyControl.y1 = ioStudyControl.m_y1;
	studyControl.x2 = ioStudyControl.m_x2;
	studyControl.y2 = ioStudyControl.m_y2;
	studyControl.x1Value = ioStudyControl.m_x1Value;//
	studyControl.x2Value = ioStudyControl.m_x2Value;//
	studyControl.y1Value = ioStudyControl.m_y1Value;//
	studyControl.y2Value = ioStudyControl.m_y2Value;//
	studyControl.x1JDate = ioStudyControl.m_x1JDate;//
	studyControl.x2JDate = ioStudyControl.m_x2JDate;//
	studyControl.selectable = ioStudyControl.m_selectable;//
	studyControl.selected = ioStudyControl.m_selected;//
    studyControl.zOrder = ioStudyControl.m_zOrder;//
	studyControl.lineStyle = ioStudyControl.m_lineStyle;
	studyControl.lineWeight = ioStudyControl.m_lineWeight;
	studyControl.x1Value_2 = ioStudyControl.m_x1Value_2;
	studyControl.y1Value_2 = ioStudyControl.m_y1Value_2;
	studyControl.x2Value_2 = ioStudyControl.m_x2Value_2;
	studyControl.y2Value_2 = ioStudyControl.m_y2Value_2;
	studyControl.x1JDate_2 = ioStudyControl.m_x1JDate_2;
	studyControl.x2JDate_2 = ioStudyControl.m_x2JDate_2;
	studyControl.valuePosition = ioStudyControl.m_valuePosition;
	studyControl.xValues.clear();
	studyControl.xJDates.clear();
	studyControl.yValues.clear();
	studyControl.params.clear();
	for(int i=0;ioStudyControl.m_yValues[i]!=NULL_VALUE;i++){
		studyControl.xValues.push_back(ioStudyControl.m_xValues[i]);
		studyControl.xJDates.push_back(ioStudyControl.m_xJDates[i]);
		studyControl.yValues.push_back(ioStudyControl.m_yValues[i]);
	}
	for(int i=0;(i<10);i++){
		studyControl.params.push_back(ioStudyControl.m_params[i]);
	}
	studyControl.upOrDown = ioStudyControl.m_upOrDown;
	studyControl.fillStyle = ioStudyControl.m_fillStyle;
	studyControl.vFixed = ioStudyControl.m_vFixed;
	studyControl.hFixed = ioStudyControl.m_hFixed;
    studyControl.isFirstPointArrow = ioStudyControl.m_isFirstPointArrow;
	studyControl.radiusExtension = ioStudyControl.m_radiusExtension;
	studyControl.rightExtension = ioStudyControl.m_rightExtension;
	studyControl.leftExtension = ioStudyControl.m_leftExtension;
	studyControl.startX = ioStudyControl.m_startX;//
	studyControl.startY = ioStudyControl.m_startY;//
	studyControl.fontSize = ioStudyControl.m_fontSize;//
	studyControl.bitmapID = ioStudyControl.m_bitmapID;
	studyControl.nType = ioStudyControl.m_nType;   
	studyControl.typing = ioStudyControl.m_typing;
	studyControl.gotUserInput = ioStudyControl.m_gotUserInput;//
	m_ChartControl.userStudyLine.push_back(studyControl);//
#ifdef _CONSOLE_DEBUG
	printf("\n\tStudy %s unserialized with:",studyControl.SerieName);
							for(int i=0;i<10;i++){
								printf("\n\t\tparamsLoad[%d]=%f params[%d]=%f",i,ioStudyControl.m_params[i],i,studyControl.params[i]);
							}
#endif

 }
  
  
}
