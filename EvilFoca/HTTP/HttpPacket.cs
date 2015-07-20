/* 
Evil FOCA
Copyright (C) 2015 ElevenPaths

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace evilfoca.HTTP
{
    public class HttpPacket
    {
        public string Method { get; private set; }
        public string Version { get; private set; }
        public string Host { get; private set; }
        public string ResourceRequest { get; private set; }
        public string ContentType { get; private set; }
        public long ContentLength { get; private set; }
        public CookieContainer Cookies { get; private set; }
        public string Connection { get; private set; }
        public string P3P { get; private set; }
        public string Accept { get; private set; }
        public string AcceptLanguage { get; private set; }
        public string Referer { get; private set; }
        public bool IsCompleted { get; private set; }
        public string FullUrlRequest
        {
            get
            {
                return string.Format("http://{0}{1}", Host, ResourceRequest);
            }
        }
        public byte[] Data { get; private set; }

        public string UserAgent { get; private set; }

        public HttpPacket(byte[] payload)
        {
            try
            {
                string query = System.Web.HttpUtility.HtmlDecode(System.Text.Encoding.ASCII.GetString(payload));
                string[] headerContentSplit = query.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (headerContentSplit.Length > 0)
                {
                    IsCompleted = true;
                    CookieCollection cookieCol = null;
                    string[] splitRequest = headerContentSplit[0].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitRequest)
                    {
                        if (item.Contains(' '))
                        {

                            string[] splitSpace = item.Split(' ');
                            switch (splitSpace[0].ToLower())
                            {
                                case "host:":
                                    Host = splitSpace[1];
                                    break;
                                case "user-agent:":
                                    UserAgent = item.Replace(splitSpace[0], "").Trim();
                                    break;
                                case "head":
                                case "post":
                                case "get":
                                    if (splitSpace.Length > 0)
                                        Method = splitSpace[0];
                                    if (splitSpace.Length > 1)
                                        ResourceRequest = splitSpace[1];
                                    if (splitSpace.Length > 2)
                                        Version = splitSpace[2];
                                    break;
                                case "content-type:":
                                    ContentType = splitSpace[1];
                                    break;
                                case "content-length:":
                                    long longLength = 0;
                                    long.TryParse(splitSpace[1], out longLength);
                                    ContentLength = longLength;
                                    break;
                                case "cookie:":
                                    Cookies = new CookieContainer();
                                    cookieCol = ParseCookies(item.Replace(splitSpace[0], string.Empty), Host);
                                    break;
                                case "connection:":
                                    this.Connection = splitSpace[1];
                                    break;
                                case "accept-language:":
                                    this.AcceptLanguage = splitSpace[1];
                                    break;
                                case "accept:":
                                    this.Accept = splitSpace[1];
                                    break;
                                case "p3p:":
                                    if (splitSpace.Length > 1)
                                        this.P3P = splitSpace[1];
                                    break;
                                case "referer:":
                                    if (splitSpace.Length > 1)
                                        this.Referer = splitSpace[1];
                                    break;
                            }
                        }
                    }
                    if (Cookies != null && cookieCol != null && !string.IsNullOrEmpty(Host))
                        Cookies.Add(new Uri("http://" + Host, UriKind.Absolute), cookieCol);

                    if (headerContentSplit.Length > 1)
                    {
                        Data = System.Text.Encoding.ASCII.GetBytes(headerContentSplit[1]);
                    }

                    if (string.IsNullOrEmpty(this.Method))
                        IsCompleted = false;
                    else if (this.Method.ToLower() == "post" && (this.Data == null || this.Data.Length < this.ContentLength))
                        IsCompleted = false;
                }
                else
                    IsCompleted = false;

            }
            catch (Exception ex)
            {
                IsCompleted = false;
                ex.ToString();
            }
        }

        public static byte[] Get404Packet()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(System.Text.Encoding.ASCII.GetBytes("HTTP/1.1 404 Not Found\r\n"));
                    writer.Write(System.Text.Encoding.ASCII.GetBytes("Connection: Close\r\n\r\n"));
                }
                return ms.ToArray();
            }
        }


        private CookieCollection ParseCookies(string stringCookies, string domain)
        {
            string[] splitSemiColon = stringCookies.Split(';');
            CookieCollection col = new CookieCollection();
            foreach (var item in splitSemiColon)
            {
                if (item.IndexOf('=') > -1)
                    col.Add(new Cookie(item.Substring(0, item.IndexOf('=')).Trim(), item.Substring(item.IndexOf('=') + 1).Trim()));
            }
            return col;
        }

        public static string CookieToSetCookieHeader(Cookie cookie)
        {
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append(string.Format("Set-Cookie: {0}={1}", cookie.Name, cookie.Value));
            if (cookie.Expires > DateTime.MinValue)
                headerBuilder.Append(string.Format(";Expires={0}", cookie.Expires.ToString("dd-MMM-yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture)));
            if (!string.IsNullOrEmpty(cookie.Path))
                headerBuilder.Append(string.Format(";path={0}", cookie.Path));
            if (!string.IsNullOrEmpty(cookie.Domain))
                headerBuilder.Append(string.Format(";domain={0}", cookie.Domain));
            if (cookie.HttpOnly)
                headerBuilder.Append(";HttpOnly");

            headerBuilder.Append("\r\n");
            return headerBuilder.ToString();
        }

        public static CookieCollection ParseSetCookies(string stringSetCookies)
        {
            CookieCollection cookies = new CookieCollection();
            try
            {
                Regex expiresEqualsDay = _expiresEqualsDay ?? (_expiresEqualsDay = new Regex(EXPIRES_EQUALS_DAY_PATTERN, RegexOptions.IgnoreCase | RegexOptions.Compiled));
                string[] splitCookies = expiresEqualsDay.Replace(stringSetCookies, EXPIRES_EQUALS).Split(',');
                foreach (var item in splitCookies)
                {
                    string[] splitField = item.Split(';');
                    if (splitField.Length > 0)
                    {
                        Cookie newCookie = new Cookie();
                        newCookie.Name = splitField[0].Substring(0, splitField[0].IndexOf('=')).Trim();
                        newCookie.Value = splitField[0].Substring(splitField[0].IndexOf('=') + 1).Trim();
                        for (int i = 1; i < splitField.Length; i++)
                        {
                            string key = splitField[i].ToLower().Trim();
                            if (splitField[i].Contains('='))
                                key = splitField[i].Substring(0, splitField[i].IndexOf('='));
                            string value = string.Empty;
                            switch (key)
                            {
                                case "expires":
                                    DateTime time;
                                    if (DateTime.TryParseExact(splitField[i].Substring(splitField[i].IndexOf('=') + 1).Trim(), new string[] { @"dd-MMM-yyyy HH:mm:ss 'GMT'", @"dd MMM yyyy HH:mm:ss 'GMT'" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out time))
                                        newCookie.Expires = time;
                                    break;
                                case "path":
                                    newCookie.Path = "/";
                                    break;
                                case "domain":
                                    newCookie.Domain = splitField[i].Substring(splitField[i].IndexOf('=') + 1).Trim();
                                    break;
                                case "httponly":
                                    newCookie.HttpOnly = true;
                                    break;
                                case "secure":
                                    newCookie.Secure = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        cookies.Add(newCookie);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }


            return cookies;
        }

        private static Regex _expiresEqualsDay;     // Lazy-initialized.
        private const String EXPIRES_EQUALS_DAY_PATTERN = "; *expires=[A-Za-z]+, *";
        private const String EXPIRES_EQUALS = "; Expires=";
    }
    public static class HttpResponse
    {
        public static IEnumerable<byte[]> GetBytes(this HttpWebResponse response, int maxLength)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("HTTP/{0} {1} {2} \r\n", response.ProtocolVersion, (int)response.StatusCode, response.StatusDescription)));

                    if (response.Headers[HttpResponseHeader.Location] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Location: {0}\r\n", response.Headers[HttpResponseHeader.Location].Replace("https", "http"))));
                    if (response.Headers[HttpResponseHeader.ContentType] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}\r\n", response.Headers[HttpResponseHeader.ContentType])));
                    if (response.Headers[HttpResponseHeader.Connection] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Connection: {0}\r\n", response.Headers[HttpResponseHeader.Connection])));
                    else if (response.Headers[HttpResponseHeader.KeepAlive] != null)
                        response.Headers[HttpResponseHeader.KeepAlive].ToString();
                    if (response.Headers[HttpResponseHeader.Date] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Date: {0}\r\n", response.Headers[HttpResponseHeader.Date])));
                    if (response.Headers[HttpResponseHeader.Allow] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Allow: {0}\r\n", response.Headers[HttpResponseHeader.Allow])));
                    if (response.Headers[HttpResponseHeader.CacheControl] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Cache-Control: {0}\r\n", response.Headers[HttpResponseHeader.CacheControl])));
                    if (response.Headers["P3P"] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("P3P: {0}\r\n", response.Headers["P3P"])));
                    if (response.Headers[HttpResponseHeader.Vary] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Vary: {0}\r\n", response.Headers[HttpResponseHeader.Vary])));
                    if (response.Headers[HttpResponseHeader.Expires] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Expires: {0}\r\n", response.Headers[HttpResponseHeader.Expires])));
                    if (response.Headers[HttpResponseHeader.Server] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Server: {0}\r\n", response.Headers[HttpResponseHeader.Server])));
                    if (response.Headers[HttpResponseHeader.ContentEncoding] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Content-Encoding: {0}\r\n", response.Headers[HttpResponseHeader.ContentEncoding])));
                    if (response.Headers[HttpResponseHeader.ContentLanguage] != null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(string.Format("Content-Language: {0}\r\n", response.Headers[HttpResponseHeader.ContentLanguage])));

                    if (response.Headers[HttpResponseHeader.TransferEncoding] != null && response.Headers[HttpResponseHeader.TransferEncoding].Contains("chunked") && response.Headers[HttpResponseHeader.Connection] == null)
                        writer.Write(System.Text.Encoding.ASCII.GetBytes("Connection: close\r\n"));

                    if (response.Headers[HttpResponseHeader.SetCookie] != null)
                    {
                        CookieCollection cookieCol = HttpPacket.ParseSetCookies(response.Headers[HttpResponseHeader.SetCookie]);
                        foreach (Cookie item in cookieCol)
                            writer.Write(System.Text.Encoding.ASCII.GetBytes(HttpPacket.CookieToSetCookieHeader(item)));
                    }

                    writer.Write(System.Text.Encoding.ASCII.GetBytes("\r\n"));
                }
                using (MemoryStream headerStream = new MemoryStream(ms.ToArray()))
                {
                    using (BinaryReader reader = new BinaryReader(headerStream))
                    {
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                            yield return reader.ReadBytes(maxLength);
                    }
                }
            }
            using (BinaryReader reader = new BinaryReader(response.GetResponseStream()))
            {
                int bytesRead;
                byte[] buffer = new byte[maxLength];
                Encoding responseEncoding = null;
                while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Array.Resize(ref buffer, bytesRead);
                    if (response.ContentType.ToLower().Contains("html") || response.ContentType.ToLower().Contains("javascript") || response.ContentType.ToLower().Contains("json"))
                    {
                        if (responseEncoding == null)
                            responseEncoding = GetResponseEncoding(response.CharacterSet);
                        string htmlContent = responseEncoding.GetString(buffer);
                        htmlContent = htmlContent.Replace("https", "http").Replace("Https", "http");

                        byte[] htmlBytes = responseEncoding.GetBytes(htmlContent);

                        yield return htmlBytes;
                    }
                    else
                        yield return buffer;
                    buffer = new byte[maxLength];
                }
            }
        }

        private static Encoding GetResponseEncoding(string encodingValue)
        {
            Encoding enc;
            try
            {
                enc = Encoding.GetEncoding(encodingValue);
            }
            catch (ArgumentException)
            {
                switch (encodingValue)
                {
                    case "cp1252":
                        enc = Encoding.GetEncoding("Windows-1252");
                        break;
                    case "utf8":
                        enc = Encoding.UTF8;
                        break;
                    default:
                        enc = Encoding.UTF8;
                        break;
                }
            }


            return enc;
        }

    }

    public static class HttpUtils
    {
        public static void CreateListenerResponse(HttpWebResponse webResponse, ref HttpListenerResponse listenerResponse)
        {

            listenerResponse.ContentType = webResponse.ContentType;
            listenerResponse.KeepAlive = false;
            listenerResponse.StatusCode = (int)webResponse.StatusCode;

            if (webResponse.Headers[HttpResponseHeader.Location] != null)
                listenerResponse.Headers[HttpResponseHeader.Location] = webResponse.Headers[HttpResponseHeader.Location].Replace("https:", "http:");
            if (webResponse.Headers[HttpResponseHeader.SetCookie] != null)
                listenerResponse.Headers[HttpResponseHeader.SetCookie] = webResponse.Headers[HttpResponseHeader.SetCookie];
            if (webResponse.Headers[HttpResponseHeader.Pragma] != null)
                listenerResponse.Headers[HttpResponseHeader.Pragma] = webResponse.Headers[HttpResponseHeader.Pragma];
            if (webResponse.Headers[HttpResponseHeader.Expires] != null)
                listenerResponse.Headers[HttpResponseHeader.Expires] = webResponse.Headers[HttpResponseHeader.Expires];
            if (webResponse.Headers[HttpResponseHeader.ContentEncoding] != null)
                listenerResponse.Headers[HttpResponseHeader.ContentEncoding] = webResponse.Headers[HttpResponseHeader.ContentEncoding];

            if (webResponse.Headers[HttpResponseHeader.CacheControl] != null)
                listenerResponse.Headers[HttpResponseHeader.CacheControl] = webResponse.Headers[HttpResponseHeader.CacheControl];
            if (webResponse.Headers[HttpResponseHeader.Server] != null)
                listenerResponse.Headers[HttpResponseHeader.Server] = webResponse.Headers[HttpResponseHeader.Server];
            if (webResponse.Headers[HttpResponseHeader.Date] != null)
                listenerResponse.Headers[HttpResponseHeader.Date] = webResponse.Headers[HttpResponseHeader.Date];
            if (webResponse.Headers[HttpResponseHeader.Vary] != null)
                listenerResponse.Headers[HttpResponseHeader.Vary] = webResponse.Headers[HttpResponseHeader.Vary];

            if (webResponse.Headers["X-Frame-Options"] != null)
                listenerResponse.Headers["X-Frame-Options"] = webResponse.Headers["X-Frame-Options"];
            if (webResponse.Headers["X-XSS-Protection"] != null)
                listenerResponse.Headers["X-XSS-Protection"] = webResponse.Headers["X-XSS-Protection"];

        }
        public static HttpWebRequest CreateWebRequest(HttpListenerRequest listenerrequest, Uri url)
        {
            UriBuilder u = (url == null) ? new UriBuilder(listenerrequest.Url) : new UriBuilder(url);
            if (u.Scheme.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                u.Port = 443;
            else
                u.Port = 80;

            HttpWebRequest evilReq = HttpWebRequest.Create(u.Uri) as HttpWebRequest;
            evilReq.Timeout = 5000;
            evilReq.KeepAlive = false;
            evilReq.AllowAutoRedirect = false;
            evilReq.UserAgent = listenerrequest.UserAgent;
            evilReq.Method = listenerrequest.HttpMethod;

            if (listenerrequest.ContentType != null)
                evilReq.ContentType = listenerrequest.ContentType;
            if (listenerrequest.UrlReferrer != null)
                evilReq.Referer = listenerrequest.UrlReferrer.ToString();
            if (listenerrequest.Headers["Host"] != null)
                evilReq.Host = listenerrequest.Headers["Host"];
            if (listenerrequest.Headers["Accept"] != null)
                evilReq.Accept = listenerrequest.Headers["Accept"];
            if (listenerrequest.Headers["Accept-Language"] != null)
                evilReq.Headers[HttpRequestHeader.AcceptLanguage] = listenerrequest.Headers["Accept-Language"];
            if (listenerrequest.Headers["Cache-Control"] != null)
                evilReq.Headers[HttpRequestHeader.CacheControl] = listenerrequest.Headers["Cache-Control"];
            if (listenerrequest.Headers["Cookie"] != null)
                evilReq.Headers.Add(HttpRequestHeader.Cookie, listenerrequest.Headers["Cookie"]);

            if (evilReq.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                byte[] buffer = new byte[500];
                using (BinaryReader reader = new BinaryReader(listenerrequest.InputStream))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int readed = 0;
                        while ((readed = reader.Read(buffer, 0, 500)) > 0)
                            ms.Write(buffer, 0, readed);

                        evilReq.ContentLength = ms.Length;
                        using (Stream str = evilReq.GetRequestStream())
                        {
                            str.Write(ms.ToArray(), 0, (int)ms.Length);
                        }
                    }
                }
            }

            return evilReq;
        }

        public static HttpWebRequest CreateWebRequest(HttpListenerRequest listenerrequest)
        {
            return CreateWebRequest(listenerrequest, null);

        }
    }
}
