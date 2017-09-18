using System;
using System.Drawing;
using System.IO;
using System.Xml;
using M4Core.Entities;
using M4Utils;

namespace M4.M4v2.Settings
{
    public class ListConfigStudies
    {
        #region Properties

        private readonly string _nameArchiveXml = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\CONFIG\\STUDIES.xml";
        private static ListConfigStudies _listConfigStudies;

        #endregion

        #region Instance

        public static ListConfigStudies Instance()
        {
            return _listConfigStudies ?? new ListConfigStudies();
        }

        #endregion

        #region Methods

        private void VerifyPath()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\CONFIG"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\CONFIG");
            if (!File.Exists(_nameArchiveXml))
                CreateXmlStudies();
        }

        /// <summary>
        /// Carrega as configuração de estudos do XML
        /// </summary>
        /// <returns>Arquivo de configuração de estudos</returns>
        public ConfigStudies LoadListConfigStudies()
        {
            ConfigStudies configStudies = new ConfigStudies();
            VerifyPath();
            try
            {
                XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_nameArchiveXml);

                XmlNode node = xmlDocument.DocumentElement.GetElementsByTagName("STUDIES")[0];

                configStudies = new ConfigStudies
                {
                    Color = Color.FromArgb(int.Parse(node.SelectSingleNode("COLOR").InnerText)),
                    LineThickness = decimal.Parse(node.SelectSingleNode("LINETHICKNESS").InnerText),
                    Retracements = new[,]
                                                                     {
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[0]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[0]["STATUS"].InnerText
                                                                         },
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[1]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[1]["STATUS"].InnerText
                                                                         },
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[2]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[2]["STATUS"].InnerText
                                                                         },
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[3]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[3]["STATUS"].InnerText
                                                                         },
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[4]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[4]["STATUS"].InnerText
                                                                         },
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[5]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[5]["STATUS"].InnerText
                                                                         },
                                                                         {
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[6]["RETRACEMENT"].InnerText,
                                                                             node.SelectSingleNode("RETRACEMENTSREG").
                                                                                 ChildNodes[6]["STATUS"].InnerText
                                                                         },
                                                                     },
                    Projections = new[,]
                                                                   {
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[0]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[0]["STATUS"].InnerText
                                                                       },
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[1]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[1]["STATUS"].InnerText
                                                                       },
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[2]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[2]["STATUS"].InnerText
                                                                       },
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[3]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[3]["STATUS"].InnerText
                                                                       },
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[4]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[4]["STATUS"].InnerText
                                                                       },
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[5]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[5]["STATUS"].InnerText
                                                                       },
                                                                       {
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[6]["PROJECTION"].InnerText,
                                                                           node.SelectSingleNode("PROJECTIONSREG").
                                                                               ChildNodes[6]["STATUS"].InnerText
                                                                       },
                                                                   },
                };
            }
            catch (Exception ex)
            {

                configStudies = new ConfigStudies
                {
                    Color = Color.Red,
                    LineThickness = 1,
                    Retracements = new[,]
                                                                     {
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                         {
                                                                             "true","true"
                                                                         },
                                                                     },
                    Projections = new[,]
                                                                   {
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                       {
                                                                             "true","true"
                                                                       },
                                                                   },
                };
            }
            return configStudies;
        }

        public void CreateXmlStudies()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlConfig = xmlDocument.CreateElement("CONFIG");

                XmlNode xmlStudies = xmlDocument.CreateNode(XmlNodeType.Element, "STUDIES", null);

                    XmlNode xmlLineThickness = xmlDocument.CreateNode(XmlNodeType.Element, "LINETHICKNESS", null);
                    xmlLineThickness.InnerText = "1";
                    xmlStudies.AppendChild(xmlLineThickness);
                    XmlNode xmlColor = xmlDocument.CreateNode(XmlNodeType.Element, "COLOR", null);
                    xmlColor.InnerText = "62966";
                    xmlStudies.AppendChild(xmlColor);

                    XmlNode xmlRetracementsReg = xmlDocument.CreateNode(XmlNodeType.Element, "RETRACEMENTSREG", null);
                        for(int i=0;i<7;i++)
                        {
                            XmlNode xmlRetracements = xmlDocument.CreateNode(XmlNodeType.Element, "RETRACEMENTS", null);
                                XmlNode xmlRetracement = xmlDocument.CreateNode(XmlNodeType.Element, "RETRACEMENT", null);
                                xmlRetracement.InnerText = "True";
                                xmlRetracements.AppendChild(xmlRetracement);
                                XmlNode xmlStatus = xmlDocument.CreateNode(XmlNodeType.Element, "STATUS", null);
                                xmlStatus.InnerText = "True";
                                xmlRetracements.AppendChild(xmlStatus);
                            xmlRetracementsReg.AppendChild(xmlRetracements);
                        }
                    xmlStudies.AppendChild(xmlRetracementsReg);

                    XmlNode xmlProjectionsReg = xmlDocument.CreateNode(XmlNodeType.Element, "PROJECTIONSREG", null);
                        for (int i = 0; i < 7; i++)
                        {
                            XmlNode xmlProjections = xmlDocument.CreateNode(XmlNodeType.Element, "PROJECTIONS", null);
                                XmlNode xmlProjection = xmlDocument.CreateNode(XmlNodeType.Element, "PROJECTION", null);
                                xmlProjection.InnerText = "True";
                                xmlProjections.AppendChild(xmlProjection);
                                XmlNode xmlStatus = xmlDocument.CreateNode(XmlNodeType.Element, "STATUS", null);
                                xmlStatus.InnerText = "True";
                                xmlProjections.AppendChild(xmlStatus);
                            xmlProjectionsReg.AppendChild(xmlProjections);
                        }
                    xmlStudies.AppendChild(xmlProjectionsReg);
                xmlConfig.AppendChild(xmlStudies);
            xmlDocument.AppendChild(xmlConfig);
            xmlDocument.Save(_nameArchiveXml);
            VersionChecker.VersionChecker.InsertVersion(_nameArchiveXml, Program.VERSION);
        }

        private XmlDocument GetXmlTemplate()
        {
            FileStream fs = new FileStream(_nameArchiveXml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fs);
            fs.Close();
            return xmldoc;
        }

        public void Update(ConfigStudies configStudies)
        {
            XmlDocument xmlDocument = GetXmlTemplate();
            XmlNode node = xmlDocument.DocumentElement.GetElementsByTagName("STUDIES")[0];

            XmlNode nLinethickness = node.SelectSingleNode("LINETHICKNESS");
            nLinethickness.InnerText = configStudies.LineThickness.ToString();

            XmlNode nColor = node.SelectSingleNode("COLOR");
            nColor.InnerText = configStudies.Color.ToArgb().ToString();

            XmlNodeList nRetracements = node.SelectSingleNode("RETRACEMENTSREG").ChildNodes;
            int index = 0;
            foreach (XmlNode retracement in nRetracements)
            {
                retracement["RETRACEMENT"].InnerText = configStudies.Retracements[index, 0];
                retracement["STATUS"].InnerText = configStudies.Retracements[index, 1];
                index++;
            }

            XmlNodeList nProjection = node.SelectSingleNode("PROJECTIONSREG").ChildNodes;
            index = 0;
            foreach (XmlNode projection in nProjection)
            {
                projection["PROJECTION"].InnerText = configStudies.Projections[index, 0];
                projection["STATUS"].InnerText = configStudies.Projections[index, 1];
                index++;
            }

            xmlDocument.Save(_nameArchiveXml);
        }

        #endregion
    }
}