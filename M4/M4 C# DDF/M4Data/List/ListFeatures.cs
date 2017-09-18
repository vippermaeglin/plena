using System;
using System.IO;
using System.Linq;
using System.Xml;
using M4Core.Entities;
using M4Core.Enums;
using M4Utils;
using M4Utils.Language;
using System.Collections.Generic;

namespace M4Data.List
{
    public class ListFeatures
    {
        #region Properties

        public static string _path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\FEATURES\\";
        private static ListFeatures _listFeatures;
        public static LanguageDefault LanguageDefault;

        #endregion

        #region Instance

        public ListFeatures()
        {
            CreatePath();
        }

        public static ListFeatures Instance()
        {
            return _listFeatures ?? new ListFeatures();
        }

        public void SetLanguage(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
        }

        #endregion

        #region Methods

        public void CreatePath()
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        public List<Features> GetFeatures(string login)
        {
            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_path + login + ".xml");

            if (xmlDocument == null)
                return new List<Features>();

            List<Features> listFeatures = null;

            if (xmlDocument != null)
            {
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("FEATURE");

                listFeatures = (from XmlNode node in nodeList
                                select new Features
                                {
                                    Description = (EFeatures)StringValue.Parse(typeof(EFeatures), Utility.Decript(node["DESCRIPTION"].InnerText)),
                                    Permission = (EPermission)int.Parse(Utility.Decript(node["PERMISSION"].InnerText)),
                                }).ToList();
            }

            return listFeatures;
        }

        public bool PermissionFeature(EFeatures efeature, EPermission ePermission, List<Features> features)
        {
            return features.Any(feature => (feature.Permission.Equals(ePermission)) && (StringValue.GetStringValue(efeature).Equals(feature.Description)));
        }

        #endregion
    }
}
