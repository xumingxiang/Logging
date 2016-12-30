//using System;
//using System.Collections.Specialized;
//using System.Diagnostics;
//using System.Text;
//using System.Web;

//namespace Logging.Client
//{
//    public class Error
//    {
//        private NameValueCollection _serverVariables;
//        private NameValueCollection _queryString;
//        private NameValueCollection _form;
//        private NameValueCollection _cookies;
//        private NameValueCollection _requestHeaders;

//        internal const string CollectionErrorKey = "CollectionFetchError";

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Error"/> class.
//        /// </summary>
//        public Error() { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Error"/> class from a given <see cref="Exception"/> instance.
//        /// </summary>
//        public Error(Exception e) : this(e, null) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Error"/> class
//        /// from a given <see cref="Exception"/> instance and
//        /// <see cref="HttpContext"/> instance representing the HTTP
//        /// context during the exception.
//        /// </summary>
//        public Error(Exception e, HttpContextBase context)
//        {
//            if (e == null) throw new ArgumentNullException("e");

//            Exception = e;
//            var baseException = e;

//            // if it's not a .Net core exception, usually more information is being added
//            // so use the wrapper for the message, type, etc.
//            // if it's a .Net core exception type, drill down and get the innermost exception
//            if (IsBuiltInException(e))
//                baseException = e.GetBaseException();
//            MachineName = Environment.MachineName;
//            Source = baseException.Source;
//            Detail = e.ToString();
//            var httpException = e as HttpException;
//            if (httpException != null)
//            {
//                StatusCode = httpException.GetHttpCode();
//            }
//            SetContextProperties(context);
//        }

//        /// <summary>
//        /// Sets Error properties pulled from HttpContext, if present
//        /// </summary>
//        /// <param name="context">The HttpContext related to the request</param>
//        private void SetContextProperties(HttpContextBase context)
//        {
//            if (context == null) return;

//            var request = context.Request;
//            AbsUrl = request.Url.ToString();
//            Func<Func<HttpRequestBase, NameValueCollection>, NameValueCollection> tryGetCollection = getter =>
//                {
//                    try
//                    {
//                        return new NameValueCollection(getter(request));
//                    }
//                    catch (HttpRequestValidationException e)
//                    {
//                        Trace.WriteLine("Error parsing collection: " + e.Message);
//                        return new NameValueCollection { { CollectionErrorKey, e.Message } };
//                    }
//                };

//            _serverVariables = tryGetCollection(r => r.ServerVariables);
//            //_queryString = tryGetCollection(r => r.QueryString);
//            //_form = tryGetCollection(r => r.Form);
//            try
//            {
//                _cookies = new NameValueCollection(request.Cookies.Count);
//                for (var i = 0; i < request.Cookies.Count; i++)
//                {
//                    var name = request.Cookies[i].Name;
//                    _cookies.Add(name, request.Cookies[i].Value);
//                }
//            }
//            catch (HttpRequestValidationException e)
//            {
//                Trace.WriteLine("Error parsing cookie collection: " + e.Message);
//            }

//            _requestHeaders = new NameValueCollection(request.Headers.Count);
//            foreach (var header in request.Headers.AllKeys)
//            {
//                // Cookies are handled above, no need to repeat
//                if (string.Compare(header, "Cookie", StringComparison.OrdinalIgnoreCase) == 0)
//                    continue;

//                if (request.Headers[header] != null)
//                    _requestHeaders[header] = request.Headers[header];
//            }
//        }
//        /// <summary>
//        /// returns if the type of the exception is built into .Net core
//        /// </summary>
//        /// <param name="e">The exception to check</param>
//        /// <returns>True if the exception is a type from within the CLR, false if it's a user/third party type</returns>
//        private bool IsBuiltInException(Exception e)
//        {
//            return e.GetType().Name == "CommonLanguageRuntimeLibrary";
//        }

//        public Exception Exception { get; set; }

//        public string AbsUrl { get; set; }
//        /// <summary>
//        /// Gets the hostname of where the exception occured
//        /// </summary>
//        public string MachineName { get; set; }

//        /// <summary>
//        /// Gets the source of this error
//        /// </summary>
//        public string Source { get; set; }

//        /// <summary>
//        /// Gets the detail/stack trace of this error
//        /// </summary>
//        public string Detail { get; set; }

//        /// <summary>
//        /// Gets the HTTP Status code associated with the request
//        /// </summary>
//        public int StatusCode { get; set; }

//        ///// <summary>
//        ///// The URL host of the request causing this error
//        ///// </summary>
//        //public string Host { get { return _host ?? (_host = _serverVariables == null ? "" : _serverVariables["HTTP_HOST"]); } set { _host = value; } }
//        //private string _host;

//        ///// <summary>
//        ///// The URL path of the request causing this error
//        ///// </summary>
//        //public string Url { get { return _url ?? (_url = _serverVariables == null ? "" : _serverVariables["URL"]); } set { _url = value; } }
//        //private string _url;

//        /// <summary>
//        /// The HTTP Method causing this error, e.g. GET or POST
//        /// </summary>
//        public string HTTPMethod { get { return _httpMethod ?? (_httpMethod = _serverVariables == null ? "" : _serverVariables["REQUEST_METHOD"]); } set { _httpMethod = value; } }
//        private string _httpMethod;

//        /// <summary>
//        /// The IPAddress of the request causing this error
//        /// </summary>
//        public string IPAddress { get { return _ipAddress ?? (_ipAddress = _serverVariables == null ? "" : _serverVariables.GetRemoteIP()); } set { _ipAddress = value; } }
//        private string _ipAddress;

//        private StringBuilder GetPairs(NameValueCollection nvc)
//        {
//            var result = new StringBuilder();
//            if (nvc == null)
//                return result;

//            for (int i = 0; i < nvc.Count; i++)
//            {
//                result.AppendFormat("{0}:{1},", nvc.GetKey(i), nvc.Get(i));
//            }
//            return result;
//        }

//        public override string ToString()
//        {
//            return string.Format("Url:{0}\r\nMachineName:{1}\r\nSource:{2}\r\nDetail:{3}\r\nStatusCode:{4}\r\nHTTPMethod:{5}\r\nIPAddress:{6}\r\nRequestHeaders {7}\r\nCookies {8}",
//                AbsUrl, MachineName, Source, Detail, StatusCode.ToString(), HTTPMethod, IPAddress, GetPairs(_requestHeaders).ToString(), GetPairs(_cookies).ToString());
//        }
//    }

//    /// <summary>
//    /// Serialization class in place of the NameValueCollection pairs
//    /// </summary>
//    /// <remarks>This exists because things like a querystring can havle multiple values, they are not a dictionary</remarks>
//    public class NameValuePair
//    {
//        /// <summary>
//        /// The name for this variable
//        /// </summary>
//        public string Name { get; set; }
//        /// <summary>
//        /// The value for this variable
//        /// </summary>
//        public string Value { get; set; }
//    }
//}