
//=======================================================================
#ifndef GBITMAP_PRINTER_H
#define GBITMAP_PRINTER_H
//=======================================================================

#include <stddef.h>
#include <windows.h>
#include <stdexcept>
//=======================================================================

// change namespace name appropriately to suit your needs
namespace printer {
	
typedef std::runtime_error GBitmapPrinterError; 
enum GPrinterScale {
   GBP_SCALE_HORZ, GBP_SCALE_VERT, GBP_SCALE_PAGE, GBP_SCALE_CUSTOM
   };

class GBitmapPrinter
{
public:
   GBitmapPrinter();
   ~GBitmapPrinter();

   // sets the bitmap to print (DDB or DIB section)
   void SetBitmap(HBITMAP hBmp, HPALETTE hPal = NULL)
      { DoSetBitmap(hBmp, hPal); }

   // main printing member functions   
   void PrintBitmap(HDC hPrinterDC)
      { DoPrintBitmap(hPrinterDC); }
   void PrintBitmapDoc(HDC hPrinterDC)
      { DoPrintBitmapDoc(hPrinterDC); }

   // gets/sets the document name
   LPCSTR GetDocName() { return DoGetDocName(); }
   void SetDocName(LPCSTR doc_name) { DoSetDocName(doc_name); }

   // gets/sets the target printing rectangle
   RECT GetPrintRect() { return DoGetPrintRect(); }
   void SetPrintRect(const RECT& RPrint) { DoSetPrintRect(RPrint); }

   // gets/sets the scaling options
   GPrinterScale GetPrintScale() { return DoGetPrintScale(); }
   void SetPrintScale(GPrinterScale scale) { DoSetPrintScale(scale); }

protected:
   // main printing member functions
   virtual void DoPrintBitmap(HDC hPrinterDC);
   virtual void DoPrintBitmapDoc(HDC hPrinterDC);
   virtual void DoInternalPrintBitmap(HDC hPrinterDC);

   // gets/sets class properties
   virtual void DoSetBitmap(HBITMAP hBmp, HPALETTE hPal);
   virtual LPCSTR DoGetDocName() { return doc_name_; }
   virtual void DoSetDocName(LPCSTR doc_name);
   virtual RECT DoGetPrintRect() { return RPrint_; }
   virtual void DoSetPrintRect(const RECT& RPrint);
   virtual GPrinterScale DoGetPrintScale() { return scale_; }
   void DoSetPrintScale(GPrinterScale scale);

   // internal utility member functions
   virtual void DoCalculatePrintRect(HDC hPrinterDC);
   virtual bool DoCalculateBand(RECT& RBand, RECT& RImage);
   virtual void DoDIBFromDDB(HBITMAP hDDB, HPALETTE hPal);
   virtual void DoDIBFromDIBSection(HBITMAP hDIBSection);

private:
   // pointer to the internal DIB (header and color table)
   PBITMAPINFO pDIB_;
   // pointer to the internal DIB's pixels
   unsigned char* pBits_;
   // document name
   TCHAR doc_name_[MAX_PATH];
   // bounding rectangle to print to
   RECT RPrint_;
   // scaling option
   GPrinterScale scale_;
};

}  // namespace printer

//=======================================================================
#endif  // BITMAP_PRINTER_H
//=======================================================================
