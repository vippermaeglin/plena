#include "stdafx.h"
#include "PDCHandler.h"
#include "COL.h"

//#define _CONSOLE_DEBUG


PDCHandler::PDCHandler()
{
	//GDI+ Example:
	//HDC hDC = pCtrl->m_memDC.GetSafeHdc();
	//Graphics^ gDC = Graphics::FromHdc( (IntPtr)hDC /*pDC->m_hDC*/ );
	//PointF p1 = PointF(xt1,yt1);
	//PointF p2 = PointF(xt2,yt2);
	//gDC->DrawLine(gcnew Pen( Color::Black,1.0f ),p1,p2);
}

void PDCHandler::Connect(CStockChartXCtrl* Ctl)
{
	pCtrl=Ctl;
}

PDCHandler::~PDCHandler(void)
{
}

void PDCHandler::Setup()
{
	
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &pCtrl->m_pD2DFactory);
		#ifdef _CONSOLE_DEBUG
			//printf("\n\nD2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &m_pD2DFactory);\n");
		#endif

		D2D1_RENDER_TARGET_PROPERTIES props = D2D1::RenderTargetProperties(
			D2D1_RENDER_TARGET_TYPE_DEFAULT,
			D2D1::PixelFormat(
				DXGI_FORMAT_B8G8R8A8_UNORM,
				D2D1_ALPHA_MODE_IGNORE),
			0,
			0,
			D2D1_RENDER_TARGET_USAGE_NONE,
			D2D1_FEATURE_LEVEL_DEFAULT
		);
		pCtrl->m_pD2DFactory->CreateDCRenderTarget(&props, &pCtrl->m_pDCRT);
		// Create a brush
		pCtrl->m_pDCRT->CreateSolidColorBrush(D2D1::ColorF(RGB(255,0,0),1.0F),& pCtrl->m_Brush);

		//Create Text Resources
		DWriteCreateFactory(
				DWRITE_FACTORY_TYPE_SHARED,
				__uuidof(IDWriteFactory),
				reinterpret_cast<IUnknown**>(&pCtrl->pDWriteFactory)
				);

		pCtrl->m_pD2DFactory->CreatePathGeometry(&pCtrl->pathGeometry);*/
	}	
	#pragma endregion
		
	#pragma region GDI+
	else if(pCtrl->PDCType==1){
	}
	#pragma endregion

	#pragma region GDI
	else if(pCtrl->PDCType==0){

	}
	#pragma endregion
}

void PDCHandler::Initialize(CDC* memDC, CRect* rect)
{	
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*CRect rectTemp = CRect(449,165,567,41);
		pCtrl->m_pDCRT->BindDC(memDC->GetSafeHdc(), rect);  */
	}
	#pragma endregion
	
	#pragma region GDI+
	else if(pCtrl->PDCType==1){
		pCtrl->m_memHDC = pCtrl->m_memDC.GetSafeHdc();
	}
	#pragma endregion
	
	#pragma region GDI
	else if(pCtrl->PDCType==0){

	}
	#pragma endregion

}

void PDCHandler::Release(){

		/*if (pCtrl->m_pD2DFactory != NULL)
		{
			pCtrl->m_pD2DFactory->Release();

			pCtrl->m_pD2DFactory = NULL;
		}
		if (pCtrl->m_pDCRT != NULL)
		{
			pCtrl->m_pDCRT->Release();

			pCtrl->m_pDCRT = NULL;
		}
		if (pCtrl->m_Brush != NULL)
		{
			pCtrl->m_Brush->Release();

			pCtrl->m_Brush = NULL;
		}			
			
		if (pCtrl->pDWriteFactory != NULL)
		{
			pCtrl->pDWriteFactory->Release();

			pCtrl->pDWriteFactory = NULL;
		}		*/
}

void PDCHandler::DrawLine(CPointF P1, CPointF P2, long Width, long Style, OLE_COLOR Color, CDC* pDC, bool Smooth/*=true*/)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_POINT_2F p1,p2;
		p1.x=P1.x;
		p1.y=P1.y;
		p2.x=P2.x;
		p2.y=P2.y;

		if(Style==0)pCtrl->m_pDCRT->DrawLine(p1,p2,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawLine(p1,p2,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawLine(p1,p2,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();*/
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){		
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		if(Smooth)gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(Color),Width);
		// Create a custom dash pattern. EX: array<Single>^temp0 = {4.0F,2.0F,1.0F,3.0F};
		array<Single>^tempDASH={10.0f,3.0f,10.0f,3.0f}; //DASHED
		array<Single>^tempDOT={1.5f,2.0f,1.5f,2.0f}; //DOTTED
		/*blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;*/
		if(Style == PS_SOLID) blackPen->DashStyle = System::Drawing::Drawing2D::DashStyle::Solid;
		else{		
			if(Style == PS_DASH)blackPen->DashPattern = tempDASH;
			else blackPen->DashPattern = tempDOT;
		}
		if (Width < 2.0f && (abs(P1.x-P2.x)<1.0F || abs(P1.y-P2.y)<1.0F)) gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::Default;
		PointF p1 = PointF(P1.x,P1.y);
		PointF p2 = PointF(P2.x,P2.y);
		try{

			//Test CLIP:
			if(IsClipRegion){
				Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
				gDC->ExcludeClip(clipRec);
			}
			
#ifdef _CONSOLE_DEBUG
		//printf("\nWick=%f", p1.X);
#endif
			gDC->DrawLine(blackPen,p1,p2);
		}
		catch(...){
			//CString msgerr=CString.Format("\nERRO DRAWLINE: p1.x=%f,p1.y=%f,p2.x=%f,p2.y=%f",p1.x,p1.y,p2.x,p2.y);
			//ThrowError( CUSTOM_CTL_SCODE(1003), msgerr );
			printf("\n\n\nERRO DRAWLINE: p1.x=%f p1.y=%f p2.x=%f p2.y=%f",P1.x,P1.y,P2.x,P2.y);
		}
		delete tempDASH;
		delete tempDOT;
		delete blackPen;
		delete gDC;
	}
	#pragma endregion
	
	#pragma region GDI
	else{
		pDC->MoveTo((int)P1.x,(int)P1.y);
		pDC->LineTo((int)P2.x,(int)P2.y);
	}
	#pragma endregion
}

void PDCHandler::DrawLine(Graphics^ gDC, Pen^ blackPen, CPointF P1, CPointF P2, long Width, long Style, OLE_COLOR Color, CDC* pDC, bool Smooth/*=true*/)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_POINT_2F p1,p2;
		p1.x=P1.x;
		p1.y=P1.y;
		p2.x=P2.x;
		p2.y=P2.y;

		if(Style==0)pCtrl->m_pDCRT->DrawLine(p1,p2,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawLine(p1,p2,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawLine(p1,p2,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();*/
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){		
		// Create a custom dash pattern. EX: array<Single>^temp0 = {4.0F,2.0F,1.0F,3.0F};
		//array<Single>^tempDASH={10.0f,3.0f,10.0f,3.0f}; //DASHED
		//array<Single>^tempDOT={1.5f,2.0f,1.5f,2.0f}; //DOTTED
		if(Style == PS_SOLID) blackPen->DashStyle = System::Drawing::Drawing2D::DashStyle::Solid;
		//else{		
		//	if(Style == PS_DASH)blackPen->DashPattern = tempDASH;
		//	else blackPen->DashPattern = tempDOT;
		//}
		if (Width < 2.0f && (abs(P1.x-P2.x)<1.0F || abs(P1.y-P2.y)<1.0F)) gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::Default;
		PointF p1 = PointF(P1.x,P1.y);
		PointF p2 = PointF(P2.x,P2.y);
		try{

			//Test CLIP:
			if(IsClipRegion){
				Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
				gDC->ExcludeClip(clipRec);
			}
			
#ifdef _CONSOLE_DEBUG
		//printf("\nWick=%f", p1.X);
#endif
			gDC->DrawLine(blackPen,p1,p2);
		}
		catch(...){
			//CString msgerr=CString.Format("\nERRO DRAWLINE: p1.x=%f,p1.y=%f,p2.x=%f,p2.y=%f",p1.x,p1.y,p2.x,p2.y);
			//ThrowError( CUSTOM_CTL_SCODE(1003), msgerr );
			printf("\n\n\nERRO DRAWLINE: p1.x=%f p1.y=%f p2.x=%f p2.y=%f",P1.x,P1.y,P2.x,P2.y);
		}
		//delete tempDASH;
		//delete tempDOT;
	}
	#pragma endregion
	
	#pragma region GDI
	else{
		pDC->MoveTo((int)P1.x,(int)P1.y);
		pDC->LineTo((int)P2.x,(int)P2.y);
	}
	#pragma endregion
}

void PDCHandler::FillRectangle(CRectF rect, OLE_COLOR Color, CDC* pDC)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_RECT_F dRect = D2D1_RECT_F();
		dRect.left = rect.left;
		dRect.top = rect.top;
		dRect.right = rect.right;
		dRect.bottom = rect.bottom;

		pCtrl->m_pDCRT->FillRectangle(dRect,pCtrl->m_Brush);

			
		pCtrl->m_pDCRT->EndDraw();*/


	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){
		//Normalize rect:
		if(rect.left>rect.right){
			double t = rect.left;
			rect.left=rect.right;
			rect.right=t;
		}
		if(rect.top>rect.bottom){
			double t = rect.top;
			rect.top=rect.bottom;
			rect.bottom=t;
		}
		double Width = 1.0F;
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		SolidBrush^ blackBrush = gcnew SolidBrush(ColorTranslator::FromOle(Color));

		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		gDC->FillRectangle(blackBrush,(float)rect.left,(float)rect.top,(float)rect.right-rect.left,(float)rect.bottom-rect.top);
		delete blackBrush;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		CBrush	br( Color );
		pDC->FillRect( CRect((int)rect.left,(int)rect.top,(int)rect.right,(int)rect.bottom), &br );
	}
	#pragma endregion
}

void PDCHandler::FillCircle(CRectF rect, OLE_COLOR Color, CDC* pDC)
{
#pragma region Direct2D
	if (pCtrl->PDCType == 2){
		/*HRESULT hr;

		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));

		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_RECT_F dRect = D2D1_RECT_F();
		dRect.left = rect.left;
		dRect.top = rect.top;
		dRect.right = rect.right;
		dRect.bottom = rect.bottom;

		pCtrl->m_pDCRT->FillRectangle(dRect,pCtrl->m_Brush);


		pCtrl->m_pDCRT->EndDraw();*/


	}
#pragma endregion

#pragma region GDI+	
	else if (pCtrl->PDCType == 1){
		//Normalize rect:
		if (rect.left>rect.right){
			double t = rect.left;
			rect.left = rect.right;
			rect.right = t;
		}
		if (rect.top>rect.bottom){
			double t = rect.top;
			rect.top = rect.bottom;
			rect.bottom = t;
		}
		double Width = 0.5F;
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc);
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		SolidBrush^ blackBrush = gcnew SolidBrush(ColorTranslator::FromOle(Color));

		//Test CLIP:
		if (IsClipRegion){
			Drawing::Rectangle clipRec(0, 0, pCtrl->GetX(pCtrl->endIndex - 2) - pCtrl->GetX(pCtrl->startIndex), pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		array<PointF>^ pointsF = gcnew array<PointF>(4);
		pointsF[0] = PointF(rect.left, rect.top);
		pointsF[1] = PointF(rect.left, rect.bottom);
		pointsF[2] = PointF(rect.right, rect.bottom);
		pointsF[3] = PointF(rect.right, rect.top);


		gDC->FillClosedCurve(blackBrush,pointsF);
		delete blackBrush;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
#pragma endregion

#pragma region GDI
	else{
		CBrush	br(Color);
		pDC->FillRect(CRect((int)rect.left, (int)rect.top, (int)rect.right, (int)rect.bottom), &br);
	}
#pragma endregion
}

void PDCHandler::FillRectangleGradient(CRectF rect, OLE_COLOR Color1, OLE_COLOR Color2, CDC* pDC)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_RECT_F dRect = D2D1_RECT_F();
		dRect.left = rect.left;
		dRect.top = rect.top;
		dRect.right = rect.right;
		dRect.bottom = rect.bottom;

		pCtrl->m_pDCRT->FillRectangle(dRect,pCtrl->m_Brush);

			
		pCtrl->m_pDCRT->EndDraw();*/


	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){
		//Normalize rect:
		if(rect.left>rect.right){
			double t = rect.left;
			rect.left=rect.right;
			rect.right=t;
		}
		if(rect.top>rect.bottom){
			double t = rect.top;
			rect.top=rect.bottom;
			rect.bottom=t;
		}
		Graphics^ gDC = Graphics::FromHdc( (IntPtr)pCtrl->m_memHDC /*pDC->m_hDC*/ );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		PointF p1 = PointF(rect.left,rect.top);
		PointF p2 = PointF(rect.left,rect.bottom);
		Color drawColor1 = ColorTranslator::FromOle(Color1);
		Color drawColor2 = ColorTranslator::FromOle(Color2);
		LinearGradientBrush^ gradBrush = gcnew LinearGradientBrush(p1,p2,drawColor1,drawColor2);

		
		//Test CLIP:
		if(IsClipRegion){	
#ifdef _CONSOLE_DEBUG
			printf("\nCLIP: Ctl->width=%d Ctl->height=%d sEnd=%d",pCtrl->width, pCtrl->height, pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex));
#endif
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		gDC->FillRectangle(gradBrush,(float)rect.left,(float)rect.top,(float)rect.right-rect.left,(float)rect.bottom-rect.top+5);
		delete gradBrush;
		delete gDC;		

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		//CBrush	br( Color );
		//pDC->FillRect( CRect((int)rect.left,(int)rect.top,(int)rect.right,(int)rect.bottom), &br );
	}
	#pragma endregion
}

void PDCHandler::DrawRectangle(CRectF rect, long Width, long Style, OLE_COLOR Color, CDC*pDC)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_RECT_F dRect = D2D1_RECT_F();
		dRect.left = rect.left;
		dRect.top = rect.top;
		dRect.right = rect.right;
		dRect.bottom = rect.bottom;

		if(Style==0)pCtrl->m_pDCRT->DrawRectangle(dRect,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawRectangle(dRect,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawRectangle(dRect,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
			
		pCtrl->m_pDCRT->EndDraw();*/

		

	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){
		//Normalize rect:
		if(rect.left>rect.right){
			double t = rect.left;
			rect.left=rect.right;
			rect.right=t;
		}
		if(rect.top>rect.bottom){
			double t = rect.top;
			rect.top=rect.bottom;
			rect.bottom=t;
		}
				
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(Color),Width);
		blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;
		if (Width < 2.0f) gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::Default;
		
		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		gDC->DrawRectangle(blackPen,(float)rect.left,(float)rect.top,(float)rect.right-rect.left,(float)rect.bottom-rect.top);
		
#ifdef _CONSOLE_DEBUG
		//printf("\nx1=%f x2=%f",rect.left,rect.right);
#endif
		/*DrawLine(CPointF(rect.left,rect.top),CPointF(rect.right,rect.top),Width,Style,Color,pDC);
		DrawLine(CPointF(rect.left,rect.top),CPointF(rect.left,rect.bottom),Width,Style,Color,pDC);
		DrawLine(CPointF(rect.right,rect.top),CPointF(rect.right,rect.bottom),Width,Style,Color,pDC);
		DrawLine(CPointF(rect.right,rect.bottom),CPointF(rect.left,rect.bottom),Width,Style,Color,pDC);
		*/
		delete blackPen;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		CRect crect = CRect((int)rect.left,(int)rect.top,(int)rect.right,(int)rect.bottom);
		CCOL ccol = CCOL();
		ccol.DrawRect(pDC,crect);
	}
	#pragma endregion
}

void PDCHandler::DrawEllipse(CRectF rect, long Width, long Style, OLE_COLOR Color, CDC*pDC)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_ELLIPSE ellipse = D2D1_ELLIPSE();
		ellipse.point.x = (rect.left+rect.right)/2;
		ellipse.point.y = (rect.top+rect.bottom)/2;
		ellipse.radiusX = (rect.left-rect.right)/2;
		ellipse.radiusY = (rect.top-rect.bottom)/2;

		if(Style==0)pCtrl->m_pDCRT->DrawEllipse(ellipse,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawEllipse(ellipse,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawEllipse(ellipse,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
			
		pCtrl->m_pDCRT->EndDraw();*/

		
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){
		//Normalize rect:
		if(rect.left>rect.right){
			double t = rect.left;
			rect.left=rect.right;
			rect.right=t;
		}
		if(rect.top>rect.bottom){
			double t = rect.top;
			rect.top=rect.bottom;
			rect.bottom=t;
		}
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(Color),Width);
		blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;
		
		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		gDC->DrawEllipse(blackPen, (float)rect.left, (float)rect.top, (float)rect.right - (float)rect.left, (float)rect.bottom - (float)rect.top);
		delete blackPen;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		pDC->Arc((int)rect.left,(int)rect.top,(int)rect.right,(int)rect.bottom,(int)rect.right,(int)rect.bottom,(int)rect.right,(int)rect.bottom);
	}
	#pragma endregion
}

void PDCHandler::DrawArc(CRectF rectBounds, CPointF pSrc, CPointF pStart, CPointF pEnd, bool dirClockwise, long Width, long Style, OLE_COLOR Color,CDC*pDC)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		ID2D1GeometrySink *pSink = NULL;
		
		D2D1_ARC_SEGMENT arc = D2D1_ARC_SEGMENT();
		D2D1_POINT_2F endPoint = D2D1::Point2F(pEnd.x,pEnd.y);
		D2D1_SIZE_F sizeRadius = D2D1::SizeF((rectBounds.bottom-rectBounds.top)/2,(rectBounds.right-rectBounds.left)/2);

		arc.point =  endPoint;
		arc.size = sizeRadius;
		arc.rotationAngle = 0.0F;		
		if(!dirClockwise)arc.sweepDirection = D2D1_SWEEP_DIRECTION_CLOCKWISE;
		else arc.sweepDirection = D2D1_SWEEP_DIRECTION_COUNTER_CLOCKWISE;
		arc.arcSize = D2D1_ARC_SIZE_SMALL;
		
        hr = pCtrl->pathGeometry->Open(&pSink);

        if (SUCCEEDED(hr))
        {
            pSink->SetFillMode(D2D1_FILL_MODE_WINDING);
            
            pSink->BeginFigure(
                D2D1::Point2F(pStart.x, pStart.y),
				D2D1_FIGURE_BEGIN_FILLED
                );
            pSink->AddArc(arc);            
            pSink->EndFigure(D2D1_FIGURE_END_OPEN);
		}
		
        hr = pSink->Close();
		
		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		if(Style==0)pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();

		
		if (pSink != NULL)
		{
			pSink->Release();

			pSink = NULL;
		}*/
		
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){
		//Normalize rect:
		if(rectBounds.left>rectBounds.right){
			double t = rectBounds.left;
			rectBounds.left=rectBounds.right;
			rectBounds.right=t;
		}
		if(rectBounds.top>rectBounds.bottom){
			double t = rectBounds.top;
			rectBounds.top=rectBounds.bottom;
			rectBounds.bottom=t;
		}
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(Color),Width);
		blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;
		RectangleF rectF = RectangleF(rectBounds.left,rectBounds.top,rectBounds.right-rectBounds.left,rectBounds.bottom-rectBounds.top);
		double start = 0.0F;
		if(dirClockwise)start = 180.0F;
		
		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		gDC->DrawArc(blackPen,rectF,start,180.0F);
		delete blackPen;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{		
		CRect crect = CRect((int)rectBounds.left,(int)rectBounds.top,(int)rectBounds.right,(int)rectBounds.bottom);
		pDC->MoveTo((int)pSrc.x,(int)pSrc.y);
		pDC->Arc( crect, CPoint(pStart.x,pStart.y), CPoint(pEnd.x, pEnd.y) );
	}
	#pragma endregion
}

void PDCHandler::DrawText(CString text, CRectF rectBounds, LPCSTR fontType, double fontSize, long alignment, OLE_COLOR Color, int opacity,CDC* pDC)
{
	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		D2D1_RECT_F layoutRect = D2D1::RectF(rectBounds.left,rectBounds.top,rectBounds.right,rectBounds.bottom);
		IDWriteTextFormat* textFormat;

		//Convert Text to WCSTRING
		size_t origsize = strlen(fontType) + 1;
		const size_t newsize = 100;
		size_t convertedChars = 0;
		wchar_t wcstring[newsize];
		mbstowcs_s(&convertedChars, wcstring, origsize, fontType, _TRUNCATE);

		hr = pCtrl->pDWriteFactory->CreateTextFormat(
            wcstring,                // Font family name.
            NULL,                       // Font collection (NULL sets it to use the system font collection).
            DWRITE_FONT_WEIGHT_REGULAR,
            DWRITE_FONT_STYLE_NORMAL,
            DWRITE_FONT_STRETCH_NORMAL,
            fontSize,
            L"en-us",
            &textFormat
            );

		//Convert Text to WCSTRING
		origsize = strlen(text) + 1;
		convertedChars = 0;
		wcstring[newsize];
		mbstowcs_s(&convertedChars, wcstring, origsize, text, _TRUNCATE);

		if(alignment==1)alignment=2;
		else if(alignment==2)alignment=1;
		textFormat->SetTextAlignment((DWRITE_TEXT_ALIGNMENT)alignment);
		pCtrl->m_pDCRT->DrawText(wcstring,origsize,textFormat,layoutRect,pCtrl->m_Brush,D2D1_DRAW_TEXT_OPTIONS_NONE,DWRITE_MEASURING_MODE_NATURAL); //DrawLine(p1,p2,m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
					
		pCtrl->m_pDCRT->EndDraw();

		if (textFormat != NULL)
		{
			textFormat->Release();

			textFormat = NULL;
		}*/

	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){
		fontSize-=4;
		//Normalize rect:
		if(rectBounds.left>rectBounds.right){
			double t = rectBounds.left;
			rectBounds.left=rectBounds.right;
			rectBounds.right=t;
		}
		if(rectBounds.top>rectBounds.bottom){
			double t = rectBounds.top;
			rectBounds.top=rectBounds.bottom;
			rectBounds.bottom=t;
		}
		double Width = 1.0F;
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		SolidBrush^ blackBrush = gcnew SolidBrush(Color::FromArgb(opacity,ColorTranslator::FromOle(Color)));
		RectangleF rectF = RectangleF(rectBounds.left,rectBounds.top,rectBounds.right-rectBounds.left,rectBounds.bottom-rectBounds.top);
		
		//Convert Font to WCSTRING
		size_t origsize = strlen(fontType) + 1;
		const size_t newsize = 100;
		size_t convertedChars = 0;
		wchar_t wcstring[newsize];
		mbstowcs_s(&convertedChars, wcstring, origsize, fontType, _TRUNCATE);

		String^ fontFamily = System::Runtime::InteropServices::Marshal::PtrToStringAnsi((IntPtr)(void*)fontType);
		String^ textStr = gcnew String(text);
		StringFormat^ format = gcnew StringFormat();
		if(alignment==DT_LEFT)format->Alignment = System::Drawing::StringAlignment::Near;
		if(alignment==DT_CENTER)format->Alignment = System::Drawing::StringAlignment::Center;
		if(alignment==DT_RIGHT)format->Alignment = System::Drawing::StringAlignment::Far;
		Font^ myFont = gcnew Font(fontFamily,fontSize);

		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		gDC->DrawString(textStr,myFont,blackBrush,rectF,format);
		delete blackBrush;
		delete format;
		delete textStr;
		delete fontFamily;
		delete myFont;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		CRect rectText = CRect((int)rectBounds.left,(int)rectBounds.top,(int)rectBounds.right,(int)rectBounds.bottom);
		pDC->DrawText(text, -1, rectText, alignment);
	}
	#pragma endregion
}

void PDCHandler::DrawPath(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR uColor, OLE_COLOR dColor, CDC* pDC, bool spline/*=false*/, bool smooth /*=true*/)
{
	if(points.size()<=0) return;

	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		ID2D1GeometrySink *pSink = NULL;
		D2D1_POINT_2F* pointsF = NULL;
		pointsF = new D2D1_POINT_2F[points.size()];
		for(int i=0;i<points.size();i++){
			pointsF[i]=D2D1::Point2F(points[i].x,points[i].y);
		}
		
        pCtrl->pathGeometry->Open(&pSink);

        if (SUCCEEDED(hr))
        {
            pSink->SetFillMode(D2D1_FILL_MODE_WINDING);
            
            pSink->BeginFigure(
                D2D1::Point2F(0.0F, 0.0F),
				D2D1_FIGURE_BEGIN_FILLED
                );
			pSink->AddLines(pointsF,points.size());            
            pSink->EndFigure(D2D1_FIGURE_END_OPEN);
		}
		
        hr = pSink->Close();
		
		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		if(Style==0)pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();
		
		
		if (pSink != NULL)
		{
			pSink->Release();

			pSink = NULL;
		}*/
		
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){	
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		if(smooth)gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		else gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::Default;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(uColor),Width);
		/*blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;*/
		if(Style == PS_SOLID) blackPen->DashStyle = System::Drawing::Drawing2D::DashStyle::Solid;
		else{		
			// Create a custom dash pattern. EX: array<Single>^temp0 = {4.0F,2.0F,1.0F,3.0F};
			array<Single>^tempDASH={10.0f,3.0f,10.0f,3.0f}; //DASHED
			array<Single>^tempDOT={1.5f,2.0f,1.5f,2.0f}; //DOTTED
			if(Style == PS_DASH)blackPen->DashPattern = tempDASH;
			else blackPen->DashPattern = tempDOT;
		}
		//if (Width < 2.0f && (abs(P1.x-P2.x)<1.0F || abs(P1.y-P2.y)<1.0F)) gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::Default;
		
		array<PointF>^ pointsF = gcnew array<PointF>(points.size());
		for(int i=0;i<points.size();i++){
			pointsF[i]=PointF(points[i].x,points[i].y);
		}
		
		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		// Create graphics path object and add ellipse.
		//GraphicsPath^ graphPath = gcnew GraphicsPath;
		//graphPath->AddLines( pointsF );
		try{
			//Test single DrawLine(): (bad performance)
			//for(int i=0;i<points.size()-1;i++){				
			//	gDC->DrawLine(blackPen,pointsF[i],pointsF[i+1]);
			//}

			//Test DrawPath with 2 colors: (bad performance)
			/*if(uColor == dColor){
				if(spline&&(double)(pCtrl->width/(pCtrl->endIndex-pCtrl->startIndex)<4.0F)){
					gDC->DrawCurve( blackPen, pointsF, 0.4F );
				}
				else{
					blackPen->LineJoin = System::Drawing::Drawing2D::LineJoin::Round;
					if(pointsF[0].X<pCtrl->panels[0]->GetX(pCtrl->endIndex))gDC->DrawLines( blackPen, pointsF );
				}
			}
			else{
				GraphicsPath^ pathUp = gcnew GraphicsPath();
				GraphicsPath^ pathDown = gcnew GraphicsPath();
				for(int i=0;i<points.size()-4;i+=2){				
					gDC->DrawLine(blackPen,pointsF[i],pointsF[i+1]);
					pathUp->AddLine(pointsF[i],pointsF[i+1]);
					pathDown->AddLine(pointsF[i+1],pointsF[i+2]);
			    }
				gDC->DrawPath(blackPen,pathUp);
				blackPen->Color = ColorTranslator::FromOle(dColor);
				gDC->DrawPath(blackPen,pathDown);
			}*/


			if(spline&&(double)(pCtrl->width/(pCtrl->endIndex-pCtrl->startIndex)<4.0F)){
				gDC->DrawCurve( blackPen, pointsF, 0.4F );
			}
			else{
				blackPen->LineJoin = System::Drawing::Drawing2D::LineJoin::Round;
				if(pointsF[0].X<pCtrl->panels[0]->GetX(pCtrl->endIndex))gDC->DrawLines( blackPen, pointsF );
			}
		}
		catch(...)
		{
			//printf("Excecao em DrawPath>>DrawLines() PARAMETERS INVALID: point[0].X=%f endIndex.X=%f",pointsF[0].X,pCtrl->panels[0]->GetX(pCtrl->endIndex));
		}

		//delete graphPath;
		delete blackPen;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		for(int i=0;i<points.size()-1;i++){
			pDC->MoveTo((int)points[i].x,(int)points[i].y);
			pDC->LineTo((int)points[i+1].x,(int)points[i+1].y);
		}
	}
	#pragma endregion

}

void PDCHandler::DrawSemiPath(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR uColor, OLE_COLOR dColor, CDC* pDC, bool spline/*=false*/)
{
	if(points.size()<=0) return;

	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		ID2D1GeometrySink *pSink = NULL;
		D2D1_POINT_2F* pointsF = NULL;
		pointsF = new D2D1_POINT_2F[points.size()];
		for(int i=0;i<points.size();i++){
			pointsF[i]=D2D1::Point2F(points[i].x,points[i].y);
		}
		
        pCtrl->pathGeometry->Open(&pSink);

        if (SUCCEEDED(hr))
        {
            pSink->SetFillMode(D2D1_FILL_MODE_WINDING);
            
            pSink->BeginFigure(
                D2D1::Point2F(0.0F, 0.0F),
				D2D1_FIGURE_BEGIN_FILLED
                );
			pSink->AddLines(pointsF,points.size());            
            pSink->EndFigure(D2D1_FIGURE_END_OPEN);
		}
		
        hr = pSink->Close();
		
		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		if(Style==0)pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();
		
		
		if (pSink != NULL)
		{
			pSink->Release();

			pSink = NULL;
		}*/
		
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){	
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(uColor),Width);
		/*blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;*/
		if(Style == PS_SOLID) blackPen->DashStyle = System::Drawing::Drawing2D::DashStyle::Solid;
		else{		
			// Create a custom dash pattern. EX: array<Single>^temp0 = {4.0F,2.0F,1.0F,3.0F};
			array<Single>^tempDASH={10.0f,3.0f,10.0f,3.0f}; //DASHED
			array<Single>^tempDOT={1.5f,2.0f,1.5f,2.0f}; //DOTTED
			if(Style == PS_DASH)blackPen->DashPattern = tempDASH;
			else blackPen->DashPattern = tempDOT;
		}
		
		array<PointF>^ pointsF = gcnew array<PointF>(points.size());
		for(int i=0;i<points.size();i++){
			pointsF[i]=PointF(points[i].x,points[i].y);
		}
		
		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		try{			
			GraphicsPath^ path = gcnew GraphicsPath();
			for(int i=0;i<points.size()-1;i+=2){	
				gDC->DrawLine(blackPen,pointsF[i],pointsF[i+1]);			
				//path->AddLine(pointsF[i],pointsF[i+1]);
			}
			//gDC->DrawPath(blackPen,path);

			/*if(spline&&(double)(pCtrl->width/(pCtrl->endIndex-pCtrl->startIndex)<4.0F)){
				gDC->DrawCurve( blackPen, pointsF, 0.4F );
			}
			else{
				blackPen->LineJoin = System::Drawing::Drawing2D::LineJoin::Round;
				if(pointsF[0].X<pCtrl->panels[0]->GetX(pCtrl->endIndex))gDC->DrawLines( blackPen, pointsF );
			}*/
		}
		catch(...)
		{
			//printf("Excecao em DrawPath>>DrawLines() PARAMETERS INVALID: point[0].X=%f endIndex.X=%f",pointsF[0].X,pCtrl->panels[0]->GetX(pCtrl->endIndex));
		}

		//delete graphPath;
		delete blackPen;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
	#pragma region GDI
	else{
		for(int i=0;i<points.size()-1;i++){
			pDC->MoveTo((int)points[i].x,(int)points[i].y);
			pDC->LineTo((int)points[i+1].x,(int)points[i+1].y);
		}
	}
	#pragma endregion

}

void PDCHandler::DrawLadder(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR Color, CDC* pDC)
{
	if(points.size()<=0) return;

	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		/*HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		ID2D1GeometrySink *pSink = NULL;
		D2D1_POINT_2F* pointsF = NULL;
		pointsF = new D2D1_POINT_2F[points.size()];
		for(int i=0;i<points.size();i++){
			pointsF[i]=D2D1::Point2F(points[i].x,points[i].y);
		}
		
        pCtrl->pathGeometry->Open(&pSink);

        if (SUCCEEDED(hr))
        {
            pSink->SetFillMode(D2D1_FILL_MODE_WINDING);
            
            pSink->BeginFigure(
                D2D1::Point2F(0.0F, 0.0F),
				D2D1_FIGURE_BEGIN_FILLED
                );
			pSink->AddLines(pointsF,points.size());            
            pSink->EndFigure(D2D1_FIGURE_END_OPEN);
		}
		
        hr = pSink->Close();
		
		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		if(Style==0)pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				double dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				double dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();
		
		
		if (pSink != NULL)
		{
			pSink->Release();

			pSink = NULL;
		}*/
		
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){	

		CPointF p1,p2;
		std::vector<CPointF> semiPoints;

		double x1Temp = pCtrl->panels[0]->GetX(0);
		double x2Temp = pCtrl->panels[0]->GetX(1);
		double widthDistance = (x2Temp - x1Temp)/2;
		double widthCandle = (x2Temp - x1Temp)*0.75;
		for(int i=0;i<points.size();i++){
			// Horizontal steps:
			/*p1.y=p2.y=points[i].y;
			if(points[i].x==NULL_VALUE || points[i-1].x==NULL_VALUE)continue;
			p1.x=pCtrl->panels[0]->GetX(i+1)-widthDistance;
			p2.x=pCtrl->panels[0]->GetX(i+1)+widthDistance;
			if(p1.x>p2.x) continue;
			if(p1.y!=NULL_VALUE)DrawLine(p1,p2,Width,Style,Color,pDC,false);
			// Vertical steps:
			p2.x = p1.x;
			p2.y = points[i-1].y;
			if(p1.y!=NULL_VALUE && p2.y!=NULL_VALUE)DrawLine(p1,p2,Width,Style,Color,pDC,false);*/


			//Using DrawPath

			if (points[i].x == NULL_VALUE || points[i].y == NULL_VALUE ){
				if (semiPoints.size() > 0)DrawPath(semiPoints, semiPoints.size(), Width, Style, Color, Color, pDC,false,false);
				semiPoints.clear();
				continue;
			}
			p1.y=p2.y=points[i].y;
			p1.x = pCtrl->panels[0]->GetX(i+1) - widthDistance;
			p2.x = pCtrl->panels[0]->GetX(i+1) + widthDistance;
			semiPoints.push_back(p1);
			semiPoints.push_back(p2);
#ifdef _CONSOLE_DEBUG
			if(i<6) printf("\nDrawLadder(%d) p1.x=%f p1.y = %f p2.x=%f p2.y=%f",i,p1.x,p1.y,p2.x,p2.y);
#endif

		}
		if (semiPoints.size() > 0)DrawPath(semiPoints, semiPoints.size(), Width, Style, Color, Color, pDC, false, false);


	}
	#pragma endregion
	
	#pragma region GDI
	else{
		for(int i=0;i<points.size()-1;i++){
			pDC->MoveTo((int)points[i].x,(int)points[i].y);
			pDC->LineTo((int)points[i+1].x,(int)points[i+1].y);
		}
	}
	#pragma endregion

}

void PDCHandler::DrawImage(CString name, CPointF point, double scale, CDC*pDC){

	#pragma region GDI+	
	if(pCtrl->PDCType==1){
		// Create image.
		//Image^ newImage =  Image::FromFile("C:\\Users\\Vinicius\\Desktop\\StockChartX\\trunk\\StockChartX\\Source\\plena_cinza.png");
		CString path = pCtrl->m_ApplicationDirectory;
		path+="\\Base\\plena_cinza.png"; //Size=124x60
		String^ pathStr = gcnew String(path);
		Image^ newImage =  Image::FromFile(pathStr);
		IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc );
		/*gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(Color),Width);
		blackPen->DashStyle = (Style == PS_DOT) ? System::Drawing::Drawing2D::DashStyle::Dot : 
				                (Style == PS_DASH) ? System::Drawing::Drawing2D::DashStyle::Dash :
									System::Drawing::Drawing2D::DashStyle::Solid;
		if (Width < 2.0f) gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::Default;*/
		PointF pointF = PointF(point.x,point.y);
		
		//Change Opacity:


		//Test CLIP:
		if(IsClipRegion){
			Drawing::Rectangle clipRec(0,0,pCtrl->GetX(pCtrl->endIndex-2)-pCtrl->GetX(pCtrl->startIndex),pCtrl->height);
			gDC->ExcludeClip(clipRec);
		}

		//It's possible to change scale:
		gDC->DrawImage(newImage,pointF.X,pointF.Y,newImage->Width*scale,newImage->Height*scale);

		//delete blackPen;
		delete pathStr;
		delete newImage;
		//gDC->ReleaseHdc(hdc);
		delete gDC;

	}
	#pragma endregion
	
}


void PDCHandler::DrawDot(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR Color, CDC* pDC)
{
	if(points.size()<=0) return;

	#pragma region Direct2D
	if(pCtrl->PDCType==2){
		HRESULT hr;
		
		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color,1.0F));
        
		pCtrl->m_Brush->SetOpacity(0.5F);

		ID2D1GeometrySink *pSink = NULL;
		D2D1_POINT_2F* pointsF = NULL;
		pointsF = new D2D1_POINT_2F[points.size()];
		for(int i=0;i<points.size();i++){
			pointsF[i]=D2D1::Point2F(points[i].x,points[i].y);
		}
		
        pCtrl->pathGeometry->Open(&pSink);

        if (SUCCEEDED(hr))
        {
            pSink->SetFillMode(D2D1_FILL_MODE_WINDING);
            
            pSink->BeginFigure(
                D2D1::Point2F(0.0F, 0.0F),
				D2D1_FIGURE_BEGIN_FILLED
                );
			pSink->AddLines(pointsF,points.size());            
            pSink->EndFigure(D2D1_FIGURE_END_OPEN);
#ifdef _CONSOLE_DEBUG
	//printf("\nDrawArc()");
#endif
		}
		
        hr = pSink->Close();
		
		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		if(Style==0)pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,(ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if(Style==1){ //Dashed
				float dashes[] = {10.0f,3.0f,10.0f,3.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
			else{ //Doted
				float dashes[] = {1.5f,2.0f,1.5f,2.0f};
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties,dashes,4,&m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry,pCtrl->m_Brush,(double)Width,m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();
		
		
		if (pSink != NULL)
		{
			pSink->Release();

			pSink = NULL;
		}
		
	}
	#pragma endregion
	
	#pragma region GDI+	
	else if(pCtrl->PDCType==1){	
		double scale = (double)Width+7;

#ifdef _CONSOLE_DEBUG
		printf ("\n\n Width = %d ", Width );
#endif

		CPointF p1,p2;
		for(int i=0;i<points.size();i++){
			p1.y=points[i].y - (scale/2);
			p1.x=points[i].x - (scale/2);
			if(points[i].x==NULL_VALUE || points[i-1].x==NULL_VALUE)continue;
			p2.y = p1.y + (scale);
			p2.x = p1.x + (scale);

			if(p1.y!=NULL_VALUE){
				CRectF rect ( p1.x, p1.y, p2.x, p2.y);
				FillRectangle ( rect, Color, pDC );
		
			}
		}
	}
			
	#pragma endregion
	
	#pragma region GDI
	else{
		for(int i=0;i<points.size()-1;i++){
			pDC->MoveTo((int)points[i].x,(int)points[i].y);
			pDC->LineTo((int)points[i+1].x,(int)points[i+1].y);
		}
	}
	#pragma endregion

}

void PDCHandler::DrawRoundDot(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR Color, CDC* pDC)
{
	if (points.size() <= 0) return;

#pragma region Direct2D
	if (pCtrl->PDCType == 2){
		HRESULT hr;

		pCtrl->m_Brush->SetColor(D2D1::ColorF(Color, 1.0F));

		pCtrl->m_Brush->SetOpacity(0.5F);

		ID2D1GeometrySink *pSink = NULL;
		D2D1_POINT_2F* pointsF = NULL;
		pointsF = new D2D1_POINT_2F[points.size()];
		for (int i = 0; i<points.size(); i++){
			pointsF[i] = D2D1::Point2F(points[i].x, points[i].y);
		}

		pCtrl->pathGeometry->Open(&pSink);

		if (SUCCEEDED(hr))
		{
			pSink->SetFillMode(D2D1_FILL_MODE_WINDING);

			pSink->BeginFigure(
				D2D1::Point2F(0.0F, 0.0F),
				D2D1_FIGURE_BEGIN_FILLED
				);
			pSink->AddLines(pointsF, points.size());
			pSink->EndFigure(D2D1_FIGURE_END_OPEN);
#ifdef _CONSOLE_DEBUG
			//printf("\nDrawArc()");
#endif
		}

		hr = pSink->Close();

		pCtrl->m_pDCRT->BeginDraw();
		pCtrl->m_pDCRT->SetTransform(D2D1::Matrix3x2F::Identity());
		if (Style == 0)pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry, pCtrl->m_Brush, (double)Width, (ID2D1StrokeStyle*)0);
		else{
			ID2D1StrokeStyle* m_strokeStyle;

			D2D1_STROKE_STYLE_PROPERTIES properties = D2D1::StrokeStyleProperties();
			properties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
			if (Style == 1){ //Dashed
				float dashes[] = { 10.0f, 3.0f, 10.0f, 3.0f };
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties, dashes, 4, &m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry, pCtrl->m_Brush, (double)Width, m_strokeStyle);
			}
			else{ //Doted
				float dashes[] = { 1.5f, 2.0f, 1.5f, 2.0f };
				pCtrl->m_pD2DFactory->CreateStrokeStyle(properties, dashes, 4, &m_strokeStyle);
				pCtrl->m_pDCRT->DrawGeometry(pCtrl->pathGeometry, pCtrl->m_Brush, (double)Width, m_strokeStyle);
			}
		}
		pCtrl->m_pDCRT->EndDraw();


		if (pSink != NULL)
		{
			pSink->Release();

			pSink = NULL;
		}

	}
#pragma endregion

#pragma region GDI+	
	else if (pCtrl->PDCType == 1){
		double scale = (double)Width;
		if (points[1].x - points[0].x < 1) scale += 2;
		else if (points[1].x - points[0].x < 2) scale += 4;
		else if (points[1].x - points[0].x < 4) scale += 6;
		else if (points[1].x - points[0].x < 8) scale += 7;
		else if (points[1].x - points[0].x < 15) scale += 8;
		else if (points[1].x - points[0].x < 20) scale += 9;
		else scale+=10; 

		scale /= 5;
		//scale = (points[1].x - points[0].x)/6.0;

#ifdef _CONSOLE_DEBUG
		printf("\n\n Width = %d ", Width);
#endif

		/*IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
		Graphics^ gDC = Graphics::FromHdc(hdc);
		gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
		gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
		Pen^ blackPen = gcnew Pen(ColorTranslator::FromOle(Color), Width+2);
		blackPen->DashStyle = System::Drawing::Drawing2D::DashStyle::Solid;
		PointF p1; 
		PointF p2;
		try{

			for (int i = 0; i < points.size(); i++){
				p1 = PointF(points[i].x,points[i].y);
				p2 = PointF(points[i].x+2, points[i].y);
				gDC->DrawLine(blackPen, p1, p2);
			}
		}
		catch (...){
		}
		delete blackPen;
		delete gDC;*/

		

		CPointF p1,p2;
		for (int i = 0; i<points.size(); i++){
			p1.y = points[i].y - (scale);
			p1.x = points[i].x - (scale);
			if (points[i].x == NULL_VALUE || points[i - 1].x == NULL_VALUE)continue;
			p2.y = points[i].y + (scale);
			p2.x = points[i].x + (scale);

			if (p1.y != NULL_VALUE){
				CRectF rect(p1.x, p1.y, p2.x, p2.y);
				FillCircle(rect, Color, pDC);

			}
		}
	}

#pragma endregion

#pragma region GDI
	else{
		for (int i = 0; i<points.size() - 1; i++){
			pDC->MoveTo((int)points[i].x, (int)points[i].y);
			pDC->LineTo((int)points[i + 1].x, (int)points[i + 1].y);
		}
	}
#pragma endregion

}


