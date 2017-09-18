using System.Reflection;
using Nevron.Globalization;

namespace M4
{
    public class LanguageDictionary
    {
        private static LanguageDictionary _languageDictionary;

        public static LanguageDictionary Instance()
        {
            return _languageDictionary ?? (_languageDictionary = new LanguageDictionary());
        }

        public void Load()
        {
            string name = Assembly.GetEntryAssembly().GetName().CultureInfo.Name;
            CreatePtBr();
            //CreateSp();
        }

        private static void CreatePtBr()
        {
            NDictionary portugues = new NDictionary("PtBr");
            portugues.Add("&File", "&Arquivo");
            portugues.Add("&Help", "&Ajuda");

            NLocalizationManager.Instance.Enabled = true;
            NLocalizationManager.Instance.GlobalDictionary.CombineWith(portugues);
        }

        private static void CreateSp()
        {
            NDictionary portugues = new NDictionary("Sp");
            portugues.Add("&Help", "&Ayuda");

            NLocalizationManager.Instance.Enabled = true;
            NLocalizationManager.Instance.GlobalDictionary.CombineWith(portugues);
        }
    }
}
