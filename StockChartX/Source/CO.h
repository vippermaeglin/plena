// CO.h: interface for the CCO class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CO_H__543E3E97_681B_449C_B12B_49EB40A366CE__INCLUDED_)
#define AFX_CO_H__543E3E97_681B_449C_B12B_49EB40A366CE__INCLUDED_

class CChartPanel;
class CStockChartXCtrl;
class CRGB;

class CCO  
{
public:
	CString fileName;
	virtual void OnRButtonUp(CPoint point);
	virtual void OnMessage(LPCTSTR MsgGuid, int MsgID);
	CString guid;
	virtual void OnDoubleClick(CPoint point);
	virtual void SetXY();
	virtual void Connect(CStockChartXCtrl *Ctrl);
	virtual void OnRButtonDown(CPoint point);
	virtual void OnLButtonDown(CPoint point);
	virtual void OnLButtonUp(CPoint point);
	virtual void OnPaintXOR(CDC* pDC);
	virtual void OnMouseMove(CPoint point);
	virtual CRect GetRect();
	virtual void UpdatePoints();
	virtual void ResetPoints();
	long	bitmapID;
	bool	connected;
	
	int		Height();
	int		Width();
	void	Reset(bool updateRect);
	
	CBitmap*	oldBmp;
	CBitmap*	oldCacheBmp;
	CDC	memDC;
	CDC	cacheDC;
	void	Show();
	void	Hide();
	void	Initialize();	
	void	OnClick(int x, int y);
	virtual	void	OnPaint(CDC* pDC);
	bool	selectable;
	bool	selected;
	OLE_COLOR	backColor;
	OLE_COLOR	foreColor;
	int		lineWeight;
	int		lineStyle;
	bool	visible;
	int	zOrder;
	double	x1;
	double	x2;
	double	y1;
	double	y2;
	double x1Value;
	double x2Value;
	double y1Value;
	double y2Value;
	double x1JDate;
	double x2JDate;
	CString key;
	CString text;
	CString objectType;
	int nType;
	CChartPanel* ownerPanel;
	CCO(CChartPanel* owner);
	CCO();
	virtual	~CCO();
	CBitmap	cache_bitmap;
	CBitmap	background_bitmap;
	bool	background_cached;
	CRect oldRect;
	CRect newRect;
	CStockChartXCtrl* pCtrl;
	void CalculateXJulianDate(void);
};
/////////////////////////////////////////////////////////////////////////////

class CFileBitmap : public CBitmap
{
public:
	BOOL CFileBitmap::LoadBMPFile(CString csFile)
	{
		HBITMAP hBitmap = (HBITMAP)::LoadImage(	NULL,  csFile.GetBuffer( csFile.GetLength() ),
												IMAGE_BITMAP, 0, 0,
												LR_LOADFROMFILE		|
												LR_CREATEDIBSECTION	|
												LR_DEFAULTSIZE		|
												LR_VGACOLOR
											);
		if( hBitmap )
		{
			this->DeleteObject();
			this->Attach( hBitmap );
			return TRUE;
		}
		else
		{
			return FALSE;
		}
	}
};
/////////////////////////////////////////////////////////////////////////////

#endif // !defined(AFX_CO_H__543E3E97_681B_449C_B12B_49EB40A366CE__INCLUDED_)
