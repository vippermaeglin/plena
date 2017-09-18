// ValueView.cpp: implementation of the CValueView class.
//
//////////////////////////////////////////////////////////////////////


#include "stdafx.h"
#include "StockChartX.h"
#include "ValueView.h"

#include "julian.h"

#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CValueView::CValueView()
{	
	okToDraw = false;
	okTrendline = true;
	overidePoints = false;
	Text = "";
	x1 = 0;
	x2 = 0;
	y1 = 0;
	y2 = 0;

	oldRect.SetRect( -1,-1,-1,-1 );
}
/////////////////////////////////////////////////////////////////////////////

CValueView::~CValueView()
{
}
/////////////////////////////////////////////////////////////////////////////

/*	SGC	31.05.2004	BEG*/

void CValueView::Reset( bool updateRect )
{	
	this->Hide();

	/*	SGC	03.06.2004	BEG*/
	if( updateRect )
	{
		if( pCtrl != NULL )
			pCtrl->UpdateRect( oldRect );
	}
	/*	SGC	03.06.2004	END*/
}
/////////////////////////////////////////////////////////////////////////////

void CValueView::Show()
{	
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW()");
#endif
	//return;
	visible = true;
	okTrendline = false;

	/*	SGC	03.06.2004	BEG*/
	if( pCtrl == NULL ){
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW() RETURNED pCtrl == NULL!!!!!!!!!");
#endif
		return;
	}
	/*	SGC	03.06.2004	END*/
	/***/
	try	{
		newRect	= GetRect();
		if( newRect.left == -1 || newRect.top == -1 || newRect.right == -1 || newRect.bottom == -1 ){
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW() RETURNED newRect = -1!!!!!!!!!");
#endif
			return;
		}
	}
	catch(...)
	{		
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW() RETURNED catch(...)!!!!!!!!!");
#endif
		return;
	}

	CRect	rect	= CRect(newRect.left,newRect.top,newRect.right,newRect.bottom+10);
	
	if( (newRect.bottom == 0 && newRect.right == 0) || (newRect.top == 0 && newRect.left == 0)){
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW() RETURNED newRect = 0!!!!!!!!!");
#endif
		return;
	}

	if( oldRect.left == -1 || oldRect.top == -1 || oldRect.right == -1 || oldRect.bottom == -1 )
		oldRect	= rect;

	int	x1	= min( rect.left  , oldRect.left   );
	int	x2	= max( rect.right , oldRect.right  );
	int	y1	= min( rect.top   , oldRect.top    );
	int	y2	= max( rect.bottom, oldRect.bottom );

	CRect	bmpRect( x1,y1, x2,y2 );
	int	bmpW	= bmpRect.Width();
	int	bmpH	= bmpRect.Height();

	int	w	= rect.Width();
	int	h	= rect.Height();
	
	CDC*	screenDC	= pCtrl->GetScreen();

	CDC		mdc;
	CBitmap	mbmp;
	mdc.CreateCompatibleDC( screenDC );
	mbmp.CreateCompatibleBitmap( screenDC, bmpW,bmpH );
	CBitmap* pOldBmp1 = mdc.SelectObject( &mbmp );  // 6/8/05 - select back on return
	mdc.SetBkMode( OPAQUE );
	mdc.BitBlt( 0,0, bmpW,bmpH, &pCtrl->m_memDC, x1,y1, SRCCOPY );

	
	CDC		dc;
	CBitmap	bmp;
	dc.CreateCompatibleDC( screenDC );
	bmp.CreateCompatibleBitmap( screenDC, w,h );
	CBitmap* pOldBmp2 = dc.SelectObject( &bmp ); // 6/8/05 - select back on return
	dc.SetBkMode( pCtrl->m_memDC.GetBkMode() );
	dc.BitBlt( 0,0, w,h, &pCtrl->m_memDC, rect.left, rect.top, SRCCOPY );

	if( dc.m_hDC == 0 || mdc.m_hDC == 0 )
	{ // 6/8/05
		dc.SelectObject( pOldBmp2 );
		bmp.DeleteObject();
		mdc.SelectObject( pOldBmp1 );
		mbmp.DeleteObject();
		pCtrl->ReleaseScreen(screenDC);
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW() RETURNED dc.m_hDC == 0!!!!!!!!!");
#endif
		return;
	}
	
	/***/
	
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::SHOW() OK!!!!!!!!!");
#endif

	newRect.left	= 0;
	newRect.right	= 0;
	newRect.top		= 0;
	newRect.bottom	= 0;	

	if( pCtrl->panels.size() == 0 )
	{ // 6/8/05
		dc.SelectObject( pOldBmp2 );
		bmp.DeleteObject();
		mdc.SelectObject( pOldBmp1 );
		mbmp.DeleteObject();
		pCtrl->ReleaseScreen(screenDC);
		return;
	}

	//~~~

	OnPaint( &dc );
	int	x	= rect.left - bmpRect.left;
	int	y	= rect.top  - bmpRect.top;

//	mdc.FillSolidRect( 0,0, bmpW, bmpH, RGB( 100,100,100 ) );
	mdc.BitBlt( x,y, w,h, &dc, 0, 0, SRCCOPY );
	screenDC->BitBlt( x1,y1, bmpW,bmpH, &mdc, 0, 0, SRCCOPY );

/*	Debug: See what rectangle I am cleaning.
	CPen*	oldPen		= (CPen*)screenDC->SelectStockObject( WHITE_PEN );
	CBrush*	oldBrush	= (CBrush*)screenDC->SelectStockObject( NULL_BRUSH );
	screenDC->Rectangle( x1,y1,x2,y2 );
	screenDC->SelectObject( oldPen );
	screenDC->SelectObject( oldBrush );
*/

	// 6/8/05
	dc.SelectObject( pOldBmp2 );
	bmp.DeleteObject();
	mdc.SelectObject( pOldBmp1 );
	mbmp.DeleteObject();


	pCtrl->ReleaseDC( screenDC );

	// Save old rect
	oldRect = rect;//newRect;
	okTrendline = true;
}
/////////////////////////////////////////////////////////////////////////////

void CValueView::Hide()
{
	if(NULL == pCtrl) return;
	visible	= false;	
	pCtrl->UpdateRect( oldRect );
	oldRect.SetRect( -1,-1,-1,-1 );
}
/*	SGC	31.05.2004	END*/

/////////////////////////////////////////////////////////////////////////////

void CValueView::OnPaint(CDC *pDC)
{
	/*	SGC	31.05.2004	BEG*/
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::ONPAINT()");
#endif
	if( ! visible ){
#ifdef _CONSOLE_DEBUG
		printf("\nVALUE VIEW RETURNED !visible");
#endif
		return;
	}

	if( ! okToDraw || ! pCtrl->displayInfoText ){
#ifdef _CONSOLE_DEBUG
		printf("\nVALUE VIEW RETURNED !oktoDraw || ! pCtrl->displayInfoText");
#endif
		return;
	}

	if( ! Text.IsEmpty() ){
		values = Text;
	}
	else{
#ifdef _CONSOLE_DEBUG
		printf("\nVALUE VIEW Text.IsEmpty()");
#endif
	}

	//	Which panel is selected
	int p;
	for( int n = 0; n != pCtrl->panels.size(); ++n)
	{
		int	py1	= (int)pCtrl->panels[n]->y1;
		int	py2	= (int)pCtrl->panels[n]->y2;
		int	y	= pCtrl->m_point.y;
		if( y > py1 && y < py2 )
		{
			p = n;
			break;
		}
	}
	CFont newFont, newFontMax, newFontMin;			
	newFont.CreatePointFont(VALUE_FONT_SIZE-2, _T("Arial"), pDC);			
	newFontMax.CreatePointFont(VALUE_FONT_SIZE-1, _T("Arial Rounded MT Bold"), pDC);	
	//newFontMax.CreatePointFont(VALUE_FONT_SIZE-1, _T("Castellar"), pDC);			
	newFontMin.CreatePointFont(VALUE_FONT_SIZE-9, _T("Arial"), pDC);	
	CFont* pOldFont = pDC->SelectObject(&newFont);	

	//~~~

	CRect	rect	= GetRect();
	int	w	= rect.Width();
	int	h	= rect.Height();
	rect.top	= 0;
	rect.left	= 0;
	rect.right	= w+1;
	rect.bottom	= h+1;




	rect.left	+= 2;
	rect.right	= rect.left + Width();
	rect.bottom	= rect.top  + Height();
	pDC->SetTextColor( RGB(0,0,0) );
	pDC->MoveTo( rect.left, rect.top );
	pDC->SetBkMode( TRANSPARENT );
	//pDC->DrawText( values, -1, &rect, DT_WORDBREAK | DT_LEFT );
	//CRectF rectf=CRectF((double)rect.left,(double)rect.top,(double)rect.right,(double)rect.bottom);
	//pCtrl->pdcHandler->DrawText(values,rectf,"Arial Rounded MT",12,DT_CENTER,pCtrl->lineColor,pDC);	

	CString tokens[50];
	CString token,tokenAux;
	//tokens = strtok(values, "\n"); //get pointer to first token found and store in 0
                                       //place in array
	/*for(int i=0;tokens[i]!= NULL;i++){   //ensure a pointer was found
        tokens[i] = strtok(NULL, "\n"); //continue to tokenize the string
	}*/
	bool intraday=false;
	if(pCtrl->GetPeriodicity()<4)intraday=true;
	double pointXValue = (pCtrl->panels[0]->GetReverseX(pCtrl->m_point.x) + 1 + pCtrl->startIndex);
	double pointYValue = (pCtrl->panels[0]->GetReverseY(pCtrl->m_point.y));
	double x1JDate = pCtrl->panels[0]->series[0]->GetJDate((int)x1Value-1);
	double pointJDate = pCtrl->panels[0]->series[0]->GetJDate((int)pointXValue-1);
	double varY = ((pointYValue-y1Value)*100)/y1Value;
	double priceInterval = pointYValue-y1Value;
	int candleInterval = (int)abs(pointXValue-x1Value)+1;
	MMDDYYHHMMSS julianX1,julianP;
	julianX1 = CJulian::FromJDate((double)x1JDate);
	julianP = CJulian::FromJDate((double)pointJDate);
	int dateInterval = (CJulian::DaysFromDate(julianP.Year,julianP.Month,julianP.Day)-CJulian::DaysFromDate(julianX1.Year,julianX1.Month,julianX1.Day)); //DaysBetweenDates(x1Value,pointXValue);
	if(dateInterval<0) dateInterval*=-1;
	int hourInterval,minuteInterval;
	if(intraday){
		if(x1Value<pointXValue){
			hourInterval = julianP.Hour-julianX1.Hour;
			if(julianX1.Minute>julianP.Minute){
				hourInterval--;
				minuteInterval = julianP.Minute+60-julianX1.Minute;
			}
			else minuteInterval = julianP.Minute-julianX1.Minute;
		}
		else{
			hourInterval = julianX1.Hour-julianP.Hour;
			if(julianP.Minute>julianX1.Minute){
				hourInterval--;
				minuteInterval = julianX1.Minute+60-julianP.Minute;
			}
			else minuteInterval = julianX1.Minute-julianP.Minute;
		}
		if(hourInterval<0) {
			dateInterval-=1;
			hourInterval+=24;
		}
	}

#pragma region Measure
	if(pCtrl->DeltaCursor && p==0){		
		if (x2Value == NULL_VALUE || y2Value == NULL_VALUE) return;
		double xt1,yt1,xt2,yt2=NULL_VALUE;	
		//pCtrl->pdcHandler->DrawRectangle(CRectF(rect.left, rect.top, nTotalWidth, nTotalHeight+10), 1, 0, RGB(255,255,255), pDC);
		try{
			// Gradient value view, MTR 7/29/04
			if(pCtrl->valueViewGradientTop != 0){
				pCtrl->FadeVert(pDC, 
					pCtrl->valueViewGradientTop,
					pCtrl->valueViewGradientBottom,
					rect);
			}
			else{
				pDC->FillSolidRect( rect.left,rect.top, nTotalWidth, nTotalHeight+10, RGB(232,232,239));
			}
			pDC->Draw3dRect( rect.left, rect.top, nTotalWidth-2, nTotalHeight+10, RGB(51,153,255), RGB(51,153,255) );
			//token.Format("DELTA CURSOR!!!\nstartX=%f startY=%f\n\nendX=%f endY=%f",x1Value,y1Value,(pCtrl->panels[0]->GetReverseX(pCtrl->m_point.x) + 1 + pCtrl->startIndex),(pCtrl->panels[0]->GetReverseY(pCtrl->m_point.y)));
			//Show % variation		
			
			//Show VARIATION
			pDC->SelectObject(&newFontMax);
			if(varY>=0){
				token.Format("+%.2f",varY);
				token+="\%";
				pDC->SetTextColor( RGB(0,155,0) );
			}
			else {
				token.Format("%.2f",varY);
				token+="\%";
				pDC->SetTextColor( RGB(255,0,0) );
			}
			pDC->DrawText( token, -1, CRect(rect.left,rect.top,rect.right,rect.bottom), DT_WORDBREAK | DT_CENTER );
			pDC->SetTextColor( RGB(0,0,0) );
			//Show SEPARATOR
			//pDC->MoveTo(rect.left,rect.top+15);
			//pDC->LineTo(rect.right,rect.top+15);
			pDC->Draw3dRect( rect.left+1, rect.top+15, rect.right-6, 0, RGB(51,153,255), RGB(232,232,239) );
			// PREÇO
			pDC->SelectObject(&newFont);
			if(pCtrl->m_Language==1) token="PREÇO";
			else token="PRICE";
			pDC->DrawText( token, -1, CRect(rect.left,rect.top+14,rect.right,rect.bottom), DT_WORDBREAK | DT_CENTER );
			//Show SEPARATOR
			//pDC->MoveTo(rect.left,rect.top+25);
			//pDC->LineTo(rect.right,rect.top+25);
			pDC->Draw3dRect( rect.left+1, rect.top+25, rect.right-6, 0, RGB(51,153,255), RGB(232,232,239) );
			//Show Y values
			if(pCtrl->m_Language==1){
				token = "Início:\nFim:\nVariação:";
			}
			else token = "Initial:\nFinal:\nVariation:";
			pDC->DrawText( token, -1, CRect(rect.left+5,rect.top+30,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );
			if(pCtrl->decimals == 0){
				token.Format("%.0f  \n",y1Value);
				tokenAux.Format("%.0f  \n",pointYValue);
				token+=tokenAux;
				tokenAux.Format("%.0f  ",priceInterval);
				token+=tokenAux;
			}
			else if(pCtrl->decimals == 1){
				token.Format("%.1f  \n",y1Value);
				tokenAux.Format("%.1f  \n",pointYValue);
				token+=tokenAux;
				tokenAux.Format("%.1f  ",priceInterval);
				token+=tokenAux;
			}
			else if(pCtrl->decimals == 3){
				token.Format("%.3f  \n",y1Value);
				tokenAux.Format("%.3f  \n",pointYValue);
				token+=tokenAux;
				tokenAux.Format("%.3f  ",priceInterval);
				token+=tokenAux;
			}
			else {
				token.Format("%.2f  \n",y1Value);
				tokenAux.Format("%.2f  \n",pointYValue);
				token+=tokenAux;
				tokenAux.Format("%.2f  ",priceInterval);
				token+=tokenAux;
			}
			pDC->DrawText( token, -1, CRect(rect.left,rect.top+30,rect.right,rect.bottom), DT_WORDBREAK | DT_RIGHT );
			//Show SEPARATOR
			//pDC->MoveTo(rect.left,rect.top+72);
			//pDC->LineTo(rect.right,rect.top+72);
			pDC->Draw3dRect( rect.left+1, rect.top+72, rect.right-6, 0, RGB(51,153,255), RGB(232,232,239) );
			// DATA
			pDC->SelectObject(&newFont);
			if(pCtrl->m_Language==1) token="DATA";
			else token="DATE";
			pDC->DrawText( token, -1, CRect(rect.left,rect.top+71,rect.right,rect.bottom), DT_WORDBREAK | DT_CENTER );
			//Show SEPARATOR
			//pDC->MoveTo(rect.left,rect.top+82);
			//pDC->LineTo(rect.right,rect.top+82);
			pDC->Draw3dRect( rect.left+1, rect.top+82, rect.right-6, 0, RGB(51,153,255), RGB(232,232,239) );
			//Show X values
			if(intraday){
				pDC->SelectObject(&newFont);
				if(pCtrl->m_Language==1){
					token = "Início:\nFim:";
				}
				else token = "Initial:\nFinal:";
				pDC->DrawText( token, -1, CRect(rect.left+5,rect.top+85,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );
				token.Format("%hi",julianX1.Day);
				token+="/";
				tokenAux.Format("%hi",julianX1.Month);
				token+=tokenAux+"/";
				tokenAux.Format("%hi",julianX1.Year);
				token+=tokenAux+" ";
				tokenAux.Format("%02hi",julianX1.Hour);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianX1.Minute);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianX1.Second);
				token+=tokenAux+" \n";
				tokenAux.Format("%hi",julianP.Day);
				token+=tokenAux+"/";
				tokenAux.Format("%hi",julianP.Month);
				token+=tokenAux+"/";
				tokenAux.Format("%hi",julianP.Year);
				token+=tokenAux+" ";
				tokenAux.Format("%02hi",julianP.Hour);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianP.Minute);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianP.Second);
				token+=tokenAux+" ";
				pDC->DrawText( token, -1, CRect(rect.left,rect.top+85,rect.right,rect.bottom), DT_WORDBREAK | DT_RIGHT );
			}
			else{
				pDC->SelectObject(&newFont);
				if(pCtrl->m_Language==1){
					token = "Início:\nFim:";
				}
				else token = "Initial:\nFinal:";
				pDC->DrawText( token, -1, CRect(rect.left+5,rect.top+85,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );
				token.Format("%hi",julianX1.Day);
				token+="/";
				tokenAux.Format("%hi",julianX1.Month);
				token+=tokenAux+"/";
				tokenAux.Format("%hi",julianX1.Year);
				token+=tokenAux+" ";
				tokenAux.Format("%02hi",julianX1.Hour);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianX1.Minute);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianX1.Second);
				token+=tokenAux+" \n";
				tokenAux.Format("%hi",julianP.Day);
				token+=tokenAux+"/";
				tokenAux.Format("%hi",julianP.Month);
				token+=tokenAux+"/";
				tokenAux.Format("%hi",julianP.Year);
				token+=tokenAux+" ";
				tokenAux.Format("%02hi",julianP.Hour);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianP.Minute);
				token+=tokenAux+":";
				tokenAux.Format("%02hi",julianP.Second);
				token+=tokenAux+" ";
				pDC->DrawText( token, -1, CRect(rect.left,rect.top+85,rect.right,rect.bottom), DT_WORDBREAK | DT_RIGHT );
			}
			//Show SEPARATOR
			//pDC->MoveTo(rect.left,rect.top+116);
			//pDC->LineTo(rect.right,rect.top+116);
			pDC->Draw3dRect( rect.left+1, rect.top+116, rect.right-6, 0, RGB(51,153,255), RGB(232,232,239) );
			//Show DATE INTERVAL
			pDC->SelectObject(&newFont);
			if(pCtrl->m_Language==1){	
				token="";
				if(dateInterval>0){
					token.Format("%d",dateInterval);
					if(dateInterval>1)token += " dias ";
					else token += " dia ";
				}
				if(intraday) {
					if(hourInterval>0){
						tokenAux.Format("%d",hourInterval);
						token +=tokenAux;
						if(hourInterval>1)token += " horas ";
						else token += " hora ";
					}
					if(minuteInterval>0){
						tokenAux.Format("%d",minuteInterval);
						if(minuteInterval>1)token += tokenAux+" minutos";
						else token += tokenAux+" minuto";
					}
				}
			}
			else{
				token="";
				if(dateInterval>0){
					token.Format("%d",dateInterval);
					if(dateInterval>1)token += " days ";
					else token += " day ";
				}
				if(intraday) {
					if(hourInterval>0){
						tokenAux.Format("%d",hourInterval);
						token +=tokenAux;
						if(hourInterval>1)token += " hours ";
						else token += " hour ";
					}
					if(minuteInterval>0){
						tokenAux.Format("%d",minuteInterval);
						if(minuteInterval>1)token += tokenAux+" minutes";
						else token += tokenAux+" minute";
					}
				}
			}
			pDC->DrawText( token, -1, CRect(rect.left,rect.top+118,rect.right,rect.bottom), DT_WORDBREAK | DT_CENTER );
			//Show CANDLES INTERVAL
			pDC->SelectObject(&newFont);
			token.Format("%d",candleInterval);
			if(candleInterval==1)token += " candle";
			else token += " candles";
			pDC->DrawText( token, -1, CRect(rect.left,rect.top+133,rect.right,rect.bottom), DT_WORDBREAK | DT_CENTER );

			xt1=x2Value; //pCtrl->panels[0]->GetX((int)(x1Value - pCtrl->startIndex));
			yt1=y2Value; //pCtrl->panels[0]->GetY(y1Value);
			xt2=pCtrl->m_point.x;
			yt2=pCtrl->m_point.y;
#ifdef _CONSOLE_DEBUG
			printf("\n\nValueView::OnPaint(): \nx1Value=%f y1Value=%f pointXValue=%f pointYValue=%f \nxt1=%f yt1=%f xt2=%f yt2=%f",x1Value,y1Value,pointXValue,pointYValue,xt1,yt1,xt2,yt2);
#endif
			//pDC->MoveTo(xt1,yt1);
			//pDC->LineTo(xt2,yt2);
			//pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),CPointF(xt2,yt2),pCtrl->panels[0]->series[0]->lineWeight,pCtrl->panels[0]->series[0]->lineStyle,pCtrl->panels[0]->series[0]->lineColor, pDC);
		}catch(...){
#ifdef _CONSOLE_DEBUG
			printf("\n\nEXCEPTION!!! ValueView::OnPaint(): \nx1Value=%f y1Value=%f xt1=%f yt1=%f xt2=%f yt2=%f",x1Value,y1Value,xt1,yt1,xt2,yt2);
#endif
		}
	}
#pragma endregion

#pragma region Info
	else{
		try{
			int valueSize;
			int j=0;
			int i=-1;
			do{
				i++;
				tokens[i] = values.Tokenize("\n",j);
			}while( tokens[i]!="" && i<50); 	
			valueSize=i;
			if(valueSize<3)return;	
			//pCtrl->pdcHandler->DrawRectangle(CRectF((double)rect.left, (double)rect.top, (double)(nTotalWidth), (double)(nTotalHeight+10)), 1, 0, RGB(51,153,255), pDC);		
			// Gradient value view, MTR 7/29/04
			if(pCtrl->valueViewGradientTop != 0){
				pCtrl->FadeVert(pDC, 
					pCtrl->valueViewGradientTop,
					pCtrl->valueViewGradientBottom,
					rect);
			}
			else{
				pDC->FillSolidRect( rect.left,rect.top, nTotalWidth, nTotalHeight+10, RGB(232,232,239));
			}
			pDC->Draw3dRect( rect.left, rect.top, nTotalWidth-2, nTotalHeight+10, RGB(51,153,255), RGB(51,153,255) );
			//Test if this panel have price series
			j=0;
			if(tokens[3].Tokenize(" ",j).Find(".open")>0){
				double var,xVal,open,close,closeAnt=NULL_VALUE;
				CString sOpen,sClose,sVar, sxVal;
				j=0;
				sOpen = tokens[3].Tokenize(" ",j);
				sOpen = tokens[3].Tokenize(" ",j);
				open = atof(sOpen);
				j=0;
				sClose = tokens[6].Tokenize(" ",j);
				sClose = tokens[6].Tokenize(" ",j);
				close = atof(sClose);
				j=0;
				sxVal = tokens[1].Tokenize(" ",j);
				sxVal = tokens[1].Tokenize(" ",j);
#ifdef _CONSOLE_DEBUG
				printf("\n OK!!! begin xVal sxVal=%s",sxVal);
#endif
				xVal=atof(sxVal);
#ifdef _CONSOLE_DEBUG
				printf("\n OK!!! end xVal=%f",xVal);
#endif
				CString sName;
				CSeriesStock *seriesStock = NULL;
				sName = pCtrl->m_symbol + ".low";
				CSeries *seriesSt = pCtrl->panels[0]->GetSeries(sName);
				if(seriesSt->seriesType==OBJECT_SERIES_CANDLE || seriesSt->seriesType == OBJECT_SERIES_STOCK || seriesSt->seriesType == OBJECT_SERIES_BAR || seriesSt->seriesType == OBJECT_SERIES_STOCK_HLC || seriesSt->seriesType == OBJECT_SERIES_STOCK_LINE){
					seriesStock = (CSeriesStock *) seriesSt;
#ifdef _CONSOLE_DEBUG
				printf("\n OK!!! seriesSt->seriesType");
#endif
				}
				if(seriesStock != NULL && (xVal-2)>=0){
					Candle c = seriesStock->GetCandle((int)(xVal-2));
					closeAnt = c.GetClose();
#ifdef _CONSOLE_DEBUG
				printf("\n OK!!! seriesStock != NULL");
#endif
				}
				if(closeAnt==0.0F)closeAnt=NULL_VALUE;
#ifdef _CONSOLE_DEBUG
				printf("\n closeAnt=%f xVal = %f", closeAnt, xVal);
#endif
				printf("");
				if(closeAnt==NULL_VALUE){
					var = NULL_VALUE;
					sVar="-";
				}
				else{
					var = (close-closeAnt)*100/closeAnt;
					sVar.Format( "%.*f", 2, var); 
				}

				//Show SERIES names
				if(pCtrl->m_Language==1)token="\n\n\nABRE\nMÁXIMO\nMÍNIMO\nFECHA";
				else token="\n\n\nOPEN\nHIGH\nLOW\nCLOSE";
				pDC->DrawText( token, -1, CRect(rect.left+2,rect.top+16,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );
				//Show SERIES values
				j=0;
				token="\n\n\n"+tokens[3].Tokenize(" ",j);
				token="\n\n\n"+tokens[3].Tokenize(" ",j)+"   ";
				j=0;
				tokenAux="\n"+tokens[4].Tokenize(" ",j);
				tokenAux="\n"+tokens[4].Tokenize(" ",j);
				token+=tokenAux+"   ";
				j=0;
				tokenAux="\n"+tokens[5].Tokenize(" ",j);
				tokenAux="\n"+tokens[5].Tokenize(" ",j);
				token+=tokenAux+"   ";
				j=0;
				tokenAux="\n"+tokens[6].Tokenize(" ",j);
				tokenAux="\n"+tokens[6].Tokenize(" ",j);
				token+=tokenAux+"   ";
				pDC->DrawText( token, -1, CRect(rect.left,rect.top+16,rect.right,rect.bottom), DT_WORDBREAK | DT_RIGHT );
				//Show DATA
				j=0;
				token=tokens[0].Tokenize(" ",j);
				if(intraday){
					token += "\n";
					tokenAux.Format("%02d",julianP.Hour);
					token += tokenAux;
					token+=":";
					tokenAux.Format("%02d",julianP.Minute);
					token += tokenAux;
					token+=":00";
				}
				pDC->SelectObject(&newFontMax);
				pDC->DrawText( token, -1, &rect, DT_WORDBREAK | DT_LEFT );
				//Show VARIATION
				if(sVar!="-")token=sVar+"\%  \n";
				else token = sVar;
				OLE_COLOR lineColor;
				if (var >= 0){
					pDC->SetTextColor(RGB(12,190,7));
					lineColor = RGB(15, 235, 10);
				}
				else {
					pDC->SetTextColor(RGB(255, 0, 0));
					lineColor = RGB(255,0,0);
				}
				pDC->DrawText( token, -1, &rect, DT_WORDBREAK | DT_RIGHT );
				//pCtrl->pdcHandler->DrawText(token,CRectF(rect.left,rect.top,rect.right,rect.bottom), "Arial Rounded MT Bold", 12, DT_LEFT, lineColor, 255, pDC);
				pDC->SetTextColor( RGB(0,0,0) );
						
				//Show COORDINATES 
				token="\n"+tokens[1]+"\n"+tokens[2];
				pDC->SelectObject(&newFontMin);
				int offset = 0;
				if(intraday) offset = 2;
				pDC->DrawText( token, -1, CRect(rect.left+2,rect.top+14+offset,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );

				if(valueSize>7){
#ifdef _CONSOLE_DEBUG
					printf("\n\n\n Valuesize > 7 ");
#endif
					//Show SEPARATOR
					//pDC->MoveTo(rect.left,rect.top+112);
					//pDC->LineTo(rect.right,rect.top+112);					
					pDC->Draw3dRect( rect.left+1, rect.top+112, rect.right-6, 0, RGB(51,153,255), RGB(232,232,239) );

					//Show INDICATORS names
					token="\n\n\n\n\n\n\n\n\n\n";
					CString tokenAux2;
					for(i=7;i<valueSize;i++){
						j=0;
						tokenAux2=tokens[i].Tokenize(" ",j);
						tokenAux=tokens[i].Tokenize(" ",j);
						if(tokenAux.Find("Top")>-1 || tokenAux.Find("Superior")>-1 || tokenAux.Find("Bottom")>-1 || tokenAux.Find("Inferior")>-1) tokenAux2+=tokenAux+"\n";
						else if(tokenAux2.Find("BB")==0)continue;
						else if(tokenAux2.Find("MAE")==0)tokenAux2+=(pCtrl->m_Language==1?"Fundo\n":"Bottom\n");
						else tokenAux2+="\n";
						token+=tokenAux2;
					}

					pDC->DrawText( token, -1, CRect(rect.left+2,rect.top-1,rect.right,rect.bottom+5), DT_WORDBREAK | DT_LEFT );
					//Show INDICATORS values
					token="\n\n\n\n\n\n\n\n\n\n";
					for(i=7;i<valueSize;i++){
						j=0;
						tokenAux2=tokens[i].Tokenize(" ",j)+"\n";
						tokenAux=tokens[i].Tokenize(" ",j)+"\n";
						if(tokenAux.Find("Top")>-1 || tokenAux.Find("Superior")>-1 || tokenAux.Find("Bottom")>-1 || tokenAux.Find("Inferior")>-1)tokenAux=tokens[i].Tokenize(" ",j)+"\n";
						else if(tokenAux2.Find("BB")==0)continue;
						tokenAux2=tokenAux;
						tokenAux.Remove(' ');
						tokenAux.Remove('\n');
						tokenAux.Remove('\t');
						if(pCtrl->CompareNoCase(tokenAux,""))token+="-"+tokenAux2;
						else token+=tokenAux2+"             ";
					}
					pDC->DrawText( token, -1, CRect(rect.left,rect.top-1,rect.right-10,rect.bottom+5), DT_WORDBREAK | DT_RIGHT );
				}
			}
			else{ //pDC->DrawText( values, -1, &rect, DT_WORDBREAK | DT_LEFT );

#ifdef _CONSOLE_DEBUG
					printf("\n\n\n Valuesize < 7 %d", valueSize);
#endif

				//Show INDICATORS names
				token="\n\n\n";
				for(i=3;i<valueSize;i++){
					j=0;
					token+=tokens[i].Tokenize(" ",j);
					if ((token.Find("Aroon")>-1)||(token.Find("OE")>-1)||(token.Find("SO")>-1)||(token.Find("SMI")>-1)||(token.Find("ME")>-1))
					{
						token+=" "+tokens[i].Tokenize(" ",j)+"\n";
					}
					else
					{
						token+="\n";
					}
				}
				pDC->DrawText( token, -1, CRect(rect.left+2,rect.top+6,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );
				//Show INDICATORS values
				token="\n\n\n";
				for(i=3;i<valueSize;i++){
					j=0;
					tokenAux=tokens[i].Tokenize(" ",j)+"\n";
					if ((tokenAux.Find("Aroon")>-1)||(tokenAux.Find("OE")>-1)||(tokenAux.Find("OE")>-1)||(tokenAux.Find("SO")>-1)||(tokenAux.Find("SMI")>-1)||(tokenAux.Find("ME")>-1))
					{
						tokenAux=tokens[i].Tokenize(" ",j)+"\n";
						tokenAux=tokens[i].Tokenize(" ",j)+"\n";
					}
					else
					{
						tokenAux=tokens[i].Tokenize(" ",j)+"\n";
					}
					token+=tokenAux+"      ";
				}
				pDC->DrawText( token, -1, CRect(rect.left,rect.top+6,rect.right-10,rect.bottom), DT_WORDBREAK | DT_RIGHT );

				//Show DATA
				j=0;
				token=tokens[0].Tokenize(" ",j);
				pDC->SelectObject(&newFontMax);
				pDC->DrawText( token, -1, &rect, DT_WORDBREAK | DT_LEFT );
						
				//Show COORDINATES 
				token="\n"+tokens[1]+"\n"+tokens[2];
				pDC->SelectObject(&newFontMin);
				pDC->DrawText( token, -1, CRect(rect.left+2,rect.top+4,rect.right,rect.bottom), DT_WORDBREAK | DT_LEFT );
			}
			
		}
		catch(...){

		}
	}
#pragma endregion



	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();
	pOldFont->DeleteObject();


	/*	SGC	31.05.2004	END	*/
}
/////////////////////////////////////////////////////////////////////////////

CRect	CValueView::GetRect()
{
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::GetRect() 1");
#endif
	if( pCtrl->m_point.x < 0)
		return	CRect( -1,-1,-1,-1 );
	
	if(pCtrl->m_point.y + 100 > pCtrl->height + CALENDAR_HEIGHT){
		y1 = pCtrl->m_point.y + 50;
	}
	
	CDC*	pDC	= &pCtrl->m_memDC;
	CRect	rect;
	rect.top	= y1;
	rect.left	= x1;
	rect.right	= x2;
	rect.bottom	= y2;

	CFont	newFont;			
	newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial"), pDC);
	TEXTMETRIC tm;
	CFont* pOldFont = pDC->SelectObject(&newFont);
	pDC->GetTextMetrics(&tm);
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
//	int nAvgWidth = (int)(2 + (tm.tmAveCharWidth * 0.9));
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 2");
#endif
	int nAvgWidth = (int)tm.tmAveCharWidth; // RDG 8/16/04

//	End Of Revision
	int nCharHeight = 1 + (tm.tmHeight);

	if( Text != "" )
	{		
		//	Find max width of longest string between CRLF
		CString szMax = "";
		CString copy = Text;
		CString szBuffer = "";
		CString strMax = "";
		int max = 0;
		int nl = 1;
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 3");
#endif
    int n;
		for( n = 0; n != Text.GetLength(); ++n )
		{
			if(copy.Mid(0,1) == char(10))
			{
				szBuffer = "";
				nl++;
			}
			szBuffer += copy.Mid(0,1);
			if(szBuffer.GetLength() > max){
				max = szBuffer.GetLength();
				strMax = szBuffer;
			}
			copy = copy.Mid(1);
		}

		
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 4");
#endif
		//	For custom sizes, overide points
		if( !overidePoints )
		{
			
//	Revision to rid of build warnings 6/10/2004
//	type cast of int

			// RDG 8/16/04
			//nTotalWidth =  (int)((double)max * (tm.tmAveCharWidth * 1.6));
	
			if(Text != ""){
				strMax += "  "; // temporary fix (symbol objects only)
				nTotalWidth = pDC->GetOutputTextExtent(strMax).cx;
			}

//	End of Revision
			nTotalHeight = nl * nCharHeight;
			x2 = x1 + nTotalWidth;
			y2 = y1 + nTotalHeight;
			rect.top = y1;
			rect.left = x1;
			rect.right = x2;
			rect.bottom = y2;
		}
		else
		{
			nTotalWidth = x2 - x1;
			nTotalHeight = y2 - y1;
			rect.top = y1;
			rect.left = x1;
			rect.right = x2;
			rect.bottom = y2;
		}
		okToDraw = true;
		pDC->SelectObject(pOldFont);
		newFont.DeleteObject();
		return rect;
	}
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 5");
#endif
	nTotalWidth = 0;
	nTotalHeight = 0;
	CString temp;
	
	int p = -1;
	
	//	Which panel is selected
  int n;
	for( n = 0; n != pCtrl->panels.size(); ++n)
	{

//	Revision to rid of build warnings 6/10/2004
//	type cast of int
		int	py1	= (int)pCtrl->panels[n]->y1;
		int	py2	= (int)pCtrl->panels[n]->y2;
//	End Of Revision
		//int	y	= y1 - CALENDAR_HEIGHT;
		int	y	= pCtrl->m_point.y;;
		
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 6");
#endif
		if( y > py1 && y < py2 )
		{
			p = n;

/*	Debug view
			CDC*	dc	= pCtrl->GetScreen();
			CBrush*	oldBrush	= (CBrush*)dc->SelectStockObject( LTGRAY_BRUSH );
			dc->SetTextColor( RGB( 0,0,0 ) );
			dc->SetBkMode( OPAQUE );
			CString s;
			s.Format( "  panel=%d p.y1=%d p.y2=%d  x=%d y=%d  y-ch=%d       ", n, py1,py2, x1,y1, y );
			dc->TextOut( 50,50, s );
			dc->SelectObject( oldBrush );
			pCtrl->ReleaseScreen( dc );
*/
			break;
		}
	}

	int bp = pCtrl->GetBottomChartPanel();
	if(bp != -1){
		if(p == -1 && pCtrl->m_point.y > pCtrl->panels[bp]->y1){
			p = pCtrl->GetBottomChartPanel();
		}
	}
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 7");
#endif
	if(p == -1){
		okToDraw = false;
		pDC->SelectObject(pOldFont);
		newFont.DeleteObject();
		return rect;
	}
	else
	{
		okToDraw = true;
	}

//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	int	revX = (int)(pCtrl->panels[p]->GetReverseX( pCtrl->m_point.x ) + pCtrl->startIndex);
//	End of Revision
	bool xyOnly = false;
	

	// Update 8/30/2006 - show the last bar info even if the
	// mouse movement is less than the precision of one pixel.
	double spacing = pCtrl->nSpacing;
	if(pCtrl->nSpacing < 1)
	{
		int lastX = (int)(pCtrl->panels[p]->GetX(pCtrl->endIndex - pCtrl->startIndex));
		if(pCtrl->m_point.x > lastX - 1 && pCtrl->m_point.x < lastX + 1)
		{
			revX = pCtrl->endIndex-1;
		}
	}
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 8");
#endif

	if( revX > pCtrl->RecordCount() - 1 || revX -  pCtrl->startIndex == -1)
		xyOnly = true;

	if( (size_t)revX < pCtrl->panels[p]->series[0]->data_slave.size())
	{
		if (pCtrl->ExtraPrecision())
		{
			values = CJulian::FormatTime(pCtrl->panels[p]->series[0]->data_slave[revX].jdate) + char(13) + char(10);
		}
		else
		{
			values	= pCtrl->FromJDate(pCtrl->panels[p]->series[0]->data_slave[revX].jdate) + char(13) + char(10);
		}
	}
	else
	{
		values = "";
	}

	CString strDate = values;	

	if( xyOnly )
		values = "";

	CString strMax = "";

	int	maxLen	= values.GetLength();
	temp.Format("%d", (int)revX + 1);
	CString strX = "X  " + temp + '\n';
	values			+= strX;	
	nTotalHeight	+= nCharHeight;
	temp.Format( "%.*f", pCtrl->decimals, pCtrl->panels[p]->GetReverseY(pCtrl->GetMousePositionY()) );
	CString strY = "Y  " + temp + '\n';
	values	+= strY;

	if( maxLen == 0 ){
		maxLen = temp.GetLength() + 8;
	}

	strMax = strX;
	if(strMax.GetLength() < strY.GetLength())
		strMax = strY;

	nTotalHeight += nCharHeight;
	CString field = "";
	CString title = "";	
	
	int rows = 0; // Changed from series.size 1/11/05
	for( n = 0; n != pCtrl->panels[p]->series.size(); ++n ){
		if(pCtrl->panels[p]->series[n]->seriesVisible){
			rows++;
		}
	}
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 9");
#endif
	if( ! xyOnly )
	{
		strMax = "";
		maxLen = 0;
		for( n = 0; n != pCtrl->panels[p]->series.size(); ++n )
		{
			if(pCtrl->panels[p]->series[n]->seriesVisible){
				
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::GetRect() 10: %s",pCtrl->panels[p]->series[n]->szTitle);
#endif
				double val=-1;
				try{
					val = pCtrl->panels[p]->series[n]->data_master[revX].value;	
				}
				catch(...){
					continue;
				}
				
				
				// 8/16/04
				// Do not draw precision scale if volume is in this panel
				CString volume = "";
				long decimals = pCtrl->decimals;
				volume = pCtrl->panels[p]->series[n]->szName;
				volume.MakeLower();
				decimals = pCtrl->decimals;
				if (true/*volume.Find(".volume") != -1 || volume == "volume"*/){
					if (volume.Find(".volume") != -1 || volume == "volume")decimals = 0;
					if (val >= 1000000000)
					{

						val = val / 1000000000;
						pCtrl->m_volumePostfix = "B";
					}
					else if (val >= 1000000)
					{

						val = val / 1000000;
						pCtrl->m_volumePostfix = "M";
					}
					else if (val >= 1000)
					{

						val = val / 1000;
						pCtrl->m_volumePostfix = "K";
					}
					else pCtrl->m_volumePostfix = "";
#ifdef _CONSOLE_DEBUG
					printf(" %f %s", val, pCtrl->m_volumePostfix);
#endif
				}
							

				temp.Format("%.*f", decimals, val);
				if(val == NULL_VALUE) temp = "";
				title = pCtrl->panels[p]->series[n]->szTitle;
				if(title == "") title = pCtrl->panels[p]->series[n]->szName;

				// Added postfix for volume 1/12/06
				if( true/*pCtrl->m_volumePostfix != "" && 
					(volume.Find(".volume") != -1 || volume == "volume")*/ )
					temp += pCtrl->m_volumePostfix;
				
				field = title + "  ";
				field += temp + '\n';
				if(field.GetLength() > maxLen){
					maxLen = field.GetLength();
					strMax = field; // RDG 8/16/04
				}
				values += field;
				nTotalHeight += nCharHeight;

			}
		}
	}

	// RDG 8/16/04
	//nTotalWidth = nAvgWidth * maxLen + 5;
	if( !xyOnly ){
		if(strDate.GetLength() > strMax.GetLength()) strMax = strDate;
	}
	//strMax = "XXXXXXXXXXXXXXXXXXXXXXXX  ";
	nTotalWidth = pDC->GetOutputTextExtent(strMax).cx;

	if( !xyOnly )
	{
		//nTotalHeight += nCharHeight + 2;		
		nTotalHeight = pDC->GetOutputTextExtent(strMax).cy * (rows + 3) + 2;
	}


	int h = y2 - y1;
	int y = pCtrl->m_point.y;

	if( y + 100 > pCtrl->height + CALENDAR_HEIGHT )
	{
		y1 = y - 50;	
	}
	
	x2 = x1 + nTotalWidth;


	y2 = y1 + nTotalHeight;
	rect.top = y1;
	rect.left = x1;
	rect.right = x2;
	rect.bottom = y2;

	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 11");
#endif
	bool turnedLeft = false;
	bool turnedTop = false;
	double xBase = 0;
	double yBase = 0;
	//Adjust Delta Cursor under or above the box:
	if(pCtrl->DeltaCursor && p==0){	
		xBase = pCtrl->panels[0]->GetX((int)(x1Value - pCtrl->startIndex));
		yBase = pCtrl->panels[0]->GetY(y1Value);
		if(pCtrl->m_point.x==xBase)return CRect( -1,-1,-1,-1 );;
		if(pCtrl->m_point.x<xBase) {
			rect.left=pCtrl->m_point.x-150;
			x1 = rect.left;
			turnedLeft = true;
		}
		if(pCtrl->m_point.y>yBase){
			rect.top = pCtrl->m_point.y-165;
			y1 = rect.top;
			turnedTop=true;
		}
		rect.right	= rect.left + 135;
		x2 = rect.right;
		rect.bottom	= rect.top  + 145;
		y2 = rect.bottom;
		nTotalWidth=rect.right-rect.left;
		nTotalHeight=rect.bottom-rect.top;
	}
	else{
		rect.bottom +=10;
		y2 = rect.bottom;
		nTotalHeight +=10;
	}
	
#ifdef _CONSOLE_DEBUG
		//printf("\nVALUEVIEW::GetRect() 12");
#endif

	// Analyze the borders

	//Respect bottom border:
	if (y2>pCtrl->panels[pCtrl->GetVisiblePanelCount() - 1]->y2){

		y2 = pCtrl->panels[p]->y2;
		y1 = y2 - nTotalHeight;
		rect.top = y1;
		rect.bottom = y2;
	}
	//Respect right border:
	if(x2 > /*pCtrl->width-*/pCtrl->panels[p]->yScaleRect.left)
	{
		/*if(pCtrl->DeltaCursor && p==0){
			//Adjust vertically:
			if(pCtrl->m_point.y<yBase)y1=yBase;
			else y1=pCtrl->m_point.y;
			y2 = y1+nTotalHeight;
		}*/
		x1 = (pCtrl->panels[p]->yScaleRect.left - (x2 - x1));
		x2 = x1 + nTotalWidth;
		rect.left = x1;
		rect.right = x2;
		rect.top = y1;
		rect.bottom = y2;
	}
	//Respect top border:
	if(p==0 && y1 < pCtrl->panels[p]->y1)
	{

		y1 = pCtrl->panels[p]->y1;
		y2 = pCtrl->panels[p]->y1 + nTotalHeight;
		rect.top = y1;
		rect.bottom = y2;
	}
	if(pCtrl->m_point.x > pCtrl->panels[p]->yScaleRect.left-10) return	CRect( -1,-1,-1,-1 );

	//Respect left border:
	if(x1 < 5)
	{
		/*if(pCtrl->DeltaCursor && p==0){
			//Adjust vertically:
			if(pCtrl->m_point.y<yBase)y1=yBase;
			else y1=pCtrl->m_point.y;
			y2 = y1+nTotalHeight;
		}*/
		x1 = 5;//(pCtrl->panels[p]->yScaleRect.left - (x2 - x1));
		x2 = x1 + nTotalWidth;
		rect.left = x1;
		rect.right = x2;
		rect.top = y1;
		rect.bottom = y2;
	}


	//Adjust if mouse cursor is under box:
	if (pCtrl->m_point.y>y1 && pCtrl->m_point.y<y2 && pCtrl->m_point.x>x1 && pCtrl->m_point.x<x2 && !pCtrl->DeltaCursor)	{
#ifdef _CONSOLE_DEBUG
		printf("\nVALUEVIEW::Adjust Cursor");
#endif
		int adjust = x2-pCtrl->m_point.x+2;
		x1-=adjust;
		x2-=adjust;
		rect.left = x1;
		rect.right = x2;

	}

	return rect;
}
/////////////////////////////////////////////////////////////////////////////
// Calculate how many days are between 2 dates
int CValueView::DaysBetweenDates(double x1Value, double x2Value){
	double x1JDate = pCtrl->panels[0]->series[0]->GetJDate((int)x1Value);
	double x2JDate = pCtrl->panels[0]->series[0]->GetJDate((int)x2Value);	
	MMDDYYHHMMSS julianX1 = CJulian::FromJDate((double)x1JDate);	
	MMDDYYHHMMSS julianX2 = CJulian::FromJDate((double)x2JDate);int days_in_months[] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    int first_day, second_day;
    int first_month, second_month;
    int first_year, second_year;
    int years_difference, days_difference;
    int months_total;
    int reg_year = 365;
	first_day = julianX1.Day;
	second_day = julianX2.Day;
    first_month = julianX1.Month;
	second_month = julianX2.Month;
    first_year = julianX1.Year;
	second_year = julianX2.Year;

    /////////////////////////////Years/////////////////////////////////
    if(first_year == second_year)
    {
        years_difference = 0;
    }
    else
    {
        if(first_year % 4 == 0 && first_year % 100 != 0 || first_year % 400 == 0)
        {
            if(second_year % 4 == 0 && second_year % 100 != 0 || second_year % 400 == 0)
            {
                if(first_year > second_year)
                {
                    years_difference = (first_year - second_year) * (reg_year) + 2;
                }
                else
                {
                    years_difference = (second_year - first_year) * (reg_year) + 2;
                }
                if(second_month > first_month)
                {
                    if(days_in_months[first_month - 1] > days_in_months[1])
                    {
                        --years_difference;
                    }
                }
            }
            else
            {
                if(first_year > second_year)
                {
                    years_difference = (first_year - second_year) * (reg_year) + 1;
                }
                else
                {
                    years_difference = (second_year - first_year) * (reg_year) + 1;
                }
                if(first_month > second_month)
                {
                    if(days_in_months[second_month - 1] > days_in_months[1])
                    {
                        --years_difference;
                    }
                }
            }
        }
        else
        {
            if(first_year > second_year)
            {
                years_difference = (first_year - second_year) * (reg_year);
            }
            else
            {
                years_difference = (second_year - first_year) * (reg_year);
            }
        }
    }
    /////////////////////////////Months////////////////////////////////////
    if(first_month == second_month)
    {
        months_total = 0;
    }
	else
    {
        if(first_month > second_month)
        {
            for(int i = (first_month - 1); i > (second_month - 1); i--)
            {
                static int months_total_temp = 0;
                months_total_temp += days_in_months[i];
                months_total = months_total_temp;
            }
        }
        else
        {
            for(int i = (first_month - 1); i < (second_month - 1); i++)
            {
                static int months_total_temp = 0;
                months_total_temp += days_in_months[i];
                months_total = months_total_temp;
            }
        }
    }
    ////////////////////////////Days//////////////////////////////////
    int days_total;
    if (first_day == second_day)
    {
        days_difference = 0;
        days_total = (years_difference + months_total) - days_difference;
    }
    else
    {
        if(first_day > second_day)
        {
            days_difference = first_day - second_day;
            days_total = (years_difference + months_total) - days_difference;
        }
        else
        {
            days_difference = second_day - first_day;
            days_total = (years_difference + months_total) + days_difference;
        }
    }
    //////////////////////////In Between Leap Years///////////////////////////////
    if(first_year == second_year)
    {
    }
    else
    {
        if(first_year > second_year)
        {
            for(int i = (second_year + 1); i < first_year; i++)
            {
                if(i % 4 == 0 && i % 100 != 0 || i % 400 == 0)
                {
                    ++days_total;
                }
            }
        }
        else
        {
            for(int i = (first_year + 1); i < second_year; i++)
            {
                if(i % 4 == 0 && i % 100 != 0 || i % 400 == 0)
                {
                    ++days_total;
                }
            }
        }
    }
	
	return days_total;
}