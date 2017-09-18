using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using M4.DataServer.Interface;

namespace VersionChecker
{
    public static class VersionChecker
    {

        public static string Version = "1.0.0.0";
        public static string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\BASE\\";
        /// <summary>
        /// Insert the version to the file.
        /// </summary>
        /// <param name="path"> Path of the file</param>
        /// <param name="Version"> Version which the files should be</param>
        public static void InsertVersion(string path, string Version)
        {
            if (!File.Exists(path))
                throw new System.ArgumentException("Path doesn't exist.");
            XmlDocument document = new XmlDocument();
            document.Load(path);
            if (document.DocumentElement.SelectSingleNode("Product") == null)
            {
                XmlElement commentVersion = document.CreateElement("Product");
                commentVersion.SetAttribute("Key", "Plena");
                commentVersion.SetAttribute("Version", Version);
                document.DocumentElement.InsertBefore(commentVersion, document.DocumentElement.FirstChild);
                document.Save(path);
            }
        }

        /// <summary>
        /// Insert the version to all the files inside the path and subdirectories.
        /// </summary>
        /// <param name="path"> Path of the directory</param>
        /// <param name="Version"> Version which the files should be</param>
        public static void InsertVersionToAllFiles(string path, string Version)
        {
            if (!Directory.Exists(path)) throw new System.ArgumentException("Path doesn't exist.");
            string[] filePaths = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
            foreach (var filepath in filePaths)
            {
                InsertVersion(filepath, Version);
            }
        }

        /// <summary>
        /// Change the version to all the files inside the path and subdirectories.
        /// </summary>
        /// <param name="path"> Path of the directory</param>
        /// <param name="Version"> Version which the files should be</param>
        public static void ChangeVersionOfAllFiles(string path, string Version)
        {
            if (!Directory.Exists(path)) throw new System.ArgumentException("Path doesn't exist.");
            string[] filePaths = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
            foreach (var filepath in filePaths)
            {
                ChangeVersion(filepath, Version);
            }
        }

        /// <summary>
        /// Change the version of all the files of an array of paths.
        /// </summary>
        /// <param name="path"> Path of the directory</param>
        /// <param name="Version"> Version which the files should be</param>
        public static void ChangeVersionOfAllFiles(string[] path, string Version)
        {
            foreach (var directoryPath in path)
            {
                if (!Directory.Exists(directoryPath)) throw new System.ArgumentException("Path doesn't exist.");
                string[] filePaths = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);
                foreach (var filepath in filePaths)
                {
                    ChangeVersion(filepath, Version);
                }
            }
        }

        /// <summary>
        /// Change the version of the file.
        /// </summary>
        /// <param name="path"> Path of the file</param>
        /// <param name="Version"> Version which the files should be</param>
        public static void ChangeVersion(string path, string Version)
        {
            if (!File.Exists(path))
                throw new System.ArgumentException("Path doesn't exist.");
            XmlDocument document = new XmlDocument();
            document.Load(path);
            if (document.DocumentElement.SelectSingleNode("Product") != null)
            {
                XmlNode commentVersion = document.DocumentElement.SelectSingleNode("Product");
                commentVersion.Attributes["Version"].Value = Version;

            }
            document.Save(path);
        }

        /// <summary>
        /// Return an array of strings with all the files inside the path and subdirectories which the Version is different of the gived.
        /// </summary>
        /// <param name="path"> Path of the directory</param>
        /// <param name="Version"> Version which the files should be</param>
        public static string[] GetDifferentVersion(string path, string Version)
        {
            if (!Directory.Exists(path)) throw new System.ArgumentException("Path doesn't exist.");
            string[] filePaths = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
            List<string> DifferentsFilesPaths = new List<string>();
            foreach (var filepath in filePaths)
            {
                //check if the version is the same, case false, insert to the list
                if (GetVersion(filepath) != Version)
                {
                    DifferentsFilesPaths.Add(filepath);
                }
            }
            return DifferentsFilesPaths.ToArray();
        }

        /// <summary>
        /// Return a string with the value of the Version recorded in the xml.
        /// </summary>
        /// <param name="path"> Path of the directory</param>
        public static string GetVersion(string path)
        {
            if (!File.Exists(path))
                throw new System.ArgumentException("Path doesn't exist.");
            XmlDocument document = new XmlDocument();
            document.Load(path);
            try
            {
                if (document.DocumentElement.SelectSingleNode("Product") != null)
                {
                    XmlNode commentVersion = document.DocumentElement.SelectSingleNode("Product");
                    return commentVersion.Attributes["Version"].Value;
                }
                else
                {

                    //throw new System.ArgumentException("Product version doesn't exist.");
                    return "";
                }
            }
            catch(Exception ex)
            {
                return "";
            }
        }


        /// <summary>
        /// Do all the changes necessary to each file of the path \\BASE to the new Version.
        /// </summary>
        public static void CheckFileVersions(string version)
        {
            //Check SQL database:
            SqlConnection _con = DBlocalSQL.Connect();
            string sqlVersion = DBlocalSQL.GetSqlVersion(_con);

            if (sqlVersion != version)
            {
                //do the changes to the SQL file
                // switch(version)
                // ...
                // ...
                // ...
                DBlocalSQL.SaveSqlVersion(_con, version);
            }
            DBlocalSQL.Disconnect(_con);

            if (!Directory.Exists(BasePath))
                throw new System.ArgumentException("Path doesn't exist.");
            Version = version;
            string[] oldFiles = GetDifferentVersion(BasePath, Version);
            foreach (var file in oldFiles)
            {
                if (file.Contains("WORKSPACE"))
                {
                    if (file.Contains("Workspace.xml"))
                    {
                        //do the changes to the workspace.xml file
                        // switch(version)
                        // ...
                        // ...
                        ChangeVersion(file, Version);
                    }
                    else if (file.Contains("WorkspaceLoad.xml"))
                    {
                        //do the changes to the workspaceLoad.xml file
                        // switch(version)
                        // ...
                        // ...
                        // ...
                        ChangeVersion(file, Version);
                    }
                    else if (file.Contains("ATIVOS"))
                    {
                        //do the changes to the files which starts with ATIVOS
                        // switch(version)
                        // ...
                        // ...
                        // ...
                        ChangeVersion(file, Version);
                    }
                    else
                    {
                        //do the changes to the remainder files in this path.
                        // switch(version)
                        // ...
                        // ...
                        // ...
                        ChangeVersion(file, Version);
                    }
                }
                else if (file.Contains("UPDATE"))
                {
                    //do the changes to the files in this path.
                    // ...
                    // ...
                    // ...
                    ChangeVersion(file, Version);
                }
                else if (file.Contains("TEMPLATES"))
                {
                    //do the changes to the files in this path.
                    // ...
                    // ...
                    // ...
                    ChangeVersion(file, Version);
                }
                else if (file.Contains("LOGIN"))
                {
                    //do the changes to the files in this path.
                    // ...
                    // ...
                    // ...
                    ChangeVersion(file, Version);
                }
                else if (file.Contains("FEATURES"))
                {
                    //do the changes to the files in this path.
                    // ...
                    // ...
                    // ...
                    ChangeVersion(file, Version);
                }
                else if (file.Contains("CONFIG"))
                {
                    //do the changes to the files in this path.
                    // ...
                    // ...
                    // ...
                    ChangeVersion(file, Version);
                }
            }
        }

        /// <summary>
        /// Creates all the Directories missing on the path \\BASE necessary to start Plena.
        /// </summary>
        /// <param name="path"> Path of the directory</param>
        public static void CheckAndCreateDirectories()
        {
            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);
            //Check for \\CONFIG:
            if (!Directory.Exists(BasePath + "\\CONFIG"))
            {
                Directory.CreateDirectory(BasePath + "\\CONFIG");
            }
            //Check for \\FEATURES:
            if (!Directory.Exists(BasePath + "\\FEATURES"))
            {
                Directory.CreateDirectory(BasePath + "\\FEATURES");
            }
            //Check for \\LOGIN:
            if (!Directory.Exists(BasePath + "\\LOGIN"))
            {
                Directory.CreateDirectory(BasePath + "\\LOGIN");
            }
            //Check for \\STUDIES:
            if (!Directory.Exists(BasePath + "\\STUDY"))
            {
                Directory.CreateDirectory(BasePath + "\\STUDY");
            }
            //Check for \\SYSTEM:
            if (!Directory.Exists(BasePath + "\\SYSTEM"))
            {
                Directory.CreateDirectory(BasePath + "\\SYSTEM");
            }
            //Check for \\TEMPLATES:
            if (!Directory.Exists(BasePath + "\\TEMPLATES"))
            {
                Directory.CreateDirectory(BasePath + "\\TEMPLATES");
            }
            //Check for \\UPDATE:
            if (!Directory.Exists(BasePath + "\\UPDATE"))
            {
                Directory.CreateDirectory(BasePath + "\\UPDATE");
            }
            //Check for \\WORKSPACE:
            if (!Directory.Exists(BasePath + "\\WORKSPACE"))
            {
                Directory.CreateDirectory(BasePath + "\\WORKSPACE");
            }
            //Check for \\SCRIPTS:
            if (!Directory.Exists(BasePath + "\\SCRIPTS"))
            {
                Directory.CreateDirectory(BasePath + "\\SCRIPTS");
            }
            //Check for \\SCRIPTS\\ALERTS:
            if (!Directory.Exists(BasePath + "\\SCRIPTS\\ALERTS"))
            {
                Directory.CreateDirectory(BasePath + "\\SCRIPTS\\ALERTS");
            }
            //Check for \\SCRIPTS\\SCANNER:
            if (!Directory.Exists(BasePath + "\\SCRIPTS\\SCANNER"))
            {
                Directory.CreateDirectory(BasePath + "\\SCRIPTS\\SCANNER");
            }
        }
    }
}
