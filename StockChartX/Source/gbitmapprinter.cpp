
//=======================================================================
#include "StdAfx.h"
#include "gbitmapprinter.h"
using namespace printer;
//=======================================================================


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: GBitmapPrinter() [public]               //
// SYNOPSIS: Default constructor.                    // 
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
GBitmapPrinter::GBitmapPrinter() : pDIB_(NULL), pBits_(NULL),
   scale_(GBP_SCALE_CUSTOM)
{
   memset(doc_name_, 0, sizeof(TCHAR) * MAX_PATH);
   memset(&RPrint_, 0, sizeof(RECT));
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: ~GBitmapPrinter() [public]              //
// SYNOPSIS: Default destructor.                     //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
GBitmapPrinter::~GBitmapPrinter()
{
   delete [] reinterpret_cast<unsigned char*>(pDIB_);
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoSetBitmap() [protected]               //
// SYNOPSIS: Sets the DDB or DIB section bitmap      //
//           to print; hPal specifies the DDB's      //
//           palette, when required.                 //
//                                                   //
// [Exposed via the SetBitmap() member function]     //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoSetBitmap(
    HBITMAP hBmp,
    HPALETTE hPal
   )
{
   BITMAP bmp = {0};
   const size_t size_got = GetObject(hBmp, sizeof(BITMAP), &bmp);
   if (size_got != sizeof(BITMAP))
   {
      throw GBitmapPrinterError("Invalid bitmap.");
   }
   
   // if hBmp is a DIB section
   if (bmp.bmBits)
   {
      DoDIBFromDIBSection(hBmp);
   }
   // if hbmp is a DDB
   else
   {
      DoDIBFromDDB(hBmp, hPal);
   }
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoPrintBitmap() [protected]             //
// SYNOPSIS: Prints the bitmap (previsouly set       //
//           by the SetBitmap() function) to the     //
//           specified device context.               //
//                                                   //
// [Exposed via the PrintBitmap() member function]   //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoPrintBitmap(
    HDC hPrinterDC
   )
{
   if (!pDIB_)
   {
      throw GBitmapPrinterError("No bitmap defined.");
   }
   if (!hPrinterDC)
   {
      throw GBitmapPrinterError("Invalid printer DC.");
   }

   // compute the rectangle to print to
   DoCalculatePrintRect(hPrinterDC);

   // print the bitmap
   DoInternalPrintBitmap(hPrinterDC);
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoPrintBitmapDoc() [protected]          //
// SYNOPSIS: Prints the bitmap (previsouly set       //
//           by the SetBitmap() function) to the     //
//           specified device context as a           //
//           as a separate document.                 //
//                                                   //
// [Exposed via the PrintBitmapDoc() member          //
//  function; see also the SetDocName() function]    //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoPrintBitmapDoc(
    HDC hPrinterDC
   )
{
   if (!pDIB_)
   {
      throw GBitmapPrinterError("No bitmap defined.");
   }
   if (!hPrinterDC)
   {
      throw GBitmapPrinterError("Invalid printer DC.");
   }

   DOCINFO di = {sizeof(DOCINFO), TEXT(doc_name_)};
   if (StartDoc(hPrinterDC, &di) > 0)
   {
      if (StartPage(hPrinterDC) > 0)
      {
         try
         {
            // print the DIB
            DoPrintBitmap(hPrinterDC);
         }
         catch (const GBitmapPrinterError&)
         {
            EndPage(hPrinterDC);
            EndDoc(hPrinterDC);
            throw;
         }
         EndPage(hPrinterDC);
      }
      EndDoc(hPrinterDC);
   }
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoSetDocName() [protected]              //
// SYNOPSIS: Sets the document name for use with     //
//           the PrintBitmapDoc() function.          //
//                                                   //
// [Exposed via the SetDocName() function]           //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoSetDocName(
    LPCSTR doc_name
   )
{
   lstrcpyn(doc_name_, doc_name, MAX_PATH);   
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoSetPrintRect() [protected]            //
// SYNOPSIS: Sets the target rectangle to which      //
//           bitmap is stretched-to-fit.             //
//                                                   //
// [Exposed via the SetPrintRect() function]         //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoSetPrintRect(
    const RECT& RPrint
   )
{
   RPrint_ = RPrint;
   scale_ = GBP_SCALE_CUSTOM;
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoSetPrintScale() [protected]           //
// SYNOPSIS: Sets the scaling option to one of the   //
//           GPrinterScale enumerators:              //
//                                                   //
//       o  GBP_SCALE_HORZ: fit horizontally         //
//       o  GBP_SCALE_VERT: fit vertically           //
//       o  GBP_SCALE_PAGE: fit to page              //
//       o  GBP_SCALE_CUSTOM: custom (the user       //
//          must specify the target rectangle        //
//          before printing)                         //
//                                                   //
// [Exposed via the SetPrintScale() function]        //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoSetPrintScale(
    GPrinterScale scale
   )
{
   if (scale >= GBP_SCALE_HORZ && scale <= GBP_SCALE_CUSTOM)
   {
      scale_ = scale;
   }
   else throw GBitmapPrinterError("Invalid scale.");
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoCalculatePrintRect() [protected]      //
// SYNOPSIS: Computes the target rectangle when the  //
//           scaling option is set to                //
//           GBP_SCALE_HORZ, GBP_SCALE_VERT, or      //
//           GBP_SCALE_PAGE.                         //
//                                                   //
// [Used by the DoPrintBitmap() function]            //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoCalculatePrintRect(
    HDC hPrinterDC
   )
{
   // page width
   const int page_width = GetDeviceCaps(hPrinterDC, HORZRES);
   // page height
   const int page_height = GetDeviceCaps(hPrinterDC, VERTRES);
   // pixels per inch horizontally
   const int pix_inchX = GetDeviceCaps(hPrinterDC, LOGPIXELSX);
   // pixels per inch vertically
   const int pix_inchY = GetDeviceCaps(hPrinterDC, LOGPIXELSY);

   // grab a pointer to the DIB's header
   LPBITMAPINFOHEADER pHeader =
      reinterpret_cast<LPBITMAPINFOHEADER>(pDIB_);

   //
   // adjust the target rectangle according
   // to the specified scaling options...
   //
   switch (scale_)
   {
      // fit horizontally
      case GBP_SCALE_HORZ:
      {
         RPrint_.left = RPrint_.top = 0;
         RPrint_.right = page_width;
         RPrint_.bottom =
            0.5 + abs(pHeader->biHeight) *
//	Revision notes - will not bother static_cast as it may bother inner workings of program
//	Please ignore the two warnings
            (page_width / static_cast<double>(pHeader->biWidth)) *
            (pix_inchY / static_cast<double>(pix_inchX));
         break;
      }
      // fit vertically
      case GBP_SCALE_VERT:
      {
         RPrint_.left = RPrint_.top = 0;
         RPrint_.bottom = page_height;
         RPrint_.right =
            0.5 + pHeader->biWidth *
            (page_height / static_cast<double>(abs(pHeader->biHeight))) *
            (pix_inchX / static_cast<double>(pix_inchY));
         break;
      }
      // fit to page
      case GBP_SCALE_PAGE:
      {
         RPrint_.left = RPrint_.top = 0;
         RPrint_.right = page_width;
         RPrint_.bottom = page_height;
         break;
      }
   }
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoCalculatePrintRect() [protected]      //
// SYNOPSIS: Computes a strip of the target          //
//           rectangle (RBand) and a corresponding   //
//           strip of the source image (RImage).     //
//                                                   //
// [Used by the DoInternalPrintBitmap() function     //
//  on Windows 9x/Me-based systems]                  //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
bool GBitmapPrinter::DoCalculateBand(
    RECT& RBand,
    RECT& RImage
   )
{
   // compute the printable band area
   if (IntersectRect(&RBand, &RBand, &RPrint_))
   {
      // ratio of the print width to the image width
      const double ratioX =
         static_cast<double>(RPrint_.right - RPrint_.left) /
         static_cast<double>(RImage.right - RImage.left);

      // ratio of the print height to the image height
      const double ratioY =
         static_cast<double>(RPrint_.bottom - RPrint_.top) /
         static_cast<double>(RImage.bottom - RImage.top);

      //
      // compute the scaled band (this will tell
      // us where in the image to blit from)...
      //
      RImage.left = static_cast<int>(
         0.5 + (RBand.left - RPrint_.left) / ratioX
         );  
      RImage.right = RImage.left + static_cast<int>(
         0.5 + (RBand.right - RBand.left) / ratioX
         );
      RImage.top = static_cast<int>(
         0.5 + (RBand.top - RPrint_.top) / ratioY
         );
      RImage.bottom = RImage.top + static_cast<int>(
         0.5 + (RBand.bottom - RBand.top) / ratioY
         );
      return true;
   }
   return false;
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoInternalPrintBitmap() [protected]     //
// SYNOPSIS: Main printing engine.                   //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoInternalPrintBitmap(
    HDC hPrinterDC
   )
{
   if (IsRectEmpty(&RPrint_))
   {
      throw GBitmapPrinterError("Invalid target rectangle.");
   }

   // TODO: add support for (rare) palettized printers
   const BOOL palettized =
      (GetDeviceCaps(hPrinterDC, RASTERCAPS) & RC_PALETTE);
   if (palettized)
   {
      throw GBitmapPrinterError("Unsupported printer.");
   }

   if (pDIB_ && pBits_)
   {
      // extract the width and the height from the DIB's header
      const int dib_width = pDIB_->bmiHeader.biWidth;
      const int dib_height = pDIB_->bmiHeader.biHeight;

      //
      // query the printer for the NEXTBAND capability
      // (not really needed, but better safe than sorry)
      //
      const int query_cap = NEXTBAND;
      int do_bands = Escape(
         hPrinterDC, QUERYESCSUPPORT, sizeof(int),
         reinterpret_cast<LPCSTR>(&query_cap), NULL
         );

      // test the OS version...
      OSVERSIONINFO osvi = {sizeof(OSVERSIONINFO)};
      if (GetVersionEx(&osvi))
      {
         // only do (sub)banding on Win9x/Me-based systems
         do_bands &= (osvi.dwPlatformId != VER_PLATFORM_WIN32_NT);
      }

      //
      // proceed with printing...
      //
      BOOL result = TRUE;
      if (do_bands)
      {
         RECT RBand = {0};
         while (Escape(hPrinterDC, NEXTBAND, 0, NULL, &RBand))
         {
            if (IsRectEmpty(&RBand)) break;

            const int band_height =
              min(75L, RBand.bottom - RBand.top);
            const int num_bands =
              0.5 + (RBand.bottom - RBand.top) /
              static_cast<double>(band_height);

            for (int iBand = 0; iBand < num_bands; ++iBand)
            {
               RBand.top = iBand * band_height;
               RBand.bottom = RBand.top + band_height;

               RECT RImage = {0, 0, dib_width, dib_height};
               if (DoCalculateBand(RBand, RImage))
               {
                  // set the stretching mode to preserve colors
                  SetStretchBltMode(hPrinterDC, COLORONCOLOR);

                  // send the DIB to the printer
                  result &= GDI_ERROR !=
                     StretchDIBits(
                        // draw to the printer's DC
                        hPrinterDC,
                        // dest rect
                        RBand.left, RBand.top,
                        RBand.right - RBand.left,
                        RBand.bottom - RBand.top,
                        // source rect
                        RImage.left, dib_height - RImage.bottom,
                        RImage.right - RImage.left,
                        RImage.bottom - RImage.top,
                        // source bits
                        pBits_,
                        // source DIB
                        pDIB_,
                        // DIB's color table contains RGBQUADs
                        DIB_RGB_COLORS,
                        // copy source
                        SRCCOPY
                        );
               }
            }
         }
      }
      else // no banding--draw the DIB in its entirety
      {
         // set the stretching mode to preserve colors
         SetStretchBltMode(hPrinterDC, COLORONCOLOR);

         result &= GDI_ERROR !=
            StretchDIBits(
               // draw to the printer's DC
               hPrinterDC,
               // dest rect
               RPrint_.left, RPrint_.top,
               RPrint_.right - RPrint_.left,
               RPrint_.bottom - RPrint_.top,
               // source rect (entire DIB)
               0, 0, dib_width, dib_height,
               // source bits
               pBits_,
               // source DIB
               pDIB_,
               // DIB's color table contains RGBQUADs
               DIB_RGB_COLORS,
               // copy source
               SRCCOPY
               );
      }

      // report errors
      if (!result)
      {
         throw GBitmapPrinterError("Error printing DIB.");
      }
   }
   // report errors
   else
   {
      throw GBitmapPrinterError("Error getting DIB info.");
   }
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoDIBFromDDB() [protected]              //
// SYNOPSIS: "Creates" a 24-bit DIB from a DDB.      //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoDIBFromDDB(
    HBITMAP hDDB,
    HPALETTE hPal
   )
{   
   BITMAP bmp;
   static const size_t bmpSize = sizeof(BITMAP);
   if (GetObject(hDDB, bmpSize, &bmp) == bmpSize)
   {
      // convert strictly to 24-bpp DIB (safest route for printing)
      const unsigned char bpp = 24;
      
      // compute the number of bytes per scanline
      const size_t bytes_per_scanline =
         (((bmp.bmWidth * bpp) + 31) & ~31) >> 3;

      // compute the required sizes
      const size_t header_size = sizeof(BITMAPINFOHEADER);
      const size_t pixels_size = bmp.bmHeight * bytes_per_scanline;
      const size_t total_DIB_size = header_size + pixels_size;

      // allocate memory for the DIB
      delete [] reinterpret_cast<unsigned char*>(pDIB_);
      pDIB_ = reinterpret_cast<PBITMAPINFO>(
         new unsigned char[total_DIB_size]
         );

      // grab a pointer to the DIB's pixels
      pBits_ = reinterpret_cast<unsigned char*>(pDIB_) + header_size;
      
      // initialize the header:
      PBITMAPINFOHEADER pHeader =
         reinterpret_cast<PBITMAPINFOHEADER>(pDIB_);
      memset(pHeader, 0, total_DIB_size);
      pHeader->biSize = header_size;
      pHeader->biWidth = bmp.bmWidth;
      pHeader->biHeight = bmp.bmHeight;
      pHeader->biBitCount = bpp;
      pHeader->biPlanes = 1;
      pHeader->biCompression = BI_RGB;

      // copy the pixels and the color table:
      HDC hScreenDC = GetDC(NULL);
      if (hPal)
      {
         hPal = SelectPalette(hScreenDC, hPal, TRUE);
         RealizePalette(hScreenDC);
      }
      const BOOL success = 
         GetDIBits(
            // handle the screen's DC
            hScreenDC, hDDB,
            // first and total scanline(s) to copy
            0, bmp.bmHeight,
            // pointer to the DIB
            pBits_, pDIB_,
            // no color table needed
            DIB_RGB_COLORS
            );
      if (hPal)
      {
         hPal = SelectPalette(hScreenDC, hPal, TRUE);
      }
      ReleaseDC(NULL, hScreenDC);

      // report errors
      if (!success)
      {
         throw GBitmapPrinterError("Error getting DDB bits.");
      }
   }
   // report errors
   else
   {
      throw GBitmapPrinterError("Error getting DDB info.");
   }
}
//-----------------------------------------------------------------------


// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
//                                                   //
// FUNCTION: DoDIBFromDIBSection() [protected]       //
// SYNOPSIS: "Creates" a DIB from a DIB section      //
//           bitmap.                                 //
//                                                   //
// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ //
void GBitmapPrinter::DoDIBFromDIBSection(
    HBITMAP hDIBSection
   )
{
   DIBSECTION ds;
   static const size_t dibSize = sizeof(DIBSECTION);
   if (GetObject(hDIBSection, dibSize, &ds) == dibSize)
   {
      // compute the number of bytes per scanline
      const size_t bytes_per_scanline =
         (((ds.dsBm.bmWidth * ds.dsBm.bmBitsPixel) + 31) & ~31) >> 3;

      // compute the number of color table entries         
	  
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (short)
      short num_ct_entries = (short)ds.dsBmih.biClrUsed;
//	End Of Revision
      if (num_ct_entries == 0 && ds.dsBm.bmBitsPixel <= 8)
      {
         num_ct_entries = (1 << ds.dsBm.bmBitsPixel);
      }
      else if (ds.dsBmih.biCompression != BI_RGB)
      {
         // punt to the DoDIBFromDDB() function
         DoDIBFromDDB(hDIBSection, NULL);
         return; 
      }

      // compute the required sizes
      const size_t header_size = sizeof(BITMAPINFOHEADER);
      const size_t ct_size = sizeof(RGBQUAD) * num_ct_entries;
      const size_t pixels_size = ds.dsBm.bmHeight * bytes_per_scanline;
      const size_t total_DIB_size = header_size + ct_size + pixels_size;

      // allocate memory for the DIB
      delete [] reinterpret_cast<unsigned char*>(pDIB_);
      pDIB_ = reinterpret_cast<PBITMAPINFO>(
         new unsigned char[total_DIB_size]
         );

      // grab a pointer to the DIB's pixels
      pBits_ =
         reinterpret_cast<unsigned char*>(pDIB_) +
         header_size + ct_size;

      // copy the header...
      memcpy(pDIB_, &ds.dsBmih, header_size);

      // copy the color table...
      if (num_ct_entries > 0 &&
          ds.dsBmih.biCompression != BI_BITFIELDS)
      {
         // select the DIB section into a memory DC
         HDC hMemDC = CreateCompatibleDC(NULL);
         hDIBSection = static_cast<HBITMAP>(
            SelectObject(hMemDC, hDIBSection)
            );

         // grab a pointer to the DIB's color table
         RGBQUAD* pColorTable = reinterpret_cast<RGBQUAD*>(
            reinterpret_cast<unsigned char*>(pDIB_) + header_size
            );

         // fill the DIB's color table
         GetDIBColorTable(hMemDC, 0, num_ct_entries, pColorTable);

         // clean up
         hDIBSection = static_cast<HBITMAP>(
            SelectObject(hMemDC, hDIBSection)
            );
         DeleteDC(hMemDC);
      }

      // copy the pixels
      memcpy(pBits_, ds.dsBm.bmBits, pixels_size);
   }
   // report errors
   else
   {
      throw GBitmapPrinterError("Error getting DIB section info.");
   }
}
//-----------------------------------------------------------------------

//=======================================================================

