/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace oAuth
{
  public class oAuthTwitter : OAuthBase
  {
    public enum Method { GET, POST, DELETE };
    public const string REQUEST_TOKEN = "http://twitter.com/oauth/request_token";
    public const string AUTHORIZE = "http://twitter.com/oauth/authorize";
    public const string ACCESS_TOKEN = "http://twitter.com/oauth/access_token";

    private string _consumerKey = "";
    private string _consumerSecret = "";
    private string _token = "";
    private string _tokenSecret = "";
    private string _callBackUrl = "oob";
    private string _oauthVerifier = "";


    #region Properties
    public string ConsumerKey
    {
      get
      {
        if (_consumerKey.Length == 0)
        {
          _consumerKey = M4.Properties.Settings.Default.consumerKey;
        }
        return _consumerKey;
      }
      set { _consumerKey = value; }
    }

    public string ConsumerSecret
    {
      get
      {
        if (_consumerSecret.Length == 0)
        {
          _consumerSecret = M4.Properties.Settings.Default.consumerSecret;
        }
        return _consumerSecret;
      }
      set { _consumerSecret = value; }
    }

    public string Token { get { return _token; } set { _token = value; } }
    public string TokenSecret { get { return _tokenSecret; } set { _tokenSecret = value; } }
    public string CallBackUrl { get { return _callBackUrl; } set { _callBackUrl = value; } }
    public string OAuthVerifier { get { return _oauthVerifier; } set { _oauthVerifier = value; } }

    #endregion

    /// <summary>
    /// Get the link to Twitter's authorization page for this application.
    /// </summary>
    /// <returns>The url with a valid request token, or a null string.</returns>
    public string AuthorizationLinkGet()
    {
      string ret = null;

      string response = oAuthWebRequest(Method.GET, REQUEST_TOKEN, String.Empty);
      if (response.Length > 0)
      {
        //response contains token and token secret.  We only need the token.
        NameValueCollection qs = HttpUtility.ParseQueryString(response);

        if (qs["oauth_callback_confirmed"] != null)
        {
          if (qs["oauth_callback_confirmed"] != "true")
          {
            throw new Exception("OAuth callback not confirmed.");
          }
        }

        if (qs["oauth_token"] != null)
        {
          ret = AUTHORIZE + "?oauth_token=" + qs["oauth_token"];
        }
      }
      return ret;
    }

    /// <summary>
    /// Exchange the request token for an access token.
    /// </summary>
    /// <param name="authToken">The oauth_token is supplied by Twitter's authorization page following the callback.</param>
    /// <param name="oauthVerifier">An oauth_verifier parameter is provided to the client either in the pre-configured callback URL</param>
    public void AccessTokenGet(string authToken, string oauthVerifier)
    {
      Token = authToken;
      OAuthVerifier = oauthVerifier;

      string response = oAuthWebRequest(Method.GET, ACCESS_TOKEN, String.Empty);

      if (response.Length > 0)
      {
        //Store the Token and Token Secret
        NameValueCollection qs = HttpUtility.ParseQueryString(response);
        if (qs["oauth_token"] != null)
        {
          Token = qs["oauth_token"];
        }
        if (qs["oauth_token_secret"] != null)
        {
          TokenSecret = qs["oauth_token_secret"];
        }
      }
    }

    /// <summary>
    /// Submit a web request using oAuth.
    /// </summary>
    /// <param name="method">GET or POST</param>
    /// <param name="url">The full url, including the querystring.</param>
    /// <param name="postData">Data to post (querystring format)</param>
    /// <returns>The web server response.</returns>
    public string oAuthWebRequest(Method method, string url, string postData)
    {
      string outUrl;
      string querystring;


      //Setup postData for signing.
      //Add the postData to the querystring.
      if (method == Method.POST || method == Method.DELETE)
      {
        if (postData.Length > 0)
        {
          //Decode the parameters and re-encode using the oAuth UrlEncode method.
          NameValueCollection qs = HttpUtility.ParseQueryString(postData);
          postData = "";
          foreach (string key in qs.AllKeys)
          {
            if (postData.Length > 0)
            {
              postData += "&";
            }
            qs[key] = HttpUtility.UrlDecode(qs[key]);
            qs[key] = UrlEncode(qs[key]);
            postData += key + "=" + qs[key];

          }
          if (url.IndexOf("?") > 0)
          {
            url += "&";
          }
          else
          {
            url += "?";
          }
          url += postData;
        }
      }

      Uri uri = new Uri(url);

      string nonce = GenerateNonce();
      string timeStamp = GenerateTimeStamp();

      //Generate Signature
      string sig = GenerateSignature(uri,
          ConsumerKey,
          ConsumerSecret,
          Token,
          TokenSecret,
          CallBackUrl,
          OAuthVerifier,
          method.ToString(),
          timeStamp,
          nonce,
          out outUrl,
          out querystring);

      querystring += "&oauth_signature=" + UrlEncode(sig);

      //Convert the querystring to postData
      if (method == Method.POST || method == Method.DELETE)
      {
        postData = querystring;
        querystring = "";
      }

      if (querystring.Length > 0)
      {
        outUrl += "?";
      }

      return WebRequest(method, outUrl + querystring, postData);
    }

    /// <summary>
    /// Web Request Wrapper
    /// </summary>
    /// <param name="method">Http Method</param>
    /// <param name="url">Full url to the web resource</param>
    /// <param name="postData">Data to post in querystring format</param>
    /// <returns>The web server response.</returns>
    public string WebRequest(Method method, string url, string postData)
    {
      StreamWriter requestWriter;

      HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
      webRequest.Method = method.ToString();
      webRequest.ServicePoint.Expect100Continue = false;
      //webRequest.UserAgent  = "Identify your application please.";
      //webRequest.Timeout = 20000;

      if (method == Method.POST || method == Method.DELETE)
      {
        webRequest.ContentType = "application/x-www-form-urlencoded";

        //POST the data.
        requestWriter = new StreamWriter(webRequest.GetRequestStream());
        try
        {
          requestWriter.Write(postData);
        }
        finally
        {
          requestWriter.Close();
        }
      }

      string responseData = WebResponseGet(webRequest);
      return responseData;
    }

    /// <summary>
    /// Process the web response.
    /// </summary>
    /// <param name="webRequest">The request object.</param>
    /// <returns>The response data.</returns>
    public string WebResponseGet(HttpWebRequest webRequest)
    {
      using (var stream = webRequest.GetResponse().GetResponseStream())
      {
        if (stream == null)
        {
          return string.Empty;
        }

        using (var responseReader = new StreamReader(stream))
        {
          return responseReader.ReadToEnd();
        }
      }
    }
  }
}
