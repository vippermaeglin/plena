using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using M4Core.Entities;
using M4Core.Enums;
using M4Utils;
using M4Utils.Language;
using System.Net.NetworkInformation;
using VersionChecker;

namespace M4Data.List
{
    public class ListLoginAuthentication
    {
        #region Properties

        public static string _path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\LOGIN\\";
        private static ListLoginAuthentication _listLoginAuthentication;
        public static LanguageDefault LanguageDefault;

        #endregion

        #region Instance

        public ListLoginAuthentication()
        {
            SettingsServer.Default.AccessedUrl = "";
            SettingsServer.Default.Save();

            CreatePath();
        }

        public static ListLoginAuthentication Instance(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
            return _listLoginAuthentication ?? new ListLoginAuthentication();
        }

        #endregion

        #region Methods

        public void CreatePath()
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        private static void CreateDataPath(LoginAuthentication loginAuthentication, string login)
        {
            if (File.Exists(_path + login + ".xml"))
                File.Delete(_path + login + ".xml");

            XmlDocument xmlDocument = new XmlDocument();

            XmlNode nodeConjunto = xmlDocument.CreateNode(XmlNodeType.Element, "CONJUNTO", null);
            
            XmlNode nodeData = xmlDocument.CreateNode(XmlNodeType.Element, "DATA", null);

            XmlNode nodeCodeSession = xmlDocument.CreateElement("CODESESSION");
            nodeCodeSession.InnerText = Utility.Cript(loginAuthentication.CodeSession);

            XmlNode nodeLogin = xmlDocument.CreateElement("LOGIN");
            nodeLogin.InnerText = Utility.Cript(loginAuthentication.Login);

            XmlNode nodePassword = xmlDocument.CreateElement("PASSWORD");
            nodePassword.InnerText = Utility.Cript(loginAuthentication.Password);

            XmlNode nodeLastdate = xmlDocument.CreateElement("LASTDATE");
            nodeLastdate.InnerText = Utility.Cript(DateTime.Parse(DateTime.Now.ToString(),new CultureInfo("pt-BR")).ToString());

            XmlNode nodeLimitDate = xmlDocument.CreateElement("LIMITDATE");
            nodeLimitDate.InnerText = Utility.Cript(DateTime.Parse(loginAuthentication.LimitDate.ToString(), new CultureInfo("pt-BR")).ToString());

            XmlNode nodeRemember = xmlDocument.CreateElement("REMEMBER");
            nodeRemember.InnerText = Utility.Cript(loginAuthentication.Remember.ToString());

            XmlNode nodeFeatures = xmlDocument.CreateNode(XmlNodeType.Element, "FEATURES", null);

            foreach (var feature in loginAuthentication.Features)
            {
                XmlNode nodeFeature = xmlDocument.CreateNode(XmlNodeType.Element, "FEATURE", null);

                XmlNode nodeDescription = xmlDocument.CreateElement("DESCRIPTION");
                nodeDescription.InnerText = Utility.Cript(StringValue.GetStringValue(feature.Description));

                XmlNode nodePermission = xmlDocument.CreateElement("PERMISSION");
                nodePermission.InnerText = Utility.Cript(StringValue.GetStringValue(feature.Permission));

                nodeFeature.AppendChild(nodePermission);
                nodeFeature.AppendChild(nodeDescription);

                nodeFeatures.AppendChild(nodeFeature);
            }

            nodeData.AppendChild(nodeCodeSession);
            nodeData.AppendChild(nodeLogin);
            nodeData.AppendChild(nodePassword);
            nodeData.AppendChild(nodeLastdate);
            nodeData.AppendChild(nodeLimitDate);
            nodeData.AppendChild(nodeRemember);
            nodeData.AppendChild(nodeFeatures);

            nodeConjunto.AppendChild(nodeData);

            xmlDocument.AppendChild(nodeConjunto);
            xmlDocument.Save(_path + login + ".xml");
            VersionChecker.VersionChecker.InsertVersion(_path + login + ".xml", VersionChecker.VersionChecker.Version);
            xmlDocument.Save(_path + "LoginSaved.xml");
            VersionChecker.VersionChecker.InsertVersion(_path + "LoginSaved.xml", VersionChecker.VersionChecker.Version);
        }

        private static EStatusAuthentication VerifyDate(DateTime lastDate, DateTime limitDate)
        {
            EStatusAuthentication eStatusAuthentication = EStatusAuthentication.Expired;

            if (DateTime.Now < lastDate)
                eStatusAuthentication = EStatusAuthentication.Blocked;
            else if (DateTime.Now > limitDate)
                eStatusAuthentication = EStatusAuthentication.Expired;
            else if (DateTime.Now <= limitDate)
                eStatusAuthentication = EStatusAuthentication.Permitted;

            return eStatusAuthentication;
        }

        /// <summary>
        /// Verifica o Login e Senha do usuário
        /// </summary>
        /// <returns>Lista de Ativos</returns>
        public LoginAuthentication LoadLogin(string login, string password, bool remember)
        {
            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_path + login + ".xml");

            LoginAuthentication loginAuthentication = null;

            string codigoSessao = Utility.Cript(GetMacAddress());

            if (xmlDocument != null)
            {
                XmlNodeList nodeListLogin = xmlDocument.GetElementsByTagName("DATA");

                foreach (XmlNode node in nodeListLogin.Cast<XmlNode>().Where(
                    node =>
                    node["LOGIN"].InnerText.Equals(Utility.Cript(login)) &&
                    node["PASSWORD"].InnerText.Equals(password)))
                {
                    codigoSessao = Utility.Cript(GetMacAddress());//node["CODESESSION"].InnerText;
                }
            }

            try
            {
                loginAuthentication = Server.Instance(LanguageDefault).LoadLogin(login, password, codigoSessao);
                loginAuthentication.Remember = remember;

                if (loginAuthentication.Features == null)
                    loginAuthentication.Features = new List<Features>();

                CreateDataPath(loginAuthentication, login);
            }
            catch (Exception ex)
            {
                if ((ex.Message.Equals(LanguageDefault.DictionaryLogin["notConnectServer"])) ||
                    (ex.Message.Equals("Conexão com a internet não está ativa")) ||
                    (ex.Message.Equals("Servidor não acessível")))
                {
                    try
                    {
                        loginAuthentication = LoginOffline(login, password, remember);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }
                }
                else
                {
                    throw new Exception(ex.Message);
                }

                if (loginAuthentication == null)
                    throw new Exception(ex.Message);
            }

            return loginAuthentication;
        }

        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }
        public LoginAuthentication LoginOffline(string login, string password, bool remember)
        {
            //Test if guest.xml exists:
            if (login == "guest" && Utility.Decript(password) == "123456")
            {
                if (!File.Exists(_path + login + ".xml")) CreateDataPath(new LoginAuthentication()
                                                                                { 
                                                                                    CodeSession = "GUEST",
                                                                                    Login = login,
                                                                                    Password = password,
                                                                                    LastDate = DateTime.Now,
                                                                                    LimitDate = DateTime.Now.AddDays(5),
                                                                                    Remember = false,
                                                                                    Offline = true,
                                                                                    Features = new List<Features>() { new Features() { Description = EFeatures.NEW_CHART, Permission = EPermission.Permitido }, new Features() { Description = EFeatures.HISTORICDATA, Permission = EPermission.Restringido } },
                                                                                    UserProfile = new UserRegister(){
                                                                                        UserName = "<Convidado>"
                                                                                    }
                                                                                }, login);
            }

            LoginAuthentication loginAuthentication = null;

            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_path + login + ".xml");

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("DATA");

            foreach (XmlNode node in nodeList.Cast<XmlNode>().Where(node => node["LOGIN"].InnerText.Equals(Utility.Cript(login)) &&
                node["PASSWORD"].InnerText.Equals(Utility.Cript(password))))
            {
                loginAuthentication = new LoginAuthentication
                {
                    Login = Utility.Decript(node["LOGIN"].InnerText),
                    Password = Utility.Decript(node["PASSWORD"].InnerText),
                    LastDate = DateTime.Now,
                    LimitDate = DateTime.Parse(Utility.Decript(node["LIMITDATE"].InnerText), new CultureInfo("pt-BR")),
                    Remember = remember,
                    CodeSession = "",
                    Features = new List<Features>(),
                    Offline = true,
                    UserProfile = new UserRegister()
                    {
                        UserName = "<Desconectado>"

                    }
                };

                EStatusAuthentication eStatusAuthentication = VerifyDate(loginAuthentication.LastDate, loginAuthentication.LimitDate);

                if ((eStatusAuthentication.Equals(EStatusAuthentication.Expired)) || (eStatusAuthentication.Equals(EStatusAuthentication.Blocked)))
                    throw new Exception("Período de login offline acabou, realize um login online para regularizar.");

                XmlNodeList nodeFeatures = xmlDocument.GetElementsByTagName("FEATURE");

                loginAuthentication.Features = (from XmlNode feature in nodeFeatures
                                                select new Features
                                                {
                                                    Description = (EFeatures)StringValue.Parse(typeof(EFeatures), Utility.Decript(feature["DESCRIPTION"].InnerText)),
                                                    Permission = (EPermission)StringValue.Parse(typeof(EPermission), Utility.Decript(feature["PERMISSION"].InnerText)),
                                                }).ToList();

                node["REMEMBER"].InnerText = Utility.Cript(remember.ToString());

                xmlDocument.Save(_path + login + ".xml");
                xmlDocument.Save(_path + "LoginSaved.xml");

                break;
            }

            return loginAuthentication;
        }

        public LoginAuthentication LoginSaved()
        {
            try
            {
                XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(_path + "LoginSaved.xml");

                if ((xmlDocument == null) || (!xmlDocument.HasChildNodes))
                    return null;

                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("DATA");

                //Test values:
                /*XmlNode node = nodeList[0];
                string Login =
                    Utility.Decript(node["LOGIN"].InnerText);
                MessageBox.Show("Login = "+Login);
                string Password =
                    Utility.Decript(node["PASSWORD"].InnerText);
                MessageBox.Show("Password = " + Password);
                DateTime LastDate =
                    DateTime.Parse(
                        Utility.Decript(
                            node["LASTDATE"].InnerText), new CultureInfo("pt-BR"));
                MessageBox.Show("LastDate = " + LastDate.ToString());
                DateTime LimitDate =
                    DateTime.Parse(
                        Utility.Decript(
                            node["LIMITDATE"].InnerText), new CultureInfo("pt-BR"));
                MessageBox.Show("LimitDate = " + LimitDate.ToString());
                bool Remember =
                    Utility.Decript(node["REMEMBER"].InnerText)
                        .ToLower() == "true"
                        ? true
                        : false;
                LoginAuthentication loginAuthentication = null;*/

                LoginAuthentication loginAuthentication = (from XmlNode node in nodeList
                                                           select new LoginAuthentication
                                                                      {
                                                                          Login =
                                                                              Utility.Decript(node["LOGIN"].InnerText),
                                                                          Password =
                                                                              Utility.Decript(node["PASSWORD"].InnerText),
                                                                          LastDate = node["LASTDATE"].InnerText!=""?
                                                                              DateTime.Parse(
                                                                                  Utility.Decript(
                                                                                      node["LASTDATE"].InnerText), new CultureInfo("pt-BR")):DateTime.Now,
                                                                          LimitDate = node["LIMITDATE"].InnerText!=""?
                                                                              DateTime.Parse(
                                                                                  Utility.Decript(
                                                                                      node["LIMITDATE"].InnerText), new CultureInfo("pt-BR")) : DateTime.Now.AddDays(2),
                                                                          Remember =
                                                                              Utility.Decript(node["REMEMBER"].InnerText)
                                                                                  .ToLower() == "true"
                                                                                  ? true
                                                                                  : false,
                                                                      }).FirstOrDefault();

                EStatusAuthentication eStatusAuthentication = VerifyDate(loginAuthentication.LastDate,
                                                                         loginAuthentication.LimitDate);

                if (eStatusAuthentication.Equals(EStatusAuthentication.Blocked))
                    throw new Exception("Sistema bloqueado para acesso.");

                if (eStatusAuthentication.Equals(EStatusAuthentication.Expired))
                    return null; 

                loginAuthentication.Features =
                    ListFeatures.Instance().GetFeatures(Utility.Cript(loginAuthentication.Login));

                return loginAuthentication;
            }
            catch(Exception ex)
            {
                MessageBox.Show("LoginSaved()" + ex.Message);
            }
            return null;
        }

        #endregion
    }
}
