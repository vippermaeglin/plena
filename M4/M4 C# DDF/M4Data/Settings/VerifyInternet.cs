using System;
using System.Net;
using System.Runtime.InteropServices;

namespace M4Data.Settings
{
    public class VerifyInternet
    {
        private static int _connectionAttempt;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        //Criar função para utilizar a API
        public static bool IsConnectedToInternet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }

        //Verifica se existe a conexão com endereço informado
        public static bool VerifyConnection(string address)
        {
            if (!IsConnectedToInternet())
                throw new Exception("Conexão com a internet não está ativa");

            if (String.IsNullOrEmpty(address))
                return false;

            if (address.Equals("about:blank"))
                return false;

            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
                address = "http://" + address;

            Uri uri = new Uri(address);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy = null;
            request.KeepAlive = false;

            //request.IfModifiedSince = DateTime.Now.AddSeconds(3);
            request.Timeout = 60000;
            request.ReadWriteTimeout = 60000;
            request.ServicePoint.MaxIdleTime = 60000;
            request.ServicePoint.ConnectionLeaseTimeout = 60000;
            request.ContentType = "text/xml";
            request.UserAgent = "M4";
            
            bool isConnection;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    if (_connectionAttempt == 1)
                    {
                        _connectionAttempt = 0;
                        response.Close();
                        throw new Exception("Servidor não acessível");
                    }

                    _connectionAttempt++;
                    response.Close();
                    VerifyConnection(address);
                }

                isConnection = response.StatusCode == HttpStatusCode.OK;
                response.Close();
            }
            catch
            {
                isConnection = false;
            }
            return isConnection;
        }
    }
}