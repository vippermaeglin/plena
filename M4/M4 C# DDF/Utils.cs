/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Collections.Generic;


namespace M4
{
    public static class Utils
    {
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int LCMapString(int Locale, int dwMapFlags, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpSrcStr, int cchSrc, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpDestStr, int cchDest);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern int LCMapStringA(int Locale, int dwMapFlags, [MarshalAs(UnmanagedType.LPArray)] byte[] lpSrcStr, int cchSrc, [MarshalAs(UnmanagedType.LPArray)] byte[] lpDestStr, int cchDest);

        public static string GetCurrencySymbol()
        {
            return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
        }

        public static string GetValue(string value, string defValue)
        {
            return string.IsNullOrEmpty(value) ? defValue : value;
        }

        public static bool IsNumeric(object Expression)
        {
            IConvertible convertible = Expression as IConvertible;
            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return true;

                    case TypeCode.Char:
                    case TypeCode.String:
                        {
                            double num;
                            string str = convertible.ToString(null);
                            try
                            {
                                long num2 = 0;
                                if (IsHexOrOctValue(str, ref num2))
                                {
                                    return true;
                                }
                            }
                            catch (FormatException)
                            {
                                return false;
                            }
                            return double.TryParse(str, out num);
                        }
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
            }
            return false;
        }

        internal static bool IsHexOrOctValue(string Value, ref long i64Value)
        {
            int num = 0;
            int length = Value.Length;
            while (num < length)
            {
                char ch = Value[num];
                if ((ch == '&') && ((num + 2) < length))
                {
                    ch = char.ToLower(Value[num + 1], CultureInfo.InvariantCulture);
                    string str = ToHalfwidthNumbers(Value.Substring(num + 2), GetCultureInfo());
                    switch (ch)
                    {
                        case 'h':
                            i64Value = Convert.ToInt64(str, 0x10);
                            return true;

                        case 'o':
                            i64Value = Convert.ToInt64(str, 8);
                            return true;
                    }
                    throw new FormatException();
                }
                if ((ch != ' ') && (ch != '　'))
                {
                    return false;
                }
                num++;
            }
            return false;
        }

        internal static string ToHalfwidthNumbers(string s, CultureInfo culture)
        {
            int num = culture.LCID & 0x3ff;
            if (((num != 4) && (num != 0x11)) && (num != 0x12))
            {
                return s;
            }
            return vbLCMapString(culture, 0x400000, s);
        }

        internal static string vbLCMapString(CultureInfo loc, int dwMapFlags, string sSrc)
        {
            int length = sSrc == null ? 0 : sSrc.Length;
            if (length == 0)
            {
                return "";
            }
            int lCID = loc.LCID;
            Encoding encoding = Encoding.GetEncoding(loc.TextInfo.ANSICodePage);
            if (!encoding.IsSingleByte)
            {
                string s = sSrc;
                if (s != null)
                {
                    byte[] bytes = encoding.GetBytes(s);
                    int num2 = LCMapStringA(lCID, dwMapFlags, bytes, bytes.Length, null, 0);
                    byte[] buffer = new byte[(num2 - 1) + 1];
                    LCMapStringA(lCID, dwMapFlags, bytes, bytes.Length, buffer, num2);
                    return encoding.GetString(buffer);
                }
            }
            string lpDestStr = new string(' ', length);
            LCMapString(lCID, dwMapFlags, ref sSrc, length, ref lpDestStr, length);
            return lpDestStr;
        }

        internal static CultureInfo GetCultureInfo()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        internal static int GetLocaleCodePage()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
        }
        public static char Chr(int CharCode)
        {
            if ((CharCode < -32768) || (CharCode > 65535))
            {
                throw new ArgumentException("Argument out of range");
            }
            if ((CharCode >= 0) && (CharCode <= 127))
            {
                return Convert.ToChar(CharCode);
            }
            Encoding encoding = Encoding.GetEncoding(GetLocaleCodePage());
            if (encoding.IsSingleByte && ((CharCode < 0) || (CharCode > 255)))
            {
                throw new ArgumentException("Argument out of range");
            }
            char[] chars = new char[2];
            byte[] bytes = new byte[2];
            Decoder decoder = encoding.GetDecoder();
            if ((CharCode >= 0) && (CharCode <= 255))
            {
                bytes[0] = (byte)(CharCode & 255);
                decoder.GetChars(bytes, 0, 1, chars, 0);
            }
            else
            {
                bytes[0] = (byte)((CharCode & 65280) >> 8);
                bytes[1] = (byte)(CharCode & 255);
                decoder.GetChars(bytes, 0, 2, chars, 0);
            }
            return chars[0];
        }

        public static T ValueOrDef<T>(T value, T wrongValue, T defValue)
        {
            return value.Equals(wrongValue) ? defValue : value;
        }

        public static bool IsDBNull(object expression)
        {
            if (expression == null)
                return false;
            return expression is DBNull;
        }

        [Conditional("DEBUG")]
        public static void Trace(string format, params object[] args)
        {
            Debug.WriteLine(DateTime.Now + " - " + string.Format(format, args));
        }


        public static List<double> ExtractNumbers(string expression)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            char separator = nfi.NumberDecimalSeparator.ToCharArray()[0];

            List<double> ret = new List<double>();
            char chr;
            string current = "";
            for (int i = 0; i < expression.Length; i++)
            {
                chr = Convert.ToChar(expression.Substring(i, 1));
                if (Char.IsNumber(chr) || chr == separator)
                {
                    current += chr.ToString();
                }
                else if (current != "" && current != separator.ToString())
                {
                    ret.Add(double.Parse(current));
                    current = "";
                }

                if (current != "" && current != separator.ToString())
                    ret.Add(double.Parse(current));

            }
            return ret;
        }

        /// <summary>
        /// Retorna a cor default para fundos de caixa de dialogo
        /// Necessário o RGB ser separado por ',' sem espaços. Ex.: 255,255,255
        /// </summary>
        /// <returns></returns>
        public static Color GetDefaultBackColor()
        {
            var rgb = Properties.Settings.Default.DefaultBackColor.Trim().Split(',');
            return Color.FromArgb(int.Parse(rgb[0].Trim()), int.Parse(rgb[1].Trim()), int.Parse(rgb[2].Trim()));
        }

        public static Color GetDefaultLabelGraphColor()
        {
            var rgb = Properties.Settings.Default.DefaultLabelGraphColor.Trim().Split(',');
            return Color.FromArgb(int.Parse(rgb[0].Trim()), int.Parse(rgb[1].Trim()), int.Parse(rgb[2].Trim()));
        }

        public static Color GetDefaultBackColorChart()
        {
            var rgb = Properties.Settings.Default.DefaultBackColorChart.Trim().Split(',');
            return Color.FromArgb(int.Parse(rgb[0].Trim()), int.Parse(rgb[1].Trim()), int.Parse(rgb[2].Trim()));
        }
    }
}
