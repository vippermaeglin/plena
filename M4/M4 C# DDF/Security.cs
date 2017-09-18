/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace M4
{
  public class Security
  {
    private readonly TripleDESCryptoServiceProvider m_des;
    private readonly byte[] m_iv;
    private readonly byte[] m_key;
    private readonly UTF8Encoding m_utf8;

    public Security(byte[] key, byte[] iv)
    {
      m_des = new TripleDESCryptoServiceProvider();
      m_utf8 = new UTF8Encoding();
      m_key = key;
      m_iv = iv;
    }

    public Security()
    {
      m_des = new TripleDESCryptoServiceProvider();
      m_utf8 = new UTF8Encoding();
      byte[ ] key = new byte[ ]
                      {
                        1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                        17, 18, 19, 20, 21, 22, 23, 24
                      };
      byte[ ] iv = new byte[ ] { 8, 7, 6, 5, 4, 3, 2, 1 };
      m_key = key;
      m_iv = iv;
    }

    public byte[] Encrypt(byte[] input)
    {
      return Transform(input, m_des.CreateEncryptor(m_key, m_iv));
    }

    public byte[] Decrypt(byte[] input)
    {
      return Transform(input, m_des.CreateDecryptor(m_key, m_iv));
    }

    public string Encrypt(string text)
    {
      byte[ ] input = m_utf8.GetBytes(text);
      return Convert.ToBase64String(Transform(input, m_des.CreateEncryptor(m_key, m_iv)));
    }

    public string Decrypt(string text)
    {
      byte[ ] input = Convert.FromBase64String(text);
      byte[ ] output = Transform(input, m_des.CreateDecryptor(m_key, m_iv));
      return m_utf8.GetString(output);
    }

    private static byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
    {
      MemoryStream memStream = new MemoryStream();
      CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);
      cryptStream.Write(input, 0, input.Length);
      cryptStream.FlushFinalBlock();
      memStream.Position = 0L;
      byte[] result = new byte[((int)(memStream.Length - 1L)) + 1];
      memStream.Read(result, 0, result.Length);
      memStream.Close();
      cryptStream.Close();
      return result;
    }
  }
}
