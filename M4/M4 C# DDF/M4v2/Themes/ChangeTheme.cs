using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;

namespace M4.M4v2.Themes
{
    public class ChangeTheme
    {
        public static void ChangeThemeName(string themeName)
        {
            ThemeResolutionService.ApplicationThemeName = themeName;
            //switch (themeName)
            //{
            //    case "Office2007Blue":
            //    case "Office2010Blue":
            //        {
            //            Office2010BlueTheme theme = new Office2010BlueTheme();
            //            theme.DeserializeTheme();
            //            ThemeResolutionService.ApplicationThemeName = "Office2010Blue";
            //        }
            //        break;
            //    case "Office2007Silver":
            //        {
            //            Office2007SilverTheme theme = new Office2007SilverTheme();
            //            theme.DeserializeTheme();
            //            ThemeResolutionService.ApplicationThemeName = "Office2007Silver";
            //        }
            //        break;
            //    case "Windows7":
            //    case "WindowsDefault":
            //        {
            //            Windows7Theme theme = new Windows7Theme();
            //            theme.DeserializeTheme();
            //            ThemeResolutionService.ApplicationThemeName = "Windows7";
            //        }
            //        break;
            //}
        }
    }
}
