using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DSPlena
{
    public class SeamusLog
    {
        public string Location = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SeamusLog";
        private List<string> VirtualMessages = new List<string>();
        public SeamusLog()
        {

        }
        public SeamusLog(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Location = path + @"\" + DateTime.Now.Year + (DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month) + (DateTime.Now.Day > 9 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day);
        }
        
        public void Info(string message, bool flush = false)
        {
            
            string fileName = Location + @"\" + DateTime.Now.Hour + "_00_00.log";
            try
            {
                if (!Directory.Exists(Location))
                {
                    Directory.CreateDirectory(Location);
                }
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
                //Opens a new file stream which allows asynchronous reading and writing
                using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    sw.AutoFlush = true;
                    //Writes the method name with the exception and writes the exception underneath
                    if(!flush)sw.WriteLine(String.Format("{0} ({1}) {2}", DateTime.Now.ToShortDateString(), DateTime.Now.TimeOfDay, message));
                    else sw.WriteLine(String.Format("{0}", message));
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void InfoVirtual(string message)
        {
            VirtualMessages.Add(String.Format("{0} ({1}) {2}", DateTime.Now.ToShortDateString(), DateTime.Now.TimeOfDay, message));
        }

        public void Flush()
        {
            Info("/***************************************************************");
            foreach (string virtualMessage in VirtualMessages)
            {
                Info(virtualMessage,true);
            }
            Info("***************************************************************/");
        }

    }
}
