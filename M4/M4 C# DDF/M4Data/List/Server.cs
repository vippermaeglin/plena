using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.ValidacaoAcesso;
using M4Data.RegistroUsuario;
using M4Data.UpgradeVersion;
using M4Utils;
using M4Utils.Language;
using System.ServiceModel;

namespace M4Data.List
{
    public class Server
    {
        private string _codigoSessao;
        private static Server _server;
        private int _connectionAttempt;
        private Thread _threadEndSession;
        public static LanguageDefault LanguageDefault;

        public Server()
        {
            _connectionAttempt = 0;
        }

        public static Server Instance(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
            return _server ?? new Server();
        }

        public void ReconnectRegister(UserRegister userRegister)
        {
            if (_connectionAttempt == 1)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            Register(userRegister);
        }
        public void ReconnectUpdate(UserRegister userRegister)
        {
            if (_connectionAttempt == 1)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            Update(userRegister);
        }
        public void ReconnectUpgradeVersion(string version, out bool optional)
        {
            if (_connectionAttempt == 1)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            UpgradeVersion(version, out optional);
        }
        public void ReconnectRecovery(string cpf, string email)
        {
            if (_connectionAttempt == 1)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            Recovery(cpf,email);
        }
        public void ReconnectValidation(string login, string password, string codigoSessao)
        {
            if (_connectionAttempt == 1)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            LoadLogin(login, password, codigoSessao);
        }
        public void ReconnectRevalidation(string login, string password, string codigoSessao)
        {
            if (_connectionAttempt == 1)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            ReloadLogin(login, password, codigoSessao);
        }

        public void ReconnectEndSession(string codigoSessao)
        {
            if (_connectionAttempt == 3)
            {
                _connectionAttempt = 0;
                throw new Exception(LanguageDefault.DictionaryLogin["notConnectServer"]);
            }

            _connectionAttempt++;
            Thread.Sleep(200);
            EndSession(codigoSessao);
        }

        public void EndSession(string codigoSessao)
        {
            _codigoSessao = codigoSessao;

            _threadEndSession = new Thread(EndSession) { IsBackground = true};
            _threadEndSession.Start();
        }

        private void EndSession()
        {
            try
            {
                ValidacaoAcessoSoapClient acessoSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetConfigValidacaoAcessoService();
                acessoSoapClient.FinalizarSessao(Utility.Cript(_codigoSessao));
            }
            finally
            {
                SettingsServer.Default.AccessedUrl = "";
                SettingsServer.Default.Save();
                
                //_threadEndSession.Abort();
                //_threadEndSession = null;
            }
        }

        public LoginAuthentication LoadLogin(string login, string password, string codigoSessao)
        {
            LoginAuthentication loginAuthentication = null;

            ValidacaoAcessoSoapClient acessoSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetConfigValidacaoAcessoService();


            //var c = acessoSoapClient.State;
            if (acessoSoapClient == null)
                ReconnectValidation(login, password, codigoSessao);
            XElement xElement;
            XmlDocument xmlServer = new XmlDocument();
            try
            {
                xElement = acessoSoapClient.ValidarUsuario(login, password, codigoSessao);

                //c = acessoSoapClient.State;
                using (XmlReader xmlReader = xElement.CreateReader())
                {
                    xmlServer.Load(xmlReader);
                }

                acessoSoapClient.Close();


                xmlServer.ChildNodes[0].InnerXml = Utility.Decript(xmlServer.ChildNodes[0].InnerXml);

                if (xmlServer.DocumentElement != null)
                {
                    XmlNode xmlFechamento = xmlServer.DocumentElement.SelectSingleNode("FECHAMENTO");

                    if (xmlFechamento == null)
                        ReconnectValidation(login, password, codigoSessao);

                    XmlNode xmlStatus = xmlServer.DocumentElement.SelectSingleNode("STATUS");

                    XmlNode xmlIpServer = xmlServer.DocumentElement.SelectSingleNode("IPSERVER");

                    if ((xmlStatus != null) && (xmlStatus.HasChildNodes))
                    {
                        if (xmlStatus.InnerText == "1")
                        {
                            XmlNode xmlCodigoSessao = xmlServer.DocumentElement.SelectSingleNode("CODIGOSESSAO");

                            XmlNode xmlDataExpiracao = xmlServer.DocumentElement.SelectSingleNode("DATAEXPIRACAO");

                            XmlNodeList acessos = xmlServer.GetElementsByTagName("ACESSO");

                            XmlNode xmlId = xmlServer.DocumentElement.SelectSingleNode("ID");

                            XmlNode xmlLogin = xmlServer.DocumentElement.SelectSingleNode("LOGIN");

                            XmlNode xmlSenha = xmlServer.DocumentElement.SelectSingleNode("SENHA");

                            XmlNode xmlNome = xmlServer.DocumentElement.SelectSingleNode("NOME");

                            XmlNode xmlSobreNome = xmlServer.DocumentElement.SelectSingleNode("SOBRENOME");

                            XmlNode xmlCpf = xmlServer.DocumentElement.SelectSingleNode("CPF");

                            XmlNode xmlEmail = xmlServer.DocumentElement.SelectSingleNode("EMAIL");

                            XmlNode xmlNascimento = xmlServer.DocumentElement.SelectSingleNode("DATANASCIMENTO");

                            XmlNode xmlCep = xmlServer.DocumentElement.SelectSingleNode("CEP");

                            XmlNode xmlEstado = xmlServer.DocumentElement.SelectSingleNode("ESTADO");

                            XmlNode xmlCidade = xmlServer.DocumentElement.SelectSingleNode("CIDADE");

                            XmlNode xmlBairro = xmlServer.DocumentElement.SelectSingleNode("BAIRRO");

                            XmlNode xmlLagradouro = xmlServer.DocumentElement.SelectSingleNode("LAGRADOURO");

                            XmlNode xmlNumero = xmlServer.DocumentElement.SelectSingleNode("NUMERO");

                            XmlNode xmlComplemento = xmlServer.DocumentElement.SelectSingleNode("COMPLEMENTO");

                            XmlNode xmlTipo = xmlServer.DocumentElement.SelectSingleNode("TIPO");

                            loginAuthentication = new LoginAuthentication
                            {
                                CodeSession = xmlCodigoSessao.InnerText,
                                Login = login,
                                Password = password,
                                LastDate = DateTime.Now,
                                LimitDate = String.IsNullOrEmpty(xmlDataExpiracao.InnerText) ? DateTime.Now.AddDays(2) : DateTime.Parse(xmlDataExpiracao.InnerText),
                                Remember = true,
                                Features = new List<Features>(),
                                IpServer = xmlIpServer.InnerText,
                                UserProfile = new UserRegister()
                                {
                                    Id = int.Parse(xmlId.InnerText),
                                    Birthday = String.IsNullOrEmpty(xmlDataExpiracao.InnerText) ? new DateTime(1900, 1, 1) : DateTime.Parse(xmlNascimento.InnerText),
                                    CEP = xmlCep.InnerText,
                                    City = xmlCidade.InnerText,
                                    Complement = xmlComplemento.InnerText,
                                    ConfirmPassword = xmlSenha.InnerText,
                                    CPF = xmlCpf.InnerText,
                                    District = xmlBairro.InnerText,
                                    Email = xmlEmail.InnerText,
                                    FirstName = xmlNome.InnerText,
                                    LastName = xmlSobreNome.InnerText,
                                    Number = xmlNumero.InnerText,
                                    Password = xmlSenha.InnerText,
                                    State = xmlEstado.InnerText,
                                    Street = xmlLagradouro.InnerText,
                                    UserName = xmlLogin.InnerText,
                                    Tipo = (EPerfil)int.Parse(xmlTipo.InnerText)

                                }
                            };

                            foreach (Features features in from XmlNode acesso in acessos
                                                          select new Features
                                                          {
                                                              Description = (EFeatures)StringValue.Parse(typeof(EFeatures), acesso["CODIGOACESSO"].InnerText),
                                                              Permission = (EPermission)StringValue.Parse(typeof(EPermission), acesso["MODO"].InnerText)
                                                          })
                            {
                                loginAuthentication.Features.Add(features);
                            }
                        }
                        else if (xmlStatus.InnerText == "0")
                        {
                            XmlNode xmlInformacao = xmlServer.DocumentElement.SelectSingleNode("INFORMACAO");
                            throw new Exception(xmlInformacao.InnerText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                acessoSoapClient.Close();
                throw ex;
                //ReconnectValidation(login, password, codigoSessao); 
            }


            return loginAuthentication;
        }

        public bool ReloadLogin(string login, string password, string codigoSessao)
        {
            ValidacaoAcessoSoapClient acessoSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetConfigValidacaoAcessoService();

            if (acessoSoapClient == null)
                ReconnectRevalidation(login, password, codigoSessao);

            XElement xElement = acessoSoapClient.RevalidarUsuario(login,password, codigoSessao);

            XmlDocument xmlServer = new XmlDocument();

            using (XmlReader xmlReader = xElement.CreateReader())
            {
                xmlServer.Load(xmlReader);
            }

            acessoSoapClient.Close();

            xmlServer.ChildNodes[0].InnerXml = Utility.Decript(xmlServer.ChildNodes[0].InnerXml);

            if (xmlServer.DocumentElement != null)
            {
                XmlNode xmlFechamento = xmlServer.DocumentElement.SelectSingleNode("FECHAMENTO");
                XmlNode xmlStatus = xmlServer.DocumentElement.SelectSingleNode("STATUS");
                XmlNode xmlInformacao = xmlServer.DocumentElement.SelectSingleNode("INFORMACAO");


                if ((xmlStatus != null) && (xmlStatus.InnerText == "1"))
                {
                    return true;
                }
                else 
                {
                    Console.Write(xmlInformacao.InnerText);
                    return false;
                }
                
            }

            return false;
        }

        public bool Register(UserRegister userRegister) {

            LoginAuthentication loginAuthentication = null;
            RegistroUsuarioSoapClient acessoSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetConfigRegistroUsuarioService(); 

            if (acessoSoapClient == null)
                ReconnectRegister(userRegister);

            XElement xElement = acessoSoapClient.RegistrarUsuario(userRegister.UserName, userRegister.Password, userRegister.FirstName , userRegister.LastName, userRegister.CPF, userRegister.Email, userRegister.District, userRegister.CEP, userRegister.City, userRegister.Complement, userRegister.Birthday.Value.ToString("MM/dd/yyyy 00:00:00"), userRegister.State, userRegister.Street, userRegister.Number, ((int)userRegister.Tipo).ToString());

            XmlDocument xmlServer = new XmlDocument();

            using (XmlReader xmlReader = xElement.CreateReader())
            {
                xmlServer.Load(xmlReader);
            }

            acessoSoapClient.Close();

            xmlServer.ChildNodes[0].InnerXml = Utility.Decript(xmlServer.ChildNodes[0].InnerXml);

            if (xmlServer.DocumentElement != null)
            {
                XmlNode xmlFechamento = xmlServer.DocumentElement.SelectSingleNode("FECHAMENTO");

                XmlNode xmlLogin = xmlServer.DocumentElement.SelectSingleNode("LOGIN");

                XmlNode xmlSenha = xmlServer.DocumentElement.SelectSingleNode("PASSWORD");

                XmlNode xmlInformacao = xmlServer.DocumentElement.SelectSingleNode("INFORMACAO");

                if ((xmlFechamento == null) || (xmlFechamento.InnerText != "1"))
                {
                    throw new Exception(xmlInformacao.InnerText);
                    return false;
                }
            }

            return true;
        }

        //Update user info:
        public bool Update(UserRegister userRegister)
        {

            RegistroUsuarioSoapClient acessoSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetConfigRegistroUsuarioService();
            
            if (acessoSoapClient == null)
                ReconnectUpdate(userRegister);
            acessoSoapClient.Endpoint.Binding.SendTimeout = new TimeSpan(0, 1, 0);
            acessoSoapClient.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 1, 0);
            acessoSoapClient.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 1, 0);
            ((IContextChannel)acessoSoapClient.InnerChannel).OperationTimeout = new TimeSpan(0, 1, 0);
            
            XElement xElement = acessoSoapClient.AtualizarUsuario(userRegister.Id,userRegister.UserName, userRegister.Password, userRegister.FirstName , userRegister.LastName, userRegister.CPF, userRegister.Email, userRegister.District, userRegister.CEP, userRegister.City, userRegister.Complement, userRegister.Birthday.Value.ToString("MM/dd/yyyy 00:00:00"), userRegister.State, userRegister.Street, userRegister.Number, ((int)userRegister.Tipo).ToString());
            //c = acessoSoapClient.State;
            XmlDocument xmlServer = new XmlDocument();

            using (XmlReader xmlReader = xElement.CreateReader())
            {
                xmlServer.Load(xmlReader);
            }

            acessoSoapClient.Close();

            xmlServer.ChildNodes[0].InnerXml = Utility.Decript(xmlServer.ChildNodes[0].InnerXml);

            if (xmlServer.DocumentElement != null)
            {
                XmlNode xmlFechamento = xmlServer.DocumentElement.SelectSingleNode("FECHAMENTO");

                XmlNode xmlLogin = xmlServer.DocumentElement.SelectSingleNode("LOGIN");

                XmlNode xmlSenha = xmlServer.DocumentElement.SelectSingleNode("PASSWORD");

                XmlNode xmlInformacao = xmlServer.DocumentElement.SelectSingleNode("INFORMACAO");

                if ((xmlFechamento == null) || (xmlFechamento.InnerText != "1"))
                {
                    throw new Exception(xmlInformacao.InnerText);
                    return false;
                }
            }

            return true;
        }

        public List<string> UpgradeVersion(string version, out bool optional)
        {
            optional = true;
            List<string> newVersions = new List<string>();

            UpgradeVersionSoapClient upgradeVersionSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetUpgradeVersionService();

            if (upgradeVersionSoapClient == null)
                ReconnectUpgradeVersion(version, out optional);
            upgradeVersionSoapClient.Endpoint.Binding.SendTimeout = new TimeSpan(0, 1, 0);
            upgradeVersionSoapClient.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 1, 0);
            upgradeVersionSoapClient.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 1, 0);
            ((IContextChannel)upgradeVersionSoapClient.InnerChannel).OperationTimeout = new TimeSpan(0, 1, 0);

            XElement xElement = upgradeVersionSoapClient.CheckUpgrade(version);
            //c = acessoSoapClient.State;
            XmlDocument xmlServer = new XmlDocument();

            using (XmlReader xmlReader = xElement.CreateReader())
            {
                xmlServer.Load(xmlReader);
            }

            upgradeVersionSoapClient.Close();

            xmlServer.ChildNodes[0].InnerXml = Utility.Decript(xmlServer.ChildNodes[0].InnerXml);

            if (xmlServer.DocumentElement != null)
            {
                //TYPE 0=MANDATORY 1=OPTIONAL
                XmlNode xmlType = xmlServer.DocumentElement.SelectSingleNode("TYPE");
                optional = xmlType.InnerText != "0";
                XmlNodeList xmlVersions = xmlServer.DocumentElement.SelectNodes("VERSION");

                foreach(XmlNode node in xmlVersions)
                {
                    newVersions.Add(node.InnerText);
                }
            }

            return newVersions;
        }

        public bool Recovery(string cpf, string email)
        {

            RegistroUsuarioSoapClient acessoSoapClient = Settings.DefiniServer.Instance(LanguageDefault).SetConfigRegistroUsuarioService();

            if (acessoSoapClient == null)
                ReconnectRecovery(cpf,email);
            acessoSoapClient.Endpoint.Binding.SendTimeout = new TimeSpan(0, 1, 0);
            acessoSoapClient.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 1, 0);
            acessoSoapClient.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 1, 0);
            ((IContextChannel)acessoSoapClient.InnerChannel).OperationTimeout = new TimeSpan(0, 1, 0);

            XElement xElement = acessoSoapClient.RecuperarUsuario(cpf, email);
            //c = acessoSoapClient.State;
            XmlDocument xmlServer = new XmlDocument();

            using (XmlReader xmlReader = xElement.CreateReader())
            {
                xmlServer.Load(xmlReader);
            }

            acessoSoapClient.Close();

            xmlServer.ChildNodes[0].InnerXml = Utility.Decript(xmlServer.ChildNodes[0].InnerXml);

            if (xmlServer.DocumentElement != null)
            {
                XmlNode xmlStatus = xmlServer.DocumentElement.SelectSingleNode("STATUS");

                XmlNode xmlFechamento = xmlServer.DocumentElement.SelectSingleNode("FECHAMENTO");

                XmlNode xmlInformacao = xmlServer.DocumentElement.SelectSingleNode("INFORMACAO");

                if ((xmlStatus == null) || (xmlStatus.InnerText != "1"))
                {
                    throw new Exception(xmlInformacao.InnerText);
                    return false;
                }
            }

            return true;
        }

        public List<Features> GetFeatures(string archive)
        {
            XmlDocument xmlDocument = Utility.LoadXmlWithXmlDocument(ListFeatures._path + archive + ".xml");

            if (xmlDocument == null)
                return new List<Features>();

            List<Features> listFeatures = null;

            if (xmlDocument != null)
            {
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("FEATURE");

                listFeatures = (from XmlNode node in nodeList
                                select new Features
                                {
                                    Description = (EFeatures) StringValue.Parse(typeof(EFeatures),Utility.Decript(node["DESCRIPTION"].InnerText)),
                                    Permission = (EPermission)int.Parse(Utility.Decript(node["PERMISSION"].InnerText)),
                                }).ToList();
            }

            return listFeatures;
        }
    }
}