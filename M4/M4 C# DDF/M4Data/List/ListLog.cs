using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using M4Core.Entities;
using M4Utils.Language;

namespace M4Data.List
{
    public class ListLog
    {
        #region Properties

        public static string _path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\LOG\\";
        private static ListLog _listLog;
        public static LanguageDefault LanguageDefault;

        #endregion

        #region Instance

        public static ListLog Instance()
        {
            return _listLog ?? new ListLog();
        }

        public void SetLanguage(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
        }

        #endregion

        #region Methods

        public void CreateLog(IList<Log> listLog, string archive)
        {
            StringBuilder sbLog = new StringBuilder();

            if (!File.Exists(_path + archive))
                CreateLogPath(archive);
            else
                sbLog.Append(File.ReadAllText(_path + archive));

            foreach (Log log in listLog)
            {
                sbLog.Append(log.Timer.ToString("dd/MM/yyyy hh:mm:ss") + " - " + log.Description + System.Environment.NewLine);
            }

            File.WriteAllText(_path + archive, sbLog.ToString());
        }

        public void CreateLogPath(string name)
        {
            if (!File.Exists(_path + name))
                File.Create(_path + name).Close();
        }

        #endregion
    }
}
