
/*****************************************************************************
FILE       : IOStructures.cpp
Author     : Svetoslav Chekanov
Description: 

Copyright (c) 2004 D-Bross
*****************************************************************************/
/* #    Revisions    # */

/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IOStructures.h"

/////////////////////////////////////////////////////////////////////////////

const	ioColor		IO_COLOR_BLACK	= 0x00000000;

/////////////////////////////////////////////////////////////////////////////

void	initIOString( TCHAR* ios )
{
	for( int i = 0; i < IOSTRING_SIZE; i++ )
		ios[i]	= 0x00;

	ios[IOSTRING_SIZE]	= 0x00;
}
/////////////////////////////////////////////////////////////////////////////

void	setIOString( TCHAR* ios, const TCHAR* s )
{
	int	size	= _tcslen( s );
	if( size >= IOSTRING_SIZE )
		size	= IOSTRING_SIZE;

	memmove( ios, s, size * sizeof( TCHAR ) );
	ios[size]	= 0x00;
}
/////////////////////////////////////////////////////////////////////////////

void	initIOVectorDbl( DOUBLE* ios )
{
	for( int i = 0; i < IOVECTOR_SIZE; i++ )
		ios[i]	= 0x00;

	ios[IOVECTOR_SIZE]	= 0x00;
}
/////////////////////////////////////////////////////////////////////////////

void	setIOVectorDbl( DOUBLE* ios, const DOUBLE* s )
{
	int	size	= sizeof( s );
	if( size >= IOVECTOR_SIZE )
		size	= IOVECTOR_SIZE;
	for(int i=0;i<size;i++){
		ios[i]=s[i];
	}
	ios[size]	= 0x00;
}
/////////////////////////////////////////////////////////////////////////////

void	initIOVectorInt( LONG* ios )
{
	for( int i = 0; i < IOVECTOR_SIZE; i++ )
		ios[i]	= 0x00;

	ios[IOVECTOR_SIZE]	= 0x00;
}
/////////////////////////////////////////////////////////////////////////////

void	setIOVectorInt( std::vector<int,std::allocator<int>>* ios, const std::vector<int,std::allocator<int>>* s )
{
	int	size	= s->size();
	if( size >= IOVECTOR_SIZE )
		size	= IOVECTOR_SIZE;

	memmove( ios, s, size * sizeof( TCHAR ) );

	

}
/////////////////////////////////////////////////////////////////////////////


IOWorkspace::IOWorkspace()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOWorkspace::~IOWorkspace()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOWorkspace::reset()
{
	initIOString( m_symbol );
	m_backColor			= IO_COLOR_BLACK;
	m_lineColor			= IO_COLOR_BLACK;
	m_lineThickness		= 1;
	m_foreColor			= IO_COLOR_BLACK;
	m_gridColor			= IO_COLOR_BLACK;
	m_upColor			= IO_COLOR_BLACK;
	m_downColor			= IO_COLOR_BLACK;
	m_infoPanelColor	= IO_COLOR_BLACK;
	m_displayTitles		= true;
	m_horizontalSeparators= true;
	m_horizontalSeparatorColor	= IO_COLOR_BLACK;
	m_threeDStyle		= true;
	m_startX			= 0;
	m_endX				= 0;
	m_extendedX			= 0;
	m_scalingType		= 0;
	m_priceStyle		= 0;
	m_showXGrid			= true;
	m_showYGrid			= true;
	m_YGridAlignment	= 0;
	m_realtimeXLabels	= true;
	m_darvasBoxes		= false;
	m_panelsCount		= 0;
	m_textAreaFontSize	= DEFAULT_FONT_SIZE;
	initIOString (m_textAreaFontName);
	m_wickColor			= RGB(0,255,0);

}
/////////////////////////////////////////////////////////////////////////////

IOPanel::IOPanel()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOPanel::~IOPanel()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOPanel::reset()
{
	m_Y1	= 0.0;
	m_Y2	= 0.0;

	m_seriesCount	= 0;
}
/////////////////////////////////////////////////////////////////////////////

IOIndicator::IOIndicator()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOIndicator::~IOIndicator()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOIndicator::reset()
{
	initIOString( (TCHAR*)&m_str );
	m_double	= 0.0;
	m_int		= 0;
};
/////////////////////////////////////////////////////////////////////////////

IOSeries::IOSeries()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOSeries::~IOSeries()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOSeries::reset()
{
	initIOString( (TCHAR*)&m_name  );
	initIOString( (TCHAR*)&m_title );
	m_seriesType	= 0;
	m_indicatorType	= 0;
	initIOString( (TCHAR*)&m_link );
	m_userParams	= false;
	m_lineStyle		= 0;
	m_lineWeight	= 1;
	m_lineColor		= IO_COLOR_BLACK;
	m_upColor		= IO_COLOR_BLACK;
	m_downColor		= IO_COLOR_BLACK;
	m_selected		= false;
	m_selectable	= true;
	m_shareScale	= true;

	m_indParamCount	= 0;
	m_valuesCount		= 0;

	//TWIN
	m_lineColorSignal = RGB(255,0,0);
	m_lineStyleSignal = 0;
	m_lineWeightSignal = 1;
};
/////////////////////////////////////////////////////////////////////////////


IOValue::IOValue()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOValue::~IOValue()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOValue::reset()
{
	m_value	= 0.0;
	m_jDate	= 0.0;
}
/////////////////////////////////////////////////////////////////////////////

IOTextArea::IOTextArea()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOTextArea::~IOTextArea()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOTextArea::reset()
{
	initIOString( m_key  );
	initIOString( m_text );
	m_fontColor		= IO_COLOR_BLACK;
	m_fontSize		= 12;
	m_startX		= 0;
	m_startY		= 0;
	m_X1			= 0.0;
	m_Y1			= 0.0;
	m_X2			= 0.0;
	m_Y2			= 0.0;
	m_selectable	= true;
	m_selected		= false;
}
/////////////////////////////////////////////////////////////////////////////

IOObject::IOObject()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOObject::~IOObject()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOObject::reset()
{
	initIOString( m_objectType );
	initIOString( m_key  );
	initIOString( m_text );
	initIOString( m_fileName );
	m_backColor		= IO_COLOR_BLACK;
	m_foreColor		= IO_COLOR_BLACK;
	m_lineStyle		= 0;
	m_lineWeight	= 1;
	m_X1			= 0.0;
	m_Y1			= 0.0;
	m_X2			= 0.0;
	m_Y2			= 0.0;
	m_selectable	= true;
	m_selected		= false;
	m_visible		= true;
	m_zOrder		= 0;
	

}
/////////////////////////////////////////////////////////////////////////////

IOStudyControl::IOStudyControl()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOStudyControl::~IOStudyControl()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOStudyControl::reset()
{	
	initIOString( m_SerieName );
	initIOString( m_objectType );
    initIOString( m_key );
	initIOString( m_Text );
	initIOString( m_text );
	initIOString( m_fileName );
    m_lineColor= IO_COLOR_BLACK;
	m_fontColor= IO_COLOR_BLACK;
	m_backColor= IO_COLOR_BLACK;
	m_foreColor= IO_COLOR_BLACK;
	/*
	int m_x1=0;
	int m_y1=0;
	int m_x2=0;
	int m_y2=0;
	double m_x1Value=0.0;
	double m_x2Value=0.0;
	double m_y1Value=0.0;
	double m_y2Value=0.0;
	double m_x1JDate=0.0;
	double m_x2JDate=0.0;
    bool m_selectable=true;
    bool m_selected=false;
    bool m_zOrder=false;
    long m_lineStyle=0;
    long m_lineWeight=1;
	double m_x1Value_2=0.0;
    double m_y1Value_2=0.0;
    double m_x2Value_2=0.0;
    double m_y2Value_2=0.0;
	double m_x1JDate_2=0.0;
	double m_x2JDate_2=0.0;
	long m_fillStyle=0;
	bool m_vFixed=false;
	bool m_hFixed=false;
    bool m_isFirstPointArrow=false;
	bool m_radiusExtension=false;
	bool m_rightExtension=false;
	bool m_leftExtension=false;
	int m_startX=0;
	int m_startY=0;
	int m_fontSize=0;
	long	m_bitmapID=0;
	int m_nType=0;
	bool m_typing=false;
	bool m_gotUserInput=true;
	*/
	m_x1=0;
	m_y1=0;
	m_x2=0;
	m_y2=0;
	m_x1Value=0.0;
	m_x2Value=0.0;
	m_y1Value=0.0;
	m_y2Value=0.0;
	m_x1JDate=0.0;
	m_x2JDate=0.0;
    m_selectable=true;
    m_selected=false;
    m_zOrder=false;
    m_lineStyle=0;
    m_lineWeight=1;
	m_x1Value_2=0.0;
    m_y1Value_2=0.0;
    m_x2Value_2=0.0;
    m_y2Value_2=0.0;
	m_x1JDate_2=0.0;
	m_x2JDate_2=0.0;
	m_fillStyle=0;
	m_vFixed=false;
	m_hFixed=false;
    m_isFirstPointArrow=false;
	m_radiusExtension=false;
	m_rightExtension=false;
	m_leftExtension=false;
	m_valuePosition = -1;
	m_startX=0;
	m_startY=0;
	m_fontSize=0;
	m_bitmapID=0;
	m_nType=0;
	m_typing=false;
	m_gotUserInput=true;
	for(int i=0;i<100;i++){
		m_xValues[i]=NULL_VALUE;
		m_xJDates[i]=NULL_VALUE;
		m_yValues[i]=NULL_VALUE;
	}
	for(int i=0;i<10;i++){
		m_params[i]=NULL_VALUE;
	}
}
/////////////////////////////////////////////////////////////////////////////

IOLineObject::IOLineObject()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOLineObject::~IOLineObject()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOLineObject::reset()
{
	initIOString( m_objectType );
	initIOString( m_key );
	m_lineStyle		= 0;
	m_lineWeight	= 1;
	m_lineColor		= IO_COLOR_BLACK;
	m_X1			= 0.0;
	m_Y1			= 0.0;
	m_X2			= 0.0;
	m_Y2			= 0.0;
	m_selectable	= true;
	m_selected		= false;
	m_zOrder		= 0;
	m_vFixed = false;
	m_hFixed = false;
    m_isFirstPointArrow = false;
	m_radiusExtension = false;
	m_rightExtension = false;
	m_leftExtension = false;
	m_valuePosition = -1;
	
}
/////////////////////////////////////////////////////////////////////////////

IOHorizontalLine::IOHorizontalLine()
{
	reset();
}
/////////////////////////////////////////////////////////////////////////////

IOHorizontalLine::~IOHorizontalLine()
{
}
/////////////////////////////////////////////////////////////////////////////

//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
void IOHorizontalLine::reset()
{
	m_Y	= 0;
}
/////////////////////////////////////////////////////////////////////////////

