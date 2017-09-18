
#ifndef __DPI_H__
#define __DPI_H__

static double scaleX;
static double scaleY;

template <class T> inline static T SCALEX(T argx)
{
  return argx * scaleX;
}

template <class T> inline static T SCALEY(T argy)
{
  return argy * scaleY;
}

inline static void InitScaling()
{
  static BOOL called = FALSE;
  if (called) return;
  called = TRUE;
  
  HDC screen = GetDC(0);
  scaleX = GetDeviceCaps(screen, LOGPIXELSX) / 96.0;
  scaleY = GetDeviceCaps(screen, LOGPIXELSY) / 96.0;
  ReleaseDC(0, screen);
}

inline static RECT ScaleRectangle(const RECT &rc)
{
  RECT rcOut;
  
  rcOut.left = SCALEX(rc.left);
  rcOut.top = SCALEY(rc.top);
  rcOut.right = SCALEX(rc.right);
  rcOut.bottom = SCALEY(rc.bottom);
  
  return rcOut;
}

inline static RECT ScaleRectangleProportional(const RECT &rc)
{
  RECT rcOut;
  
  rcOut = ScaleRectangle(rc);
  
  double newWidth = (rcOut.right - rcOut.left);
  double newHeight = (rcOut.bottom - rcOut.top);
  
  rcOut.left = (rc.right + rc.left) / 2 - (newWidth / 2);
  rcOut.right = (rc.right + rc.left) / 2 + (newWidth / 2);
  rcOut.top = (rc.bottom + rc.top) / 2 - (newHeight / 2);
  rcOut.bottom = (rc.bottom + rc.top) / 2 + (newHeight / 2);
  
  return rcOut;
}

inline static POINT ScalePoint(const POINT &pt)
{
  POINT ptOut;
  
  ptOut.x = SCALEX(pt.x);
  ptOut.y = SCALEY(pt.y);
  
  return ptOut;
}

class CDpiManager
{
public:
  CDpiManager()
  {
    InitScaling();
  }
};

static CDpiManager G_dpiManager;

#endif