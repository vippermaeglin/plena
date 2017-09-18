using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace My
{
    internal class AssemblyInfo
    {
        private readonly Assembly _mAssembly;
        private string _mCompanyName;
        private string _mCopyright;
        private string _mDescription;
        private string _mProductName;
        private string _mTitle;
        private string _mTrademark;

        public AssemblyInfo(Assembly currentAssembly)
        {
            if (currentAssembly == null)
                throw new ArgumentNullException("currentAssembly");

            _mAssembly = currentAssembly;
        }

        private object GetAttribute(Type attributeType)
        {
            object[] customAttributes = _mAssembly.GetCustomAttributes(attributeType, true);

            return customAttributes.Length == 0 ? null : customAttributes[0];
        }

        public string AssemblyName
        {
            get
            {
                return _mAssembly.GetName().Name;
            }
        }

        public string CompanyName
        {
            get
            {
                if (_mCompanyName == null)
                {
                    AssemblyCompanyAttribute attribute = (AssemblyCompanyAttribute)GetAttribute(typeof(AssemblyCompanyAttribute));
                    _mCompanyName = attribute == null ? "" : attribute.Company;
                }

                return _mCompanyName;
            }
        }

        public string Copyright
        {
            get
            {
                if (_mCopyright == null)
                {
                    AssemblyCopyrightAttribute attribute = (AssemblyCopyrightAttribute)GetAttribute(typeof(AssemblyCopyrightAttribute));
                    _mCopyright = attribute == null ? "" : attribute.Copyright;
                }

                return _mCopyright;
            }
        }

        public string Description
        {
            get
            {
                if (_mDescription == null)
                {
                    AssemblyDescriptionAttribute attribute = (AssemblyDescriptionAttribute)GetAttribute(typeof(AssemblyDescriptionAttribute));
                    _mDescription = attribute == null ? "" : attribute.Description;
                }

                return _mDescription;
            }
        }

        public string DirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(_mAssembly.Location);
            }
        }

        public ReadOnlyCollection<Assembly> LoadedAssemblies
        {
            get
            {
                Collection<Assembly> list = new Collection<Assembly>();
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    list.Add(assembly);
                }
                return new ReadOnlyCollection<Assembly>(list);
            }
        }

        public string ProductName
        {
            get
            {
                if (_mProductName == null)
                {
                    AssemblyProductAttribute attribute = (AssemblyProductAttribute)GetAttribute(typeof(AssemblyProductAttribute));
                    _mProductName = attribute == null ? "" : attribute.Product;
                }
                return _mProductName;
            }
        }

        public string StackTrace
        {
            get
            {
                return Environment.StackTrace;
            }
        }

        public string Title
        {
            get
            {
                if (_mTitle == null)
                {
                    AssemblyTitleAttribute attribute = (AssemblyTitleAttribute)GetAttribute(typeof(AssemblyTitleAttribute));
                    _mTitle = attribute == null ? "" : attribute.Title;
                }
                return _mTitle;
            }
        }

        public string Trademark
        {
            get
            {
                if (_mTrademark == null)
                {
                    AssemblyTrademarkAttribute attribute = (AssemblyTrademarkAttribute)GetAttribute(typeof(AssemblyTrademarkAttribute));
                    _mTrademark = attribute == null ? "" : attribute.Trademark;
                }
                return _mTrademark;
            }
        }

        public Version Version
        {
            get
            {
                return _mAssembly.GetName().Version;
            }
        }

        public long WorkingSet
        {
            get
            {
                return Environment.WorkingSet;
            }
        }
    }

    sealed class Application
    {
        private static AssemblyInfo _info;

        private static string GetDataPath(string basePath)
        {
            string companyName = Info.CompanyName;
            string productName = Info.ProductName;
            string productVersion = Info.Version.ToString();

            string dataPath = string.Format(
                CultureInfo.CurrentCulture,
                "{0}{4}{1}{4}{2}{4}{3}",
                basePath, companyName, productName, productVersion,
                Path.DirectorySeparatorChar);

            if (!(Directory.Exists(dataPath)))
                Directory.CreateDirectory(dataPath);

            return dataPath;
        }

        public static bool Restart { get; set; }

        public static AssemblyInfo Info
        {
            get
            {
                if (_info == null)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

                    _info = new AssemblyInfo(entryAssembly);
                }

                return _info;
            }
        }

        public static string LocalUserAppDataPath
        {
            get
            {
                return GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
        }

        public static string UserAppDataPath
        {
            get
            {
                return GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }
    }
}

namespace M4.WebControlSample.Common
{
    static class Utils
    {

        public static string GetLocalUserAppDataPath(this Application app)
        {
            return My.Application.LocalUserAppDataPath;
        }

        public static string GetUserAppDataPath(this Application app)
        {
            return My.Application.UserAppDataPath;
        }

        public static string GetFileSize(this FileInfo file)
        {
            return file.Length.GetFileSize();
        }

        public static string GetFileSize(this long bytes)
        {
            if (bytes >= 1073741824)
            {
                Decimal size = Decimal.Divide(bytes, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (bytes >= 1048576)
            {
                Decimal size = Decimal.Divide(bytes, 1048576);
                return String.Format("{0:##.##} MB", size);
            }
            else if (bytes >= 1024)
            {
                Decimal size = Decimal.Divide(bytes, 1024);
                return String.Format("{0:##.##} KB", size);
            }
            else if (bytes > 0 & bytes < 1024)
            {
                Decimal size = bytes;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }

        /// <summary>
        /// Converts a coordinate from the polar coordinate system to the cartesian coordinate system.
        /// </summary>
        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point((int)x, (int)y);
        }

        public static Point OffsetExt(this Point point, int X, int Y)
        {
            return new Point(point.X + X, point.Y + Y);
        }
    }
}
