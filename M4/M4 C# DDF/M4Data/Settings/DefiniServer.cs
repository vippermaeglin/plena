using System;
using System.ServiceModel;
using M4Data.ValidacaoAcesso;
using M4Data.RegistroUsuario;
using M4Data.UpgradeVersion;
using M4Utils.Language;

namespace M4Data.Settings
{
    public class DefiniServer
    {
        public string NameServer { get; set; }

        private static DefiniServer _definiServer;

        public static LanguageDefault LanguageDefault;

        public static DefiniServer Instance(LanguageDefault languageDefault)
        {
            LanguageDefault = languageDefault;
            return _definiServer ?? (_definiServer = new DefiniServer());
        }

        public DefiniServer()
        {
            NameServer = "PlenaServer";
        }

        /// <summary>
        /// Verificação de qual servidor está ok para busca de dados
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetUrlConnected(string page)
        {
            string url = "http://" + SettingsServer.Default.Server1_Ip + ":" + SettingsServer.Default.Server1_Port + "/" + NameServer + "/" + page;

            if (VerifyInternet.VerifyConnection(url))
                return url;

            url = "http://" + SettingsServer.Default.Server2_Ip + ":" + 80 + "/" + NameServer + "/" + page;

            if (VerifyInternet.VerifyConnection(url))
                return url;

            url = "http://" + SettingsServer.Default.Server3_Ip + ":" + SettingsServer.Default.Server3_Port + "/" + NameServer + "/" + page;

            return VerifyInternet.VerifyConnection(url) ? url : null;
        }

        public ValidacaoAcessoSoapClient SetConfigValidacaoAcessoService()
        {
            ValidacaoAcessoSoapClient client = new ValidacaoAcessoSoapClient();

            if (String.IsNullOrEmpty(SettingsServer.Default.AccessedUrl))
            {
                string url = GetUrlConnected("ValidacaoAcesso.asmx");

                if (url == null)
                    return null;

                SettingsServer.Default.AccessedUrl = url;
                SettingsServer.Default.Save();
            }
            else
            {
                if (!VerifyInternet.VerifyConnection(SettingsServer.Default.AccessedUrl))
                    return null;
            }

            client.Endpoint.Address = new EndpointAddress(SettingsServer.Default.AccessedUrl);
            return client;
        }

        public RegistroUsuarioSoapClient SetConfigRegistroUsuarioService()
        {
            RegistroUsuarioSoapClient client = new RegistroUsuarioSoapClient();

            string url = GetUrlConnected("RegistroUsuario.asmx");

            if (url == null)
                return null; 
            if (!VerifyInternet.VerifyConnection(url))
                return null;

            client.Endpoint.Address = new EndpointAddress(url);
            return client;
        }

        public UpgradeVersionSoapClient SetUpgradeVersionService()
        {
            UpgradeVersionSoapClient client = new UpgradeVersionSoapClient();

            string url = GetUrlConnected("UpgradeVersion.asmx");

            if (url == null)
                return null;
            if (!VerifyInternet.VerifyConnection(url))
                return null;

            client.Endpoint.Address = new EndpointAddress(url);
            return client;
        }

        public void SaveSettingsServer(string server1Ip, string server2Ip, string server3Ip, string server1Port, string server2Port, string server3Port)
        {
            SettingsServer.Default.Server1_Ip = server1Ip;
            SettingsServer.Default.Server2_Ip = server2Ip;
            SettingsServer.Default.Server3_Ip = server3Ip;
            SettingsServer.Default.Server1_Port = server1Port;
            SettingsServer.Default.Server2_Port = server2Port;
            SettingsServer.Default.Server3_Port = server3Port;
            SettingsServer.Default.Save();
        }
    }
}
