
#ifndef __IOSTRUCTURES_H__
#define __IOSTRUCTURES_H__

/*****************************************************************************
FILE       : IOStructures.h
Author     : Svetoslav Chekanov
Description: 

Copyright (c) 2004 D-Bross
*****************************************************************************/
/* #    Revisions    # */

/////////////////////////////////////////////////////////////////////////////

typedef	unsigned char		byte;
typedef	unsigned int		uint32;
//typedef	unsigned long		uint64;
typedef	unsigned int		ioColor;

#define	IOSTRING_SIZE	200
#define IOVECTOR_SIZE	200
#define	ioString(name)		TCHAR	name[IOSTRING_SIZE+1]
#define	ioVectorDbl(name)		DOUBLE	name[IOVECTOR_SIZE+1]
#define	ioVectorInt(name)		LONG	name[IOVECTOR_SIZE+1]

/////////////////////////////////////////////////////////////////////////////

void	initIOString( TCHAR* ios );
void	setIOString( TCHAR* ios, const TCHAR* s );

void	initIOVectorDbl( DOUBLE* ios );
void	setIOVectorDbl( DOUBLE* ios, const DOUBLE* s );

void	initIOVectorInt( LONG* ios );
void	setIOVectorInt( std::vector<int,std::allocator<int>>* ios, const std::vector<int,std::allocator<int>>* s );
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////

class	IOWorkspace
{
public:
	IOWorkspace();
	~IOWorkspace();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	ioString( m_symbol );
	ioColor		m_backGradientTop;
	ioColor		m_backGradientBottom;
	ioColor		m_valueViewGradientTop;
	ioColor		m_valueViewGradientBottom;
	ioColor		m_backColor;
	ioColor		m_foreColor;
	ioColor		m_gridColor;
	ioColor		m_upColor;
	ioColor		m_downColor;
	ioColor		m_infoPanelColor;
	ioColor		m_lineColor;
	int			m_lineThickness;
	bool		m_displayTitles;
	bool		m_horizontalSeparators;
	ioColor		m_horizontalSeparatorColor;
	bool		m_threeDStyle;
	int			m_startX;
	int			m_endX;
	int			m_extendedX;
	long		m_scalingType;
	long		m_priceStyle;
	bool		m_showXGrid;
	bool		m_showYGrid;
	long		m_YGridAlignment;
	bool		m_realtimeXLabels;
	bool		m_darvasBoxes;
	int			m_panelsCount;
	long		m_textAreaFontSize;
	ioString(	m_textAreaFontName);
	long		m_wickColor;
	bool		m_useVolumeUpDownColors;
};
/////////////////////////////////////////////////////////////////////////////

class	IOPanel
{
public:
	IOPanel();
	~IOPanel();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	double		m_Y1;
	double		m_Y2;

	int			m_seriesCount;
};
/////////////////////////////////////////////////////////////////////////////

class	IOIndicator
{
public:
	IOIndicator();
	~IOIndicator();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	ioString( m_str );
	double		m_double;
	int			m_int;
};
/////////////////////////////////////////////////////////////////////////////

class	IOSeries
{
public:
	IOSeries();
	~IOSeries();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	ioString( m_name  );
	ioString( m_title );
	long		m_seriesType;
	long		m_indicatorType;
	ioString( m_link );
	bool		m_userParams;
	int			m_lineStyle;
	int			m_lineWeight;
	ioColor		m_lineColor;
	ioColor		m_upColor;
	ioColor		m_downColor;
	bool		m_selected;
	bool		m_selectable;
	bool		m_shareScale;
	bool		m_seriesVisible;

	int			m_indParamCount;
	int			m_valuesCount;

	//TWIN	
	ioColor m_lineColorSignal;
	int m_lineStyleSignal;
	int m_lineWeightSignal;

};
/////////////////////////////////////////////////////////////////////////////

class	IOValue
{
public:
	IOValue();
	~IOValue();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	double		m_value;
	double		m_jDate;
};
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////

class	IOTextArea
{
public:
	IOTextArea();
	~IOTextArea();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	ioString( m_key  );
	ioString( m_text );
	ioColor		m_fontColor;
	int			m_fontSize;
	int			m_startX;
	int			m_startY;
	double		m_X1;
	double		m_Y1;
	double		m_X2;
	double		m_Y2;
	bool		m_selectable;
	bool		m_selected;
	int			m_zOrder;
};
/////////////////////////////////////////////////////////////////////////////

class	IOObject
{
public:
	IOObject();
	~IOObject();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	ioString( m_objectType );
	ioString( m_key  );
	ioString( m_text );
	ioString( m_fileName );
	ioColor		m_backColor;
	ioColor		m_foreColor;
	int			m_lineStyle;
	int			m_lineWeight;
	double		m_X1;
	double		m_Y1;
	double		m_X2;
	double		m_Y2;
	bool		m_selectable;
	bool		m_selected;
	bool		m_visible;
	int			m_zOrder;
};
/////////////////////////////////////////////////////////////////////////////

class	IOStudyControl
{
public:
	IOStudyControl();
	~IOStudyControl();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	
	ioString( m_SerieName );
	ioString( m_objectType );
    ioString( m_key );
	ioString( m_Text );
	ioString( m_text );
	ioString( m_fileName );
    ioColor m_lineColor;
	ioColor m_fontColor;
	ioColor	m_backColor;
	ioColor	m_foreColor;
	double m_x1;
	double m_y1;
	double m_x2;
	double m_y2;
	double m_x1Value;
	double m_x2Value;
	double m_y1Value;
	double m_y2Value;
	double m_x1JDate;
	double m_x2JDate;
    bool m_selectable;
    bool m_selected;
    bool m_zOrder;
    long m_lineStyle;
    long m_lineWeight;
	double m_x1Value_2;
    double m_y1Value_2;
    double m_x2Value_2;
    double m_y2Value_2;
	double m_x1JDate_2;
	double m_x2JDate_2;
	double m_xValues[100];
	double m_xJDates[100];
	double m_yValues[100];
	int m_upOrDown;
	long m_fillStyle;
	bool m_vFixed;
	bool m_hFixed;
    bool m_isFirstPointArrow;
	bool m_radiusExtension;
	bool m_rightExtension;
	bool m_leftExtension;
	double m_valuePosition;
	double m_startX;
	double m_startY;
	int m_fontSize;
	long	m_bitmapID;
	int m_nType;
	bool m_typing;
	bool m_gotUserInput;
	double m_params[10];
};
/////////////////////////////////////////////////////////////////////////////
		
class	IOLineObject
{
public:
	IOLineObject();
	~IOLineObject();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	ioString( m_objectType );
	ioString( m_key );
	int			m_lineStyle;
	int			m_lineWeight;
	ioColor		m_lineColor;
	double		m_X1;
	double		m_Y1;
	double		m_X2;
	double		m_Y2;
	bool		m_selectable;
	bool		m_selected;
	int			m_zOrder;
	int			m_fillStyle;
	bool        m_vFixed;
	bool        m_hFixed;
    bool        m_isFirstPointArrow;
	bool        m_radiusExtension;
	bool        m_rightExtension;
	bool        m_leftExtension;
	double		m_X1_2;
	double		m_Y1_2;
	double		m_X2_2;
	double		m_Y2_2;
	double		m_x1JDate;
	double		m_x2JDate;
	double		m_x1JDate_2;
	double		m_x2JDate_2;
	double		m_valuePosition;
};
/////////////////////////////////////////////////////////////////////////////

class	IOHorizontalLine
{
public:
	IOHorizontalLine();
	~IOHorizontalLine();
//	Revision to rid of build warnings 6/10/2004
//	Addition of return type to function
//	Function returns nothing so I put void
	void reset();

public:
	double		m_Y;
};
/////////////////////////////////////////////////////////////////////////////


/*	__IOSTRUCTURES_H__	*/
#endif
