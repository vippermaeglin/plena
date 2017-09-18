using System.Collections.Generic;

namespace M4Utils.Language
{
    public abstract class LanguageDefault
    {
        public Dictionary<string, string> DictionaryPlena = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryMenuPlena = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryGridAssets = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryTabAssets = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryMenuAssets = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryChartCtl = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryMenuBar = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryMessage = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryTitleMessage = new Dictionary<string, string>();
        public Dictionary<string, string> DictionarySelectChart = new Dictionary<string, string>();
        public Dictionary<string, string> DictionarySelectIndicator = new Dictionary<string, string>();
        public Dictionary<string, string> DictionarySelectStudy = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryBrowser = new Dictionary<string, string>();
        public Dictionary<string, string> DictionarySelectTools = new Dictionary<string, string>();
        public Dictionary<string, string> DictionarySettings = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryPortfolio = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryLogin = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryPermission = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryTemplate = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryWorkspace = new Dictionary<string, string>();
        public Dictionary<string, string> DictionaryOutput = new Dictionary<string, string>();

        public abstract void LoadPlena();
        public abstract void LoadMenuPlena();
        public abstract void LoadDictionaryAssets();
        public abstract void LoadTabAssets();
        public abstract void LoadMenuAssets();
        public abstract void LoadChartCtl();
        public abstract void LoadMenuBar();
        public abstract void LoadMessage();
        public abstract void LoadTitleMessage();
        public abstract void LoadSelectChart();
        public abstract void LoadSelectIndicator();
        public abstract void LoadSelectStudy();
        public abstract void LoadBrowser();
        public abstract void LoadSelectTools();
        public abstract void LoadSettings();
        public abstract void LoadPortfolio();
        public abstract void LoadLogin();
        public abstract void LoadPermission();
        public abstract void LoadTemplate();
        public abstract void LoadWorkspace();
        public abstract void LoadOutput();
    }
}
