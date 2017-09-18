using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using M4Utils;
using System.Data;
using System.Windows.Forms;

namespace M4.M4v2.Scripts
{
    class ScriptManager
    {
        public string Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SCRIPTS\\";
        public string[] ListUserData(string ClientId, string ClientPassword, string LicenseKey)
        {
            List<string> data = new List<string>();
            string[] paths = Directory.GetFiles(Path+"ALERTS\\", "*.xml");
            for (int i = 0; i < paths.Length;i++ )
            {
                data.Add("Trade Alert Settings: "+paths[i].Split(new char[]{'\\','.'})[7]);
            } 
            paths = Directory.GetFiles(Path + "SCANNER\\", "*.xml");
            for (int i = 0; i < paths.Length; i++)
            {
                data.Add("Scanner Settings: " + paths[i].Split(new char[] { '\\', '.' })[7]);
            }
            return data.ToArray();
        }
        public void SetUserData(string ClientId, string ClientPassword, string LicenseKey, string scanName, string data)
        {
            string type = "";
            if (scanName.Contains("Scanner Settings: "))
            {
                type = "SCANNER";
                scanName = scanName.Replace("Scanner Settings: ", "");
            }
            else if (scanName.Contains("Trade Alert Settings: "))
            {
                type = "ALERTS";
                scanName = scanName.Replace("Trade Alert Settings: ", "");
            }

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlScript = xmlDocument.CreateElement("SCRIPT");

            XmlNode xmlType = xmlDocument.CreateNode(XmlNodeType.Element, "TYPE", null);
            xmlType.InnerText = type;

            XmlNode xmlData = xmlDocument.CreateNode(XmlNodeType.Element, "DATA", null);
            xmlData.InnerText = data;

            xmlScript.AppendChild(xmlType);
            xmlScript.AppendChild(xmlData);
            xmlDocument.AppendChild(xmlScript);
            xmlDocument.Save(Path + type + "\\" + scanName+".xml");
            VersionChecker.VersionChecker.InsertVersion(Path + type + "\\" + scanName + ".xml", Program.VERSION);
        }
        public string GetUserData(string ClientId, string ClientPassword, string LicenseKey, string scanName)
        {
            string data = "";
            string type = "";
            if (scanName.Contains("Scanner Settings: "))
            {
                type = "SCANNER";
                scanName = scanName.Replace("Scanner Settings: ", "");
            }
            else if (scanName.Contains("Trade Alert Settings: "))
            {
                type = "ALERTS";
                scanName = scanName.Replace("Trade Alert Settings: ", "");
            }
            try
            {
                XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(Path + type + "\\" + scanName + ".xml");

                XmlNode node = xmlDocument.DocumentElement.GetElementsByTagName("DATA")[0];

                //type = node.SelectSingleNode("TYPE").InnerText;

                data = node.InnerText;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return data;
        }
        public DataSet GetAlerts(string ClientId, string ClientPassword, string LicenseKey)
        {
            return new DataSet();
        }
    }
}
