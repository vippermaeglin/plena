using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using System.Xml;

namespace M4Utils
{
    public class Utility
    {
        const string Password = "s3nh@";

        public static string Cript(string message)
        {
            if (message == "") return "";
            byte[] results;
            
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(Password));

            TripleDESCryptoServiceProvider tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            byte[] dataToEncrypt = utf8.GetBytes(message);

            try
            {
                ICryptoTransform encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            return Convert.ToBase64String(results);
        }

        public static string Decript(string message)
        {
            if (message == "") return "";
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(Password));

            TripleDESCryptoServiceProvider tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            byte[] dataToDecrypt = Convert.FromBase64String(message);

            try
            {
                ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            return utf8.GetString(results);
        }

        [DllImport("wininet.dll")]
        private extern static Boolean InternetGetConnectedState(out int description, int reservedValue);

        // Um método que verifica se esta conectado
        public static Boolean IsConnected()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        /// <summary>
        /// Carrega um xml pelo caminho informado
        /// </summary>
        /// <returns>XmlDocumento com os valores carregados</returns>
        public static XmlDocument LoadXmlWithXmlDocument(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return new XmlDocument();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                return xmlDocument;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Carrega um objeto DataSet de acordo com o nome e o caminho
        /// </summary>
        /// <returns>DataSet com os valores carregados</returns>
        public DataSet LoadXmlWithDataSet(string path, string name)
        {
            DataSet ds = new DataSet(name);
            ds.ReadXml(path);
            return ds;
        }
    }
}
