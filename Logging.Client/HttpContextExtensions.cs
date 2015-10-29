using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace Logging.Client
{
     static class HttpContextBaseExtensions
    {
       
      
     

        #region client ip
        /// <summary>
        /// When a client IP can't be determined
        /// </summary>
        public const string UnknownIP = "0.0.0.0";

        private static readonly Regex IPv4Regex = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// returns true if this is a private network IP  
        /// http://en.wikipedia.org/wiki/Private_network
        /// </summary>
        private static bool IsPrivateIP(string s)
        {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }
        /// <summary>
        /// retrieves the IP address of the current request -- handles proxies and private networks
        /// </summary>
        public static string GetRemoteIP(this NameValueCollection serverVariables)
        {
            var ip = serverVariables["REMOTE_ADDR"]; // could be a proxy -- beware
            var ipForwarded = serverVariables["HTTP_X_FORWARDED_FOR"];

            // check if we were forwarded from a proxy
            if (!string.IsNullOrEmpty(ipForwarded))
            {
                ipForwarded = IPv4Regex.Match(ipForwarded).Value;
                if (!string.IsNullOrEmpty(ipForwarded) && !IsPrivateIP(ipForwarded))
                    ip = ipForwarded;
            }

            return !string.IsNullOrEmpty(ip) ? ip : UnknownIP;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP(this HttpContextBase ctx)
        {
            string ip = null;
            if (ctx.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = ctx.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            else
            {
                ip = ctx.Request.UserHostAddress;
            }
            return ip;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIPAddress()
        {
            string clientIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(clientIP))
            {
                clientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }


            if (!string.IsNullOrEmpty(clientIP))
                return clientIP;
            else
                return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static string GetClientIP(this HttpContext ctx)
        {
            return GetClientIP(new HttpContextWrapper(ctx));
        }

        public static string GetClientIP(this HttpRequestBase hrb)
        {
            return GetClientIP(hrb.RequestContext.HttpContext);
        }

        #endregion
    }
}